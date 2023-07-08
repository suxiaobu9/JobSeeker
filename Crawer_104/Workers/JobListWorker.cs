using Model;
using Service.Db;
using Service.Http;
using Service.Mq;
using StackExchange.Redis;

namespace Crawer_104.Workers;

public class JobListWorker : BackgroundService
{
    private readonly ILogger<JobListWorker> logger;
    private readonly IHttpService get104JobService;
    private readonly IMqService mqService;
    private readonly IDbService dbService;
    private readonly IDatabase redisDb;

    public JobListWorker(ILogger<JobListWorker> logger,
        IHttpService get104JobService,
        IMqService mqService,
        IDbService dbService,
        IDatabase redisDb)
    {
        this.logger = logger;
        this.get104JobService = get104JobService;
        this.mqService = mqService;
        this.dbService = dbService;
        this.redisDb = redisDb;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var currentMethod = "JobListWorker.ExecuteAsync";
        while (!stoppingToken.IsCancellationRequested)
        {
            // 刪除 Redis 中所有已更新的公司 id
            await redisDb.KeyDeleteAsync(_104Parameters.Redis104CompanyHashSetKey);
            await redisDb.KeyDeleteAsync(_104Parameters.Redis104JobHashSetKey);

            logger.LogInformation($"{{currentMethod}} running at: {{time}}", currentMethod, DateTimeOffset.Now);

            await GetJobListAndSendMessageToMq();

            logger.LogInformation($"{{currentMethod}} end at: {{time}}", currentMethod, DateTimeOffset.Now);

            await Task.Delay(TimeSpan.FromHours(2), stoppingToken);
        }
    }

    /// <summary>
    /// 取得職缺資訊並新增訊息至 MQ
    /// </summary>
    /// <returns></returns>
    private async Task GetJobListAndSendMessageToMq()
    {
        var currentMethod = "JobListWorker.GetJobListAndSendMessageToMq";

        // 先將所有工作刪除
        await dbService.SetAllUndeleteJobToDelete();

        foreach ((string jobArea, string keyword) in _104Parameters.AreaAndKeywords)
        {
            var totalPage = 1;

            for (var i = 1; i <= totalPage; i++)
            {
                try
                {
                    var getJobListUrl = string.Format(_104Parameters.Get104JobListUrl(keyword, jobArea, i));

                    logger.LogInformation($"{{currentMethod}} Start get initial Data.", currentMethod);

                    // 取得工作清單
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
                catch (Exception ex)
                {
                    logger.LogError(ex, $"{{currentMethod}} get job list get exception.{{jobArea}} {{keyword}} {{i}}", currentMethod, jobArea, keyword, i);
                    continue;
                }
            }
        }
    }
}
