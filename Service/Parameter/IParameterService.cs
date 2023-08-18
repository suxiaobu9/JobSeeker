using Model.Dto;

namespace Service.Parameter;

public interface IParameterService
{
    /// <summary>
    /// 取得公司資訊網址
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    public abstract string CompanyInfoUrl(GetCompanyInfoDto dto);

    /// <summary>
    /// 取得公司頁面網址
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    public abstract string CompanyPageUrl(GetCompanyInfoDto dto);

    /// <summary>
    /// 取得職缺資訊網址
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    public abstract string JobInfoUrl(GetJobInfoDto dto);

    /// <summary>
    /// 取得職缺頁面網址
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    public abstract string JobPageUrl(GetJobInfoDto dto);
}
