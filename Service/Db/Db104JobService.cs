using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Model;
using Model.JobSeekerDb;
using System.Text;
using System.Text.Json;

namespace Service.Db;

public class Db104JobService : IDbService
{
    private readonly ILogger<Db104JobService> logger;
    private readonly postgresContext db;

    public Db104JobService(ILogger<Db104JobService> logger,
        postgresContext db)
    {
        this.logger = logger;
        this.db = db;
    }

    public Task<bool> CompanyExists(string companyNo)
    {
        return db.Companies.AnyAsync(x => x.Id == companyNo);
    }

    public Company? TransCompanyInfoToDbEntity(string companyInfoData)
    {
        var currentMethod = "Db104JobService.TransCompanyInfoToDbEntity";
        var companyInfo = JsonSerializer.Deserialize<_104CompanyInfoModel>(companyInfoData);

        if (companyInfo == null)
        {
            logger.LogWarning($"{{currentMethod}} deserialize company info fail.{{companyInfoData}}", currentMethod, companyInfoData);
            return null;
        }

        var now = DateTimeOffset.Now;

        var companyNo = new Uri(companyInfo.Data.CustLink).Segments.LastOrDefault();

        if (string.IsNullOrWhiteSpace(companyNo))
        {
            logger.LogWarning($"{{currentMethod}} company no is null or whitespace.{{companyInfoData}}", currentMethod, companyInfoData);
            return null;
        }

        var company = new Model.JobSeekerDb.Company
        {
            Id = companyNo,
            Name = companyInfo.Data.CustName,
            Url = companyInfo.Data.CustLink,
            CreateUtcAt = now.UtcDateTime,
            UpdateUtcAt = now.UtcDateTime,
            Ignore = false,
            Sort = 0,
        };

        return company;
    }

    /// <summary>
    /// 將工作資訊轉換成資料庫實體
    /// </summary>
    /// <param name="jobInfoJson"></param>
    /// <returns></returns>
    public Job? TransJobInfoToDbEntity(string jobInfoJson)
    {
        var currentMethod = "Db104JobService.TransJobInfoToDbEntity";
        var jobInfo = JsonSerializer.Deserialize<_104JobInfoModel>(jobInfoJson);

        if (jobInfo == null)
        {
            logger.LogWarning($"{{currentMethod}} deserialize job info fail.{{jobInfoJson}}", currentMethod, jobInfoJson);
            return null;
        }

        var analysisUrl = new StringBuilder("https:")
            .Append(jobInfo.Data.Header.AnalysisUrl
                            .TrimStart("http".ToCharArray())
                            .TrimStart("https".ToCharArray())
                            .TrimStart(":".ToCharArray())
                            .TrimStart("//".ToCharArray()))
            .ToString();

        var jobId = jobInfo.Data.Header.AnalysisUrl.Split('/').LastOrDefault();

        if (string.IsNullOrWhiteSpace(jobId))
        {
            logger.LogWarning($"{{currentMethod}} get job id fail.{{jobInfoJson}}", currentMethod, jobInfoJson); ;
            return null;
        }

        var companyId = new Uri(jobInfo.Data.Header.CustUrl).Segments.LastOrDefault();

        if (string.IsNullOrWhiteSpace(companyId))
        {
            logger.LogWarning($"{{currentMethod}} deserialize job info fail.{{jobInfoJson}}", currentMethod, jobInfoJson);
            return null;
        }

        var now = DateTimeOffset.Now;

        return new Model.JobSeekerDb.Job
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
            Ignore = false,
        };
    }

    /// <summary>
    /// 更新或新增公司資料
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task UpsertCompany(Company model)
    {
        await db.Companies.Upsert(model).On(x => x.Id)
            .RunAsync();

        await db.SaveChangesAsync();
    }

    /// <summary>
    /// 更新或新增工作資料
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task UpsertJob(Job newJobInfo)
    {
        var job = await db.Jobs.FirstOrDefaultAsync(x => x.Id == newJobInfo.Id);

        if (job == null)
        {
            await db.Jobs.Upsert(newJobInfo)
                .On(x => x.Id)
                .RunAsync();

        }
        else
        {
            var needUpdate = job.JobPlace != newJobInfo.JobPlace ||
                                 job.Salary != newJobInfo.Salary ||
                                 job.OtherRequirement != newJobInfo.OtherRequirement ||
                                 job.WorkContent != newJobInfo.WorkContent;

            if (needUpdate)
            {
                job.JobPlace = newJobInfo.JobPlace;
                job.Salary = newJobInfo.Salary;
                job.OtherRequirement = newJobInfo.OtherRequirement;
                job.WorkContent = newJobInfo.WorkContent;
                job.HaveRead = false;
            }

            job.IsDeleted = false;
            job.UpdateUtcAt = newJobInfo.UpdateUtcAt;
        }

        await db.SaveChangesAsync();
    }

    /// <summary>
    /// 將未刪除的職缺設為刪除
    /// </summary>
    /// <returns></returns>
    public async Task SetAllUndeleteJobToDelete()
    {
        var undeleteJobs = await db.Jobs.Where(x => !x.IsDeleted).ToArrayAsync();

        var now = DateTimeOffset.Now;

        foreach (var job in undeleteJobs)
        {
            job.IsDeleted = true;
            job.UpdateUtcAt = now.UtcDateTime;
        }

        await db.SaveChangesAsync();
    }
}
