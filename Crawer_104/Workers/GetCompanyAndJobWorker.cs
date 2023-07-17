using Model.Dto;
using Model.Dto104;
using Service.Cache;
using Service.Http;

namespace Crawer_104.Workers;

public class GetCompanyAndJobWorker : BackgroundService
{
    private readonly ILogger<GetCompanyAndJobWorker> logger;
    private readonly ICacheService cacheService;
    private readonly IHttpService httpService;

    public GetCompanyAndJobWorker(ILogger<GetCompanyAndJobWorker> logger,
        ICacheService cacheService,
        IHttpService httpService
        )
    {
        this.logger = logger;
        this.cacheService = cacheService;
        this.httpService = httpService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {

            logger.LogInformation($"{nameof(GetCompanyAndJobWorker)} ExecuteAsync start.");

            // delete all job and company redis key
            await cacheService.ResetExistCompanyAndJob();

            foreach (var (Area, Keyword) in Parameters104.AreaAndKeywords)
            {
                var totalPage = 1;
                for (var i = 1; i <= totalPage; i++)
                {
                    var url = Parameters104.Get104JobListUrl(Keyword, Area, i);
                    // get job list
                    var jobListData = await httpService.GetJobList<JobListWithPageDto>(url);

                    if (jobListData == null || jobListData.JobList == null)
                        break;

                    if (i == 1)
                        totalPage = jobListData.TotalPage;


                    foreach (var item in jobListData.JobList)
                    {
                        if (!await cacheService.IsKeyFieldExistsInCache(Parameters104.Redis104CompanyHashSetKey, item.CompanyId))
                        {
                            //todo: send company id to mq
                        }
                        if (!await cacheService.IsKeyFieldExistsInCache(Parameters104.Redis104JobHashSetKey, item.JobId))
                        {
                            //todo: send job id to mq
                        }
                    }
                }
            }

            logger.LogInformation($"{nameof(GetCompanyAndJobWorker)} ExecuteAsync end.");
            await Task.Delay(TimeSpan.FromHours(6), stoppingToken);
        }
    }
}
