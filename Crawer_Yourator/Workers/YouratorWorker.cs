using Model;
using Model.Dto;
using Model.DtoYourator;
using Service.Cache;
using Service.Db;
using Service.Delay;
using Service.Http;
using Service.Mq;

namespace Crawer_Yourator.Workers;

public class YouratorWorker : BackgroundService
{
    private readonly ILogger<YouratorWorker> logger;
    private readonly ITaskDelayService taskDelayService;
    private readonly IDbService dbService;
    private readonly IHttpService httpService;
    private readonly IMqService mqService;
    private readonly ICacheService cacheService;

    public YouratorWorker(ILogger<YouratorWorker> logger,
        ITaskDelayService taskDelayService,
        IDbService dbService,
        IHttpService httpService,
        IMqService mqService,
        ICacheService cacheService)
    {
        this.logger = logger;
        this.taskDelayService = taskDelayService;
        this.dbService = dbService;
        this.httpService = httpService;
        this.mqService = mqService;
        this.cacheService = cacheService;
    }

    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            logger.LogInformation($"{nameof(YouratorWorker)} ExecuteAsync start.");
            var delayTask = taskDelayService.WorkerWaiting();
            await cacheService.ResetExistCompanyAndJob();
            await dbService.MakeAllJobAsDelete(ParametersYourator.SourceFrom);

            foreach (var keyword in Parameters.Keywords)
            {
                var currentPage = 1;

                while (true)
                {
                    try
                    {
                        var url = ParametersYourator.GetJobListUrl(keyword, currentPage);

                        var jobList = await httpService.GetJobList<JobListDto<SimpleJobInfoDto>>(url);

                        if (jobList == null || jobList.JobList == null)
                            break;

                        currentPage++;

                        foreach (var item in jobList.JobList)
                        {
                            if (!await cacheService.IsKeyFieldExistsInCache(ParametersYourator.RedisKeyForCompanyIdSendToQueue, item.CompanyId))
                            {
                                await mqService.SendMessageToMq(ParametersYourator.QueueNameForCompanyId, item.CompanyId);
                            }

                            if (!await cacheService.IsKeyFieldExistsInCache(ParametersYourator.RedisKeyForJobIdSendToQueue, item.JobId))
                            {
                                await mqService.SendMessageToMq(ParametersYourator.QueueNameForJobId, item.JobId);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, $"{nameof(YouratorWorker)} get exception.");
                        break;
                    }

                }
            }

            logger.LogInformation($"{nameof(YouratorWorker)} ExecuteAsync end.");
            await delayTask;
        }

    }
}
