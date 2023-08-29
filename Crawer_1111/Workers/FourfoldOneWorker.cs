using Model;
using Model.Dto;
using Model.Dto1111;
using Service.Cache;
using Service.Db;
using Service.Delay;
using Service.Http;
using Service.Mq;

namespace Crawer_1111.Workers;

public class FourfoldOneWorker : BackgroundService
{
    private readonly ILogger<FourfoldOneWorker> logger;
    private readonly IMqService mqService;
    private readonly ICacheService cacheService;
    private readonly IDbService dbService;
    private readonly IHttpService httpService;
    private readonly ITaskDelayService taskDelayService;

    public FourfoldOneWorker(ILogger<FourfoldOneWorker> logger,
        IMqService mqService,
        ICacheService cacheService,
        IDbService dbService,
        IHttpService httpService,
        ITaskDelayService taskDelayService)
    {
        this.logger = logger;
        this.mqService = mqService;
        this.cacheService = cacheService;
        this.dbService = dbService;
        this.httpService = httpService;
        this.taskDelayService = taskDelayService;
    }

    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        //_ = mqService.ProcessMessageFromMq<BasicDeliverEventArgs>(Parameters1111.QueueNameForCompanyId, mqService.CompanyMessageHandler);
        //_ = mqService.ProcessMessageFromMq<BasicDeliverEventArgs>(Parameters1111.QueueNameForJobId, mqService.JobInfoMessageHandler);

        while (!stoppingToken.IsCancellationRequested)
        {
            logger.LogInformation($"{nameof(FourfoldOneWorker)} ExecuteAsync start.");
            var delayTask = taskDelayService.WorkerWaiting();

            // 刪除所有已存在的 company 與 job 的 Redis 資料
            await cacheService.ResetExistCompanyAndJob();
            await dbService.MakeAllJobAsDelete(Parameters1111.SourceFrom);

            foreach (var keyword in Parameters.Keywords)
            {
                var currentPage = 1;

                while (true)
                {
                    try
                    {
                        var url = Parameters1111.GetJobListUrl(keyword, currentPage);

                        var jobList = await httpService.GetJobList<JobListDto<SimpleJobInfoDto>>(url);

                        if (jobList == null || jobList.JobList == null)
                            break;

                        currentPage++;
                        
                        foreach(var item in jobList.JobList)
                        {
                            if (!await cacheService.IsKeyFieldExistsInCache(Parameters1111.RedisKeyForCompanyIdSendToQueue, item.CompanyId))
                            {
                                await mqService.SendMessageToMq(Parameters1111.QueueNameForCompanyId, item.CompanyId);
                            }

                            if (!await cacheService.IsKeyFieldExistsInCache(Parameters1111.RedisKeyForJobIdSendToQueue, item.CompanyId + item.JobId))
                            {
                                // 送 job id 到 mq
                                await mqService.SendMessageToMq(Parameters1111.QueueNameForJobId, item);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, $"{nameof(FourfoldOneWorker)} ExecuteAsync error.");
                        break;
                    }
                }
            }

            logger.LogInformation($"{nameof(FourfoldOneWorker)} ExecuteAsync end.");
            await delayTask;
        }
    }
}
