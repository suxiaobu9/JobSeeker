using HtmlAgilityPack;
using Model.Dto1111;
using Service.HtmlAnalyze;
using System.Text.RegularExpressions;

namespace Crawer_1111.Service;

public class HtmlAnalyze1111Service : IHtmlAnalyzeService
{
    private readonly ILogger<HtmlAnalyze1111Service> logger;

    public HtmlAnalyze1111Service(ILogger<HtmlAnalyze1111Service> logger)
    {
        this.logger = logger;
    }

    public KeyValuePair<string, string>? GetCompanyCardContent(HtmlNode htmlNode)
    {
        var divNode = htmlNode.SelectNodes($"//div")?[0];
        if (divNode == null)
            return null;

        var cardTitle = divNode.SelectNodes($".//h2")?[0].InnerText;

        if (cardTitle == null)
            return null;

        var filterKey = Parameters1111.CompanyContentFilter.FirstOrDefault(x => x.Value.Any(y => cardTitle.Contains(y))).Key;

        if (string.IsNullOrWhiteSpace(filterKey))
            return null;

        var cardContent = divNode.SelectNodes($".//div")?[0].InnerHtml;

        if (string.IsNullOrWhiteSpace(cardContent))
            return null;

        cardContent = cardContent.Trim();
        cardContent = Regex.Replace(cardContent, @"\s{2,}", Environment.NewLine);
        cardContent = cardContent.Replace("<br>", Environment.NewLine);
        cardContent = cardContent.Replace("<div>", "").Replace("</div>", Environment.NewLine);


        return new KeyValuePair<string, string>(filterKey, cardContent);
    }

    public HtmlNodeCollection? GetCompanyCardContentNodes(HtmlDocument htmlDoc)
    {
        var nodeCollection = new HtmlNodeCollection(null);

        var companyContentNodes = htmlDoc.DocumentNode.SelectNodes($"//div[contains(@class,'{Parameters1111.CompanyCardContentDivClassName}')]")?[0];

        if (companyContentNodes == null)
            return null;

        var dic = new Dictionary<string, string>();
        var currentH2 = "";


        var allChildNodes = companyContentNodes.ChildNodes.Where(x => !x.Name.Trim().StartsWith("#")).ToArray();

        foreach (var node in allChildNodes)
        {
            if (node.Name == "h2")
            {
                currentH2 = node.InnerText;
                continue;
            }

            if (node.Name == "div")
            {
                if (string.IsNullOrWhiteSpace(currentH2))
                    continue;
                if (!dic.ContainsKey(currentH2))
                {
                    dic.Add(currentH2, node.InnerText.Trim());
                }
                continue;
            }
        }

        foreach (var item in dic)
        {
            nodeCollection.Add(HtmlNode.CreateNode($"<div><h2>{item.Key}</h2><div>{item.Value}</div></div>"));
        }

        return nodeCollection.Count == 0 ? null : nodeCollection;
    }

    public string? GetCompanyName(HtmlDocument htmlDoc)
    {
        var companyNameNode = htmlDoc.DocumentNode.SelectNodes($"//div[contains(@class,'{Parameters1111.CompanyNameDivName}')]//h1")?[0];

        if (companyNameNode == null)
        {
            logger.LogError($"{nameof(HtmlAnalyze1111Service)} GetCompanyName error.");
            return null;
        }

        return companyNameNode.InnerText;
    }

    public string? GetJobCardContent(HtmlNode htmlNode)
    {
        throw new NotImplementedException();
    }

    public HtmlNodeCollection? GetJobCardContentNodes(HtmlDocument htmlDoc)
    {
        throw new NotImplementedException();
    }

    public string? GetJobCardTitle(HtmlNode htmlNode)
    {
        throw new NotImplementedException();
    }

    public string? GetJobLastUpdateTime(HtmlDocument htmlDoc)
    {
        var updateTimeNode = htmlDoc.DocumentNode.SelectNodes($"//small[contains(@class, '{Parameters1111.JobLastUpdateTimeSmallClass}')]")?[0].InnerText;
        return updateTimeNode;
    }

    public HtmlNodeCollection? GetJobListCardContentNode(HtmlDocument htmlDoc)
    {
        var cardContentNodes = htmlDoc.DocumentNode.SelectNodes($"//div[contains(@class,'{Parameters1111.JobListCardDivClassName}')]");

        if (cardContentNodes is null || cardContentNodes.Count == 0)
            return null;

        var result = new HtmlNodeCollection(null);

        foreach (var htmlNode in cardContentNodes)
        {
            var jobLinkNode = htmlNode.SelectNodes($"//div[contains(@class,'{Parameters1111.JobListJobDivClassName}')]//a")?[0];
            var companyLinkNode = htmlNode.SelectNodes($"//div[contains(@class,'{Parameters1111.JobListCompanyDivClassName}')]//a")?[0];

            if (jobLinkNode == null || companyLinkNode == null)
                continue;

            var jobLink = jobLinkNode.Attributes["href"]?.Value;
            var companyLink = companyLinkNode.Attributes["href"]?.Value;

            if (string.IsNullOrWhiteSpace(jobLink) || string.IsNullOrWhiteSpace(companyLink))
                continue;

            result.Add(HtmlNode.CreateNode($"<div><a href='{jobLink}' class='{Parameters1111.JobListJobAClass}'>job</a><a class='{Parameters1111.JobListCompanyAClass}' href='{companyLink}'>company</a></div>"));
        }

        return result;
    }

    public HtmlNode? GetJobListCompanyNode(HtmlNode htmlNode)
    {
        var companyNode = htmlNode.SelectNodes($"//a[contains(@class,'{Parameters1111.JobListCompanyAClass}')]")?[0];

        return companyNode;
    }

    public HtmlNode? GetJobListJobNode(HtmlNode htmlNode)
    {
        var jobNode = htmlNode.SelectNodes($"//a[contains(@class,'{Parameters1111.JobListJobAClass}')]")?[0];

        return jobNode;
    }

    public string? GetJobName(HtmlDocument htmlDoc)
    {
        var jobName = htmlDoc.DocumentNode.SelectNodes($"//h1[contains(@class, '{Parameters1111.JobTitleH1ClassName}')]")?[0].InnerText.Trim();

        if (string.IsNullOrWhiteSpace(jobName))
        {
            logger.LogWarning($"{nameof(HtmlAnalyze1111Service)} Job info title get null.");
            return null;
        }

        return jobName;
    }

    public string? GetJobPlace(HtmlDocument htmlDoc)
    {
        var jobPlaceIconNode = htmlDoc.DocumentNode.SelectNodes($"//div[contains(@class, '{Parameters1111.JobPlaceIconDivClass}')]")?[0];

        if (jobPlaceIconNode == null)
            return null;

        var jobPlaceParentNode = jobPlaceIconNode.ParentNode;

        if (jobPlaceParentNode == null)
            return null;

        var jobPlace = jobPlaceParentNode.SelectNodes($".//span[contains(@class, '{Parameters1111.JobPlaceSpanClass}')]")?[0].InnerText;

        return jobPlace;
    }

    public string? GetOtherRequirement(HtmlDocument htmlDoc)
    {
        var otherRequirementHtml = htmlDoc.DocumentNode.SelectNodes($"//div[contains(@class, '{Parameters1111.JobOtherRequirementDivClass}')]//div[contains(@class, '{Parameters1111.JobOtherRequirementConditionDivClass}')]//div[contains(@class, '{Parameters1111.JobOtherRequirementContentDivClass}')]")?[0]?.InnerHtml;

        if (otherRequirementHtml == null)
            return null;

        otherRequirementHtml = otherRequirementHtml.Trim();

        otherRequirementHtml = Regex.Replace(otherRequirementHtml, "<br>+", Environment.NewLine);
        otherRequirementHtml = Regex.Replace(otherRequirementHtml, $"{Environment.NewLine}+", Environment.NewLine);

        return otherRequirementHtml;
    }

    public string? GetSalary(HtmlDocument htmlDoc)
    {
        var salaryRegion = htmlDoc.DocumentNode.SelectNodes($"//div[contains(@class, '{Parameters1111.JobSalaryRegionDivClass}')]")?[0];

        if (salaryRegion == null)
            return null;

        var salaryNode = salaryRegion.SelectNodes($"//span[contains(@class, '{Parameters1111.JobSalarySpanClass}')]")?[0].ChildNodes.Where(x => x.Name == "#text");

        if (salaryNode == null)
            return null;

        return string.Join("", salaryNode.Select(x => x.InnerText));
    }

    public string? GetWorkContent(HtmlDocument htmlDoc)
    {
        var workContent = htmlDoc.DocumentNode.SelectNodes($"//div[contains(@class, '{Parameters1111.JobWorkContentDivClass}')]")?[0];

        if (workContent == null)
            return null;

        var contentNode = workContent.SelectNodes($".//div[contains(@class, '{Parameters1111.JobWorkContentChildDivClass}')]")?[0];

        if (contentNode == null)
            return null;

        var html = contentNode.InnerHtml.Trim();

        html = html.Replace("<p>", "").Replace("</p>", Environment.NewLine);

        html = Regex.Replace(html, $"{Environment.NewLine}+", Environment.NewLine);

        return html;
    }
}
