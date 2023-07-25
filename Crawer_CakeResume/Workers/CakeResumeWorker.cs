using Model;
using Model.Dto;
using Model.DtoCakeResume;
using Service.Cache;
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

    public CakeResumeWorker(ILogger<CakeResumeWorker> logger,
        IHttpService httpService,
        IDbService dbService,
        ICacheService cacheService,
        IDatabase redisDb)
    {
        this.logger = logger;
        this.httpService = httpService;
        this.dbService = dbService;
        this.cacheService = cacheService;
        this.redisDb = redisDb;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                logger.LogInformation($"{nameof(CakeResumeWorker)} ExecuteAsync start.");
                await cacheService.ResetExistCompanyAndJob();
                await dbService.MakeAllJobAsDelete(ParametersCakeResume.SourceFrom);

                var jobInfoList = await GetJobIdAndCompIdAry();

                await UpsertCompanies(GetCompanyDtoTaskAry(jobInfoList));

                await UpsertJobs(GetJobDtoAry(jobInfoList));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(CakeResumeWorker)} get exception.");
            }

            logger.LogInformation($"{nameof(CakeResumeWorker)} ExecuteAsync end.");
            await Task.Delay(TimeSpan.FromHours(12), stoppingToken);
        }
    }

    /// <summary>
    /// 取得職缺 id 與公司 id 的陣列
    /// </summary>
    /// <returns></returns>
    private async Task<List<SimpleJobInfoDto>> GetJobIdAndCompIdAry()
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

        return result;
    }

    /// <summary>
    /// 取得公司資訊的陣列
    /// </summary>
    /// <param name="jobInfoList"></param>
    /// <returns></returns>
    private IEnumerable<Task<CompanyDto?>> GetCompanyDtoTaskAry(List<SimpleJobInfoDto> jobInfoList)
    {
        var result = jobInfoList.Select(async jobInfo =>
        {
            var companyId = jobInfo.CompanyId;

            if (await cacheService.IsKeyFieldExistsInCache(ParametersCakeResume.RedisKeyForCompanyAlreadyGet, companyId))
                return null;

            var url = ParametersCakeResume.GetCompanyUrl(companyId);
            return await httpService.GetCompanyInfo<CompanyDto>(companyId, url);
        });

        return result;
    }

    /// <summary>
    /// 更新或新增公司
    /// </summary>
    /// <param name="companyDtoTaskAry"></param>
    /// <returns></returns>
    public async Task UpsertCompanies(IEnumerable<Task<CompanyDto?>?> companyDtoTaskAry)
    {
        foreach (var dtoTask in companyDtoTaskAry)
        {
            if (dtoTask == null)
                continue;

            var dto = await dtoTask;

            if (dto == null)
                continue;

            await dbService.UpsertCompany(dto);

        }
    }

    /// <summary>
    /// 取得職缺資訊的陣列
    /// </summary>
    /// <param name="jobInfoList"></param>
    /// <returns></returns>
    private IEnumerable<Task<JobDto?>?> GetJobDtoAry(List<SimpleJobInfoDto> jobInfoList)
    {
        return jobInfoList.Select(async jobInfo =>
        {
            if (await cacheService.IsKeyFieldExistsInCache(ParametersCakeResume.RedisKeyForJobAlreadyGet, jobInfo.CompanyId + jobInfo.JobId))
                return null;

            if (!await cacheService.CompanyExist(ParametersCakeResume.RedisKeyForCompanyUpdated, jobInfo.CompanyId))
            {
                logger.LogWarning($"{nameof(CakeResumeWorker)} GetJobDtoAry company not exist.{{compId}} {{jobId}}", jobInfo.CompanyId, jobInfo.JobId);
                return null;
            }

            var url = ParametersCakeResume.GetJobUrl(jobInfo.CompanyId, jobInfo.JobId);

            return await httpService.GetJobInfo<JobDto>(jobInfo.JobId, jobInfo.CompanyId, url);
        });
    }

    /// <summary>
    /// 更新或新增職缺資訊
    /// </summary>
    /// <param name="jobDtoTaskAry"></param>
    /// <returns></returns>
    private async Task UpsertJobs(IEnumerable<Task<JobDto?>?> jobDtoTaskAry)
    {
        foreach (var dtoTask in jobDtoTaskAry)
        {
            if (dtoTask == null)
                continue;

            var dto = await dtoTask;

            if (dto == null)
                continue;

            if (!dto.FilterPassed)
                continue;

            await dbService.UpsertJob(dto);

        }
    }
}
