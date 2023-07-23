namespace Model.DtoCakeResume;

public static class ParametersCakeResume
{
    public static string GetJobListUrl(string keyword, int page) => $"https://www.cakeresume.com/jobs/{keyword}?page={page}";

    public static string GetCompanyUrl(string companyId) => $"https://www.cakeresume.com/companies/{companyId}";

    public static string GetJobUrl(string companyId, string jobId) => $"https://www.cakeresume.com/companies/{companyId}/jobs/{jobId}";

    public static string CompanyIdForRedisAndQueue => "CakeResumeCompanyIdKey";

    public static string JobIdForRedisAndQueue => "CakeResumeJobIdKey";

    public static string CompanyUpdated => "comp_updated_cakeresume";
    
    public static string JobUpdated => "job_updated_cakeresume";

    /// <summary>
    /// CakeResume
    /// </summary>
    public static string SourceFrom => "CakeResume";

}

