using Model.Dto;
using Model.Dto104;
using Model.DtoCakeResume;
using Service.Parameter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crawer_CakeResume.Service;

public class ParameterCakeResumeService : IParameterService
{
    /// <summary>
    /// 取得公司資訊網址
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    public string CompanyInfoUrl(GetCompanyInfoDto dto)
    {
        return $"https://www.cakeresume.com/companies/{dto.CompanyId}";
    }

    /// <summary>
    /// 取得公司頁面網址
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    public string CompanyPageUrl(GetCompanyInfoDto dto)
    {
        return $"https://www.cakeresume.com/companies/{dto.CompanyId}";
    }

    /// <summary>
    /// 取得職缺資訊網址
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    public string JobInfoUrl(GetJobInfoDto dto)
    {
        return $"https://www.cakeresume.com/companies/{dto.CompanyId}/jobs/{dto.JobId}";
    }

    /// <summary>
    /// 取得職缺頁面網址
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    public string JobPageUrl(GetJobInfoDto dto)
    {
        return $"https://www.cakeresume.com/companies/{dto.CompanyId}/jobs/{dto.JobId}";
    }
}
