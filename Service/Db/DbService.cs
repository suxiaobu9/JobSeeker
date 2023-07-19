using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Model.Dto;
using Model.Dto104;
using Model.JobSeekerDb;
using StackExchange.Redis;
using System.Text.Json;

namespace Service.Db;

public abstract class DbService : IDbService
{
    private readonly ILogger<DbService> logger;
    private readonly postgresContext postgresContext;
    private readonly IDatabase redisDb;

    public DbService(ILogger<DbService> logger,
        postgresContext postgresContext,
        IDatabase redisDb)
    {
        this.logger = logger;
        this.postgresContext = postgresContext;
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
            var company = await postgresContext.Companies.FirstOrDefaultAsync(x => x.Id == companyDto.Id);

            var infoUrl = CompanyInfoUrl(companyDto);
            var pageUrl = CompanyPageUrl(companyDto);

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
            var job = await postgresContext.Jobs.FirstOrDefaultAsync(x => x.Id == jobDto.Id);

            var infoUrl = JobInfoUrl(jobDto);
            var pageUrl = JobPageUrl(jobDto);

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
                });
            }
            else
            {
                if (job.WorkContent != jobDto.WorkContent || job.Salary != jobDto.Salary ||
                    job.OtherRequirement != jobDto.OtherRequirement || job.JobPlace != jobDto.JobPlace ||
                    job.Name != jobDto.Name)
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
            }

            await postgresContext.SaveChangesAsync();

        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"{nameof(DbService)} UpsertJob get exception.{{json}}", JsonSerializer.Serialize(jobDto));
        }
    }

    public abstract string CompanyInfoUrl(CompanyDto dto);
    public abstract string CompanyPageUrl(CompanyDto dto);
    public abstract string JobInfoUrl(JobDto dto);
    public abstract string JobPageUrl(JobDto dto);

    /// <summary>
    /// 公司資訊是否存在
    /// </summary>
    /// <param name="companyId"></param>
    /// <returns></returns>
    public async Task<bool> CompanyExist(string companyId)
    {
        if (await redisDb.HashExistsAsync(Parameters104.CompanyExistInDbRedisKey, companyId))
            return true;

        if (!await postgresContext.Companies.AnyAsync(x => x.Id == companyId))
            return false;

        await redisDb.HashSetAsync(Parameters104.CompanyExistInDbRedisKey, companyId, "");

        return true;
    }
}
