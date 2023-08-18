using Model;
using Model.Dto;
using Model.DtoCakeResume;
using Service;
using Service.Cache;
using Service.Data;
using Service.Db;
using Service.Http;
using StackExchange.Redis;

namespace Crawer_CakeResume.Workers;

public class CakeResumeWorker : BackgroundService
{
    private readonly ILogger<CakeResumeWorker> logger;
    private readonly IHttpService httpService;
    private readonly IDbService dbService;
    private readonly ICacheService cacheService;
    private readonly IDatabase redisDb;
    private readonly IDataService dataService;

    public CakeResumeWorker(ILogger<CakeResumeWorker> logger,
        IHttpService httpService,
        IDbService dbService,
        ICacheService cacheService,
        IDatabase redisDb,
        IDataService dataService)
    {
        this.logger = logger;
        this.httpService = httpService;
        this.dbService = dbService;
        this.cacheService = cacheService;
        this.redisDb = redisDb;
        this.dataService = dataService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var delayTask = CommonService.WorkerWaiting();
            try
            {
                logger.LogInformation($"{nameof(CakeResumeWorker)} ExecuteAsync start.");
                await cacheService.ResetExistCompanyAndJob();
                await dbService.MakeAllJobAsDelete(ParametersCakeResume.SourceFrom);

                var jobInfoList = await GetJobIdAndCompIdAry();

                await UpsertCompanies(jobInfoList);

                await UpsertJobs(jobInfoList);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(CakeResumeWorker)} get exception.");
            }

            logger.LogInformation($"{nameof(CakeResumeWorker)} ExecuteAsync end.");
            await delayTask;
        }
    }

    /// <summary>
    /// 取得職缺 id 與公司 id 的陣列
    /// </summary>
    /// <returns></returns>
    private async Task<SimpleJobInfoDto[]> GetJobIdAndCompIdAry()
    {
        var result = new List<SimpleJobInfoDto>();

        foreach (var keyword in Parameters.Keywords)
        {
            for (var i = 1; ; i++)
            {
                var url = ParametersCakeResume.GetJobListUrl(keyword, i);

                var jobList = await httpService.GetJobList<JobListDto<SimpleJobInfoDto>>(url);

                if (jobList == null || jobList.JobList == null)
                    break;

                result.AddRange(jobList.JobList);
            }
        }

        return result.ToArray();
    }

    /// <summary>
    /// 更新或新增公司
    /// </summary>
    /// <param name="companyDtoTaskAry"></param>
    /// <returns></returns>
    public async Task UpsertCompanies(SimpleJobInfoDto[] jobInfoList)
    {
        var companyIdAry = jobInfoList.Select(x => x.CompanyId).Distinct().ToArray();

        foreach (var companyId in companyIdAry)
        {
            var dto = new GetCompanyInfoDto
            {
                CompanyId = companyId,
            };
            await dataService.GetCompanyDataAndUpsert(dto);
        }
    }

    /// <summary>
    /// 更新或新增職缺資訊
    /// </summary>
    /// <param name="jobDtoTaskAry"></param>
    /// <returns></returns>
    private async Task UpsertJobs(SimpleJobInfoDto[] jobInfoList)
    {
        foreach (var jobInfo in jobInfoList)
        {
            if (await cacheService.IsKeyFieldExistsInCache(ParametersCakeResume.RedisKeyForJobUpdated, jobInfo.CompanyId + jobInfo.JobId))
                continue;

            var dto = new GetJobInfoDto
            {
                CompanyId = jobInfo.CompanyId,
                JobId = jobInfo.JobId,
            };

            await dataService.GetJobDataAndUpsert(dto);
        }
    }
}
