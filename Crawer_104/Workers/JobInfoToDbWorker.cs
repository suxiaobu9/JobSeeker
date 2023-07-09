using Model;
using Model.JobSeekerDb;
using Service.Db;
using Service.Http;
using Service.Mq;
using StackExchange.Redis;

namespace Crawer_104.Workers;

public class JobInfoToDbWorker : BackgroundService
{
    private readonly ILogger<JobInfoToDbWorker> logger;
    private readonly IMqService mqService;
    private readonly IDbService dbService;
    private readonly IHttpService get104JobService;
    private readonly IDatabase redisDb;

    public JobInfoToDbWorker(ILogger<JobInfoToDbWorker> logger,
        IMqService mqService,
        IDbService dbService,
        IHttpService get104JobService,
        IDatabase redisDb)
    {
        this.logger = logger;
        this.mqService = mqService;
        this.dbService = dbService;
        this.get104JobService = get104JobService;
        this.redisDb = redisDb;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var currentMethod = "JobInfoToDbWorker.ExecuteAsync";
        logger.LogInformation($"{{currentMethod}} running at: {{time}}", currentMethod, DateTimeOffset.Now);

        mqService.ProcessMessageFromMq(_104Parameters._104JobInfoQueueName, GetJobInfoAndSaveToDb, 10);


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
            if (await IsKeyFieldExistsInRedis(_104Parameters.Redis104CompanyHashSetKey, companyNo))
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
            if (await IsKeyFieldExistsInRedis(_104Parameters.Redis104JobHashSetKey, jobInfo.Id))
                return;

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

    private async Task<bool> IsKeyFieldExistsInRedis(string key, string field)
    {
        var dataExist = await redisDb.HashExistsAsync(key, field);

        if (dataExist)
            return true;

        var redisTrans = redisDb.CreateTransaction();

        // 1. 如果 key 不存在才執行下一個動作
        redisTrans.AddCondition(StackExchange.Redis.Condition.HashNotExists(key, field));

        // 2. 第 1 點條件成立才會執行
        var setValueTask = redisTrans.HashSetAsync(key, field, "");

        // 有完整執行到第 2 點 committed 才會是 true
        var committed = await redisTrans.ExecuteAsync();

        if (!committed && setValueTask.IsCanceled)
            return true;

        if (committed)
            return false;

        logger.LogWarning("{key} {field} committed {committed}. {IsCanceled},{IsCompleted},{IsCompletedSuccessfully},{IsFaulted}",
            key, field, committed, setValueTask.IsCanceled, setValueTask.IsCompleted, setValueTask.IsCompletedSuccessfully, setValueTask.IsFaulted);

        return !committed;
    }
}
