#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Model;

public class _104JobListModel
{
    [JsonPropertyName("status")]
    public int Status { get; set; }
    
    [JsonPropertyName("data")]
    public JobListData Data { get; set; }

    [JsonPropertyName("statusMsg")]
    public string StatusMsg { get; set; }

    [JsonPropertyName("errorMsg")]
    public string ErrorMsg { get; set; }
}

public class JobListData
{

    [JsonPropertyName("list")]
    public List[] List { get; set; }

    [JsonPropertyName("pageNo")]
    public int PageNo { get; set; }

    [JsonPropertyName("totalPage")]
    public int TotalPage { get; set; }

    [JsonPropertyName("totalCount")]
    public int TotalCount { get; set; }
}

public class List
{

    [JsonPropertyName("jobType")]
    public string JobType { get; set; }

    [JsonPropertyName("jobNo")]
    public string JobNo { get; set; }

    [JsonPropertyName("jobName")]
    public string JobName { get; set; }

    [JsonPropertyName("jobNameSnippet")]
    public string JobNameSnippet { get; set; }

    [JsonPropertyName("jobRole")]
    public string JobRole { get; set; }

    [JsonPropertyName("jobRo")]
    public string JobRo { get; set; }

    [JsonPropertyName("jobAddrNo")]
    public string JobAddrNo { get; set; }

    [JsonPropertyName("jobAddrNoDesc")]
    public string JobAddrNoDesc { get; set; }

    [JsonPropertyName("jobAddress")]
    public string JobAddress { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("descWithoutHighlight")]
    public string DescWithoutHighlight { get; set; }

    [JsonPropertyName("optionEdu")]
    public string OptionEdu { get; set; }

    [JsonPropertyName("period")]
    public string Period { get; set; }

    [JsonPropertyName("periodDesc")]
    public string PeriodDesc { get; set; }

    [JsonPropertyName("applyCnt")]
    public string ApplyCnt { get; set; }

    [JsonPropertyName("applyType")]
    public int ApplyType { get; set; }

    [JsonPropertyName("applyDesc")]
    public string ApplyDesc { get; set; }

    [JsonPropertyName("custNo")]
    public string CustNo { get; set; }

    [JsonPropertyName("custName")]
    public string CustName { get; set; }

    [JsonPropertyName("coIndustry")]
    public string CoIndustry { get; set; }

    [JsonPropertyName("coIndustryDesc")]
    public string CoIndustryDesc { get; set; }

    [JsonPropertyName("salaryLow")]
    public string SalaryLow { get; set; }

    [JsonPropertyName("salaryHigh")]
    public string SalaryHigh { get; set; }

    [JsonPropertyName("salaryDesc")]
    public string SalaryDesc { get; set; }

    [JsonPropertyName("s10")]
    public string S10 { get; set; }

    [JsonPropertyName("appearDate")]
    public string AppearDate { get; set; }

    [JsonPropertyName("appearDateDesc")]
    public string AppearDateDesc { get; set; }

    [JsonPropertyName("optionZone")]
    public string OptionZone { get; set; }

    [JsonPropertyName("isApply")]
    public string IsApply { get; set; }

    [JsonPropertyName("applyDate")]
    public string ApplyDate { get; set; }

    [JsonPropertyName("isSave")]
    public string IsSave { get; set; }

    [JsonPropertyName("descSnippet")]
    public string DescSnippet { get; set; }

    [JsonPropertyName("tags")]
    public object Tags { get; set; }

    [JsonPropertyName("landmark")]
    public string Landmark { get; set; }

    [JsonPropertyName("link")]
    public Link Link { get; set; }

    [JsonPropertyName("jobsource")]
    public string Jobsource { get; set; }

    [JsonPropertyName("jobNameRaw")]
    public string JobNameRaw { get; set; }

    [JsonPropertyName("custNameRaw")]
    public string CustNameRaw { get; set; }

    [JsonPropertyName("lon")]
    public string Lon { get; set; }

    [JsonPropertyName("lat")]
    public string Lat { get; set; }

    [JsonPropertyName("remoteWorkType")]
    public int RemoteWorkType { get; set; }

    [JsonPropertyName("major")]
    public string[] Major { get; set; }

    [JsonPropertyName("salaryType")]
    public string SalaryType { get; set; }

    [JsonPropertyName("dist")]
    public object Dist { get; set; }

    [JsonPropertyName("mrt")]
    public object Mrt { get; set; }

    [JsonPropertyName("mrtDesc")]
    public string MrtDesc { get; set; }
}

public class Link
{

    [JsonPropertyName("applyAnalyze")]
    public string ApplyAnalyze { get; set; }

    [JsonPropertyName("job")]
    public string Job { get; set; }

    [JsonPropertyName("cust")]
    public string Cust { get; set; }
}

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.