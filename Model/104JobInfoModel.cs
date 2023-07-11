#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
using System.Text.Json.Serialization;

namespace Model;

public class _104JobInfoModel
{
    [JsonPropertyName("data")]
    public JobInfoData Data { get; set; }

    public string? GetJobId => Data.Header.AnalysisUrl.Split('/').LastOrDefault();

}

public class JobInfoData
{
    [JsonPropertyName("header")]
    public Header Header { get; set; }

    [JsonPropertyName("condition")]
    public Condition Condition { get; set; }

    [JsonPropertyName("welfare")]
    public Welfare Welfare { get; set; }

    [JsonPropertyName("jobDetail")]
    public Jobdetail JobDetail { get; set; }

    [JsonPropertyName("_switch")]
    public string Switch { get; set; }

    [JsonPropertyName("custLogo")]
    public string CustLogo { get; set; }

    [JsonPropertyName("postalCode")]
    public string PostalCode { get; set; }

    [JsonPropertyName("closeDate")]
    public string CloseDate { get; set; }

    [JsonPropertyName("industry")]
    public string Industry { get; set; }

    [JsonPropertyName("custNo")]
    public string CustNo { get; set; }

    [JsonPropertyName("reportUrl")]
    public string ReportUrl { get; set; }

    [JsonPropertyName("industryNo")]
    public string IndustryNo { get; set; }

    [JsonPropertyName("employees")]
    public string Employees { get; set; }

    [JsonPropertyName("chinaCorp")]
    public bool ChinaCorp { get; set; }
}

public class Corpimageright1
{

    [JsonPropertyName("imageUrl")]
    public string ImageUrl { get; set; }

    [JsonPropertyName("link")]
    public string Link { get; set; }
}

public class Header
{

    [JsonPropertyName("jobName")]
    public string JobName { get; set; }

    [JsonPropertyName("appearDate")]
    public string AppearDate { get; set; }

    [JsonPropertyName("custName")]
    public string CustName { get; set; }

    [JsonPropertyName("custUrl")]
    public string CustUrl { get; set; }

    [JsonPropertyName("applyDate")]
    public string ApplyDate { get; set; }

    [JsonPropertyName("analysisType")]
    public int AnalysisType { get; set; }

    [JsonPropertyName("analysisUrl")]
    public string AnalysisUrl { get; set; }

    [JsonPropertyName("isSaved")]
    public bool IsSaved { get; set; }

    [JsonPropertyName("isApplied")]
    public bool IsApplied { get; set; }
}

public class Condition
{

    [JsonPropertyName("acceptRole")]
    public Acceptrole AcceptRole { get; set; }

    [JsonPropertyName("workExp")]
    public string WorkExp { get; set; }

    [JsonPropertyName("edu")]
    public string Edu { get; set; }

    [JsonPropertyName("major")]
    public object[] Major { get; set; }

    [JsonPropertyName("language")]
    public object[] Language { get; set; }

    [JsonPropertyName("localLanguage")]
    public object[] LocalLanguage { get; set; }

    [JsonPropertyName("specialty")]
    public Specialty[] Specialty { get; set; }

    [JsonPropertyName("skill")]
    public Skill[] Skill { get; set; }

    [JsonPropertyName("certificate")]
    public object[] Certificate { get; set; }

    [JsonPropertyName("driverLicense")]
    public object[] DriverLicense { get; set; }

    [JsonPropertyName("other")]
    public string Other { get; set; }
}

public class Acceptrole
{

    [JsonPropertyName("role")]
    public object[] Role { get; set; }

    [JsonPropertyName("disRole")]
    public Disrole DisRole { get; set; }
}

public class Disrole
{

    [JsonPropertyName("needHandicapCompendium")]
    public bool NeedHandicapCompendium { get; set; }

    [JsonPropertyName("disability")]
    public object[] Disability { get; set; }
}

public class Specialty
{

    [JsonPropertyName("code")]
    public string Code { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }
}

public class Skill
{

    [JsonPropertyName("code")]
    public string Code { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }
}

public class Welfare
{

    [JsonPropertyName("tag")]
    public object[] Tag { get; set; }

    [JsonPropertyName("welfare")]
    public string Welfare1 { get; set; }

    [JsonPropertyName("legalTag")]
    public object[] LegalTag { get; set; }
}

public class Jobdetail
{

    [JsonPropertyName("jobDescription")]
    public string JobDescription { get; set; }

    [JsonPropertyName("jobCategory")]
    public Jobcategory[] JobCategory { get; set; }

    [JsonPropertyName("salary")]
    public string Salary { get; set; }

    [JsonPropertyName("salaryMin")]
    public int SalaryMin { get; set; }

    [JsonPropertyName("salaryMax")]
    public int SalaryMax { get; set; }

    [JsonPropertyName("salaryType")]
    public int SalaryType { get; set; }

    [JsonPropertyName("jobType")]
    public int JobType { get; set; }

    [JsonPropertyName("workType")]
    public object[] WorkType { get; set; }

    [JsonPropertyName("addressNo")]
    public string AddressNo { get; set; }

    [JsonPropertyName("addressRegion")]
    public string AddressRegion { get; set; }

    [JsonPropertyName("addressArea")]
    public string AddressArea { get; set; }

    [JsonPropertyName("addressDetail")]
    public string AddressDetail { get; set; }

    [JsonPropertyName("industryArea")]
    public string IndustryArea { get; set; }

    [JsonPropertyName("longitude")]
    public string Longitude { get; set; }

    [JsonPropertyName("latitude")]
    public string Latitude { get; set; }

    [JsonPropertyName("manageResp")]
    public string ManageResp { get; set; }

    [JsonPropertyName("businessTrip")]
    public string BusinessTrip { get; set; }

    [JsonPropertyName("workPeriod")]
    public string WorkPeriod { get; set; }

    [JsonPropertyName("vacationPolicy")]
    public string VacationPolicy { get; set; }

    [JsonPropertyName("startWorkingDay")]
    public string StartWorkingDay { get; set; }

    [JsonPropertyName("hireType")]
    public int HireType { get; set; }

    [JsonPropertyName("delegatedRecruit")]
    public string DelegatedRecruit { get; set; }

    [JsonPropertyName("needEmp")]
    public string NeedEmp { get; set; }

    [JsonPropertyName("landmark")]
    public string Landmark { get; set; }

    [JsonPropertyName("remoteWork")]
    public Remotework RemoteWork { get; set; }
}

public class Remotework
{

    [JsonPropertyName("type")]
    public int Type { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }
}

public class Jobcategory
{

    [JsonPropertyName("code")]
    public string Code { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }
}

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.