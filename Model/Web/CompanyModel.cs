namespace Model.Web;

public class CompanyModel
{
    public string? Id { get; set; }

    public string? Name { get; set; }

    public string? CompanyInfoUrl { get; set; }

    public bool IsIgnore { get; set; }

    public bool NeedToRead { get; set; }
}