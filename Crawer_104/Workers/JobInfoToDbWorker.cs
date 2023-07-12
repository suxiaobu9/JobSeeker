using Model;
using Model.JobSeekerDb;
using Service.Cache;
using Service.Db;
using Service.Http;
using Service.Mq;
using StackExchange.Redis;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Crawer_104.Workers;

public class JobInfoToDbWorker : BackgroundService
{
    private readonly ILogger<JobInfoToDbWorker> logger;
    private readonly IMqService mqService;
    private readonly IDbService dbService;
    private readonly IHttpService get104JobService;
    private readonly IDatabase redisDb;
    private readonly ICacheService cacheService;

    public JobInfoToDbWorker(ILogger<JobInfoToDbWorker> logger,
        IMqService mqService,
        IDbService dbService,
        IHttpService get104JobService,
        IDatabase redisDb,
        ICacheService cacheService)
    {
        this.logger = logger;
        this.mqService = mqService;
        this.dbService = dbService;
        this.get104JobService = get104JobService;
        this.redisDb = redisDb;
        this.cacheService = cacheService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var currentMethod = "JobInfoToDbWorker.ExecuteAsync";
        logger.LogInformation($"{{currentMethod}} running at: {{time}}", currentMethod, DateTimeOffset.Now);

        mqService.ProcessMessageFromMq(_104Parameters._104JobInfoQueueName, GetJobInfoAndSaveToDb);


        while (!stoppingToken.IsCancellationRequested)
            await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
    }

    private async Task GetJobInfoAndSaveToDb(string jobInfoData)
    {
        var currentMethod = "JobInfoToDbWorker.GetJobInfoAndSaveToDb";
        try
        {
            var jobInfo = dbService.TransJobInfoToDbEntity(jobInfoData);

            if (jobInfo == null)
            {
                logger.LogWarning($"{{currentMethod}} get job info from job list get null.{{jobInfoData}}", currentMethod, jobInfoData);
                return;
            }

            await UpsertCompany(jobInfo.CompanyId);

            await UpsertJob(jobInfo);

        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"{{currentMethod}} get exception.{{jobInfoData}}", currentMethod, jobInfoData);
        }
    }

    private async Task UpsertCompany(string companyNo)
    {
        var currentMethod = "JobInfoToDbWorker.UpsertCompany";

        try
        {
            if (await cacheService.IsKeyFieldExistsInCache(_104Parameters.Redis104CompanyHashSetKey, companyNo))
                return;

            logger.LogInformation($"{{currentMethod}} start upsert company info.{{companyNo}}", currentMethod, companyNo);

            var companyInfo = await get104JobService.GetCompanyInfo(companyNo);

            if (companyInfo == null)
            {
                logger.LogWarning($"{{currentMethod}} get company info get null.{{companyNo}}", currentMethod, companyNo);
                return;
            }

            var companyEntity = dbService.TransCompanyInfoToDbEntity(companyNo, companyInfo);

            if (companyEntity == null)
            {
                logger.LogWarning($"{{currentMethod}} get company entity get null.{{companyInfo}}", currentMethod, companyInfo);
                return;
            }

            await dbService.UpsertCompany(companyEntity);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "{currentMethod} Company data get exception.{companyNo}", currentMethod, companyNo);
            await redisDb.HashDeleteAsync(_104Parameters.Redis104CompanyHashSetKey, companyNo);
            throw;
        }
    }

    private async Task UpsertJob(Job jobInfo)
    {
        var currentMethod = "JobInfoToDbWorker.UpsertJob";
        try
        {
            if (!FilterPassed(jobInfo))
            {
                //logger.LogInformation("FilterPassed = false {jobUrl}.{jobInfo}", jobInfo.Url, JsonSerializer.Serialize(jobInfo));
                return;
            }

            logger.LogInformation($"{{currentMethod}} start upsert job info.{{jobId}}", currentMethod, jobInfo.Id);

            await dbService.UpsertJob(jobInfo);

        }
        catch (Exception ex)
        {
            logger.LogError(ex, "{currentMethod} get exception.{jobInfo}", currentMethod, jobInfo);
            await redisDb.HashDeleteAsync(_104Parameters.Redis104JobHashSetKey, jobInfo.Id);
            throw;
        }

    }

    private static bool FilterPassed(Job job)
    {
        var content = (job.WorkContent + job.OtherRequirement ?? "").ToLower();

        string urlPattern = @"https?://[^\s\u4E00-\u9FA5]+";

        var matches = Regex.Matches(content, urlPattern);

        foreach (Match match in matches)
        {
            content = content.Replace(match.Value, "");
        }

        return _104Parameters.KeywordsFilters.Any(x => content.Contains(x.ToLower()));

    }
}
