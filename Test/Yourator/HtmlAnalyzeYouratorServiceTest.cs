using Crawer_Yourator.Service;
using HtmlAgilityPack;
using Model.DtoYourator;
using Service.HtmlAnalyze;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Test.Yourator;

public class HtmlAnalyzeYouratorServiceTest
{
    private  ILogger<HtmlAnalyzeYouratorService> logger;
    private  IHtmlAnalyzeService HtmlAnalyzeYouratorService;

    [SetUp]
    public void Setup()
    {
        logger = A.Fake<ILogger<HtmlAnalyzeYouratorService>>();
        HtmlAnalyzeYouratorService = new HtmlAnalyzeYouratorService(logger);
    }

    [Test]
    public void GetCompanyName_HtmlDoc_GetStringEmpty()
    {
        var htmlDoc = new HtmlDocument();

        var result = HtmlAnalyzeYouratorService.GetCompanyName(htmlDoc);

        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetCompanyName_HtmlDoc_不合規則()
    {
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml("<h1>test</h1>");

        var result = HtmlAnalyzeYouratorService.GetCompanyName(htmlDoc);

        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetCompanyName_HtmlDoc_符合規則()
    {
        var content = "test";
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml($"<h1 class='flex-initial truncate'>{content}</h1>");

        var result = HtmlAnalyzeYouratorService.GetCompanyName(htmlDoc);

        Assert.That(result, Is.EqualTo(content));
    }

    [Test]
    public void GetCompanyCardContentNodes_NoContent_GetNull()
    {
        var htmlDoc = new HtmlDocument();

        var result = HtmlAnalyzeYouratorService.GetCompanyCardContentNodes(htmlDoc);
        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetCompanyCardContentNodes_NoH2_GetNull1()
    {
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml($"<div class='company__content'><section>section</section></div>");

        var result = HtmlAnalyzeYouratorService.GetCompanyCardContentNodes(htmlDoc);
        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetCompanyCardContentNodes_NoSection_GetNull1()
    {
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml($"<div class='company__content'><h2>h2</h2></div>");

        var result = HtmlAnalyzeYouratorService.GetCompanyCardContentNodes(htmlDoc);
        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetCompanyCardContentNodes_NodesCountEqual_Getdata()
    {
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml($"<div class='company__content'><section class='{ParametersYourator.CardContentSectionValidClassName}'>section1</section><h2>section</h2><section class='{ParametersYourator.CardContentSectionValidClassName}'>section2</section><h2>h22</h2></div>");

        var result = HtmlAnalyzeYouratorService.GetCompanyCardContentNodes(htmlDoc);
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void GetCompanyCardContentNodes_H2ClassNotValid_GetNull()
    {
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml($"<div class='company__content'><section class='content__area'>section1</section><h2 class='{ParametersYourator.CompanyCardContentH2NotAllowClassName}'>section</h2></div>");

        var result = HtmlAnalyzeYouratorService.GetCompanyCardContentNodes(htmlDoc);
        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetCompanyCardContentNodes_SectionClassNotValid_GetNull()
    {
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml($"<div class='company__content'><section class='content'>section1</section><h2>section</h2></div>");

        var result = HtmlAnalyzeYouratorService.GetCompanyCardContentNodes(htmlDoc);
        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetCompanyCardContent_NoH2_GetNull()
    {
        var node = HtmlNode.CreateNode($"<div><section>section1</section>></div>");

        var result = HtmlAnalyzeYouratorService.GetCompanyCardContent(node);
        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetCompanyCardContent_H2NotMatch()
    {
        var node = HtmlNode.CreateNode($"<div><h2>h21</h2>></div>");

        var result = HtmlAnalyzeYouratorService.GetCompanyCardContent(node);
        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetCompanyCardContent_NoSection()
    {
        var node = HtmlNode.CreateNode($"<div><h2>{ParametersYourator.CompanyContentFilter.First().Value[0]}</h2>></div>");

        var result = HtmlAnalyzeYouratorService.GetCompanyCardContent(node);
        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetCompanyCardContent_GetValue()
    {
        var section = "section";
        var node = HtmlNode.CreateNode($"<div><h2>{ParametersYourator.CompanyContentFilter.First().Value[0]}</h2>><section>{section}</section></div>");

        var result = HtmlAnalyzeYouratorService.GetCompanyCardContent(node);
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.Value.Key, Is.EqualTo(ParametersYourator.CompanyContentFilter.First().Key));
            Assert.That(result.Value.Value, Is.EqualTo(section));
        });
    }

    [Test]
    public void GetJobName_NotValidClassName()
    {
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml("<div><h1>test</h1></div>");

        var result = HtmlAnalyzeYouratorService.GetJobName(htmlDoc);
        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetJobName_ValidClassName()
    {
        var content = "fake data";
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml($"<div><h1 class='{ParametersYourator.JobTitleClassName}'>{content}</h1></div>");

        var result = HtmlAnalyzeYouratorService.GetJobName(htmlDoc);
        Assert.That(result, Is.EqualTo(content));
    }

    [Test]
    public void GetJobPlace_NotValidClassName()
    {
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml("<p><a>test</a></p>");

        var result = HtmlAnalyzeYouratorService.GetJobPlace(htmlDoc);
        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetJobPlace_ValidClassName_GetOneJobPlace()
    {
        var content = "fake data";
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml($"<div><p class='{ParametersYourator.JobPlaceClassName}'><a>{content}</a></p></div>");

        var result = HtmlAnalyzeYouratorService.GetJobPlace(htmlDoc);
        Assert.That(result, Is.EqualTo(content));
    }

    [Test]
    public void GetJobPlace_ValidClassName_GetMultiJobPlace()
    {
        var content = new string[] { "fake data1", "fake data2" };
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml($"<div><p class='{ParametersYourator.JobPlaceClassName}'><a>{content[0]}</a></p><p class='{ParametersYourator.JobPlaceClassName}'><a>{content[1]}</a></p></div>");

        var result = HtmlAnalyzeYouratorService.GetJobPlace(htmlDoc);
        Assert.That(result, Is.EqualTo(string.Join(' ', content)));
    }

    [Test]
    public void GetJobLastUpdateTime_NotValidClassName()
    {
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml("<p>fake data</p>");

        var result = HtmlAnalyzeYouratorService.GetJobLastUpdateTime(htmlDoc);
        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetJobLastUpdateTime_ValidClassName()
    {
        var content = "fake data";
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml($"<p class='{ParametersYourator.JobLastUpdateTimeClassName}'>{content}</p>");

        var result = HtmlAnalyzeYouratorService.GetJobLastUpdateTime(htmlDoc);
        Assert.That(result, Is.EqualTo(content));
    }

    [Test]
    public void GetJobCardContentNodes_NotValidDivClassName()
    {
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml("<div>fake data</p>");

        var result = HtmlAnalyzeYouratorService.GetJobCardContentNodes(htmlDoc);
        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetJobCardContentNodes_NoH2()
    {
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml($"<div class='{ParametersYourator.JobCardContentClassName}'><section class='{ParametersYourator.CardContentSectionValidClassName}'></section></p>");

        var result = HtmlAnalyzeYouratorService.GetJobCardContentNodes(htmlDoc);
        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetJobCardContentNodes_NoSection()
    {
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml($"<div class='{ParametersYourator.JobCardContentClassName}'><h2 class='{ParametersYourator.JobCardContentH2ClassName}'></h2></p>");

        var result = HtmlAnalyzeYouratorService.GetJobCardContentNodes(htmlDoc);
        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetJobCardContentNodes_SectionNotValid()
    {
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml($"<div class='{ParametersYourator.JobCardContentClassName}'><h2 class='{ParametersYourator.JobCardContentH2ClassName}'></h2><section class='{ParametersYourator.CardContentSectionValidClassName} {ParametersYourator.JobCardContentSectionNotValidClassName}'></section></p>");

        var result = HtmlAnalyzeYouratorService.GetJobCardContentNodes(htmlDoc);
        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetJobCardContentNodes_H2AndSectionCountNotMatch()
    {
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml($"<div class='{ParametersYourator.JobCardContentClassName}'><h2 class='{ParametersYourator.JobCardContentH2ClassName}'></h2><section class='{ParametersYourator.CardContentSectionValidClassName}'></section><section class='{ParametersYourator.CardContentSectionValidClassName}'></section></p>");

        var result = HtmlAnalyzeYouratorService.GetJobCardContentNodes(htmlDoc);
        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetJobCardContentNodes_H2AndSectionCountMatch_withNotAllowSection()
    {
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml($"<div class='{ParametersYourator.JobCardContentClassName}'><h2 class='{ParametersYourator.JobCardContentH2ClassName}'></h2><section class='{ParametersYourator.CardContentSectionValidClassName}'></section><section class='{ParametersYourator.CardContentSectionValidClassName} {ParametersYourator.JobCardContentSectionNotValidClassName}'></section></p>");

        var result = HtmlAnalyzeYouratorService.GetJobCardContentNodes(htmlDoc);
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void GetJobCardContentNodes_H2AndSectionCountMatch()
    {
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml($"<div class='{ParametersYourator.JobCardContentClassName}'><h2 class='{ParametersYourator.JobCardContentH2ClassName}'></h2><section class='{ParametersYourator.CardContentSectionValidClassName}'></section></p>");

        var result = HtmlAnalyzeYouratorService.GetJobCardContentNodes(htmlDoc);
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void GetJobCardContentNodes_ValidData()
    {
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml($"<div class='{ParametersYourator.JobCardContentClassName}'><h2 class='{ParametersYourator.JobCardContentH2ClassName}'></h2><section class='{ParametersYourator.CardContentSectionValidClassName}'></section></p>");

        var result = HtmlAnalyzeYouratorService.GetJobCardContentNodes(htmlDoc);
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void GetJobCardTitle_NoH2()
    {
        var node = HtmlNode.CreateNode("<div></div>");

        var result = HtmlAnalyzeYouratorService.GetJobCardTitle(node);
        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetJobCardTitle_GetH2InnerText()
    {
        var content = "fake data";
        var node = HtmlNode.CreateNode($"<div><h2>{content}</h2></div>");

        var result = HtmlAnalyzeYouratorService.GetJobCardTitle(node);
        Assert.That(result, Is.EqualTo(content));
    }

    [Test]
    public void GetJobCardContent_NoSection()
    {
        var node = HtmlNode.CreateNode("<div></div>");

        var result = HtmlAnalyzeYouratorService.GetJobCardContent(node);
        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetJobCardContent_GetSectionInnerText()
    {
        var content = "fake data";
        var node = HtmlNode.CreateNode($"<div><section>{content}</section></div>");

        var result = HtmlAnalyzeYouratorService.GetJobCardContent(node);
        Assert.That(result, Is.EqualTo(content));
    }
}
