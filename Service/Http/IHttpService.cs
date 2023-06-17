namespace Service.Http;

public interface IHttpService
{
    /// <summary>
    /// 取得工作清單
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="url"></param>
    /// <returns></returns>
    public Task<T?> GetJobListAsync<T>(string url);

}
