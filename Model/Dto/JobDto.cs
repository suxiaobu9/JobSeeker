namespace Model.Dto;

public class JobDto
{
    public string JobId { get; set; } = null!;

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
}
