using Model;
using Service.Cache;
using Service.Http;
using Service.Mq;

namespace Crawer_104.Workers;

public class JobInfoWorker : BackgroundService
{
    private readonly ILogger<JobInfoWorker> logger;
    private readonly IHttpService get104JobService;
    private readonly IMqService mqService;
    private readonly ICacheService cacheService;

    public JobInfoWorker(ILogger<JobInfoWorker> logger,
        IHttpService get104JobService,
        IMqService mqService,
        ICacheService cacheService)
    {
        this.logger = logger;
        this.get104JobService = get104JobService;
        this.mqService = mqService;
        this.cacheService = cacheService;
    }


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var currentMethod = "JobInfoWorker.ExecuteAsync";
        logger.LogInformation($"{{currentMethod}} running at: {{time}}", currentMethod, DateTimeOffset.Now);

        mqService.ProcessMessageFromMq(_104Parameters._104JobListQueueName, GetJobListAndSendJobInfoToMq, 20);

        while (!stoppingToken.IsCancellationRequested)
            await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
    }

    private async Task GetJobListAndSendJobInfoToMq(string jobListData)
    {
        var currentMethod = "JobInfoWorker.GetJobListAndSendJobInfoToMq";
        try
        {
            logger.LogInformation($"{{currentMethod}} get job info url from job list.", currentMethod);

            // 取得工作清單中的所有職缺 Id
            var jobIds = get104JobService.GetJobIdFromJobList(jobListData);

            if (jobIds == null)
            {
                logger.LogWarning($"{{currentMethod}} get job info url from job list get null. {{jobListData}}", currentMethod, jobListData);
                return;
            }

            foreach (var jobId in jobIds)
            {
                if (await cacheService.IsKeyFieldExistsInCache(_104Parameters.Redis104JobHashSetKey, jobId))
                    continue;

                var jobInfoUrl = _104Parameters.Get104JobInfoUrl(jobId);

                var jobInfo = await get104JobService.GetJobInfoAsync<_104JobInfoModel>(jobInfoUrl);

                if (jobInfo == null || string.IsNullOrWhiteSpace(jobInfo.GetJobId))
                    continue;

                mqService.SendMessageToMq(_104Parameters._104JobInfoQueueName, jobInfo);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"{{currentMethod}} get exception.{{jobListData}}", currentMethod, jobListData);
        }

    }
}
