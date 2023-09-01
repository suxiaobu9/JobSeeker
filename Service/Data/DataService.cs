using Microsoft.Extensions.Logging;
using Model;
using Model.Dto;
using Model.Dto104;
using Service.Cache;
using Service.Db;
using Service.Delay;
using Service.Http;
using Service.Parameter;

namespace Service.Data;

public class DataService : IDataService
{
    private readonly ILogger<DataService> logger;
    private readonly IHttpService httpService;
    private readonly ICacheService cacheService;
    private readonly IParameterService parameterService;
    private readonly ITaskDelayService taskDelayService;
    private readonly IDbService dbService;

    public DataService(ILogger<DataService> logger,
        IHttpService httpService,
        ICacheService cacheService,
        IParameterService parameterService,
        ITaskDelayService taskDelayService,
        IDbService dbService)
    {
        this.logger = logger;
        this.httpService = httpService;
        this.cacheService = cacheService;
        this.parameterService = parameterService;
        this.taskDelayService = taskDelayService;
        this.dbService = dbService;
    }

    /// <summary>
    /// 取得公司資訊並更新
    /// </summary>
    /// <param name="companyId"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<ReturnStatus> GetCompanyDataAndUpsert(GetCompanyInfoDto dto)
    {
        try
        {
            // get company dto
            var companyDto = await httpService.GetCompanyInfo<CompanyDto>(dto);

            if (companyDto == null)
            {
                logger.LogWarning($"{nameof(DataService)} GetCompanyDataAndUpsert get null companyDto.{{companyId}}", dto.CompanyId);
                return ReturnStatus.Fail;
            }

            // save to db
            await dbService.UpsertCompany(companyDto);

            return ReturnStatus.Success;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"{nameof(DataService)} GetCompanyDataAndUpsert error.");
            throw;
        }
    }

    /// <summary>
    /// 取得職缺資訊並更新
    /// </summary>
    /// <param name="simpleJobInfo"></param>
    /// <returns></returns>
    public async Task<ReturnStatus> GetJobDataAndUpsert(GetJobInfoDto dto)
    {
        try
        {
            // 確定公司資訊是否新增了
            if (!await cacheService.CompanyExist(Parameters104.RedisKeyForCompanyUpdated, dto.CompanyId))
            {
                logger.LogInformation($"{nameof(DataService)} GetJobDataAndUpsert company not exist, renew message.");

                await taskDelayService.Delay(TimeSpan.FromSeconds(10));

                return ReturnStatus.Retry;
            }

            // get job dto
            var jobDto = await httpService.GetJobInfo<JobDto>(dto);

            if (jobDto == null)
            {
                logger.LogWarning($"{nameof(DataService)} GetJobDataAndUpsert get null jobDto.{{jobId}}", dto.JobId);
                return ReturnStatus.Fail;
            }

            if (!jobDto.FilterPassed)
                return ReturnStatus.Success;

            // save to db
            await dbService.UpsertJob(jobDto);

            return ReturnStatus.Success;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"{nameof(DataService)} GetJobDataAndUpsert error.");
            throw;
        }
    }
}
