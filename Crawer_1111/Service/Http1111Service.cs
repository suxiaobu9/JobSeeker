using HtmlAgilityPack;
using Model.Dto;
using Model.Dto1111;
using Service.HtmlAnalyze;
using Service.Http;
using Service.Parameter;
using System.Net.Mime;
using System.Text.Json;

namespace Crawer_1111.Service;

public class Http1111Service : BaseHttpService, IHttpService
{
    private readonly HttpClient httpClient;
    private readonly IHtmlAnalyzeService htmlAnalyzeService;
    private readonly IParameterService parameterService;
    private readonly ILogger<BaseHttpService> logger;

    public Http1111Service(HttpClient httpClient,
        IHtmlAnalyzeService htmlAnalyzeService,
        IParameterService parameterService,
        ILogger<BaseHttpService> logger) : base(httpClient, logger)
    {
        this.httpClient = httpClient;
        this.htmlAnalyzeService = htmlAnalyzeService;
        this.parameterService = parameterService;
        this.logger = logger;
    }

    public async Task<T?> GetCompanyInfo<T>(GetCompanyInfoDto dto) where T : CompanyDto
    {
        var content = "";
        try
        {
            var url = parameterService.CompanyInfoUrl(dto);

            content = await GetDataFromHttpRequest(url);

            if (string.IsNullOrWhiteSpace(content))
            {
                logger.LogWarning($"{nameof(Http1111Service)} Company info content get null.{{url}}", url);
                return null;
            }

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(content);

            // 公司名稱
            var compTitle = htmlAnalyzeService.GetCompanyName(htmlDoc);
            if (string.IsNullOrWhiteSpace(compTitle))
            {
                logger.LogWarning($"{nameof(Http1111Service)} Get Company title fail.{{url}}", url);
                return null;
            }

            var result = new CompanyDto
            {
                Id = dto.CompanyId,
                SourceFrom = Parameters1111.SourceFrom,
                Name = compTitle,
                Product = "N/A",
                Profile = "N/A",
                Welfare = "N/A"
            };

            var cardContentNodes = htmlAnalyzeService.GetCompanyCardContentNodes(htmlDoc);

            if (cardContentNodes == null)
                return result as T;

            foreach (var cardContentNode in cardContentNodes)
            {
                var companyCardContent = htmlAnalyzeService.GetCompanyCardContent(cardContentNode);

                if (companyCardContent == null)
                    continue;

                var cardKey = companyCardContent.Value.Key;

                if (!Parameters1111.CompanyContentFilter.ContainsKey(cardKey))
                    continue;

                var cardContent = companyCardContent?.Value;

                switch (cardKey)
                {
                    case nameof(CompanyDto.Profile):
                        result.Profile = cardContent;
                        break;
                    case nameof(CompanyDto.Welfare):
                        result.Welfare = cardContent;
                        break;
                    case nameof(CompanyDto.Product):
                        result.Product = cardContent;
                        break;
                    default:
                        break;
                }
            }

            return result as T;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"{nameof(Http1111Service)} GetCompanyInfo error.");
            throw;
        }
    }

    public async Task<T?> GetJobInfo<T>(GetJobInfoDto dto) where T : JobDto
    {
        var content = "";
        try
        {
            var url = parameterService.JobInfoUrl(dto);

            content = await GetDataFromHttpRequest(url);

            if (string.IsNullOrWhiteSpace(content))
            {
                logger.LogWarning($"{nameof(Http1111Service)} Job info content get null.{{url}}", url);
                return null;
            }

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(content);

            // 公司名稱
            var jobTitle = htmlAnalyzeService.GetJobName(htmlDoc);

            if (string.IsNullOrWhiteSpace(jobTitle))
            {
                logger.LogWarning($"{nameof(Http1111Service)} Job info title get null.{{url}}", url);
                return null;
            }

            var result = new JobDto
            {
                CompanyId = dto.CompanyId,
                Id = dto.JobId,
                WorkContent = htmlAnalyzeService.GetWorkContent(htmlDoc) ?? "N/A",
                JobPlace = htmlAnalyzeService.GetJobPlace(htmlDoc) ?? "N/A",
                Name = jobTitle,
                OtherRequirement = htmlAnalyzeService.GetOtherRequirement(htmlDoc) ?? "N/A",
                Salary = htmlAnalyzeService.GetSalary(htmlDoc) ?? "N/A",
                LatestUpdateDate = htmlAnalyzeService.GetJobLastUpdateTime(htmlDoc) ?? "N/A",
                CompanySourceFrom = Parameters1111.SourceFrom,
            };

            return result as T;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"{nameof(Http1111Service)} GetJobInfo error.");
            throw;
        }
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

            return new JobListWithPageDto
            {
                TotalPage = data.Pc.Value,
                JobList = jobList,
            } as T;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"{nameof(Http1111Service)} GetJobList fail.{{url}} {{content}}", url, content);
            throw;
        }

    }
}
