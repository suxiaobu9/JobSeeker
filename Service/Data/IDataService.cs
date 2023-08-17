using Model;
using Model.Dto;

namespace Service.Data;

public interface IDataService
{
    /// <summary>
    /// 取得職缺資訊並更新
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public Task<ReturnStatus> GetJobDataAndUpsert(SimpleJobInfoDto? simpleJobInfo);

    /// <summary>
    /// 取得公司資訊並更新
    /// </summary>
    /// <param name="companyId"></param>
    /// <returns></returns>
    public Task GetCompanyDataAndUpsert(string companyId);

}
