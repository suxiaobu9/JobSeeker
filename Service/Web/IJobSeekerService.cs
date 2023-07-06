using Model.Web;

namespace Service.Web;

public interface IJobSeekerService
{
    /// <summary>
    /// 取得公司列表
    /// </summary>
    /// <param name="includeIgnore"></param>
    /// <returns></returns>
    public Task<JobSeekerHomePageModel> GetCompanies(bool includeIgnore);

    /// <summary>
    /// 取得工作清單
    /// </summary>
    /// <param name="companyId"></param>
    /// <param name="includeIgnore"></param>
    /// <returns></returns>
    public Task<JobModel[]?> GetJobs(string companyId, bool includeIgnore);

    /// <summary>
    /// 已讀公司的所有職缺
    /// </summary>
    /// <param name="companyId"></param>
    /// <returns></returns>
    public Task ReadedAllJobs(string companyId);

    /// <summary>
    /// 忽略公司
    /// </summary>
    /// <param name="companyId"></param>
    /// <returns></returns>
    public Task IgnoreCompany(string companyId);

    /// <summary>
    /// 已讀職缺
    /// </summary>
    /// <param name="jobId"></param>
    /// <returns></returns>
    public Task ReadedJob(string jobId);

    /// <summary>
    /// 忽略職缺
    /// </summary>
    /// <param name="jobId"></param>
    /// <returns></returns>
    public Task IgnoreJob(string jobId);


}
