using HtmlAgilityPack;
using Model.Dto1111;
using Service.HtmlAnalyze;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

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
        throw new NotImplementedException();
    }

    public HtmlNodeCollection? GetCompanyCardContentNodes(HtmlDocument htmlDoc)
    {
        throw new NotImplementedException();
    }

    public string? GetCompanyName(HtmlDocument htmlDoc)
    {
        throw new NotImplementedException();
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
        throw new NotImplementedException();
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
        throw new NotImplementedException();
    }

    public string? GetJobPlace(HtmlDocument htmlDoc)
    {
        throw new NotImplementedException();
    }

    public string? GetSalary(HtmlDocument htmlDoc)
    {
        throw new NotImplementedException();
    }
}
