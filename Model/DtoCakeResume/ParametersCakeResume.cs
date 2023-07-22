namespace Model.DtoCakeResume;

public static class ParametersCakeResume
{
    public static string GetJobListUrl(string keyword, int page) => $"https://www.cakeresume.com/jobs/{keyword}?page={page}";

    public static string GetCompanyUrl(string companyId) => $"https://www.cakeresume.com/companies/{companyId}";

    public static string GetJobUrl(string companyId, string jobId) => $"https://www.cakeresume.com/companies/{companyId}/jobs/{jobId}";

    public static string CompanyIdForRedisAndQueue => "CakeResumeCompanyIdKey";

    public static string JobIdForRedisAndQueue => "CakeResumeJobIdKey";

    /// <summary>
    /// CakeResume
    /// </summary>
    public static string SourceFrom => "CakeResume";

}

