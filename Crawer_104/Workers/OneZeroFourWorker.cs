using Model.Dto;
using Model.Dto104;
using RabbitMQ.Client.Events;
using Service;
using Service.Cache;
using Service.Db;
using Service.Delay;
using Service.Http;
using Service.Mq;

namespace Crawer_104.Workers;

public class OneZeroFourWorker : BackgroundService
{
    private readonly ILogger<OneZeroFourWorker> logger;
    private readonly ICacheService cacheService;
    private readonly IHttpService httpService;
    private readonly IMqService mqService;
    private readonly ITaskDelayService taskDelayService;
    private readonly IDbService dbService;

    public OneZeroFourWorker(ILogger<OneZeroFourWorker> logger,
        ICacheService cacheService,
        IHttpService httpService,
        IMqService mqService,
        ITaskDelayService taskDelayService,
        IDbService dbService)
    {
        this.logger = logger;
        this.cacheService = cacheService;
        this.httpService = httpService;
        this.mqService = mqService;
        this.taskDelayService = taskDelayService;
        this.dbService = dbService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _ = mqService.ProcessMessageFromMq<BasicDeliverEventArgs>(Parameters104.QueueNameForCompanyId, mqService.CompanyMessageHandler);
        _ = mqService.ProcessMessageFromMq<BasicDeliverEventArgs>(Parameters104.QueueNameForJobId, mqService.JobInfoMessageHandler);

        while (!stoppingToken.IsCancellationRequested)
        {
            logger.LogInformation($"{nameof(OneZeroFourWorker)} ExecuteAsync start.");

            var delayTask = taskDelayService.WorkerWaiting();

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
}
