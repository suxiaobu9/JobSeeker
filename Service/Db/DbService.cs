using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Model.Dto;
using Model.JobSeekerDb;
using Service.Parameter;
using StackExchange.Redis;
using System.Text.Json;

namespace Service.Db;

public class DbService : IDbService
{
    private readonly ILogger<DbService> logger;
    private readonly IParameterService parameterService;
    private readonly IServiceScope serviceScope;
    private readonly IDatabase redisDb;

    public DbService(ILogger<DbService> logger,
        IServiceScopeFactory serviceScopeFactory,
        IParameterService parameterService,
        IDatabase redisDb)
    {
        this.logger = logger;
        this.parameterService = parameterService;
        this.serviceScope = serviceScopeFactory.CreateScope();
        this.redisDb = redisDb;
    }

    /// <summary>
    /// 將所有職缺資訊標示為刪除
    /// </summary>
    /// <param name="sourceFrom"></param>
    /// <returns></returns>
    public async Task MakeAllJobAsDelete(string sourceFrom)
    {
        var rawSql = @"
UPDATE job
SET is_deleted  = TRUE
FROM company
WHERE job.company_id  = company.id
AND company.source_from  = {0}
";
        var postgresContext = GetPostgresContext();

        await postgresContext.Database.ExecuteSqlRawAsync(rawSql, sourceFrom);
    }

    /// <summary>
    /// 更新或是新增公司資訊
    /// </summary>
    /// <param name="companyDto"></param>
    /// <returns></returns>
    public async Task UpsertCompany(CompanyDto companyDto)
    {
        var now = DateTimeOffset.Now;

        try
        {
            var postgresContext = GetPostgresContext();

            var company = await postgresContext.Companies.FirstOrDefaultAsync(x => x.Id == companyDto.Id);

            var getCompanyInfoDto = new GetCompanyInfoDto
            {
                CompanyId = companyDto.Id,
            };

            var infoUrl = parameterService.CompanyInfoUrl(getCompanyInfoDto);
            var pageUrl = parameterService.CompanyPageUrl(getCompanyInfoDto);

            if (company == null)
            {
                await postgresContext.Companies.AddAsync(new Company
                {
                    Id = companyDto.Id,
                    CreateUtcAt = now.UtcDateTime,
                    GetInfoUrl = infoUrl,
                    Ignore = false,
                    Name = companyDto.Name,
                    Product = companyDto.Product,
                    Profile = companyDto.Profile,
                    SourceFrom = companyDto.SourceFrom,
                    UpdateUtcAt = now.UtcDateTime,
                    Url = pageUrl,
                    Welfare = companyDto.Welfare,
                });
            }
            else
            {
                company.GetInfoUrl = infoUrl;
                company.Name = companyDto.Name;
                company.Product = companyDto.Product;
                company.Profile = companyDto.Profile;
                company.SourceFrom = companyDto.SourceFrom;
                company.UpdateUtcAt = now.UtcDateTime;
                company.Url = pageUrl;
                company.Welfare = companyDto.Welfare;
            }

            await postgresContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"{nameof(DbService)} UpsertCompany get exception.{{json}}", JsonSerializer.Serialize(companyDto));
        }

    }

    /// <summary>
    /// 更新或是新增職缺資訊
    /// </summary>
    /// <param name="jobDto"></param>
    /// <returns></returns>
    public async Task UpsertJob(JobDto jobDto)
    {
        var now = DateTimeOffset.Now;
        try
        {
            var postgresContext = GetPostgresContext();

            var job = await postgresContext.Jobs
                        .FirstOrDefaultAsync(x => x.Id == jobDto.Id && x.CompanyId == jobDto.CompanyId);

            var getJobInfoDto = new GetJobInfoDto
            {
                CompanyId = jobDto.CompanyId,
                JobId = jobDto.Id
            };

            var infoUrl = parameterService.JobInfoUrl(getJobInfoDto);
            var pageUrl = parameterService.JobPageUrl(getJobInfoDto);

            if (job == null)
            {
                await postgresContext.Jobs.AddAsync(new Job
                {
                    Id = jobDto.Id,
                    CompanyId = jobDto.CompanyId,
                    CreateUtcAt = now.UtcDateTime,
                    GetInfoUrl = infoUrl,
                    HaveRead = false,
                    Ignore = false,
                    IsDeleted = false,
                    JobPlace = jobDto.JobPlace,
                    Name = jobDto.Name,
                    OtherRequirement = jobDto.OtherRequirement,
                    Salary = jobDto.Salary,
                    UpdateUtcAt = now.UtcDateTime,
                    Url = pageUrl,
                    WorkContent = jobDto.WorkContent,
                    LatestUpdateDate = jobDto.LatestUpdateDate,
                    CompanySourceFrom = jobDto.CompanySourceFrom,
                });
            }
            else
            {
                if (job.JobPlace != jobDto.JobPlace || job.Name != jobDto.Name ||
                    job.OtherRequirement != jobDto.OtherRequirement || job.Salary != jobDto.Salary ||
                    job.WorkContent != jobDto.WorkContent)
                    job.HaveRead = false;

                job.IsDeleted = false;
                job.GetInfoUrl = infoUrl;
                job.JobPlace = jobDto.JobPlace;
                job.Name = jobDto.Name;
                job.OtherRequirement = jobDto.OtherRequirement;
                job.Salary = jobDto.Salary;
                job.UpdateUtcAt = now.UtcDateTime;
                job.Url = pageUrl;
                job.WorkContent = jobDto.WorkContent;
                job.LatestUpdateDate = jobDto.LatestUpdateDate;
                job.CompanySourceFrom = jobDto.CompanySourceFrom;
            }

            await postgresContext.SaveChangesAsync();

        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"{nameof(DbService)} UpsertJob get exception.{{json}}", JsonSerializer.Serialize(jobDto));
        }
    }

    /// <summary>
    /// 公司資訊存在
    /// </summary>
    /// <param name="companyId"></param>
    /// <returns></returns>
    public Task<bool> CompanyExist(string companyId)
    {
        var postgresContext = GetPostgresContext();

        return postgresContext.Companies.AnyAsync(x => x.Id == companyId);
    }

    /// <summary>
    /// 職缺存在
    /// </summary>
    /// <param name="companyId"></param>
    /// <param name="jobId"></param>
    /// <returns></returns>
    public Task<bool> JobExist(string companyId, string jobId)
    {
        var postgresContext = GetPostgresContext();

        return postgresContext.Jobs.AnyAsync(x => x.Id == jobId && x.CompanyId == companyId);
    }

    public postgresContext GetPostgresContext()
    {
        return serviceScope.ServiceProvider.GetRequiredService<postgresContext>();
    }
}
