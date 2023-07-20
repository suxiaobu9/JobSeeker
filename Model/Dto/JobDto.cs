using System.Text.RegularExpressions;

namespace Model.Dto;

public class JobDto
{
    public string Id { get; set; } = null!;

    public string CompanyId { get; set; } = null!;

    /// <summary>
    /// 名稱
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// 工作內容
    /// </summary>
    public string WorkContent { get; set; } = null!;

    /// <summary>
    /// 薪水
    /// </summary>
    public string Salary { get; set; } = null!;

    /// <summary>
    /// 其他
    /// </summary>
    public string? OtherRequirement { get; set; }

    /// <summary>
    /// 工作地點
    /// </summary>
    public string JobPlace { get; set; } = null!;

    /// <summary>
    /// 判斷職缺是否符合條件
    /// </summary>
    public bool FilterPassed
    {
        get
        {
            var content = (Name + WorkContent + OtherRequirement).ToLower();

            string urlPattern = @"https?://[^\s\u4E00-\u9FA5]+";

            var matches = Regex.Matches(content, urlPattern);
            foreach (Match match in matches)
            {
                content = content.Replace(match.Value, "");
            }

            return Parameters.KeywordsFilters.Any(x => content.Contains(x.ToLower()));

        }
    }
}
