using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Model.JobSeekerDb;
using Model.Web;

namespace Service.Web;

public class JobSeekerService : IJobSeekerService
{
    private readonly postgresContext postgresContext;
    private readonly ILogger<JobSeekerService> logger;

    public JobSeekerService(postgresContext postgresContext, 
        ILogger<JobSeekerService> logger)
    {
        this.postgresContext = postgresContext;
        this.logger = logger;
    }

    /// <summary>
    /// 取得公司資料
    /// </summary>
    /// <returns></returns>
    public Task<CompanyViewModel?> GetCompany()
    {
        return GetUnReadCompanyQuery()
                .OrderByDescending(x => x.Jobs.Max(y => y.UpdateUtcAt))
                .Select(x => new CompanyViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    CompanyPageUrl = x.Url,
                    IsIgnore = x.Ignore,
                    NeedToRead = x.Jobs.Any(x => !x.IsDeleted && !x.HaveRead),
                    Product = x.Product,
                    Profile = x.Profile,
                    Welfare = x.Welfare,
                    UpdateCount = x.UpdateCount,
                    SourceFrom = x.SourceFrom,
                    Jobs = x.Jobs.Where(y => !y.IsDeleted && !y.Ignore && !y.HaveRead)
                            .Select(y => new JobViewModel
                            {
                                JobId = y.Id,
                                Name = y.Name,
                                JobUrl = y.Url,
                                Salary = y.Salary,
                                JobPlace = y.JobPlace,
                                OtherRequirement = y.OtherRequirement,
                                WorkContent = y.WorkContent,
                                HaveRead = y.HaveRead,
                                Ignore = y.Ignore,
                                UpdateCount = y.UpdateCount,
                                LatestUpdateDate = y.LatestUpdateDate
                            }).ToArray(),
                }).FirstOrDefaultAsync();
    }

    /// <summary>
    /// 取得未讀公司數量
    /// </summary>
    /// <returns></returns>
    public Task<int> GetUnReadCompanyCount()
    {
        return GetUnReadCompanyQuery().CountAsync();
    }

    /// <summary>
    /// 忽略公司
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task IgnoreCompany(CompanyViewModel model)
    {
        var company = await postgresContext.Companies.FirstOrDefaultAsync(x => x.Id == model.Id);

        if (company == null)
        {
            logger.LogWarning($"{nameof(JobSeekerService)} IgnoreCompany get null company.{{companyId}}", model.Id);
            return;
        }

        company.Ignore = true;

        await postgresContext.SaveChangesAsync();
    }

    /// <summary>
    /// 忽略工作
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task IgnoreJob(JobViewModel model)
    {
        var job = await postgresContext.Jobs.FirstOrDefaultAsync(x => x.Id == model.JobId);

        if (job == null)
        {
            logger.LogWarning($"{nameof(JobSeekerService)} IgnoreJob get null job.{{jobId}}", model.JobId);
            return;
        }

        job.Ignore = true;

        await postgresContext.SaveChangesAsync();
    }

    /// <summary>
    /// 已讀所有工作
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task ReadAllJobs(CompanyViewModel model)
    {
        var jobs = await postgresContext.Jobs.Where(x => x.CompanyId == model.Id).ToArrayAsync();

        foreach (var job in jobs)
        {
            job.HaveRead = true;
        }

        await postgresContext.SaveChangesAsync();
    }

    /// <summary>
    /// 已讀公司
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task ReadJob(JobViewModel model)
    {
        var job = await postgresContext.Jobs.Where(x => x.Id == model.JobId).FirstOrDefaultAsync();

        if (job == null)
        {
            logger.LogWarning($"{nameof(JobSeekerService)} ReadJob get null job.{{jobId}}", model.JobId);
            return;
        }

        job.HaveRead = true;

        await postgresContext.SaveChangesAsync();
    }

    /// <summary>
    /// 取得未讀公司的條件
    /// </summary>
    /// <returns></returns>
    private IQueryable<Company> GetUnReadCompanyQuery()
    {
        return postgresContext.Companies.Where(x => !x.Ignore).AsNoTracking()
                .Where(x => x.Jobs.Any(y => !y.IsDeleted && !y.Ignore && !y.HaveRead));
    }
}
