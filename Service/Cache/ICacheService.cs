namespace Service.Cache;

public interface ICacheService
{
    /// <summary>
    /// key field 是否存在於 cache 中
    /// </summary>
    /// <param name="key"></param>
    /// <param name="field"></param>
    /// <returns></returns>
    public Task<bool> IsKeyFieldExistsInCache(string key, string field);

}
