using Grpc.Core.Logging;
using Microsoft.Extensions.Logging;
using Model;
using Model.Dto;
using Model.Dto104;
using Serilog.Core;
using Service.Cache;
using Service.Db;
using Service.Http;

namespace Service.Data;

public class DataService : IDataService
{
    private readonly ILogger<DataService> logger;
    private readonly IHttpService httpService;
    private readonly ICacheService cacheService;
    private readonly IDbService dbService;

    public DataService(ILogger<DataService> logger,
        IHttpService httpService,
        ICacheService cacheService,
        IDbService dbService)
    {
        this.logger = logger;
        this.httpService = httpService;
        this.cacheService = cacheService;
        this.dbService = dbService;
    }

    /// <summary>
    /// 取得公司資訊並更新
    /// </summary>
    /// <param name="companyId"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task GetCompanyDataAndUpsert(string companyId)
    {

        // get company dto
        var companyDto = await httpService.GetCompanyInfo<CompanyDto>(companyId, Parameters104.Get104CompanyInfoUrl(companyId));

        if (companyDto == null)
        {
            logger.LogWarning($"{nameof(DataService)} GetCompanyDataAndUpsert get null companyDto.{{companyId}}", companyId);
            return;
        }

        // save to db
        await dbService.UpsertCompany(companyDto);
    }

    /// <summary>
    /// 取得職缺資訊並更新
    /// </summary>
    /// <param name="simpleJobInfo"></param>
    /// <returns></returns>
    public async Task<ReturnStatus> GetJobDataAndUpsert(SimpleJobInfoDto? simpleJobInfo)
    {
        try
        {
            if (simpleJobInfo == null || string.IsNullOrWhiteSpace(simpleJobInfo.JobId) || string.IsNullOrWhiteSpace(simpleJobInfo.CompanyId))
            {
                logger.LogWarning($"{nameof(DataService)} GetJobDataAndUpsert get null simpleJobInfo.");
                return ReturnStatus.Fail;
            }

            // 確定公司資訊是否新增了
            if (!await cacheService.CompanyExist(Parameters104.RedisKeyForCompanyUpdated, simpleJobInfo.CompanyId))
            {
                logger.LogInformation($"{nameof(DataService)} GetJobDataAndUpsert company not exist, renew message.");

                await Task.Delay(TimeSpan.FromSeconds(10));

                return ReturnStatus.Retry;
            }

            // get job dto
            var jobDto = await httpService.GetJobInfo<JobDto>(simpleJobInfo.JobId, simpleJobInfo.CompanyId, Parameters104.Get104JobInfoUrl(simpleJobInfo.JobId));

            if (jobDto == null)
            {
                logger.LogWarning($"{nameof(DataService)} GetJobDataAndUpsert get null jobDto.{{jobId}}", simpleJobInfo.JobId);
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
            return ReturnStatus.Exception;
        }
    }
}
