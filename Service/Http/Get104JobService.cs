using Microsoft.Extensions.Logging;
using Model;
using System.Text.Json;

namespace Service.Http;

public class Get104JobService : IHttpService
{
    private readonly ILogger<Get104JobService> logger;
    private readonly HttpClient httpClient;

    public Get104JobService(ILogger<Get104JobService> logger,
        IHttpClientFactory httpClientFactory)
    {
        this.logger = logger;
        this.httpClient = httpClientFactory.CreateClient(_104Parameters.Referer);
    }

    /// <summary>
    /// 取得工作清單
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="url"></param>
    /// <returns></returns>
    public async Task<T?> GetJobListAsync<T>(string url)
    {
        logger.LogInformation($"{nameof(Get104JobService)} {nameof(GetJobListAsync)} start.{{url}}", url);

        var response = await httpClient.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            logger.LogError($"{nameof(Get104JobService)} {nameof(GetJobListAsync)} get response fail.{{url}}, {{statusCode}}, {{content}}", url, response.StatusCode, content);
            return default;
        }

        var jobList = JsonSerializer.Deserialize<T>(content);

        return jobList;
    }
}
