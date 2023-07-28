namespace Model.Web;

public class CompanyViewModel
{
    public string? Id { get; set; }

    public string? Name { get; set; }

    public string? CompanyPageUrl { get; set; }

    public bool IsIgnore { get; set; }

    public bool NeedToRead { get; set; }

    public string? Product { get; set; }

    public string? Profile { get; set; }

    public string? Welfare { get; set; }

    public int UpdateCount { get; set; }

    public string? SourceFrom { get; set; }

    public JobViewModel[]? Jobs { get; set; }
}