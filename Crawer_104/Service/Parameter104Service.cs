using Microsoft.AspNetCore.Components.RenderTree;
using Model.Dto;
using Model.Dto104;
using Service.Parameter;

namespace Crawer_104.Service;

public class Parameter104Service : IParameterService
{
    /// <summary>
    /// 取得公司資訊網址
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    public string CompanyInfoUrl(GetCompanyInfoDto dto)
    {
        return $"{Parameters104.Referer}/company/ajax/content/{dto.CompanyId}";
    }

    /// <summary>
    /// 取得公司頁面網址
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    public string CompanyPageUrl(GetCompanyInfoDto dto)
    {
        return $"{Parameters104.Referer}/company/{dto.CompanyId}";
    }

    /// <summary>
    /// 取得職缺資訊網址
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    public string JobInfoUrl(GetJobInfoDto dto)
    {
        return $"{Parameters104.Referer}/job/ajax/content/{dto.JobId}";
    }

    /// <summary>
    /// 取得職缺頁面網址
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    public string JobPageUrl(GetJobInfoDto dto)
    {
        return $"{Parameters104.Referer} /job/ {dto.JobId}";
    }
}
