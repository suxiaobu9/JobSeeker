using Model.Dto;

namespace Model.Dto1111;

public static class Parameters1111
{
    /// <summary>
    /// 1111
    /// </summary>
    public const string SourceFrom = "1111";

    /// <summary>
    /// queue_company_id_for_1111
    /// </summary>
    public const string QueueNameForCompanyId = "queue_company_id_for_1111";

    /// <summary>
    /// queue_job_id_for_1111
    /// </summary>
    public const string QueueNameForJobId = "queue_job_id_for_1111";

    /// <summary>
    /// company_id_send_to_queue_1111
    /// </summary>
    public const string RedisKeyForCompanyIdSendToQueue = "company_id_send_to_queue_1111";

    /// <summary>
    /// job_id_send_to_queue_1111
    /// </summary>
    public const string RedisKeyForJobIdSendToQueue = "job_id_send_to_queue_1111";

    /// <summary>
    /// company_id_updated_1111
    /// </summary>
    public const string RedisKeyForCompanyUpdated = "company_id_updated_1111";

    public static Dictionary<string, string[]> CompanyContentFilter => new()
    {
        { nameof(CompanyDto.Product), ProductsOrServicesHtmlTitle},
        { nameof(CompanyDto.Profile), CompanySummaryHtmlTitle},
        { nameof(CompanyDto.Welfare), EmployeeBenefitsHtmlTitle},
    };

    /// <summary>
    /// 用來判斷 html div 的內文 title
    /// </summary>
    public readonly static string[] ProductsOrServicesHtmlTitle = new string[] { "產品/服務" };

    /// <summary>
    /// 用來判斷 html div 的內文 title
    /// </summary>
    public readonly static string[] CompanySummaryHtmlTitle = new string[] { "公司簡介" };

    /// <summary>
    /// 用來判斷 html div 的內文 title
    /// </summary>
    public readonly static string[] EmployeeBenefitsHtmlTitle = new string[] { "公司福利" };

    public static string GetJobListUrl(string keyword, int page) => $"https://www.1111.com.tw/search/job?c0=100100%2C100200&ks={keyword}&page={page}&act=load_page";

    public const string JobListCardDivClassName = "job_item_profile";
    public const string JobListJobDivClassName = "job_item_info";
    public const string JobListCompanyDivClassName = "subtitle";
    public const string JobListJobAClass = "job";
    public const string JobListCompanyAClass = "company";
    public const string CompanyNameDivName = "title";
    public const string CompanyCardContentDivClassName = "corp_body";
    public const string JobWorkContentDivClass = "job_description";
    public const string JobWorkContentChildDivClass = "description_info";
    public const string JobTitleH1ClassName = "title";
    public const string JobPlaceIconDivClass = "icon_location";
    public const string JobPlaceSpanClass = "job_info_content";
    public const string JobOtherRequirementDivClass = "job_skill";
    public const string JobOtherRequirementConditionDivClass = "conditions";
    public const string JobOtherRequirementContentDivClass = "ui_items_group";
    public const string JobSalaryRegionDivClass = "job_salary_region";
    public const string JobSalarySpanClass = "color_Secondary_1";
    public const string JobLastUpdateTimeSmallClass = "job_item_date";

}
