using Azure.Messaging.ServiceBus;
using Model.Dto;
using Model.Dto104;
using Service;
using Service.Cache;
using Service.Db;
using Service.Http;
using Service.Mq;
using System.Text.Json;

namespace Crawer_104.Workers;

public class OneZeroFourWorker : BackgroundService
{
    private readonly ILogger<OneZeroFourWorker> logger;
    private readonly ICacheService cacheService;
    private readonly IHttpService httpService;
    private readonly IMqService mqService;
    private readonly IDbService dbService;

    public OneZeroFourWorker(ILogger<OneZeroFourWorker> logger,
        ICacheService cacheService,
        IHttpService httpService,
        IMqService mqService,
        IDbService dbService)
    {
        this.logger = logger;
        this.cacheService = cacheService;
        this.httpService = httpService;
        this.mqService = mqService;
        this.dbService = dbService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _ = mqService.ProcessMessageFromMq<ProcessMessageEventArgs>(Parameters104.QueueNameForCompanyId, CompanyMessageHandler);
        _ = mqService.ProcessMessageFromMq<ProcessMessageEventArgs>(Parameters104.QueueNameForJobId, JobInfoMessageHandler);

        while (!stoppingToken.IsCancellationRequested)
        {
            logger.LogInformation($"{nameof(OneZeroFourWorker)} ExecuteAsync start.");

            var delayTask = CommonService.WorkerWaiting();

            // 刪除所有已存在的 company 與 job 的 Redis 資料
            await cacheService.ResetExistCompanyAndJob();
            await dbService.MakeAllJobAsDelete(Parameters104.SourceFrom);

            foreach (var (Area, Keyword) in Parameters104.AreaAndKeywords)
            {
                var totalPage = 1;
                for (var i = 1; i <= totalPage; i++)
                {
                    var url = Parameters104.Get104JobListUrl(Keyword, Area, i);
                    try
                    {

                        // 取得職缺清單
                        var jobListData = await httpService.GetJobList<JobListWithPageDto>(url);

                        if (jobListData == null || jobListData.JobList == null)
                            break;

                        if (i == 1)
                            totalPage = jobListData.TotalPage;


                        foreach (var item in jobListData.JobList)
                        {
                            if (!await cacheService.IsKeyFieldExistsInCache(Parameters104.RedisKeyForCompanyIdSendToQueue, item.CompanyId))
                            {
                                // 送 company id 到 mq
                                await mqService.SendMessageToMq(Parameters104.QueueNameForCompanyId, item.CompanyId);
                            }

                            if (!await cacheService.IsKeyFieldExistsInCache(Parameters104.RedisKeyForJobIdSendToQueue, item.CompanyId + item.JobId))
                            {
                                // 送 job id 到 mq
                                await mqService.SendMessageToMq(Parameters104.QueueNameForJobId, item);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, $"{nameof(OneZeroFourWorker)} ExecuteAsync get exception.{{url}}", url);
                    }
                }
            }

            logger.LogInformation($"{nameof(OneZeroFourWorker)} ExecuteAsync end.");
            await delayTask;
        }
    }

    /// <summary>
    /// 處理 Company id mq 訊息
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    private async Task CompanyMessageHandler(ProcessMessageEventArgs args)
    {
        logger.LogInformation($"CompanyMessageHandler start.");

        // get mq (comp_id_for_104)
        string companyId = args.Message.Body.ToString();

        try
        {
            // get company dto
            var companyDto = await httpService.GetCompanyInfo<CompanyDto>(companyId, Parameters104.Get104CompanyInfoUrl(companyId));

            if (companyDto == null)
            {
                logger.LogWarning($"CompanyMessageHandler get null companyDto.{{companyId}}", companyId);
                await args.CompleteMessageAsync(args.Message);
                return;
            }

            // save to db
            await dbService.UpsertCompany(companyDto);

        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"CompanyMessageHandler get exception.{{companyId}}", companyId);
        }
        await args.CompleteMessageAsync(args.Message);
    }

    /// <summary>
    /// 處理 Jobinfo mq 訊息
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    private async Task JobInfoMessageHandler(ProcessMessageEventArgs args)
    {
        logger.LogInformation($"JobInfoMessageHandler start.");

        // get get mq (job_id_for_104)
        string message = args.Message.Body.ToString();
        try
        {
            var simpleJobInfo = JsonSerializer.Deserialize<SimpleJobInfoDto>(message);

            if (simpleJobInfo == null || string.IsNullOrWhiteSpace(simpleJobInfo.JobId) || string.IsNullOrWhiteSpace(simpleJobInfo.CompanyId))
            {
                logger.LogWarning($"JobInfoMessageHandler MessageHandler get null simpleJobInfo.{{message}}", message);
                await args.CompleteMessageAsync(args.Message);
                return;
            }

            var jobId = simpleJobInfo.JobId;
            var companyId = simpleJobInfo.CompanyId;

            // 確定公司資訊是否新增了
            if (!await cacheService.CompanyExist(Parameters104.RedisKeyForCompanyUpdated, companyId))
            {
                logger.LogInformation($"JobInfoMessageHandler company not exist, renew message.{{message}}", message);

                await Task.Delay(TimeSpan.FromSeconds(10));

                // 把 Message 推回 MQ
                await args.AbandonMessageAsync(args.Message);

                return;
            }

            // get job dto
            var jobDto = await httpService.GetJobInfo<JobDto>(jobId, companyId, Parameters104.Get104JobInfoUrl(jobId));

            if (jobDto == null)
            {
                logger.LogWarning($"JobInfoMessageHandler MessageHandler get null jobDto.{{jobId}}", jobId);
                await args.CompleteMessageAsync(args.Message);
                return;
            }

            if (!jobDto.FilterPassed)
            {
                //logger.LogWarning($"JobInfoMessageHandler JobDto filter failed.{{jobId}}", jobId);
                await args.CompleteMessageAsync(args.Message);
                return;
            }

            // save to db
            await dbService.UpsertJob(jobDto);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"JobInfoMessageHandler MessageHandler get exception.{{message}}", message);
        }

        await args.CompleteMessageAsync(args.Message);
    }
}
