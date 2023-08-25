using Model.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DtoYourator;

public class ParametersYourator
{
    public static string CompanyNameH1Selector => "contains(@class, 'flex-initial') and contains(@class, 'truncate')";
    public static string CompanyCardContentSelector => "contains(@class, 'company__content')";
    public static string CompanyCardContentH2NotAllowClassName => "jobs";
    public static string JobTitleClassName => "basic-info__title__text";
    public static string JobPlaceClassName => "basic-info__address";
    public static string JobLastUpdateTimeClassName => "basic-info__last_updated_at";
    public static string JobCardContentClassName => "job__content";
    public static string JobCardContentH2ClassName => "job-heading";
    public static string CardContentSectionValidClassName => "content__area";
    public static string JobCardContentSectionNotValidClassName => "apply";
    
    public static string SourceFrom => "Yourator";

    public static string GetJobListUrl(string keyword, int page) => $"https://www.yourator.co/api/v4/jobs?area[]=TPE&area[]=NWT&page={page}&sort=most_related&term[]={keyword}";

    /// <summary>
    /// company_id_send_to_queue_Yourator
    /// </summary>
    public static string RedisKeyForCompanyIdSendToQueue => "company_id_send_to_queue_Yourator";

    /// <summary>
    /// job_id_send_to_queue_Yourator
    /// </summary>
    public static string RedisKeyForJobIdSendToQueue => "job_id_send_to_queue_Yourator";

    /// <summary>
    /// queue_company_id_for_Yourator
    /// </summary>
    public static string QueueNameForCompanyId => "queue_company_id_for_Yourator";

    /// <summary>
    /// queue_job_id_for_Yourator
    /// </summary>
    public static string QueueNameForJobId => "queue_job_id_for_Yourator";

    /// <summary>
    /// 用來判斷 html div 的內文 title
    /// </summary>
    public readonly static string[] CompanySummaryHtmlTitle = new string[] { "公司介紹" };

    /// <summary>
    /// 用來判斷 html div 的內文 title
    /// </summary>
    public readonly static string[] EmployeeBenefitsHtmlTitle = new string[] { "公司福利" };

    public static Dictionary<string, string[]> CompanyContentFilter => new()
    {
        { nameof(CompanyDto.Profile), CompanySummaryHtmlTitle},
        { nameof(CompanyDto.Welfare), EmployeeBenefitsHtmlTitle},
    };

    public static Dictionary<string, string[]> JobContentFilter => new()
    {
        { nameof(JobDto.WorkContent), JobDescriptionHtmlTitle},
        { nameof(JobDto.OtherRequirement), RequirementsHtmlTitle},
        { nameof(JobDto.Salary), SalaryHtmlTitle},
    };



    /// <summary>
    /// 用來判斷 html div 的內文 title
    /// </summary>
    public readonly static string[] JobDescriptionHtmlTitle = new string[] { "工作內容" };

    /// <summary>
    /// 用來判斷 html div 的內文 title
    /// </summary>
    public readonly static string[] RequirementsHtmlTitle = new string[] { "加分條件" };

    /// <summary>
    /// 用來判斷 html div 的內文 title
    /// </summary>
    public readonly static string[] SalaryHtmlTitle = new string[] { "薪資範圍" };
}
