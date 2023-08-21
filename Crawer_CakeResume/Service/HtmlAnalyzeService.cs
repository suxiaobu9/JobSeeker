using HtmlAgilityPack;
using Model.DtoCakeResume;
using System;

namespace Crawer_CakeResume.Service;

public class HtmlAnalyzeService
{
    private readonly ILogger<HtmlAnalyzeService> logger;

    public HtmlAnalyzeService(ILogger<HtmlAnalyzeService> logger)
    {
        this.logger = logger;
    }

    /// <summary>
    /// 取得公司名稱
    /// </summary>
    /// <param name="htmlDoc"></param>
    /// <returns></returns>
    public string? GetCompanyName(HtmlDocument htmlDoc)
    {
        var compTitle = htmlDoc.DocumentNode.SelectNodes($"//div[@class='{ParametersCakeResume.CompanyNameDivClass}']")?[0].InnerText.Trim();

        if (string.IsNullOrWhiteSpace(compTitle))
        {
            logger.LogWarning($"{nameof(HtmlAnalyzeService)} Get company name fail.");
            return null;
        }

        return compTitle;
    }

    /// <summary>
    /// 取得公司介紹卡片節點
    /// </summary>
    /// <param name="htmlDoc"></param>
    /// <returns></returns>
    public HtmlNodeCollection? GetCompanyCardContentNodes(HtmlDocument htmlDoc)
    {
        var nodes = htmlDoc.DocumentNode.SelectNodes($"//div[contains(@class, '{ParametersCakeResume.CompanyInfoDivClass}')]");

        if (nodes == null || nodes.Count == 0)
        {
            logger.LogWarning($"{nameof(HtmlAnalyzeService)} GetCompanyCardContentNodes fail.");
            return null;
        }

        return nodes;
    }

    /// <summary>
    /// 取得公司介紹卡片內容
    /// </summary>
    /// <param name="htmlDoc"></param>
    /// <returns></returns>
    public KeyValuePair<string, string>? GetCompanyCardContent(HtmlNode htmlNode)
    {
        // 內文的標題
        var cardTitle = htmlNode.SelectNodes($".//h2[contains(@class, '{ParametersCakeResume.CompanyCardH2Class}')]")?[0].InnerText.Trim();

        if (string.IsNullOrWhiteSpace(cardTitle))
            return null;

        var filterKey = ParametersCakeResume.CompanyContentFilter.FirstOrDefault(x => x.Value.Any(y => cardTitle.Contains(y))).Key;

        if (string.IsNullOrWhiteSpace(filterKey))
            return null;

        // 內文的內容
        var cardContent = htmlNode.SelectNodes($".//div[contains(@class, '{ParametersCakeResume.CompanyCardDivClass}')]")?[0].InnerText.Trim();

        if (string.IsNullOrWhiteSpace(cardContent))
            return null;

        return new KeyValuePair<string, string>(filterKey, cardContent);
    }

    /// <summary>
    /// 取得職缺名稱
    /// </summary>
    /// <param name="htmlDoc"></param>
    /// <returns></returns>
    public string? GetJobName(HtmlDocument htmlDoc)
    {
        var jobName = htmlDoc.DocumentNode.SelectNodes($"//h2[@class='{ParametersCakeResume.JobNameDivClass}']")?[0].InnerText.Trim();

        if (string.IsNullOrWhiteSpace(jobName))
        {
            logger.LogWarning($"{nameof(HtmlAnalyzeService)} Job info title get null.");
            return null;
        }

        return jobName;
    }

    /// <summary>
    /// 取得工作地點
    /// </summary>
    /// <param name="htmlDoc"></param>
    /// <returns></returns>
    public string? GetJobPlace(HtmlDocument htmlDoc) => htmlDoc.DocumentNode.SelectNodes($"//a[@class='{ParametersCakeResume.JobPlaceAClass}']")?[0].InnerText.Trim();

    /// <summary>
    /// 取得薪水
    /// </summary>
    /// <param name="htmlDoc"></param>
    /// <returns></returns>
    public string? GetSalary(HtmlDocument htmlDoc) => htmlDoc.DocumentNode.SelectNodes($"//div[@class='{ParametersCakeResume.SalaryDivClass}']")?[0].InnerText.Trim();

    /// <summary>
    /// 取得最後更新時間
    /// </summary>
    public string? GetJobLastUpdateTime(HtmlDocument htmlDoc) => 
        htmlDoc.DocumentNode.SelectNodes($"//div[@class='{ParametersCakeResume.LatestUpdateDateOuterDivClass}']")?[0]
                            .SelectNodes($"//div[@class='{ParametersCakeResume.LatestUpdateDateDivClass}']")?[0].InnerText;

    /// <summary>
    /// 取得職缺內容節點
    /// </summary>
    /// <param name="htmlDoc"></param>
    /// <returns></returns>
    public HtmlNodeCollection? GetJobCardContentNodes(HtmlDocument htmlDoc) => htmlDoc.DocumentNode.SelectNodes($"//div[contains(@class, '{ParametersCakeResume.JobCardContentDivClass}')]");

    /// <summary>
    /// 取得職缺卡片內文標題
    /// </summary>
    /// <param name="htmlNode"></param>
    /// <returns></returns>
    public string? GetJobCardTitle(HtmlNode htmlNode) => htmlNode.SelectNodes($".//h3[contains(@class, '{ParametersCakeResume.JobCardInnerTitleH3ClassName}')]")?[0].InnerText.Trim();

    /// <summary>
    /// 取得職缺卡片內文
    /// </summary>
    /// <param name="htmlNode"></param>
    /// <returns></returns>
    public string? GetJobCardContent(HtmlNode htmlNode) => htmlNode.SelectNodes($".//div[contains(@class, '{ParametersCakeResume.JobCardInnerContentDivClass}')]")?[0].InnerText.Trim();

    /// <summary>
    /// 取得職缺清單內容傑點
    /// </summary>
    /// <param name="htmlDoc"></param>
    /// <returns></returns>
    public HtmlNodeCollection? GetJobListCardContentNode(HtmlDocument htmlDoc) => htmlDoc.DocumentNode.SelectNodes($"//div[contains(@class, '{ParametersCakeResume.JobListCardContentDivClassName}')]");

    /// <summary>
    /// 取得職缺清單公司傑點
    /// </summary>
    /// <param name="htmlDoc"></param>
    /// <returns></returns>
    public HtmlNode? GetJobListCompanyNode(HtmlNode htmlNode) => htmlNode.SelectNodes($".//a[contains(@class, '{ParametersCakeResume.JobListCompanyNodeAClassName}')]")?[0];

    /// <summary>
    /// 取得職缺清單職缺傑點
    /// </summary>
    /// <param name="htmlDoc"></param>
    /// <returns></returns>
    public HtmlNode? GetJobListJobNode(HtmlNode htmlNode) => htmlNode.SelectNodes($".//a[contains(@class, '{ParametersCakeResume.JobListJobNodeAClassName}')]")?[0];
}
