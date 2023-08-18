namespace Model.DtoCakeResume;

public static class ParametersCakeResume
{
    public static string GetJobListUrl(string keyword, int page) => $"https://www.cakeresume.com/jobs/{keyword}?page={page}";

    /// <summary>
    /// company_id_updated_cakeresume
    /// </summary>
    public static string RedisKeyForCompanyUpdated => "company_id_updated_cakeresume";

    /// <summary>
    /// job_id_updated_cakeresume
    /// </summary>
    public static string RedisKeyForJobUpdated => "job_id_updated_cakeresume";

    /// <summary>
    /// CakeResume
    /// </summary>
    public static string SourceFrom => "CakeResume";

}

