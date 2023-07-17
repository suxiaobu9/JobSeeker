using Model.Dto;
using Model.Dto104;
using Service.Http;
using System.Text;
using System.Text.Json;

namespace Crawer_104.Service;

internal class Http104Service : BaseHttpService, IHttpService
{
    private readonly HttpClient httpClient;
    private readonly ILogger<BaseHttpService> logger;

    public Http104Service(ILogger<BaseHttpService> logger, IHttpClientFactory httpClientFactory) : base(httpClientFactory, Parameters104.Referer, logger)
    {
        this.httpClient = httpClientFactory.CreateClient(Parameters104.Referer);

        this.logger = logger;
    }

    public Task<T?> GetCompanyInfo<T>(string url) where T : CompanyDto
    {
        throw new NotImplementedException();
    }

    public Task<T?> GetJobInfo<T>(string url) where T : JobDto
    {
        throw new NotImplementedException();
    }

    public async Task<T?> GetJobList<T>(string url) where T : JobListDto<SimpleJobInfoDto>
    {
        var content = await GetDataFromHttpRequest(url);

        if (string.IsNullOrWhiteSpace(content))
        {
            logger.LogWarning($"{nameof(Http104Service)} Job list content get null.{{url}}", url);
            return null;
        }

        var data = JsonSerializer.Deserialize<JobList104Model>(content);

        if (data == null)
        {
            logger.LogWarning($"{nameof(Http104Service)} Job list data Deserialize get null.{{url}} {{content}}", url, content);
            return null;
        }

        var result = new JobListWithPageDto
        {
            TotalPage = data.Data.TotalPage,
            JobList = data.Data.List.Select(x =>
            {
                string jobId = new Uri("https:" + x.Link.Job).Segments.LastOrDefault() ?? "";
                string companyId = new Uri("https:" + x.Link.Cust).Segments.LastOrDefault() ?? "";

                return new SimpleJobInfoDto
                {
                    JobId = jobId,
                    CompanyId = companyId
                };
            }).Where(x => !string.IsNullOrWhiteSpace(x.CompanyId) && !string.IsNullOrWhiteSpace(x.JobId))
        };
        return result as T;
    }
}
