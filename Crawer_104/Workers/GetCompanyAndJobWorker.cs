using Model.Dto;
using Model.Dto104;
using Service.Cache;
using Service.Http;
using Service.Mq;

namespace Crawer_104.Workers;

public class GetCompanyAndJobWorker : BackgroundService
{
    private readonly ILogger<GetCompanyAndJobWorker> logger;
    private readonly ICacheService cacheService;
    private readonly IHttpService httpService;
    private readonly IMqService mqService;

    public GetCompanyAndJobWorker(ILogger<GetCompanyAndJobWorker> logger,
        ICacheService cacheService,
        IHttpService httpService,
        IMqService mqService)
    {
        this.logger = logger;
        this.cacheService = cacheService;
        this.httpService = httpService;
        this.mqService = mqService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {

            logger.LogInformation($"{nameof(GetCompanyAndJobWorker)} ExecuteAsync start.");

            // 刪除所有已存在的 company 與 job 的 Redis 資料
            await cacheService.ResetExistCompanyAndJob();

            foreach (var (Area, Keyword) in Parameters104.AreaAndKeywords)
            {
                var totalPage = 1;
                for (var i = 1; i <= totalPage; i++)
                {
                    var url = Parameters104.Get104JobListUrl(Keyword, Area, i);

                    // 取得職缺清單
                    var jobListData = await httpService.GetJobList<JobListWithPageDto>(url);

                    if (jobListData == null || jobListData.JobList == null)
                        break;

                    if (i == 1)
                        totalPage = jobListData.TotalPage;


                    foreach (var item in jobListData.JobList)
                    {
                        if (!await cacheService.IsKeyFieldExistsInCache(Parameters104.Redis104CompanyHashSetKey, item.CompanyId))
                        {
                            // 送 company id 到 mq
                            await mqService.SendMessageToMq(Parameters104.CompanyIdQueueName, item.CompanyId);
                        }
                        if (!await cacheService.IsKeyFieldExistsInCache(Parameters104.Redis104JobHashSetKey, item.JobId))
                        {
                            // 送 job id 到 mq
                            await mqService.SendMessageToMq(Parameters104.JobIdQueueName, item.JobId);
                        }
                    }
                }
            }

            logger.LogInformation($"{nameof(GetCompanyAndJobWorker)} ExecuteAsync end.");
            await Task.Delay(TimeSpan.FromHours(6), stoppingToken);
        }
    }
}
