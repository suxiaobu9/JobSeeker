using HtmlAgilityPack;
using Model.Dto;
using Model.DtoCakeResume;
using Service.Http;

namespace Crawer_CakeResume.Service;

public class HttpCakeResumeService : BaseHttpService, IHttpService
{
    private readonly HttpClient httpClient;
    private readonly ILogger<BaseHttpService> logger;

    public HttpCakeResumeService(HttpClient httpClient, ILogger<BaseHttpService> logger) : base(httpClient, logger)
    {
        this.httpClient = httpClient;
        this.logger = logger;
    }

    /// <summary>
    /// 取得公司資訊
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="companyId"></param>
    /// <param name="url"></param>
    /// <returns></returns>
    public async Task<T?> GetCompanyInfo<T>(string companyId, string url) where T : CompanyDto
    {

        Task? delayTask = null;

        try
        {
            var content = await GetDataFromHttpRequest(url);

            delayTask = Task.Delay(TimeSpan.FromSeconds(2));

            if (string.IsNullOrWhiteSpace(content))
            {
                logger.LogWarning($"{nameof(HttpCakeResumeService)} Company info content get null.{{url}}", url);
                return null;
            }

            HtmlDocument htmlDoc = new();
            htmlDoc.LoadHtml(content);

            // 公司名稱
            var compTitle = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='CompanyHeader_companyName__Glj9i']").InnerText.Trim();
            if (string.IsNullOrWhiteSpace(compTitle))
            {
                logger.LogWarning($"{nameof(HttpCakeResumeService)} Get Company title fail.{{url}}", url);
                return null;
            }

            var result = new CompanyDto
            {
                Id = companyId,
                SourceFrom = ParametersCakeResume.SourceFrom,
                Name = compTitle,
                Product = "N/A",
                Profile = "N/A",
                Welfare = "N/A"
            };

            // 公司介紹內容
            var cardContentNodes = htmlDoc.DocumentNode.SelectNodes("//div[contains(@class, 'Card_container__PERSv')]");

            if (cardContentNodes == null)
                return result as T;

            var contentFilter = new Dictionary<string, string[]>
            {
                { nameof(CompanyDto.Product), new string[]{ "產品或服務", "Products or services" }},
                { nameof(CompanyDto.Profile), new string[]{ "公司介紹", "Company summary" }},
                { nameof(CompanyDto.Welfare), new string[]{ "員工福利", "Employee benefits" }},
            };

            foreach (var cardContentNode in cardContentNodes)
            {
                // 內文的標題
                var cardTitle = cardContentNode.SelectSingleNode(".//h2[contains(@class, 'Card_title__4iRRv')]").InnerText.Trim();

                if (string.IsNullOrWhiteSpace(cardTitle))
                    continue;

                var filterKey = contentFilter.FirstOrDefault(x => x.Value.Any(y => cardTitle.Contains(y))).Key;

                if (string.IsNullOrWhiteSpace(filterKey))
                    continue;
                // 內文的內容
                var cardContent = cardContentNode.SelectSingleNode(".//div[contains(@class, 'RailsHtml_container__VVQ7u')]").InnerText;

                if (string.IsNullOrWhiteSpace(cardContent))
                    continue;

                switch (filterKey)
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
            logger.LogError(ex, $"{nameof(HttpCakeResumeService)} get exception.");
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
    public async Task<T?> GetJobInfo<T>(string jobId, string companyId, string url) where T : JobDto
    {
        Task? delayTask = null;
        try
        {
            var content = await GetDataFromHttpRequest(url);

            delayTask = Task.Delay(TimeSpan.FromSeconds(2));

            if (string.IsNullOrWhiteSpace(content))
            {
                logger.LogWarning($"{nameof(HttpCakeResumeService)} Job info content get null.{{url}}", url);
                return null;
            }

            HtmlDocument htmlDoc = new();
            htmlDoc.LoadHtml(content);

            // 抓取職缺名稱
            var jobTitle = htmlDoc.DocumentNode.SelectSingleNode("//h2[@class='JobDescriptionLeftColumn_title__heKvX']")?.InnerText.Trim();

            if (string.IsNullOrWhiteSpace(jobTitle))
            {
                logger.LogWarning($"{nameof(HttpCakeResumeService)} Job info title get null.{{url}}", url);
                return null;
            }

            var result = new JobDto
            {
                CompanyId = companyId,
                Id = jobId,
                WorkContent = "N/A",
                JobPlace = htmlDoc.DocumentNode.SelectSingleNode("//a[@class='CompanyInfoItem_link__E841d']")?.InnerText ?? "N/A",
                Name = jobTitle,
                OtherRequirement = "N/A",
                Salary = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='JobDescriptionRightColumn_salaryWrapper__mYzNx']")?.InnerText ?? "N/A",
            };

            // 職缺內容
            var cardContentNodes = htmlDoc.DocumentNode.SelectNodes("//div[contains(@class, 'ContentSection_contentSection__k5CRR')]");

            if (cardContentNodes == null)
                return result as T;

            var contentFilter = new Dictionary<string, string[]>
            {
                { nameof(JobDto.WorkContent), new string[]{ "職缺描述", "Job Description" }},
                { nameof(JobDto.OtherRequirement), new string[]{ "職務需求", "Requirements" }},
            };

            foreach (var cardContentNode in cardContentNodes)
            {
                // 內文標題
                var cardTitle = cardContentNode.SelectSingleNode(".//h3[contains(@class, 'ContentSection_title__Ox8_s')]")?.InnerText.Trim();
                if (string.IsNullOrWhiteSpace(cardTitle))
                    continue;

                var filterKey = contentFilter.FirstOrDefault(x => x.Value.Any(y => cardTitle.Contains(y))).Key;

                if (string.IsNullOrWhiteSpace(filterKey))
                    continue;

                // 內文內容
                var cardContent = cardContentNode.SelectSingleNode(".//div[contains(@class, 'RailsHtml_container__VVQ7u')]")?.InnerText;

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
            logger.LogError(ex, $"{nameof(HttpCakeResumeService)} get exception.");
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

        try
        {
            var content = await GetDataFromHttpRequest(url);

            delayTask = Task.Delay(TimeSpan.FromSeconds(2));

            if (string.IsNullOrWhiteSpace(content))
            {
                logger.LogWarning($"{nameof(HttpCakeResumeService)} Job list content get null.{{url}}", url);
                return null;
            }

            HtmlDocument doc = new();
            doc.LoadHtml(content);

            // 職缺內容
            var cardContentNodes = doc.DocumentNode.SelectNodes("//div[contains(@class, 'JobSearchItem_headerContent__Ka56W')]");

            if (cardContentNodes == null)
                return null;

            var jobList = new List<SimpleJobInfoDto>();

            foreach (var cardContentNode in cardContentNodes)
            {
                HtmlNode jobNode = cardContentNode.SelectSingleNode(".//a[contains(@class, 'JobSearchItem_jobTitle__Fjzv2')]");
                HtmlNode companyNode = cardContentNode.SelectSingleNode(".//a[contains(@class, 'JobSearchItem_companyName__QKkj5')]");

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
            logger.LogError(ex, $"{nameof(HttpCakeResumeService)} get exception.");
            return null;
        }
        finally
        {
            if (delayTask != null)
                await delayTask;
        }

    }
}
