using Model.Dto;
using Model.Dto104;
using Service.Http;
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
    /// <summary>
    /// 取得公司資訊
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="companyId"></param>
    /// <param name="url"></param>
    /// <returns></returns>
    public async Task<T?> GetCompanyInfo<T>(string companyId, string url) where T : CompanyDto
    {
        string? content = null;
        try
        {

            content = await GetDataFromHttpRequest(url);

            if (string.IsNullOrWhiteSpace(content))
            {
                logger.LogWarning($"{nameof(Http104Service)} Company info content get null.{{url}}", url);
                return null;
            }

            var companyInfo = JsonSerializer.Deserialize<CompanyInfo104Model>(content);

            if (companyInfo == null)
            {
                logger.LogWarning($"{nameof(Http104Service)} Company info data Deserialize get null.{{url}} {{content}}", url, content);
                return null;
            }

            var result = new CompanyDto
            {
                Id = companyId,
                Name = companyInfo.Data.CustName,
                Product = companyInfo.Data.Product,
                Profile = companyInfo.Data.Profile,
                Welfare = companyInfo.Data.Welfare,
                SourceFrom = Parameters104.SourceFrom
            };

            return result as T;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"{nameof(Http104Service)} GetCompanyInfo get exception.{{content}}", content);
            throw;
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
        var content = "";
        try
        {
            content = await GetDataFromHttpRequest(url);

            if (string.IsNullOrWhiteSpace(content))
            {
                logger.LogWarning($"{nameof(Http104Service)} Job info content get null.{{url}}", url);
                return null;
            }

            var jobInfo = JsonSerializer.Deserialize<JobInfo104Model>(content);

            if (jobInfo == null)
            {
                logger.LogWarning($"{nameof(Http104Service)} Job info data Deserialize get null.{{url}} {{content}}", url, content);
                return null;
            }

            var result = new JobDto
            {
                Id = jobId,
                Name = jobInfo.Data.Header.JobName,
                CompanyId = companyId,
                JobPlace = jobInfo.Data.JobDetail.AddressRegion,
                OtherRequirement = jobInfo.Data.Condition.Other,
                Salary = jobInfo.Data.JobDetail.Salary,
                WorkContent = jobInfo.Data.JobDetail.JobDescription,
                LatestUpdateDate = jobInfo.Data.Header.AppearDate
            };

            return result as T;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"{nameof(Http104Service)} GetJobInfo get exception.{{content}}", content);
            throw;
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
        var content = "";
        try
        {
            content = await GetDataFromHttpRequest(url);

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
        catch (Exception ex)
        {
            logger.LogError(ex, $"{nameof(Http104Service)} GetJobList get exception.{{content}}", content);
            throw;
        }
    }
}
