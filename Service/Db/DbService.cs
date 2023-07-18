using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Model.Dto;
using Model.Dto104;
using Model.JobSeekerDb;
using System;
using System.ComponentModel.Design;

namespace Service.Db;

public class DbService : IDbService
{
    private readonly ILogger<DbService> logger;
    private readonly postgresContext postgresContext;

    public DbService(ILogger<DbService> logger,
        postgresContext postgresContext)
    {
        this.logger = logger;
        this.postgresContext = postgresContext;
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

            if (company == null)
            {
                await postgresContext.Companies.AddAsync(new Company
                {
                    Id = companyDto.Id,
                    CreateUtcAt = now.UtcDateTime,
                    GetInfoUrl = Parameters104.Get104CompanyInfoUrl(companyDto.Id),
                    Ignore = false,
                    Name = companyDto.Name,
                    Product = companyDto.Product,
                    Profile = companyDto.Profile,
                    SourceFrom = companyDto.SourceFrom,
                    UpdateUtcAt = now.UtcDateTime,
                    Url = Parameters104.Get104CompanyPageUrl(companyDto.Id),
                    Welfare = companyDto.Welfare,
                });
            }
            else
            {
                company.GetInfoUrl = Parameters104.Get104CompanyInfoUrl(companyDto.Id);
                company.Name = companyDto.Name;
                company.Product = companyDto.Product;
                company.Profile = companyDto.Profile;
                company.SourceFrom = companyDto.SourceFrom;
                company.UpdateUtcAt = now.UtcDateTime;
                company.Url = Parameters104.Get104CompanyPageUrl(companyDto.Id);
                company.Welfare = companyDto.Welfare;
            }

            await postgresContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"{nameof(DbService)} UpsertCompany get exception.");
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

            if (job == null)
            {
                await postgresContext.Jobs.AddAsync(new Job
                {
                    Id = jobDto.Id,
                    CompanyId = jobDto.CompanyId,
                    CreateUtcAt = now.UtcDateTime,
                    GetInfoUrl = Parameters104.Get104JobInfoUrl(jobDto.Id),
                    HaveRead = false,
                    Ignore = false,
                    IsDeleted = false,
                    JobPlace = jobDto.JobPlace,
                    Name = jobDto.Name,
                    OtherRequirement = jobDto.OtherRequirement,
                    Salary = jobDto.Salary,
                    UpdateUtcAt = now.UtcDateTime,
                    Url = Parameters104.Get104JobPageUrl(jobDto.Id),
                    WorkContent = jobDto.WorkContent,
                });
            }
            else
            {
                if (job.WorkContent == jobDto.WorkContent && job.Salary == jobDto.Salary &&
                    job.OtherRequirement == jobDto.OtherRequirement && job.JobPlace == jobDto.JobPlace &&
                    job.Name == jobDto.Name)
                    job.HaveRead = false;

                job.IsDeleted = false;
                job.GetInfoUrl = Parameters104.Get104JobInfoUrl(jobDto.Id);
                job.JobPlace = jobDto.JobPlace;
                job.Name = jobDto.Name;
                job.OtherRequirement = jobDto.OtherRequirement;
                job.Salary = jobDto.Salary;
                job.UpdateUtcAt = now.UtcDateTime;
                job.Url = Parameters104.Get104JobPageUrl(jobDto.Id);
                job.WorkContent = jobDto.WorkContent;
            }

            await postgresContext.SaveChangesAsync();

        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"{nameof(DbService)} UpsertJob get exception.");
        }
    }
}
