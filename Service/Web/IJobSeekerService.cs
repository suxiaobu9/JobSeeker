using Model.Web;

namespace Service.Web;

public interface IJobSeekerService
{
    /// <summary>
    /// 取得公司資料
    /// </summary>
    /// <returns></returns>
    public Task<CompanyViewModel?> GetCompany();

    /// <summary>
    /// 取得未讀公司數量
    /// </summary>
    /// <returns></returns>
    public Task<int> GetUnReadCompanyCount();

    /// <summary>
    /// 已讀所有工作
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public Task ReadAllJobs(CompanyViewModel model);

    /// <summary>
    /// 忽略公司
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public Task IgnoreCompany(CompanyViewModel model);

    /// <summary>
    /// 已讀公司
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public Task ReadJob(JobViewModel model);

    /// <summary>
    /// 忽略工作
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public Task IgnoreJob(JobViewModel model);

}
