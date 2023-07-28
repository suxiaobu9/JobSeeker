using Model.Dto;
using Model.JobSeekerDb;

namespace Service.Db;

public interface IDbService
{
    /// <summary>
    /// 新增或更新公司資訊
    /// </summary>
    /// <param name="companyDto"></param>
    /// <returns></returns>
    public Task UpsertCompany(CompanyDto companyDto);

    /// <summary>
    /// 新增或更新職缺資訊
    /// </summary>
    /// <param name="jobDto"></param>
    /// <returns></returns>
    public Task UpsertJob(JobDto jobDto);

    /// <summary>
    /// 將指定來源的工作全部設為刪除
    /// </summary>
    /// <param name="sourceFrom"></param>
    /// <returns></returns>
    public Task MakeAllJobAsDelete(string sourceFrom);

    /// <summary>
    /// 公司存在
    /// </summary>
    /// <param name="companyId"></param>
    /// <returns></returns>
    public Task<bool> CompanyExist(string companyId);

    /// <summary>
    /// 職缺存在
    /// </summary>
    /// <param name="companyId"></param>
    /// <param name="jobId"></param>
    /// <returns></returns>
    public Task<bool> JobExist(string companyId, string jobId);

    /// <summary>
    /// 取得 PostgresContext
    /// </summary>
    /// <returns></returns>
    public postgresContext GetPostgresContext();
}
