namespace Service.Cache;

public interface ICacheService
{
    /// <summary>
    /// 重置已存在的公司與工作
    /// </summary>
    /// <returns></returns>
    public Task ResetExistCompanyAndJob();

    /// <summary>
    /// 公司存在
    /// </summary>
    /// <param name="companyId"></param>
    /// <returns></returns>
    public Task<bool> CompanyExist(string redisKey, string companyId);

    /// <summary>
    /// 職缺存在
    /// </summary>
    /// <param name="companyId"></param>
    /// <param name="jobId"></param>
    /// <returns></returns>
    public Task<bool> JobExist(string redisKey, string companyId, string jobId);

    /// <summary>
    /// key field 是否存在於 cache 中
    /// </summary>
    /// <param name="key"></param>
    /// <param name="field"></param>
    /// <returns></returns>
    public Task<bool> IsKeyFieldExistsInCache(string key, string field);

}
