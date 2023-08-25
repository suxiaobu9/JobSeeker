using Model.Dto;
using Service.Parameter;

namespace Crawer_Yourator.Service;

public class ParameterYouratorService : IParameterService
{
    public string CompanyInfoUrl(GetCompanyInfoDto dto)
    {
        return $"https://www.yourator.co/companies/{dto.CompanyId}";
    }

    public string CompanyPageUrl(GetCompanyInfoDto dto)
    {
        return $"https://www.yourator.co/companies/{dto.CompanyId}";
    }

    public string JobInfoUrl(GetJobInfoDto dto)
    {
        return $"https://www.yourator.co/companies/{dto.CompanyId}/jobs/{dto.JobId}";
    }

    public string JobPageUrl(GetJobInfoDto dto)
    {
        return $"https://www.yourator.co/companies/{dto.CompanyId}/jobs/{dto.JobId}";
    }
}
