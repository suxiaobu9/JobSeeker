using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Model.Dto1111;

public class JobList1111Model
{
    /// <summary>
    /// 目前頁數
    /// </summary>
    [JsonPropertyName("pi")]
    public int? Pi { get; set; }

    /// <summary>
    /// 總頁數
    /// </summary>
    [JsonPropertyName("pc")]
    public int? Pc { get; set; }

    [JsonPropertyName("html0_d")]
    public string? Html0D { get; set; }

    [JsonPropertyName("html1_d")]
    public string? Html1D { get; set; }

    [JsonPropertyName("html2_d")]
    public string? Html2D { get; set; }

    public string[] HtmlTotal => new string?[3] { Html0D, Html1D, Html2D }.Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x!).ToArray();
}

