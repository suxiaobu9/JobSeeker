using Model.JobSeekerDb;

namespace Service.Db;

public interface IDbService
{
    /// <summary>
    /// 將工作資訊轉換成資料庫實體
    /// </summary>
    /// <param name="jobInfoJson"></param>
    /// <returns></returns>
    public Model.JobSeekerDb.Job? TransJobInfoToDbEntity(string jobInfoJson);

    /// <summary>
    /// 公司是否存在
    /// </summary>
    /// <param name="companyNo"></param>
    /// <returns></returns>
    public Task<bool> CompanyExists(string companyNo);

    /// <summary>
    /// 更新或新增公司資料
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public Task UpsertCompany(Company model);

    /// <summary>
    /// 更新或新增職缺資料
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public Task UpsertJob(Job model);

    /// <summary>
    /// 將公司資訊轉換成資料庫實體
    /// </summary>
    /// <param name="companyInfoData"></param>
    /// <returns></returns>
    public Model.JobSeekerDb.Company? TransCompanyInfoToDbEntity(string companyNo, string companyInfoData);

    /// <summary>
    /// 將未刪除的職缺設為刪除
    /// </summary>
    /// <returns></returns>
    public Task SetAllUndeleteJobToDelete();


}

