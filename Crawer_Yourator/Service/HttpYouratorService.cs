using Model.Dto;
using Model.Dto104;
using Model.DtoYourator;
using Service.Http;
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
    private readonly ILogger<BaseHttpService> logger;

    public HttpYouratorService(HttpClient httpClient, ILogger<BaseHttpService> logger) : base(httpClient, logger)
    {
        this.httpClient = httpClient;
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
