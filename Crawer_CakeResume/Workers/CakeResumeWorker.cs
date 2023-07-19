using Model;
using Model.Dto;
using Model.Dto104;
using Model.DtoCakeResume;
using Nacos.V2.Utils;
using Service.Cache;
using Service.Db;
using Service.Http;
using System;
using System.Text;

namespace Crawer_CakeResume.Workers;

public class CakeResumeWorker : BackgroundService
{
    private readonly ILogger<CakeResumeWorker> logger;
    private readonly IHttpService httpService;
    private readonly IDbService dbService;
    private readonly ICacheService cacheService;

    public CakeResumeWorker(ILogger<CakeResumeWorker> logger,
        IHttpService httpService,
        IDbService dbService,
        ICacheService cacheService)
    {
        this.logger = logger;
        this.httpService = httpService;
        this.dbService = dbService;
        this.cacheService = cacheService;
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

            await Task.Delay(TimeSpan.FromHours(6), stoppingToken);
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
                //todo: 
                break;
            }
            //todo: 
            break;
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

            if (await cacheService.IsKeyFieldExistsInCache(ParametersCakeResume.CompanyIdForRedisAndQueue, companyId))
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
            {
                logger.LogWarning($"{nameof(CakeResumeWorker)} UpsertCompanies get null dto.");
                continue;
            }

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
            if (await cacheService.IsKeyFieldExistsInCache(ParametersCakeResume.JobIdForRedisAndQueue, jobInfo.JobId))
                return null;

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
            {
                logger.LogWarning($"{nameof(CakeResumeWorker)} UpsertJobs get null dto.");
                continue;
            }

            await dbService.UpsertJob(dto);

        }
    }
}
