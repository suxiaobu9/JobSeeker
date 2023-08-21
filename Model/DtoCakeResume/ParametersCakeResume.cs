using Model.Dto;
using Model.DtoCakeResume;
using System.Security;

namespace Model.DtoCakeResume;

public class ParametersCakeResume
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

    public static Dictionary<string, string[]> CompanyContentFilter => new()
    {
        { nameof(CompanyDto.Product), ProductsOrServicesHtmlTitle},
        { nameof(CompanyDto.Profile), CompanySummaryHtmlTitle},
        { nameof(CompanyDto.Welfare), EmployeeBenefitsHtmlTitle},
    };

    /// <summary>
    /// 用來判斷 html div 的內文 title
    /// </summary>
    public readonly static string[] ProductsOrServicesHtmlTitle = new string[] { "產品或服務", "Products or services" };

    /// <summary>
    /// 用來判斷 html div 的內文 title
    /// </summary>
    public readonly static string[] CompanySummaryHtmlTitle = new string[] { "公司介紹", "Company summary" };

    /// <summary>
    /// 用來判斷 html div 的內文 title
    /// </summary>
    public readonly static string[] EmployeeBenefitsHtmlTitle = new string[] { "員工福利", "Employee benefits" };

    /// <summary>
    /// 公司名稱 div class name
    /// </summary>
    public readonly static string CompanyNameDivClass = "CompanyHeader_companyName__Glj9i";

    /// <summary>
    /// 公司資訊 div class name
    /// </summary>
    public readonly static string CompanyInfoDivClass = "Card_container__PERSv";

    /// <summary>
    /// 公司資訊卡片 h2 class name
    /// </summary>
    public readonly static string CompanyCardH2Class = "Card_title__4iRRv";

    /// <summary>
    /// 公司資訊卡片 div class name
    /// </summary>
    public readonly static string CompanyCardDivClass = "RailsHtml_container__VVQ7u";

    /// <summary>
    /// 職缺名稱 div class name
    /// </summary>
    public readonly static string JobNameDivClass = "JobDescriptionLeftColumn_title__heKvX";

    /// <summary>
    /// 工作地點 a class name
    /// </summary>
    public readonly static string JobPlaceAClass = "CompanyInfoItem_link__E841d";

    /// <summary>
    /// 薪水 div class name
    /// </summary>
    public readonly static string SalaryDivClass = "JobDescriptionRightColumn_salaryWrapper__mYzNx";

    /// <summary>
    /// 最後更新時間 div class name
    /// </summary>
    public readonly static string LatestUpdateDateOuterDivClass = "JobDescriptionLeftColumn_inlineJobMeta__2t1il";

    /// <summary>
    /// 最後更新時間 div class name
    /// </summary>
    public readonly static string LatestUpdateDateDivClass = "InlineMessage_label__hP3Fk";

    /// <summary>
    /// 職缺資訊 div class name
    /// </summary>
    public readonly static string JobCardContentDivClass = "ContentSection_contentSection__k5CRR";

    /// <summary>
    /// 職缺標題 h3 class name
    /// </summary>
    public readonly static string JobCardInnerTitleH3ClassName = "ContentSection_title__Ox8_s";

    /// <summary>
    /// 職缺內文 div class name
    /// </summary>
    public readonly static string JobCardInnerContentDivClass = "RailsHtml_container__VVQ7u";

    public static Dictionary<string, string[]> JobContentFilter => new()
    {
        { nameof(JobDto.WorkContent), JobDescriptionHtmlTitle},
        { nameof(JobDto.OtherRequirement), RequirementsHtmlTitle},
    };

    /// <summary>
    /// 用來判斷 html div 的內文 title
    /// </summary>
    public readonly static string[] JobDescriptionHtmlTitle = new string[] { "職缺描述", "Job Description" };

    /// <summary>
    /// 用來判斷 html div 的內文 title
    /// </summary>
    public readonly static string[] RequirementsHtmlTitle = new string[] { "職務需求", "Requirements" };

    /// <summary>
    /// 職缺清單 div class name
    /// </summary>
    public readonly static string JobListCardContentDivClassName = "JobSearchItem_headerContent__Ka56W";
    
    /// <summary>
    /// 職缺清單 職缺 a class name
    /// </summary>
    public readonly static string JobListJobNodeAClassName = "JobSearchItem_jobTitle__Fjzv2";
    
    /// <summary>
    /// 職缺清單 公司 a class name
    /// </summary>
    public readonly static string JobListCompanyNodeAClassName = "JobSearchItem_companyName__QKkj5";
}

