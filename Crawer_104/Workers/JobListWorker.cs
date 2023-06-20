using Model;
using Service.Http;
using Service.Mq;

namespace Crawer_104.Workers;

public class JobListWorker : BackgroundService
{
    private readonly ILogger<JobListWorker> logger;
    private readonly IHttpService get104JobService;
    private readonly IMqService mqService;

    public JobListWorker(ILogger<JobListWorker> logger,
        IHttpService get104JobService,
        IMqService mqService)
    {
        this.logger = logger;
        this.get104JobService = get104JobService;
        this.mqService = mqService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var currentMethod = "JobListWorker.ExecuteAsync";
        while (!stoppingToken.IsCancellationRequested)
        {
            logger.LogInformation($"{{currentMethod}} running at: {{time}}", currentMethod, DateTimeOffset.Now);

            await GetJobListAndSendMessageToMq();

            await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
        }
    }

    private async Task GetJobListAndSendMessageToMq()
    {
        var currentMethod = "JobListWorker.GetJobListAndSendMessageToMq";
        foreach ((string jobArea, string keyword) in _104Parameters.AreaAndKeywords)
        {
            try
            {
                var totalPage = 1;

                for (var i = 1; i <= totalPage; i++)
                {
                    var getJobListUrl = string.Format(_104Parameters.Get104JobListUrl(keyword, jobArea, i));

                    logger.LogInformation($"{{currentMethod}} Start get initial Data.", currentMethod);

                    var jobList = await get104JobService.GetJobListAsync<_104JobListModel>(getJobListUrl);

                    if (i == 1)
                    {
                        if (jobList == null)
                        {
                            logger.LogWarning($"{{currentMethod}} Get initial job list fail.{{getJobListUrl}}", currentMethod, getJobListUrl);
                            break;
                        }

                        totalPage = jobList.Data.TotalPage;
                    }

                    if (jobList == null)
                    {
                        logger.LogWarning($"{{currentMethod}} Get job list fail.{{getJobListUrl}}", currentMethod, getJobListUrl);
                        continue;
                    }

                    if (jobList.Data.List.Length == 0)
                        break;

                    mqService.SendMessageToMq(_104Parameters._104JobListQueueName, jobList);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{{currentMethod}} get job list get exception.", currentMethod);
                continue;
            }
        }
    }
}
