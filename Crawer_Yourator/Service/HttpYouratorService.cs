using HtmlAgilityPack;
using Model.Dto;
using Model.Dto104;
using Model.DtoCakeResume;
using Model.DtoYourator;
using Service.HtmlAnalyze;
using Service.Http;
using Service.Parameter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Crawer_Yourator.Service;

public class HttpYouratorService : BaseHttpService, IHttpService
{
    private readonly HttpClient httpClient;
    private readonly IParameterService parameterService;
    private readonly IHtmlAnalyzeService htmlAnalyzeService;
    private readonly ILogger<BaseHttpService> logger;

    public HttpYouratorService(HttpClient httpClient,
        IParameterService parameterService,
        IHtmlAnalyzeService htmlAnalyzeService,
        ILogger<BaseHttpService> logger) : base(httpClient, logger)
    {
        this.httpClient = httpClient;
        this.parameterService = parameterService;
        this.htmlAnalyzeService = htmlAnalyzeService;
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
                logger.LogWarning($"{nameof(HttpYouratorService)} Company info content get null.{{url}}", url);
                return null;
            }

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(content);

            // 公司名稱
            var compTitle = htmlAnalyzeService.GetCompanyName(htmlDoc);
            if (string.IsNullOrWhiteSpace(compTitle))
            {
                logger.LogWarning($"{nameof(HtmlAnalyzeYouratorService)} Get Company title fail.{{url}}", url);
                return null;
            }

            var result = new CompanyDto
            {
                Id = dto.CompanyId,
                SourceFrom = ParametersYourator.SourceFrom,
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

                if (!ParametersCakeResume.CompanyContentFilter.ContainsKey(cardKey))
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
                    default:
                        break;
                }
            }

            return result as T;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"{nameof(HttpYouratorService)} GetCompanyInfo error.");
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
                logger.LogWarning($"{nameof(HttpYouratorService)} Job info content get null.{{url}}", url);
                return null;
            }

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(content);

            // 公司名稱
            var jobTitle = htmlAnalyzeService.GetJobName(htmlDoc);

            if (string.IsNullOrWhiteSpace(jobTitle))
            {
                logger.LogWarning($"{nameof(HttpYouratorService)} Job info title get null.{{url}}", url);
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
                Salary =  "N/A",
                LatestUpdateDate = htmlAnalyzeService.GetJobLastUpdateTime(htmlDoc) ?? "N/A",
            };

            var cardContentNodes = htmlAnalyzeService.GetJobCardContentNodes(htmlDoc);

            if (cardContentNodes == null)
                return result as T;

            foreach (var cardContentNode in cardContentNodes)
            {
                // 內文標題
                var cardTitle = htmlAnalyzeService.GetJobCardTitle(cardContentNode);
                if (string.IsNullOrWhiteSpace(cardTitle))
                    continue;

                var filterKey = ParametersYourator.JobContentFilter.FirstOrDefault(x => x.Value.Any(y => cardTitle.Contains(y))).Key;

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
                    case nameof(JobDto.Salary):
                        result.Salary = cardContent;
                        break;
                    default:
                        break;
                }
            }

            return result as T;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"{nameof(HttpYouratorService)} GetJobInfo error.");
            throw;
        }
    }

    public async Task<T?> GetJobList<T>(string url) where T : JobListDto<SimpleJobInfoDto>
    {
        var content = "";
        try
        {
            content = await GetDataFromHttpRequest(url);

            if (string.IsNullOrWhiteSpace(content))
            {
                logger.LogWarning($"{nameof(HttpYouratorService)} Job list content get null.{{url}}", url);
                return null;
            }

            var data = JsonSerializer.Deserialize<JobListYouratorDto>(content);

            if (data == null || data.Payload == null)
            {
                logger.LogWarning($"{nameof(HttpYouratorService)} Job list data Deserialize get null.{{url}} {{content}}", url, content);
                return null;
            }

            if (data.Payload.Jobs == null || data.Payload.Jobs.Length == 0)
                return null;

            var jobList = new List<SimpleJobInfoDto>();

            foreach (var item in data.Payload.Jobs)
            {
                if (item?.Id == null)
                {
                    logger.LogWarning($"{nameof(HttpYouratorService)} GetJobList job id null.{{url}} {{content}}", url, content);
                    continue;
                }

                if (string.IsNullOrWhiteSpace(item.Company?.Path))
                {
                    logger.LogWarning($"{nameof(HttpYouratorService)} GetJobList company data null.{{url}} {{content}}", url, content);
                    continue;
                }

                var companyId = item.Company.Path.Split('/').LastOrDefault();

                if (string.IsNullOrWhiteSpace(companyId))
                {
                    logger.LogWarning($"{nameof(HttpYouratorService)} GetJobList company id null.{{url}} {{content}}", url, content);
                    continue;
                }

                jobList.Add(new SimpleJobInfoDto { CompanyId = companyId, JobId = item.Id.Value.ToString() });
            }

            return new JobListDto<SimpleJobInfoDto>
            {
                JobList = jobList,
            } as T;

        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"{nameof(HttpYouratorService)} GetJobList error.{{url}} {{content}}", url, content);
            throw;
        }
    }
}
