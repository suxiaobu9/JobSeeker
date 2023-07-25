namespace Model.DtoCakeResume;

public static class ParametersCakeResume
{
    public static string GetJobListUrl(string keyword, int page) => $"https://www.cakeresume.com/jobs/{keyword}?page={page}";

    public static string GetCompanyUrl(string companyId) => $"https://www.cakeresume.com/companies/{companyId}";

    public static string GetJobUrl(string companyId, string jobId) => $"https://www.cakeresume.com/companies/{companyId}/jobs/{jobId}";

    /// <summary>
    /// company_id_already_get_cakeresume
    /// </summary>
    public static string RedisKeyForCompanyAlreadyGet => "company_id_already_get_cakeresume";

    /// <summary>
    /// job_id_already_get_cakeresume
    /// </summary>
    public static string RedisKeyForJobAlreadyGet => "job_id_already_get_cakeresume";

    /// <summary>
    /// company_id_updated_cakeresume
    /// </summary>
    public static string RedisKeyForCompanyUpdated => "company_id_updated_cakeresume";

    /// <summary>
    /// CakeResume
    /// </summary>
    public static string SourceFrom => "CakeResume";

}

