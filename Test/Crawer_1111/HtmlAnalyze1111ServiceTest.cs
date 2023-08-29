using Crawer_1111.Service;
using HtmlAgilityPack;
using Model.Dto1111;
using NUnit.Framework;

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
}
