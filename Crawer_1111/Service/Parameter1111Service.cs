using Model.Dto;
using Model.Dto104;
using Model.DtoCakeResume;
using Service.Parameter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crawer_1111.Service;

public class Parameter1111Service : IParameterService
{
    public string CompanyInfoUrl(GetCompanyInfoDto dto)
    {
        return CompanyPageUrl(dto);
    }

    public string CompanyPageUrl(GetCompanyInfoDto dto)
    {
        return $"https://www.1111.com.tw/corp/{dto.CompanyId}";
    }

    public string JobInfoUrl(GetJobInfoDto dto)
    {
        return JobPageUrl(dto);
    }

    public string JobPageUrl(GetJobInfoDto dto)
    {
        return $"https://www.1111.com.tw/job/{dto.JobId}";
    }
}
