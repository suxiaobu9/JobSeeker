using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Model;
using Model.JobSeekerDb;
using Model.Web;

namespace Service.Web;

public class JobSeeker104Service : IJobSeekerService
{
    private readonly postgresContext db;
    private readonly ILogger<JobSeeker104Service> logger;

    public JobSeeker104Service(postgresContext db,
        ILogger<JobSeeker104Service> logger)
    {
        this.db = db;
        this.logger = logger;
    }

    /// <summary>
    /// 取得 104 的公司列表
    /// </summary>
    /// <param name="includeIgnore"></param>
    /// <returns></returns>
    public async Task<JobSeekerHomePageModel> GetCompanies(bool includeIgnore)
    {
        var companyDatas = await db.Companies
            .Where(x => includeIgnore || x.Ignore == false)
            .OrderByDescending(x => x.Jobs.Any(y => !y.IsDeleted && !y.Ignore && !y.HaveRead))
            .ThenBy(x => x.Id)
            .ThenBy(x => x.Sort)
            .Select(x => new CompanyModel
            {
                Id = x.Id,
                Name = x.Name,
                IsIgnore = x.Ignore,
                CompanyInfoUrl = _104Parameters.Get104CompanyPageUrl(x.Id),
                NeedToRead = x.Jobs.Any(x => !x.IsDeleted && !x.HaveRead)
            }).ToArrayAsync();

        var result = new JobSeekerHomePageModel { Companies = companyDatas };

        return result;
    }

    /// <summary>
    /// 取得 104 職缺清單
    /// </summary>
    /// <param name="companyId"></param>
    /// <param name="includeIgnore"></param>
    /// <returns></returns>
    public async Task<JobModel[]?> GetJobs(string companyId, bool includeIgnore)
    {
        var result = await db.Jobs.Where(x => x.CompanyId == companyId)
            .Where(x => includeIgnore || x.Ignore == false)
            .Where(x => x.IsDeleted == false)
            .OrderBy(x => x.HaveRead)
            .ThenBy(x => x.Ignore)
            .Select(x => new JobModel
            {
                JobId = x.Id,
                Name = x.Name,
                JobUrl = _104Parameters.Get104JobPageUrl(x.Id),
                Salary = x.Salary,
                JobPlace = x.JobPlace,
                OtherRequirement = x.OtherRequirement,
                WorkContent = x.WorkContent,
                HaveRead = x.HaveRead,
                Ignore = x.Ignore
            }).ToArrayAsync();

        return result;
    }

    /// <summary>
    /// 忽略公司
    /// </summary>
    /// <param name="companyId"></param>
    /// <returns></returns>
    public async Task IgnoreCompany(string companyId)
    {
        var company = await db.Companies.FirstOrDefaultAsync(x => x.Id == companyId);

        if (company == null)
        {
            logger.LogWarning("IgnoreCompany : Can't get company info.{companyId}", companyId);
            return;
        }

        company.Ignore = true;

        await db.SaveChangesAsync();
    }

    /// <summary>
    /// 忽略職缺
    /// </summary>
    /// <param name="jobId"></param>
    /// <returns></returns>
    public async Task IgnoreJob(string jobId)
    {
        var job = await db.Jobs.FirstOrDefaultAsync(x => x.Id == jobId);

        if (job == null)
        {
            logger.LogWarning("IgnoreJob : Can't get job info.{jobId}", jobId);
            return;
        }

        job.Ignore = true;

        await db.SaveChangesAsync();

    }

    /// <summary>
    /// 已讀所有職缺
    /// </summary>
    /// <param name="companyId"></param>
    /// <returns></returns>
    public async Task ReadedAllJobs(string companyId)
    {
        var jobs = await db.Jobs.Where(x => x.CompanyId == companyId).ToArrayAsync();

        if (!jobs.Any())
        {
            logger.LogWarning("ReadedAllJobs : Can't get jobs info by company id. {companyId}", companyId);
            return;
        }

        foreach (var job in jobs)
        {
            job.HaveRead = true;
        }

        await db.SaveChangesAsync();

    }

    /// <summary>
    /// 已讀職缺
    /// </summary>
    /// <param name="jobId"></param>
    /// <returns></returns>
    public async Task ReadedJob(string jobId)
    {
        var job = await db.Jobs.FirstOrDefaultAsync(x => x.Id == jobId);

        if (job == null)
        {
            logger.LogWarning("ReadedJob : Can't get jobs info.{jobId}", jobId);
            return;
        }

        job.HaveRead = true;

        await db.SaveChangesAsync();
    }
}
