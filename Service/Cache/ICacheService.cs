namespace Service.Cache;

public interface ICacheService
{
    /// <summary>
    /// 重置已存在的公司與工作
    /// </summary>
    /// <returns></returns>
    public Task ResetExistCompanyAndJob();

    /// <summary>
    /// key field 是否存在於 cache 中
    /// </summary>
    /// <param name="key"></param>
    /// <param name="field"></param>
    /// <returns></returns>
    public Task<bool> IsKeyFieldExistsInCache(string key, string field);

}
