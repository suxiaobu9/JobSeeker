namespace Service.Http;

public interface IHttpService
{
    /// <summary>
    /// 取得職缺內容
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="url"></param>
    /// <returns></returns>
    public Task<T?> GetJobListAsync<T>(string url);

    /// <summary>
    /// 取得工作資訊
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="url"></param>
    /// <returns></returns>
    public Task<T?> GetJobInfoAsync<T>(string url);

    /// <summary>
    /// 從工作清單取得工作內容網址
    /// </summary>
    /// <param name="jobListData"></param>
    /// <returns></returns>
    public IEnumerable<string>? GetJobInfoUrlFromJobList(string jobListData);

    /// <summary>
    /// 取得公司資訊
    /// </summary>
    /// <param name="string"></param>
    /// <returns></returns>
    public Task<string?> GetCompanyInfo(string companyNo);
}
