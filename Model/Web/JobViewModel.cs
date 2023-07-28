namespace Model.Web;

public class JobViewModel
{
    public string? JobId { get; set; }

    public string? Name { get; set; }

    public string? JobUrl { get; set; }

    public string? Salary { get; set; }

    public string? JobPlace { get; set; }

    public string? WorkContent { get; set; }

    public string? OtherRequirement { get; set; }

    public bool HaveRead { get; set; }

    public bool Ignore { get; set; }

    public int UpdateCount { get; set; }

    public string? LatestUpdateDate { get; set; }
}
