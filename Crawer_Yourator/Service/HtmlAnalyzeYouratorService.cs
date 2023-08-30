using HtmlAgilityPack;
using Model.DtoYourator;
using Service.HtmlAnalyze;

namespace Crawer_Yourator.Service;

public class HtmlAnalyzeYouratorService : IHtmlAnalyzeService
{
    private readonly ILogger<HtmlAnalyzeYouratorService> logger;

    public HtmlAnalyzeYouratorService(ILogger<HtmlAnalyzeYouratorService> logger)
    {
        this.logger = logger;
    }

    public KeyValuePair<string, string>? GetCompanyCardContent(HtmlNode htmlNode)
    {
        // 內文的標題
        var cardTitle = htmlNode.SelectNodes($".//h2")?[0].InnerText.Trim();

        if (string.IsNullOrWhiteSpace(cardTitle))
            return null;

        var filterKey = ParametersYourator.CompanyContentFilter.FirstOrDefault(x => x.Value.Any(y => cardTitle.Contains(y))).Key;

        if (string.IsNullOrWhiteSpace(filterKey))
            return null;

        // 內文的內容
        var cardContent = htmlNode.SelectNodes($".//section")?[0].InnerText.Trim();

        if (string.IsNullOrWhiteSpace(cardContent))
            return null;

        return new KeyValuePair<string, string>(filterKey, cardContent);
    }

    public HtmlNodeCollection? GetCompanyCardContentNodes(HtmlDocument htmlDoc)
    {
        var nodeCollection = new HtmlNodeCollection(null);

        var companyContentNodes = htmlDoc.DocumentNode.SelectNodes($"//div[{ParametersYourator.CompanyCardContentSelector}]")?[0];

        if (companyContentNodes == null)
        {
            logger.LogWarning($"{nameof(HtmlAnalyzeYouratorService)} Get company card content fail.");
            return null;
        }

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

            if (node.Name == "section")
            {
                if (string.IsNullOrWhiteSpace(currentH2))
                    continue;
                if (!dic.ContainsKey(currentH2))
                    dic.Add(currentH2, node.InnerText);

                continue;
            }
        }

        foreach (var item in dic)
        {
            nodeCollection.Add(HtmlNode.CreateNode($"<div><h2>{item.Key}</h2><section>{item.Value}</section></div>"));
        }

        return nodeCollection.Count == 0 ? null : nodeCollection;
        //var htmlNodes = new HtmlNodeCollection(null);

        //var h2Nodes = cardContent.SelectNodes($".//h2[not(@class='{ParametersYourator.CompanyCardContentH2NotAllowClassName}')]");
        //var sectionNodes = cardContent.SelectNodes($".//section[contains(@class, '{ParametersYourator.CardContentSectionValidClassName}')]");

        //if (h2Nodes == null || sectionNodes == null)
        //{
        //    logger.LogWarning($"{nameof(HtmlAnalyzeYouratorService)} Get company card content fail.h2Nodes or sectionNodes is null");
        //    return null;
        //}

        //if (h2Nodes.Count != sectionNodes.Count)
        //{
        //    logger.LogWarning($"{nameof(HtmlAnalyzeYouratorService)} Get company card content fail.nodes count not equal");
        //    return null;
        //}

        //for (int i = 0; i < h2Nodes.Count; i++)
        //{
        //    HtmlNode node = HtmlNode.CreateNode($"<div><h2>{h2Nodes[i].InnerText}</h2><section>{sectionNodes[i].InnerText}</section></div>");

        //    htmlNodes.Add(node);
        //}

        //return htmlNodes;
    }

    public string? GetCompanyName(HtmlDocument htmlDoc)
    {
        var compTitle = htmlDoc.DocumentNode.SelectNodes($"//h1[{ParametersYourator.CompanyNameH1Selector}]")?[0].InnerText.Trim();

        if (string.IsNullOrWhiteSpace(compTitle))
        {
            logger.LogWarning($"{nameof(HtmlAnalyzeYouratorService)} Get company name fail.");
            return null;
        }

        return compTitle;
    }

    public string? GetJobCardContent(HtmlNode htmlNode)
    {
        return htmlNode.SelectNodes($".//section")?[0].InnerText.Trim();
    }

    public HtmlNodeCollection? GetJobCardContentNodes(HtmlDocument htmlDoc)
    {
        var cardContent = htmlDoc.DocumentNode.SelectNodes($"//div[contains(@class,'{ParametersYourator.JobCardContentClassName}')]")?[0];
        if (cardContent == null)
        {
            logger.LogWarning($"{nameof(HtmlAnalyzeYouratorService)} Get job card content fail.");
            return null;
        }

        var htmlNodes = new HtmlNodeCollection(null);

        var h2Nodes = cardContent.SelectNodes($".//h2[contains(@class,'{ParametersYourator.JobCardContentH2ClassName}')]");
        var sectionNodes = cardContent.SelectNodes($".//section[contains(@class, '{ParametersYourator.CardContentSectionValidClassName}') and not(contains(@class, '{ParametersYourator.JobCardContentSectionNotValidClassName}'))]");

        if (h2Nodes == null || sectionNodes == null)
        {
            logger.LogWarning($"{nameof(HtmlAnalyzeYouratorService)} Get job card content fail.h2Nodes or sectionNodes is null");
            return null;
        }

        if (h2Nodes.Count != sectionNodes.Count)
        {
            logger.LogWarning($"{nameof(HtmlAnalyzeYouratorService)} Get job card content fail.nodes count not equal");
            return null;
        }


        for (int i = 0; i < h2Nodes.Count; i++)
        {
            HtmlNode node = HtmlNode.CreateNode($"<div><h2>{h2Nodes[i].InnerText}</h2><section>{sectionNodes[i].InnerText}</section></div>");

            htmlNodes.Add(node);
        }

        return htmlNodes;
    }

    public string? GetJobCardTitle(HtmlNode htmlNode)
    {
        return htmlNode.SelectNodes($".//h2")?[0].InnerText.Trim();
    }

    public string? GetJobLastUpdateTime(HtmlDocument htmlDoc)
    {
        var jobLastUpdateTime = htmlDoc.DocumentNode.SelectNodes($"//p[contains(@class,'{ParametersYourator.JobLastUpdateTimeClassName}')]")?[0].InnerText.Trim();

        if (string.IsNullOrWhiteSpace(jobLastUpdateTime))
        {
            logger.LogWarning($"{nameof(HtmlAnalyzeYouratorService)} Job info title get null.");
            return null;
        }

        return jobLastUpdateTime;
    }

    public HtmlNodeCollection? GetJobListCardContentNode(HtmlDocument htmlDoc)
    {
        throw new NotImplementedException();
    }

    public HtmlNode? GetJobListCompanyNode(HtmlNode htmlNode)
    {
        throw new NotImplementedException();
    }

    public HtmlNode? GetJobListJobNode(HtmlNode htmlNode)
    {
        throw new NotImplementedException();
    }

    public string? GetJobName(HtmlDocument htmlDoc)
    {
        var jobName = htmlDoc.DocumentNode.SelectNodes($"//h1[@class='{ParametersYourator.JobTitleClassName}']")?[0].InnerText.Trim();

        if (string.IsNullOrWhiteSpace(jobName))
        {
            logger.LogWarning($"{nameof(HtmlAnalyzeYouratorService)} Job info title get null.");
            return null;
        }

        return jobName;
    }

    public string? GetJobPlace(HtmlDocument htmlDoc)
    {
        var jobPlaceNodes = htmlDoc.DocumentNode.SelectNodes($"//p[contains(@class, '{ParametersYourator.JobPlaceClassName}')]");

        if (jobPlaceNodes == null)
        {
            logger.LogWarning($"{nameof(HtmlAnalyzeYouratorService)} Job info place get null.");
            return null;
        }

        var result = new List<string>();

        foreach (var node in jobPlaceNodes)
        {
            var jobPlace = node.SelectNodes(".//a")?[0].InnerText.Trim();
            if (string.IsNullOrWhiteSpace(jobPlace))
                continue;

            result.Add(jobPlace);
        }

        return string.Join(' ', result);
    }

    public string? GetOtherRequirement(HtmlDocument htmlDoc)
    {
        throw new NotImplementedException();
    }

    public string? GetSalary(HtmlDocument htmlDoc)
    {
        throw new NotImplementedException();
    }

    public string? GetWorkContent(HtmlDocument htmlDoc)
    {
        throw new NotImplementedException();
    }
}
