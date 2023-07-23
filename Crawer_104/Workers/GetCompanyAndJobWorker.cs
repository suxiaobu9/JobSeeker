using Model.Dto;
using Model.Dto104;
using Service.Cache;
using Service.Db;
using Service.Http;
using Service.Mq;

namespace Crawer_104.Workers;

public class GetCompanyAndJobWorker : BackgroundService
{
    private readonly ILogger<GetCompanyAndJobWorker> logger;
    private readonly ICacheService cacheService;
    private readonly IHttpService httpService;
    private readonly IMqService mqService;
    private readonly IDbService dbService;

    public GetCompanyAndJobWorker(ILogger<GetCompanyAndJobWorker> logger,
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
        while (!stoppingToken.IsCancellationRequested)
        {
            logger.LogInformation($"{nameof(GetCompanyAndJobWorker)} ExecuteAsync start.");

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
                            if (!await cacheService.IsKeyFieldExistsInCache(Parameters104.CompanyUpdated, item.CompanyId))
                            {
                                // 送 company id 到 mq
                                await mqService.SendMessageToMq(Parameters104.CompanyIdForRedisAndQueue, item.CompanyId);
                            }

                            if (!await cacheService.IsKeyFieldExistsInCache(Parameters104.JobUpdated, item.CompanyId + item.JobId))
                            {
                                // 送 job id 到 mq
                                await mqService.SendMessageToMq(Parameters104.JobIdForRedisAndQueue, item);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, $"{nameof(GetCompanyAndJobWorker)} ExecuteAsync get exception.{{url}}", url);
                    }
                }
            }

            logger.LogInformation($"{nameof(GetCompanyAndJobWorker)} ExecuteAsync end.");
            await Task.Delay(TimeSpan.FromHours(12), stoppingToken);
        }
    }
}
