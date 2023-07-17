using Grpc.Core.Logging;
using Microsoft.Extensions.Logging;
using System.Net.Http;

namespace Service.Http;

public class BaseHttpService
{
    private readonly HttpClient httpClient;
    private readonly ILogger<BaseHttpService> logger;

    public BaseHttpService(HttpClient httpClient,
        ILogger<BaseHttpService> logger)
    {
        this.httpClient = httpClient;
        this.logger = logger;
    }

    public BaseHttpService(IHttpClientFactory httpClientFactory,
        string httpClientName,
        ILogger<BaseHttpService> logger)
    {
        this.httpClient = httpClientFactory.CreateClient(httpClientName);
        this.logger = logger;
    }

    /// <summary>
    /// 從 url get data
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    protected async Task<string?> GetDataFromHttpRequest(string url)
    {
        var currentMethod = "BaseHttpService.GetDataFromHttpRequest";
        var response = await httpClient.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            logger.LogWarning($"{{currentMethod}} get response fail.{{url}}, {{statusCode}}, {{content}}", currentMethod, url, response.StatusCode, content);
            return default;
        }

        return content;
    }
}
