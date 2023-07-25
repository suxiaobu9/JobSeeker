using Azure.Messaging.ServiceBus;
using Model.Dto;
using Model.Dto104;
using Service.Cache;
using Service.Db;
using Service.Http;
using Service.Mq;
using StackExchange.Redis;
using System.Text.Json;

namespace Crawer_104.Workers;

public class JobInfoToDbWorker : BackgroundService
{
    private readonly ILogger<JobInfoToDbWorker> logger;
    private readonly IMqService mqService;
    private readonly ICacheService cacheService;
    private readonly IHttpService httpService;
    private readonly IDbService dbService;
    private readonly IDatabase redisDb;

    public JobInfoToDbWorker(ILogger<JobInfoToDbWorker> logger,
        IMqService mqService,
        ICacheService cacheService,
        IHttpService httpService,
        IDbService dbService,
        IDatabase redisDb)
    {
        this.logger = logger;
        this.mqService = mqService;
        this.cacheService = cacheService;
        this.httpService = httpService;
        this.dbService = dbService;
        this.redisDb = redisDb;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation($"{nameof(CompanyToDbWorker)} ExecuteAsync start.");
        await mqService.ProcessMessageFromMq(Parameters104.QueueNameForJobId, MessageHandler);

        while (true)
            await Task.Delay(TimeSpan.FromDays(1), stoppingToken);

    }

    /// <summary>
    /// 處理 Company id mq 訊息
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    private async Task MessageHandler(ProcessMessageEventArgs args)
    {
        // get get mq (job_id_for_104)
        string message = args.Message.Body.ToString();
        try
        {
            var simpleJobInfo = JsonSerializer.Deserialize<SimpleJobInfoDto>(message);

            if (simpleJobInfo == null || string.IsNullOrWhiteSpace(simpleJobInfo.JobId) || string.IsNullOrWhiteSpace(simpleJobInfo.CompanyId))
            {
                logger.LogWarning($"{nameof(JobInfoToDbWorker)} MessageHandler get null simpleJobInfo.{{message}}", message);
                await args.CompleteMessageAsync(args.Message);
                return;
            }

            var jobId = simpleJobInfo.JobId;
            var companyId = simpleJobInfo.CompanyId;

            // 確定公司資訊是否新增了
            if (!await cacheService.CompanyExist(Parameters104.RedisKeyForCompanyUpdated, companyId))
            {
                logger.LogInformation($"{nameof(JobInfoToDbWorker)} company not exist, renew message.{{message}}", message);
                
                await Task.Delay(TimeSpan.FromSeconds(10));
                
                // 把 Message 推回 MQ
                await args.AbandonMessageAsync(args.Message);

                return;
            }

            // get job dto
            var jobDto = await httpService.GetJobInfo<JobDto>(jobId, companyId, Parameters104.Get104JobInfoUrl(jobId));

            if (jobDto == null)
            {
                logger.LogWarning($"{nameof(JobInfoToDbWorker)} MessageHandler get null jobDto.{{jobId}}", jobId);
                await args.CompleteMessageAsync(args.Message);
                return;
            }

            if (!jobDto.FilterPassed)
            {
                //logger.LogWarning($"{nameof(JobInfoToDbWorker)} JobDto filter failed.{{jobId}}", jobId);
                await args.CompleteMessageAsync(args.Message);
                return;
            }

            // save to db
            await dbService.UpsertJob(jobDto);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"{nameof(JobInfoToDbWorker)} MessageHandler get exception.{{message}}", message);
        }

        await args.CompleteMessageAsync(args.Message);
    }
}
