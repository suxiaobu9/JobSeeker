using Microsoft.Extensions.Logging;
using Model;
using System.Text;
using System.Text.Json;

namespace Service.Http;

public class Http104JobService : IHttpService
{
    private readonly ILogger<Http104JobService> logger;
    private readonly HttpClient httpClient;

    public Http104JobService(ILogger<Http104JobService> logger,
        IHttpClientFactory httpClientFactory)
    {
        this.logger = logger;
        this.httpClient = httpClientFactory.CreateClient(_104Parameters.Referer);
    }

    /// <summary>
    /// 從工作清單取得工作內容網址
    /// </summary>
    /// <param name="jobListData"></param>
    /// <returns></returns>
    public IEnumerable<string>? GetJobInfoUrlFromJobList(string jobListData)
    {
        var currentMethod = "Http104JobService.GetJobInfoUrlFromJobList";
        var jobList = JsonSerializer.Deserialize<_104JobListModel>(jobListData);

        if (jobList == null)
        {
            logger.LogWarning($"{{currentMethod}} deserialize job list fail.{{jobListData}}", currentMethod, jobListData);
            return null;
        }

        return jobList.Data.List
                .Select(x =>
                {
                    var jobInfoWebUrl =
                    new StringBuilder("https://")
                        .Append(x.Link.Job.TrimStart("https:".ToCharArray()).TrimStart("http:".ToCharArray()).TrimStart("//".ToCharArray()))
                        .ToString();

                    return new Uri(jobInfoWebUrl).Segments.LastOrDefault();
                }).Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => _104Parameters.Get104JobInfoUrl(x!));
    }

    /// <summary>
    /// 取得職缺內容
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="url"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public Task<T?> GetJobInfoAsync<T>(string url)
    {
        var currentMethod = "Http104JobService.GetJobInfoAsync";
        logger.LogInformation($"{{currentMethod}} start.", currentMethod);

        return GetDataFromHttpRequest<T>(url);
    }

    /// <summary>
    /// 取得工作清單
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="url"></param>
    /// <returns></returns>
    public Task<T?> GetJobListAsync<T>(string url)
    {
        var currentMethod = "Http104JobService.GetJobListAsync";

        logger.LogInformation($"{{currentMethod}} start.", currentMethod);

        return GetDataFromHttpRequest<T>(url);
    }

    private async Task<T?> GetDataFromHttpRequest<T>(string url)
    {
        var currentMethod = "Http104JobService.GetDataFromHttpRequest";
        var response = await httpClient.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            logger.LogWarning($"{{currentMethod}} get response fail.{{url}}, {{statusCode}}, {{content}}", currentMethod, url, response.StatusCode, content);
            return default;
        }

        return JsonSerializer.Deserialize<T>(content);
    }

    /// <summary>
    /// 取得公司資訊
    /// </summary>
    /// <param name="companyNo"></param>
    /// <returns></returns>
    public async Task<string?> GetCompanyInfo(string companyNo)
    {
        var currentMethod = "Http104JobService.GetCompanyInfo";
        var url = _104Parameters.Get104CompanyUrl(companyNo);

        var response = await httpClient.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            logger.LogWarning($"{{currentMethod}} get response fail.{{url}}, {{statusCode}}, {{content}}", currentMethod, url, response.StatusCode, content);
            return null;
        }

        var companyInfo = JsonSerializer.Deserialize<_104CompanyInfoModel>(content);

        if (companyInfo == null)
        {
            logger.LogWarning($"{{currentMethod}} deserialize company info fail.{{companyInfo}}", currentMethod, content);
            return null;
        }

        companyInfo.Data.CustLink = url;

        return JsonSerializer.Serialize(companyInfo);
    }
}
