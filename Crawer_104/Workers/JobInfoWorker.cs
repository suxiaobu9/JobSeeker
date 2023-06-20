using Model;
using Service.Http;
using Service.Mq;

namespace Crawer_104.Workers;

public class JobInfoWorker : BackgroundService
{
    private readonly ILogger<JobInfoWorker> logger;
    private readonly IHttpService get104JobService;
    private readonly IMqService mqService;

    public JobInfoWorker(ILogger<JobInfoWorker> logger,
        IHttpService get104JobService,
        IMqService mqService)
    {
        this.logger = logger;
        this.get104JobService = get104JobService;
        this.mqService = mqService;
    }


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var currentMethod = "JobInfoWorker.ExecuteAsync";
        logger.LogInformation($"{{currentMethod}} running at: {{time}}", currentMethod, DateTimeOffset.Now);

        mqService.ProcessMessageFromMq(_104Parameters._104JobListQueueName, GetJobListAndSendJobInfoToMq, 10);

        while (!stoppingToken.IsCancellationRequested)
            await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
    }

    private async Task GetJobListAndSendJobInfoToMq(string jobListData)
    {
        var currentMethod = "JobInfoWorker.GetJobListAndSendJobInfoToMq";
        try
        {
            logger.LogInformation($"{{currentMethod}} get job info url from job list.", currentMethod);

            var jobInfoUrls = get104JobService.GetJobInfoUrlFromJobList(jobListData);

            if (jobInfoUrls == null)
            {
                logger.LogWarning($"{{currentMethod}} get job info url from job list get null. {{jobListData}}", currentMethod, jobListData);
                return;
            }

            foreach (var jobInfoUrl in jobInfoUrls)
            {
                var jobInfo = await get104JobService.GetJobInfoAsync<_104JobInfoModel>(jobInfoUrl);
                mqService.SendMessageToMq(_104Parameters._104JobInfoQueueName, jobInfo);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"{{currentMethod}} get exception.", currentMethod);
        }

    }
}
