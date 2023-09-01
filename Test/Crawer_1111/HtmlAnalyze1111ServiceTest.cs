using Crawer_1111.Service;
using HtmlAgilityPack;
using Model.Dto1111;

namespace Test.Crawer_1111;

public class HtmlAnalyze1111ServiceTest
{
    private readonly ILogger<HtmlAnalyze1111Service> logger = A.Fake<ILogger<HtmlAnalyze1111Service>>();
    private readonly string TestUrl = "https://www.example.com";

    [Test]
    public void GetJobListCardContentNode_HtmlDoc_NotContainTargetHtmlTage()
    {
        var html = "<div></div>";
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(html);

        var service = new HtmlAnalyze1111Service(logger);

        var result = service.GetJobListCardContentNode(htmlDoc);

        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetJobListCardContentNode_HtmlDoc_ContainCompanyAndJobHref()
    {
        var html = $"<div class='{Parameters1111.JobListCardDivClassName}'><div class='{Parameters1111.JobListJobDivClassName}'><a href='{TestUrl}'></a><div class='{Parameters1111.JobListCompanyDivClassName}'><a href='{TestUrl}'></a></div></div></div>";
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(html);

        var service = new HtmlAnalyze1111Service(logger);

        var result = service.GetJobListCardContentNode(htmlDoc);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Has.Count.EqualTo(1));
    }

    [Test]
    public void GetJobListCardContentNode_HtmlDoc_NoJobHref()
    {
        var html = $"<div class='{Parameters1111.JobListCardDivClassName}'><div class='{Parameters1111.JobListJobDivClassName}'><a></a><div class='{Parameters1111.JobListCompanyDivClassName}'><a href='{TestUrl}'></a></div></div></div>";
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(html);

        var service = new HtmlAnalyze1111Service(logger);

        var result = service.GetJobListCardContentNode(htmlDoc);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Has.Count.EqualTo(0));
    }

    [Test]
    public void GetJobListCardContentNode_HtmlDoc_NoCompanyHref()
    {
        var html = $"<div class='{Parameters1111.JobListCardDivClassName}'><div class='{Parameters1111.JobListJobDivClassName}'><a href='{TestUrl}'></a><div class='{Parameters1111.JobListCompanyDivClassName}'><a></a></div></div></div>";
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(html);

        var service = new HtmlAnalyze1111Service(logger);

        var result = service.GetJobListCardContentNode(htmlDoc);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Has.Count.EqualTo(0));
    }

    [Test]
    public void GetJobListCardContentNode_HtmlDoc_NoJobDiv()
    {
        var html = $"<div class='{Parameters1111.JobListCardDivClassName}'><div class='{Parameters1111.JobListCompanyDivClassName}'><a href='{TestUrl}'></a></div></div>";
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(html);

        var service = new HtmlAnalyze1111Service(logger);

        var result = service.GetJobListCardContentNode(htmlDoc);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Has.Count.EqualTo(0));
    }

    [Test]
    public void GetJobListCardContentNode_HtmlDoc_NoCompanyDiv()
    {
        var html = $"<div class='{Parameters1111.JobListCardDivClassName}'><div class='{Parameters1111.JobListJobDivClassName}'><a href='{TestUrl}'></a></div></div></div>";
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(html);

        var service = new HtmlAnalyze1111Service(logger);

        var result = service.GetJobListCardContentNode(htmlDoc);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Has.Count.EqualTo(0));
    }

    [Test]
    public void GetJobListJobNode_NotContainTargetHtml()
    {
        var html = "<div><a></a></div>";

        var htmlNode = HtmlNode.CreateNode(html);

        var service = new HtmlAnalyze1111Service(logger);

        var result = service.GetJobListJobNode(htmlNode);

        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetJobListJobNode_ContainTargetHtml()
    {
        var html = $"<div><a class='{Parameters1111.JobListJobAClass}'></a></div>";

        var htmlNode = HtmlNode.CreateNode(html);

        var service = new HtmlAnalyze1111Service(logger);

        var result = service.GetJobListJobNode(htmlNode);

        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void GetCompanyListJobNode_NotContainTargetHtml()
    {
        var html = "<div><a></a></div>";

        var htmlNode = HtmlNode.CreateNode(html);

        var service = new HtmlAnalyze1111Service(logger);

        var result = service.GetJobListCompanyNode(htmlNode);

        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetJobListCompanyNode_ContainTargetHtml()
    {
        var html = $"<div><a class='{Parameters1111.JobListCompanyAClass}'></a></div>";

        var htmlNode = HtmlNode.CreateNode(html);

        var service = new HtmlAnalyze1111Service(logger);

        var result = service.GetJobListCompanyNode(htmlNode);

        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void GetCompanyName_NoTargetHtml()
    {
        var html = $"<div></div>";

        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(html);

        var service = new HtmlAnalyze1111Service(logger);

        var result = service.GetCompanyName(htmlDoc);

        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetCompanyName_NotValidClassName()
    {
        var content = "fake data";
        var html = $"<div class='fake_class'><h1>{content}</h1></div>";

        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(html);

        var service = new HtmlAnalyze1111Service(logger);

        var result = service.GetCompanyName(htmlDoc);

        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetCompanyName_HasTargetHtml()
    {
        var content = "fake data";
        var html = $"<div class='{Parameters1111.CompanyNameDivName}'><h1>{content}</h1></div>";

        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(html);

        var service = new HtmlAnalyze1111Service(logger);

        var result = service.GetCompanyName(htmlDoc);

        Assert.That(result, Is.EqualTo(content));
    }

    [Test]
    public void GetCompanyCardContentNodes_NoCardContentDivName()
    {
        var html = $"<div></div>";
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(html);

        var service = new HtmlAnalyze1111Service(logger);

        var result = service.GetCompanyCardContentNodes(htmlDoc);

        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetCompanyCardContentNodes_NoH2()
    {
        var html = $"<div><div class='{Parameters1111.CompanyCardContentDivClassName}'><div class='corp1'></div></div></div>";
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(html);

        var service = new HtmlAnalyze1111Service(logger);

        var result = service.GetCompanyCardContentNodes(htmlDoc);

        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetCompanyCardContentNodes_NoDivWithClassName()
    {
        var html = $"<div><div class='{Parameters1111.CompanyCardContentDivClassName}'><h2>fake data</h2></div></div>";
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(html);

        var service = new HtmlAnalyze1111Service(logger);

        var result = service.GetCompanyCardContentNodes(htmlDoc);

        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetCompanyCardContentNodes_HasData()
    {
        var html = $"<div><div class='{Parameters1111.CompanyCardContentDivClassName}'><h2>fake data</h2><div class='corp1'><p>faek content</p></div></div></div>";
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(html);

        var service = new HtmlAnalyze1111Service(logger);

        var result = service.GetCompanyCardContentNodes(htmlDoc);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Has.Count.EqualTo(1));
    }

    [Test]
    public void GetCompanyCardContent_NoData()
    {
        var html = $"<article></article>";
        var htmlDoc = HtmlNode.CreateNode(html);

        var service = new HtmlAnalyze1111Service(logger);

        var result = service.GetCompanyCardContent(htmlDoc);

        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetCompanyCardContent_NoH2()
    {
        var html = $"<div></div>";
        var htmlDoc = HtmlNode.CreateNode(html);

        var service = new HtmlAnalyze1111Service(logger);

        var result = service.GetCompanyCardContent(htmlDoc);

        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetCompanyCardContent_NoH2TextNotInFilter()
    {
        var html = $"<div><h2>fake</h2></div>";
        var htmlDoc = HtmlNode.CreateNode(html);

        var service = new HtmlAnalyze1111Service(logger);

        var result = service.GetCompanyCardContent(htmlDoc);

        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetCompanyCardContent_NoH2TextFilterPass_NoContent()
    {
        var html = $"<div><h2>{Parameters1111.ProductsOrServicesHtmlTitle[0]}</h2></div>";
        var htmlDoc = HtmlNode.CreateNode(html);

        var service = new HtmlAnalyze1111Service(logger);

        var result = service.GetCompanyCardContent(htmlDoc);

        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetCompanyCardContent_NoH2TextFilterPass_HasContent()
    {
        var html = $"<div><h2>{Parameters1111.ProductsOrServicesHtmlTitle[0]}</h2><div>{TestValue.JustRandomComment}</div></div>";
        var htmlDoc = HtmlNode.CreateNode(html);

        var service = new HtmlAnalyze1111Service(logger);

        var result = service.GetCompanyCardContent(htmlDoc);

        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.Value.Key, Is.EqualTo(Parameters1111.CompanyContentFilter.First().Key));
            Assert.That(result.Value.Value, Is.EqualTo(TestValue.JustRandomComment));
        });
    }

    [Test]
    public void GetJobName_NoData()
    {
        var html = $"<div></div>";
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(html);

        var service = new HtmlAnalyze1111Service(logger);

        var result = service.GetJobName(htmlDoc);

        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetJobName_WrongH1ClassName()
    {
        var html = $"<h1>{TestValue.JustRandomComment}</h1>";
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(html);

        var service = new HtmlAnalyze1111Service(logger);

        var result = service.GetJobName(htmlDoc);

        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetJobName_GetData()
    {
        var html = $"<h1 class='title_'>{TestValue.JustRandomComment}</h1>";
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(html);

        var service = new HtmlAnalyze1111Service(logger);

        var result = service.GetJobName(htmlDoc);

        Assert.That(result, Is.EqualTo(TestValue.JustRandomComment));
    }

    [Test]
    public void GetWorkContent_NoData()
    {
        var html = $"<div></div>";
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(html);

        var service = new HtmlAnalyze1111Service(logger);

        var result = service.GetWorkContent(htmlDoc);

        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetWorkContent_NoChildDivData()
    {
        var html = $"<div class='{Parameters1111.JobWorkContentDivClass}'><div>{TestValue.JustRandomComment}</div></div>";
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(html);

        var service = new HtmlAnalyze1111Service(logger);

        var result = service.GetWorkContent(htmlDoc);

        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetWorkContent_GetData()
    {
        var html = $"<div class='{Parameters1111.JobWorkContentDivClass}'><div class='{Parameters1111.JobWorkContentChildDivClass}'>{TestValue.JustRandomComment}</div></div>";
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(html);

        var service = new HtmlAnalyze1111Service(logger);

        var result = service.GetWorkContent(htmlDoc);

        Assert.That(result, Is.EqualTo(TestValue.JustRandomComment));
    }

    [Test]
    public void GetJobPlace_NoData()
    {
        var html = $"<div></div>";
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(html);

        var service = new HtmlAnalyze1111Service(logger);

        var result = service.GetJobPlace(htmlDoc);

        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetJobPlace_NoSpanContent()
    {
        var html = $"<div><div class='{Parameters1111.JobPlaceIconDivClass}'></div></div>";
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(html);

        var service = new HtmlAnalyze1111Service(logger);

        var result = service.GetJobPlace(htmlDoc);

        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetJobPlace_GetContent()
    {
        var html = $"<div><div class='{Parameters1111.JobPlaceIconDivClass}'></div><span class='{Parameters1111.JobPlaceSpanClass}'>{TestValue.JustRandomComment}</span></div>";
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(html);

        var service = new HtmlAnalyze1111Service(logger);

        var result = service.GetJobPlace(htmlDoc);

        Assert.That(result, Is.EqualTo(TestValue.JustRandomComment));
    }

    [Test]
    public void GetOtherRequirement_NoData()
    {
        var html = $"<div></div>";
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(html);

        var service = new HtmlAnalyze1111Service(logger);

        var result = service.GetOtherRequirement(htmlDoc);

        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetOtherRequirement_NoContentDiv()
    {
        var html = $"<div class='{Parameters1111.JobOtherRequirementDivClass}'>{TestValue.JustRandomComment}</div>"; ;
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(html);

        var service = new HtmlAnalyze1111Service(logger);

        var result = service.GetOtherRequirement(htmlDoc);

        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetOtherRequirement_GetData()
    {
        var html = $"<div class='{Parameters1111.JobOtherRequirementDivClass}'><div class='{Parameters1111.JobOtherRequirementConditionDivClass}'><div class='{Parameters1111.JobOtherRequirementContentDivClass}'>{TestValue.JustRandomComment}</div></div></div>";
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(html);

        var service = new HtmlAnalyze1111Service(logger);

        var result = service.GetOtherRequirement(htmlDoc);

        Assert.That(result, Is.EqualTo(TestValue.JustRandomComment));
    }

    [Test]
    public void GetSalary_NoData()
    {
        var html = $"<div></div>";
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(html);

        var service = new HtmlAnalyze1111Service(logger);

        var result = service.GetSalary(htmlDoc);

        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetSalary_NoSpan()
    {
        var html = $"<div class='{Parameters1111.JobSalaryRegionDivClass}'></div>";
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(html);

        var service = new HtmlAnalyze1111Service(logger);

        var result = service.GetSalary(htmlDoc);

        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetSalary_GetData()
    {
        var html = $"<div class='{Parameters1111.JobSalaryRegionDivClass}'><span class='{Parameters1111.JobSalarySpanClass}'>{TestValue.JustRandomComment}</span></div>";
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(html);

        var service = new HtmlAnalyze1111Service(logger);

        var result = service.GetSalary(htmlDoc);

        Assert.That(result, Is.EqualTo(TestValue.JustRandomComment));
    }

    [Test]
    public void GetJobLastUpdateTime_NoData()
    {
        var html = $"<div></div>";
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(html);

        var service = new HtmlAnalyze1111Service(logger);

        var result = service.GetJobLastUpdateTime(htmlDoc);

        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetJobLastUpdateTime_GetData()
    {
        var html = $"<small class='{Parameters1111.JobLastUpdateTimeSmallClass}'>{TestValue.JustRandomComment}</small>";
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(html);

        var service = new HtmlAnalyze1111Service(logger);

        var result = service.GetJobLastUpdateTime(htmlDoc);

        Assert.That(result, Is.EqualTo(TestValue.JustRandomComment));
    }
}
