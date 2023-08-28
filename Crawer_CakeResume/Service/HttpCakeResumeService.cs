using HtmlAgilityPack;
using Model.Dto;
using Model.DtoCakeResume;
using Service.Delay;
using Service.HtmlAnalyze;
using Service.Http;
using Service.Parameter;

namespace Crawer_CakeResume.Service;

public class HttpCakeResumeService : BaseHttpService, IHttpService
{
    private readonly HttpClient httpClient;
    private readonly IParameterService parameterService;
    private readonly ITaskDelayService taskDelayService;
    private readonly IHtmlAnalyzeService htmlAnalyzeService;
    private readonly ILogger<BaseHttpService> logger;

    public HttpCakeResumeService(HttpClient httpClient,
        IParameterService parameterService,
        ITaskDelayService taskDelayService,
        IHtmlAnalyzeService htmlAnalyzeService,
        ILogger<BaseHttpService> logger) : base(httpClient, logger)
    {
        this.httpClient = httpClient;
        this.parameterService = parameterService;
        this.taskDelayService = taskDelayService;
        this.htmlAnalyzeService = htmlAnalyzeService;
        this.logger = logger;
    }

    /// <summary>
    /// 取得公司資訊
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="companyId"></param>
    /// <param name="url"></param>
    /// <returns></returns>
    public async Task<T?> GetCompanyInfo<T>(GetCompanyInfoDto dto) where T : CompanyDto
    {

        Task? delayTask = null;
        var content = "";
        try
        {
            var url = parameterService.CompanyInfoUrl(dto);
            content = await GetDataFromHttpRequest(url);

            delayTask = taskDelayService.Delay(TimeSpan.FromSeconds(2));

            if (string.IsNullOrWhiteSpace(content))
            {
                logger.LogWarning($"{nameof(HttpCakeResumeService)} Company info content get null.{{url}}", url);
                return null;
            }

            HtmlDocument htmlDoc = new();
            htmlDoc.LoadHtml(content);

            // 公司名稱
            var compTitle = htmlAnalyzeService.GetCompanyName(htmlDoc);
            if (string.IsNullOrWhiteSpace(compTitle))
            {
                logger.LogWarning($"{nameof(HttpCakeResumeService)} Get Company title fail.{{url}}", url);
                return null;
            }

            var result = new CompanyDto
            {
                Id = dto.CompanyId,
                SourceFrom = ParametersCakeResume.SourceFrom,
                Name = compTitle,
                Product = "N/A",
                Profile = "N/A",
                Welfare = "N/A"
            };

            // 公司介紹內容
            var cardContentNodes = htmlAnalyzeService.GetCompanyCardContentNodes(htmlDoc);

            if (cardContentNodes == null)
                return result as T;

            foreach (var cardContentNode in cardContentNodes)
            {
                var companyCardContent = htmlAnalyzeService.GetCompanyCardContent(cardContentNode);

                if (companyCardContent == null)
                    continue;

                var cardKey = companyCardContent.Value.Key;

                if (!ParametersCakeResume.CompanyContentFilter.ContainsKey(cardKey))
                    continue;

                var cardContent = companyCardContent?.Value;

                switch (cardKey)
                {
                    case nameof(CompanyDto.Product):
                        result.Product = cardContent;
                        break;
                    case nameof(CompanyDto.Profile):
                        result.Profile = cardContent;
                        break;
                    case nameof(CompanyDto.Welfare):
                        result.Welfare = cardContent;
                        break;
                    default:
                        break;
                }
            }

            return result as T;

        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"{nameof(HttpCakeResumeService)} GetCompanyInfo get exception.{{content}}", content);
            return null;
        }
        finally
        {
            if (delayTask != null)
                await delayTask;
        }
    }

    /// <summary>
    /// 取得職缺資訊
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="jobId"></param>
    /// <param name="companyId"></param>
    /// <param name="url"></param>
    /// <returns></returns>
    public async Task<T?> GetJobInfo<T>(GetJobInfoDto dto) where T : JobDto
    {
        Task? delayTask = null;
        var content = "";
        try
        {
            var url = parameterService.JobInfoUrl(dto);
            content = await GetDataFromHttpRequest(url);

            delayTask = taskDelayService.Delay(TimeSpan.FromSeconds(2));

            if (string.IsNullOrWhiteSpace(content))
            {
                logger.LogWarning($"{nameof(HttpCakeResumeService)} Job info content get null.{{url}}", url);
                return null;
            }

            HtmlDocument htmlDoc = new();
            htmlDoc.LoadHtml(content);

            // 抓取職缺名稱
            var jobTitle = htmlAnalyzeService.GetJobName(htmlDoc);

            if (string.IsNullOrWhiteSpace(jobTitle))
            {
                logger.LogWarning($"{nameof(HttpCakeResumeService)} Job info title get null.{{url}}", url);
                return null;
            }

            var result = new JobDto
            {
                CompanyId = dto.CompanyId,
                Id = dto.JobId,
                WorkContent = "N/A",
                JobPlace = htmlAnalyzeService.GetJobPlace(htmlDoc) ?? "N/A",
                Name = jobTitle,
                OtherRequirement = "N/A",
                Salary = htmlAnalyzeService.GetSalary(htmlDoc) ?? "N/A",
                LatestUpdateDate = htmlAnalyzeService.GetJobLastUpdateTime(htmlDoc) ?? "N/A",
                CompanySourceFrom = ParametersCakeResume.SourceFrom,
            };

            // 職缺內容
            var cardContentNodes = htmlAnalyzeService.GetJobCardContentNodes(htmlDoc);

            if (cardContentNodes == null)
                return result as T;

            foreach (var cardContentNode in cardContentNodes)
            {
                // 內文標題
                var cardTitle = htmlAnalyzeService.GetJobCardTitle(cardContentNode);
                if (string.IsNullOrWhiteSpace(cardTitle))
                    continue;

                var filterKey = ParametersCakeResume.JobContentFilter.FirstOrDefault(x => x.Value.Any(y => cardTitle.Contains(y))).Key;

                if (string.IsNullOrWhiteSpace(filterKey))
                    continue;

                // 內文內容
                var cardContent = htmlAnalyzeService.GetJobCardContent(cardContentNode);

                if (string.IsNullOrWhiteSpace(cardContent))
                    continue;

                switch (filterKey)
                {
                    case nameof(JobDto.WorkContent):
                        result.WorkContent = cardContent;
                        break;
                    case nameof(JobDto.OtherRequirement):
                        result.OtherRequirement = cardContent;
                        break;
                    default:
                        break;
                }
            }

            return result as T;

        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"{nameof(HttpCakeResumeService)} GetJobInfo get exception.{{content}}", content);
            return null;
        }
        finally
        {
            if (delayTask != null)
                await delayTask;
        }
    }

    /// <summary>
    /// 取得職缺清單
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="url"></param>
    /// <returns></returns>
    public async Task<T?> GetJobList<T>(string url) where T : JobListDto<SimpleJobInfoDto>
    {
        Task? delayTask = null;

        var content = "";
        try
        {
            content = await GetDataFromHttpRequest(url);

            delayTask = taskDelayService.Delay(TimeSpan.FromSeconds(2));

            if (string.IsNullOrWhiteSpace(content))
            {
                logger.LogWarning($"{nameof(HttpCakeResumeService)} Job list content get null.{{url}}", url);
                return null;
            }

            HtmlDocument doc = new();
            doc.LoadHtml(content);

            // 職缺內容
            var cardContentNodes = htmlAnalyzeService.GetJobListCardContentNode(doc);

            if (cardContentNodes == null || cardContentNodes.Count == 0)
                return null;

            var jobList = new List<SimpleJobInfoDto>();

            foreach (var cardContentNode in cardContentNodes)
            {
                HtmlNode? jobNode = htmlAnalyzeService.GetJobListJobNode(cardContentNode);
                HtmlNode? companyNode = htmlAnalyzeService.GetJobListCompanyNode(cardContentNode);

                if (jobNode == null || companyNode == null)
                    continue;

                var jobId = jobNode.GetAttributeValue("href", "").Split("/").LastOrDefault();
                var companyId = companyNode.GetAttributeValue("href", "").Split("/").LastOrDefault();

                if (string.IsNullOrWhiteSpace(jobId) || string.IsNullOrWhiteSpace(companyId))
                    continue;

                jobList.Add(new SimpleJobInfoDto
                {
                    JobId = jobId,
                    CompanyId = companyId
                });
            }

            return new JobListDto<SimpleJobInfoDto> { JobList = jobList } as T;

        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"{nameof(HttpCakeResumeService)} GetJobList get exception.{{content}}", content);
            return null;
        }
        finally
        {
            if (delayTask != null)
                await delayTask;
        }

    }
}
