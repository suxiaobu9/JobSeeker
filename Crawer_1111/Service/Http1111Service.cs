using HtmlAgilityPack;
using Model.Dto;
using Model.Dto1111;
using Service.HtmlAnalyze;
using Service.Http;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Crawer_1111.Service;

public class Http1111Service : BaseHttpService, IHttpService
{
    private readonly HttpClient httpClient;
    private readonly IHtmlAnalyzeService htmlAnalyzeService;
    private readonly ILogger<BaseHttpService> logger;

    public Http1111Service(HttpClient httpClient,
        IHtmlAnalyzeService htmlAnalyzeService,
        ILogger<BaseHttpService> logger) : base(httpClient, logger)
    {
        this.httpClient = httpClient;
        this.htmlAnalyzeService = htmlAnalyzeService;
        this.logger = logger;
    }

    public Task<T?> GetCompanyInfo<T>(GetCompanyInfoDto dto) where T : CompanyDto
    {
        throw new NotImplementedException();
    }

    public Task<T?> GetJobInfo<T>(GetJobInfoDto dto) where T : JobDto
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// 取得職缺清單
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="url"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<T?> GetJobList<T>(string url) where T : JobListDto<SimpleJobInfoDto>
    {
        var content = "";
        try
        {
            content = await GetDataFromHttpRequest(url);

            if (string.IsNullOrWhiteSpace(content))
            {
                logger.LogWarning($"{nameof(Http1111Service)} Job list content get null.{{url}}", url);
                return null;
            }

            var data = JsonSerializer.Deserialize<JobList1111Model>(content);

            if (data == null || !data.HtmlTotal.Any())
            {
                logger.LogWarning($"{nameof(Http1111Service)} Job list data Deserialize get null.{{url}} {{content}}", url, content);
                return null;
            }

            if (data.Pi == null || data.Pc == null || data.Pi > data.Pc)
                return null;

            var jobList = new List<SimpleJobInfoDto>();

            foreach (var html in data.HtmlTotal)
            {
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(html);

                var jobListHtmlCollection = htmlAnalyzeService.GetJobListCardContentNode(htmlDoc);

                if (jobListHtmlCollection == null)
                    continue;

                foreach (var jobListHtml in jobListHtmlCollection)
                {
                    var jobNode = htmlAnalyzeService.GetJobListJobNode(jobListHtml);
                    var companyNode = htmlAnalyzeService.GetJobListCompanyNode(jobListHtml);

                    if (jobNode is null || companyNode is null)
                        continue;

                    var jobHrefSplit = jobNode.GetAttributeValue("href", "").Split("/").Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
                    var companyHrefSplit = companyNode.GetAttributeValue("href", "").Split("/").Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();

                    if (jobHrefSplit?.Length == null || companyHrefSplit?.Length == null || jobHrefSplit.Length < 2 || companyHrefSplit.Length < 2)
                        continue;

                    var jobId = jobHrefSplit.LastOrDefault();
                    var companyId = companyHrefSplit.LastOrDefault();

                    if (string.IsNullOrWhiteSpace(jobId) || string.IsNullOrWhiteSpace(companyId))
                        continue;

                    jobList.Add(new SimpleJobInfoDto
                    {
                        JobId = jobId,
                        CompanyId = companyId
                    });
                }
            }

            return new JobListDto<SimpleJobInfoDto>
            {
                JobList = jobList,
            } as T;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"{nameof(Http1111Service)} GetJobList fail.{{url}}", url);
            throw;
        }

    }
}
