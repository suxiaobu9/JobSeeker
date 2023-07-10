#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
using System.Text.Json.Serialization;

namespace Model;

public class _104CompanyInfoModel
{
    [JsonPropertyName("data")]
    public CompanyData Data { get; set; }
}

public class CompanyData
{
    [JsonPropertyName("custSwitch")]
    public string CustSwitch { get; set; }

    [JsonPropertyName("custName")]
    public string CustName { get; set; }

    [JsonPropertyName("custNo")]
    public string CustNo { get; set; }

    [JsonPropertyName("industryDesc")]
    public string IndustryDesc { get; set; }

    [JsonPropertyName("indcat")]
    public string Indcat { get; set; }

    [JsonPropertyName("empNo")]
    public string EmpNo { get; set; }

    [JsonPropertyName("capital")]
    public string Capital { get; set; }

    [JsonPropertyName("address")]
    public string Address { get; set; }

    [JsonPropertyName("industryArea")]
    public object IndustryArea { get; set; }

    [JsonPropertyName("custLink")]
    public string CustLink { get; set; }

    [JsonPropertyName("profile")]
    public string Profile { get; set; }

    [JsonPropertyName("product")]
    public string Product { get; set; }

    [JsonPropertyName("welfare")]
    public string Welfare { get; set; }

    [JsonPropertyName("management")]
    public string Management { get; set; }

    [JsonPropertyName("phone")]
    public string Phone { get; set; }

    [JsonPropertyName("fax")]
    public string Fax { get; set; }

    [JsonPropertyName("hrName")]
    public string HrName { get; set; }

    [JsonPropertyName("lat")]
    public string Lat { get; set; }

    [JsonPropertyName("lon")]
    public string Lon { get; set; }

    [JsonPropertyName("logo")]
    public string Logo { get; set; }

    [JsonPropertyName("news")]
    public string News { get; set; }

    [JsonPropertyName("newsLink")]
    public string NewsLink { get; set; }

    [JsonPropertyName("corpImage2")]
    public string CorpImage2 { get; set; }

    [JsonPropertyName("corpImage1")]
    public string CorpImage1 { get; set; }

    [JsonPropertyName("corpImage3")]
    public string CorpImage3 { get; set; }

    [JsonPropertyName("corpLink2")]
    public string CorpLink2 { get; set; }

    [JsonPropertyName("corpLink1")]
    public string CorpLink1 { get; set; }

    [JsonPropertyName("corpLink3")]
    public string CorpLink3 { get; set; }

    [JsonPropertyName("productPictures")]
    public object[] ProductPictures { get; set; }

    [JsonPropertyName("tagNames")]
    public string[] TagNames { get; set; }

    [JsonPropertyName("legalTagNames")]
    public string[] LegalTagNames { get; set; }

    [JsonPropertyName("historys")]
    public object[] Historys { get; set; }

    [JsonPropertyName("isSaved")]
    public bool IsSaved { get; set; }

    [JsonPropertyName("isTracked")]
    public bool IsTracked { get; set; }

    [JsonPropertyName("addrNoDesc")]
    public string AddrNoDesc { get; set; }

    [JsonPropertyName("reportUrl")]
    public string ReportUrl { get; set; }

    [JsonPropertyName("postalCode")]
    public int PostalCode { get; set; }
}

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.