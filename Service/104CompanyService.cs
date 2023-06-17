using Microsoft.EntityFrameworkCore;
using Model;
using Model.JobSeekerDb;
using RabbitMQ.Client.Events;
using Serilog;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace Service
{
    public class _104CompanyService
    {
        private readonly postgresContext db;
        private readonly HttpClient httpClient;

        public _104CompanyService(postgresContext db,
            HttpClient httpClient)
        {
            this.db = db;
            this.httpClient = httpClient;
        }

        public async Task AddJobInfoToDb(BasicDeliverEventArgs args)
        {
            string currentTag = "Crawer_ToDb_104";

            var body = args.Body.ToArray();
            var jobInfoFileNames = Encoding.UTF8.GetString(body);

            if (!File.Exists(jobInfoFileNames))
                return;

            var jobInfo = JsonSerializer.Deserialize<_104JobInfoModel>(File.ReadAllText(jobInfoFileNames));

            File.Delete(jobInfoFileNames);

            if (jobInfo == null)
                return;

            var companyId = new Uri(jobInfo.Data.Header.CustUrl).Segments.LastOrDefault();

            if (companyId == null)
                return;

            var now = DateTimeOffset.Now;

            if (!await db.Companies.AnyAsync(x => x.Id == companyId))
            {
                var url = _104Parameters.Get104CompanyUrl(companyId);

                var response = await httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    Log.Information($"{{tag}} get company info fail. {{companyId}}", currentTag, companyId);
                    return;
                }

                var companyInfo = await response.Content.ReadFromJsonAsync<_104CompanyInfoModel>();

                if (companyInfo == null)
                {
                    Log.Information($"{{tag}} get company info is null. {{companyId}}", currentTag, companyId);
                    return;
                }

                await db.Companies.Upsert(new Company
                {
                    Id = companyId,
                    Name = companyInfo.Data.CustName,
                    Sort = 0,
                    Url = url,
                    CreateUtcAt = now.UtcDateTime,
                    UpdateUtcAt = now.UtcDateTime,
                }).On(x => new { x.Id })
                .RunAsync();

                await db.SaveChangesAsync();
            }

            var jobId = jobInfo.Data.Header.AnalysisUrl.Split('/').LastOrDefault();

            if (string.IsNullOrWhiteSpace(jobId))
            {
                //todo: logger
                return;
            }

            var job = await db.Jobs.FirstOrDefaultAsync(x => x.Id == jobId);
            if (job == null)
            {

                await db.Jobs.Upsert(new Job
                {
                    Id = jobId,
                    CompanyId = companyId,
                    Name = jobInfo.Data.Header.JobName,
                    Url = _104Parameters.Get104JobInfoUrl(jobId),
                    CreateUtcAt = now.UtcDateTime,
                    UpdateUtcAt = now.UtcDateTime,
                    Sort = 0,
                    Salary = jobInfo.Data.JobDetail.Salary,
                    HaveRead = false,
                    JobPlace = jobInfo.Data.JobDetail.AddressRegion,
                    IsDeleted = false,
                    OtherRequirement = jobInfo.Data.Condition.Other,
                    WorkContent = jobInfo.Data.JobDetail.JobDescription,
                }).RunAsync();
            }
            else
            {
                var needUpdate = job.JobPlace != jobInfo.Data.JobDetail.AddressRegion ||
                                 job.Salary != jobInfo.Data.JobDetail.Salary ||
                                 job.OtherRequirement != jobInfo.Data.Condition.Other ||
                                 job.WorkContent != jobInfo.Data.JobDetail.JobDescription;

                if (!needUpdate)
                    return;

                job.JobPlace = jobInfo.Data.JobDetail.AddressRegion;
                job.Salary = jobInfo.Data.JobDetail.Salary;
                job.OtherRequirement = jobInfo.Data.Condition.Other;
                job.WorkContent = jobInfo.Data.JobDetail.JobDescription;
            }

            await db.SaveChangesAsync();
        }

    }
}
