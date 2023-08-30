using Crawer_1111.Service;
using HtmlAgilityPack;
using Model.Dto;
using Model.Dto1111;
using Service.HtmlAnalyze;
using Service.Http;
using Service.Parameter;
using System.Net;
using System.Text.Json;

namespace Test.Crawer_1111;

public class Http1111ServiceTest
{
    private readonly IHtmlAnalyzeService htmlAnalyzeService = A.Fake<IHtmlAnalyzeService>();
    private readonly ILogger<BaseHttpService> logger = A.Fake<ILogger<BaseHttpService>>();
    private readonly IParameterService parameterService = A.Fake<IParameterService>();
    private readonly string TestUrl = "https://www.example.com";
    private readonly string TestUrlWithId = "https://www.example.com/testid";

    private Http1111Service GetService(string content)
    {
        var httpMessageHandler = new FakeHttpMessageHandler(new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(content)
        });
        var httpClient = new HttpClient(httpMessageHandler);

        return new Http1111Service(httpClient, htmlAnalyzeService, parameterService, logger);
    }

    [Test]
    public async Task GetJobList_HttpResponse_GetStringEmpty()
    {
        var service = GetService("");

        var result = await service.GetJobList<JobListDto<SimpleJobInfoDto>>(TestUrl);

        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetJobList_HttpResponse_HttpContent_RandomComment_throw()
    {
        var service = GetService(TestValue.JustRandomComment);

        Assert.ThrowsAsync<JsonException>(async () => await service.GetJobList<JobListDto<SimpleJobInfoDto>>(TestUrl));
    }

    [Test]
    public async Task GetJobList_HttpResponse_HttpContent_JsonDeserialize_NotValidJsonContent()
    {
        var service = GetService(TestValue.NotValidJsonContent);
        var result = await service.GetJobList<JobListDto<SimpleJobInfoDto>>(TestUrl);

        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task GetJobList_HttpResponse_HttpContent_JsonDeserialize_NotData()
    {
        var fakeData = new JobList1111Model();
        var service = GetService(JsonSerializer.Serialize(fakeData));
        var result = await service.GetJobList<JobListDto<SimpleJobInfoDto>>(TestUrl);

        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task GetJobList_HttpResponse_HttpContent_JsonDeserialize_CurrentPage_IsNull()
    {
        var fakeData = new JobList1111Model
        {
            Html0D = TestValue.JustRandomComment,
            Pi = null,
            Pc = 1
        };
        var service = GetService(JsonSerializer.Serialize(fakeData));
        var result = await service.GetJobList<JobListDto<SimpleJobInfoDto>>(TestUrl);

        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task GetJobList_HttpResponse_HttpContent_JsonDeserialize_TotalPage_IsNull()
    {
        var fakeData = new JobList1111Model
        {
            Html0D = TestValue.JustRandomComment,
            Pi = 1,
            Pc = null
        };
        var service = GetService(JsonSerializer.Serialize(fakeData));
        var result = await service.GetJobList<JobListDto<SimpleJobInfoDto>>(TestUrl);

        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task GetJobList_HttpResponse_HttpContent_JsonDeserialize_CurrentPage_BiggerThan_TotalPage_IsNull()
    {
        var fakeData = new JobList1111Model
        {
            Html0D = TestValue.JustRandomComment,
            Pi = 2,
            Pc = 1
        };
        var service = GetService(JsonSerializer.Serialize(fakeData));
        var result = await service.GetJobList<JobListDto<SimpleJobInfoDto>>(TestUrl);

        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task GetJobList_HttpResponse_NoJobListHtmlCollection()
    {
        var fakeData = new JobList1111Model
        {
            Html0D = TestValue.JustRandomComment,
            Pi = 1,
            Pc = 2
        };

        A.CallTo(() => htmlAnalyzeService.GetJobListCardContentNode(A<HtmlDocument>._)).Returns(null);
        var service = GetService(JsonSerializer.Serialize(fakeData));
        var result = await service.GetJobList<JobListDto<SimpleJobInfoDto>>(TestUrl);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.JobList, Is.Not.Null);
        Assert.That(result.JobList.Count, Is.EqualTo(0));
    }

    [Test]
    public async Task GetJobList_HttpResponse_JobListJobNodeIsNull()
    {
        var fakeData = new JobList1111Model
        {
            Html0D = TestValue.JustRandomComment,
            Pi = 1,
            Pc = 2
        };

        var htmlCollection = new HtmlNodeCollection(null);
        var node = HtmlNode.CreateNode("<div></div>");
        htmlCollection.Add(node);

        A.CallTo(() => htmlAnalyzeService.GetJobListCardContentNode(A<HtmlDocument>._)).Returns(htmlCollection);
        A.CallTo(() => htmlAnalyzeService.GetJobListJobNode(A<HtmlNode>._)).Returns(null);
        A.CallTo(() => htmlAnalyzeService.GetJobListCompanyNode(A<HtmlNode>._)).Returns(node);

        var service = GetService(JsonSerializer.Serialize(fakeData));
        var result = await service.GetJobList<JobListDto<SimpleJobInfoDto>>(TestUrl);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.JobList, Is.Not.Null);
        Assert.That(result.JobList.Count, Is.EqualTo(0));
    }

    [Test]
    public async Task GetJobList_HttpResponse_JobListCompanyNodeIsNull()
    {
        var fakeData = new JobList1111Model
        {
            Html0D = TestValue.JustRandomComment,
            Pi = 1,
            Pc = 2
        };

        var htmlCollection = new HtmlNodeCollection(null);
        var node = HtmlNode.CreateNode("<div></div>");
        htmlCollection.Add(node);

        A.CallTo(() => htmlAnalyzeService.GetJobListCardContentNode(A<HtmlDocument>._)).Returns(htmlCollection);
        A.CallTo(() => htmlAnalyzeService.GetJobListJobNode(A<HtmlNode>._)).Returns(node);
        A.CallTo(() => htmlAnalyzeService.GetJobListCompanyNode(A<HtmlNode>._)).Returns(null);

        var service = GetService(JsonSerializer.Serialize(fakeData));
        var result = await service.GetJobList<JobListDto<SimpleJobInfoDto>>(TestUrl);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.JobList, Is.Not.Null);
        Assert.That(result.JobList.Count, Is.EqualTo(0));
    }

    [Test]
    public async Task GetJobList_HttpResponse_JobList_NoJobId()
    {
        var fakeData = new JobList1111Model
        {
            Html0D = TestValue.JustRandomComment,
            Pi = 1,
            Pc = 2
        };

        var htmlCollection = new HtmlNodeCollection(null);
        var node = HtmlNode.CreateNode($"<a href='fakedata'></a>");
        var nodeWithId = HtmlNode.CreateNode($"<a href='{TestUrlWithId}'></a>");
        htmlCollection.Add(node);

        A.CallTo(() => htmlAnalyzeService.GetJobListCardContentNode(A<HtmlDocument>._)).Returns(htmlCollection);
        A.CallTo(() => htmlAnalyzeService.GetJobListJobNode(A<HtmlNode>._)).Returns(node);
        A.CallTo(() => htmlAnalyzeService.GetJobListCompanyNode(A<HtmlNode>._)).Returns(nodeWithId);

        var service = GetService(JsonSerializer.Serialize(fakeData));
        var result = await service.GetJobList<JobListDto<SimpleJobInfoDto>>(TestUrl);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.JobList, Is.Not.Null);
        Assert.That(result.JobList.Count, Is.EqualTo(0));
    }

    [Test]
    public async Task GetJobList_HttpResponse_JobList_NoCompanyId()
    {
        var fakeData = new JobList1111Model
        {
            Html0D = TestValue.JustRandomComment,
            Pi = 1,
            Pc = 2
        };

        var htmlCollection = new HtmlNodeCollection(null);
        var node = HtmlNode.CreateNode($"<a href='{TestUrl}'></a>");
        var nodeWithId = HtmlNode.CreateNode($"<a href='fakedata'></a>");
        htmlCollection.Add(node);

        A.CallTo(() => htmlAnalyzeService.GetJobListCardContentNode(A<HtmlDocument>._)).Returns(htmlCollection);
        A.CallTo(() => htmlAnalyzeService.GetJobListJobNode(A<HtmlNode>._)).Returns(node);
        A.CallTo(() => htmlAnalyzeService.GetJobListCompanyNode(A<HtmlNode>._)).Returns(nodeWithId);

        var service = GetService(JsonSerializer.Serialize(fakeData));
        var result = await service.GetJobList<JobListDto<SimpleJobInfoDto>>(TestUrl);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.JobList, Is.Not.Null);
        Assert.That(result.JobList.Count, Is.EqualTo(0));
    }

    [Test]
    public async Task GetJobList_HttpResponse_JobList_GetOneData()
    {
        var fakeData = new JobList1111Model
        {
            Html0D = TestValue.JustRandomComment,
            Pi = 1,
            Pc = 2
        };

        var htmlCollection = new HtmlNodeCollection(null);
        var node1 = HtmlNode.CreateNode($"<a href='{TestUrlWithId}'></a>");
        var node2 = HtmlNode.CreateNode($"<a href='{TestUrlWithId}'></a>");
        htmlCollection.Add(node1);

        A.CallTo(() => htmlAnalyzeService.GetJobListCardContentNode(A<HtmlDocument>._)).Returns(htmlCollection);
        A.CallTo(() => htmlAnalyzeService.GetJobListJobNode(A<HtmlNode>._)).Returns(node1);
        A.CallTo(() => htmlAnalyzeService.GetJobListCompanyNode(A<HtmlNode>._)).Returns(node2);

        var service = GetService(JsonSerializer.Serialize(fakeData));
        var result = await service.GetJobList<JobListDto<SimpleJobInfoDto>>(TestUrl);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.JobList, Is.Not.Null);
        Assert.That(result.JobList.Count, Is.EqualTo(1));
    }

    [Test]
    public async Task GetJobList_HttpResponse_JobList_GetMultiData()
    {
        var fakeData = new JobList1111Model
        {
            Html0D = TestValue.JustRandomComment,
            Pi = 1,
            Pc = 2
        };

        var htmlCollection = new HtmlNodeCollection(null);
        var node1 = HtmlNode.CreateNode($"<a href='{TestUrlWithId}'></a>");
        var node2 = HtmlNode.CreateNode($"<a href='{TestUrlWithId}'></a>");
        htmlCollection.Add(node1);
        htmlCollection.Add(node2);

        A.CallTo(() => htmlAnalyzeService.GetJobListCardContentNode(A<HtmlDocument>._)).Returns(htmlCollection);
        A.CallTo(() => htmlAnalyzeService.GetJobListJobNode(A<HtmlNode>._)).Returns(node1);
        A.CallTo(() => htmlAnalyzeService.GetJobListCompanyNode(A<HtmlNode>._)).Returns(node2);

        var service = GetService(JsonSerializer.Serialize(fakeData));
        var result = await service.GetJobList<JobListDto<SimpleJobInfoDto>>(TestUrl);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.JobList, Is.Not.Null);
        Assert.That(result.JobList.Count, Is.EqualTo(2));
    }

}
