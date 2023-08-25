using Crawer_104.Service;
using Crawer_Yourator.Service;
using Service.Http;
using Service.Parameter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Model.Dto;
using System.Text.Json;
using System.Runtime.CompilerServices;
using Service.HtmlAnalyze;
using HtmlAgilityPack;
using Model.DtoYourator;
using FakeItEasy;

namespace Test.Yourator;

public class HttpYouratorServiceTest
{
    private readonly HttpClient httpClient = A.Fake<HttpClient>();
    private readonly IParameterService parameterService = A.Fake<IParameterService>();
    private readonly IHtmlAnalyzeService htmlAnalyzeService = A.Fake<IHtmlAnalyzeService>();
    private readonly ILogger<BaseHttpService> logger = A.Fake<ILogger<BaseHttpService>>();

    private static string CompanyId => "TestCompanyId";
    private static string JobId => "TestJobId";
    private static string FakeUrl => "https://www.example.com.tw";
    private static string JobListJson => "{\"payload\":{\"jobs\":[{\"id\":100,\"company\":{\"path\":\"/companies/comp1\"}},{\"id\":200,\"company\":{\"path\":\"/companies/comp2\"}}]}}";
    private static string JobListJson_JobsIsZero => "{\"payload\":{\"jobs\":[]}}";
    private static string JobListJson_NoJobs => "{\"payload\":{}}";

    private readonly GetJobInfoDto getJobInfoDto = new()
    {
        CompanyId = CompanyId,
        JobId = JobId
    };

    private readonly GetCompanyInfoDto getCompanyInfoDto = new()
    {
        CompanyId = CompanyId
    };

    private HttpYouratorService GetService(string content)
    {
        var httpMessageHandler = new FakeHttpMessageHandler(new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(content)
        });
        var httpClient = new HttpClient(httpMessageHandler);

        return new HttpYouratorService(httpClient, parameterService, htmlAnalyzeService, logger);
    }


    [Test]
    public async Task GetJobList_HttpResponse_GetStringEmpty()
    {
        var service = GetService("");

        var result = await service.GetJobList<JobListDto<SimpleJobInfoDto>>(FakeUrl);

        Assert.That(result, Is.Null);
    }


    [Test]
    public void GetJobList_HttpResponse_取得非Json資料()
    {
        var service = GetService("fake data");

        Assert.ThrowsAsync<JsonException>(async () => await service.GetJobList<JobListDto<SimpleJobInfoDto>>(FakeUrl));
    }

    [Test]
    public async Task GetJobList_HttpResponse_取得Json資料_非dto格式()
    {
        var service = GetService(TestValue.NotValidJsonContent);

        var result = await service.GetJobList<JobListDto<SimpleJobInfoDto>>(FakeUrl);

        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task GetJobList_HttpResponse_取得Json資料_無職缺資訊()
    {
        var service = GetService(JobListJson_NoJobs);

        var result = await service.GetJobList<JobListDto<SimpleJobInfoDto>>(FakeUrl);

        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task GetJobList_HttpResponse_取得Json資料_職缺資訊為0()
    {
        var service = GetService(JobListJson_JobsIsZero);

        var result = await service.GetJobList<JobListDto<SimpleJobInfoDto>>(FakeUrl);

        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task GetJobList_HttpResponse_取得Json資料()
    {
        var service = GetService(JobListJson);

        var result = await service.GetJobList<JobListDto<SimpleJobInfoDto>>(FakeUrl);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.JobList, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.JobList.ToArray()[0].JobId, Is.EqualTo("100"));
            Assert.That(result.JobList.ToArray()[0].CompanyId, Is.EqualTo("comp1"));
            Assert.That(result.JobList.ToArray()[1].JobId, Is.EqualTo("200"));
            Assert.That(result.JobList.ToArray()[1].CompanyId, Is.EqualTo("comp2"));
        });
    }

    [Test]
    public async Task GetCompanyInfo_HttpResponse_GetStringEmpty()
    {
        A.CallTo(() => parameterService.CompanyInfoUrl(A<GetCompanyInfoDto>.Ignored)).Returns(FakeUrl);
        var service = GetService("");

        var result = await service.GetCompanyInfo<CompanyDto>(getCompanyInfoDto);

        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task GetCompanyInfo_HttpResponse_取得空的公司名稱()
    {
        A.CallTo(() => parameterService.CompanyInfoUrl(A<GetCompanyInfoDto>.Ignored)).Returns(FakeUrl);
        A.CallTo(() => htmlAnalyzeService.GetCompanyName(A<HtmlDocument>.Ignored)).Returns("");
        var service = GetService("fake data");

        var result = await service.GetCompanyInfo<CompanyDto>(getCompanyInfoDto);

        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task GetCompanyInfo_HttpResponse_取得公司名稱()
    {
        A.CallTo(() => parameterService.CompanyInfoUrl(A<GetCompanyInfoDto>.Ignored)).Returns(FakeUrl);
        A.CallTo(() => htmlAnalyzeService.GetCompanyName(A<HtmlDocument>.Ignored)).Returns(CompanyId);
        var service = GetService("fake data");

        var result = await service.GetCompanyInfo<CompanyDto>(getCompanyInfoDto);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.EqualTo(CompanyId));
        Assert.Multiple(() =>
        {
            Assert.That(result.SourceFrom, Is.EqualTo(ParametersYourator.SourceFrom));
            Assert.That(result.Name, Is.EqualTo(CompanyId));
        });
    }

    [Test]
    public async Task GetCompanyInfo_NoCardContent()
    {
        A.CallTo(() => parameterService.CompanyInfoUrl(A<GetCompanyInfoDto>.Ignored)).Returns(FakeUrl);
        A.CallTo(() => htmlAnalyzeService.GetCompanyName(A<HtmlDocument>.Ignored)).Returns(CompanyId);
        A.CallTo(() => htmlAnalyzeService.GetCompanyCardContentNodes(A<HtmlDocument>.Ignored)).Returns(null);
        var service = GetService("fake data");

        var result = await service.GetCompanyInfo<CompanyDto>(getCompanyInfoDto);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.EqualTo(CompanyId));
        Assert.Multiple(() =>
        {
            Assert.That(result.SourceFrom, Is.EqualTo(ParametersYourator.SourceFrom));
            Assert.That(result.Name, Is.EqualTo(CompanyId));
        });
    }

    [Test]
    public async Task GetJobInfo_HttpResponse_NoContent()
    {
        A.CallTo(() => parameterService.JobInfoUrl(A<GetJobInfoDto>.Ignored)).Returns(FakeUrl);
        var service = GetService("");

        var result = await service.GetJobInfo<JobDto>(getJobInfoDto);

        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task GetJobInfo_HttpResponse_NoJobTitle()
    {
        A.CallTo(() => parameterService.JobInfoUrl(A<GetJobInfoDto>.Ignored)).Returns(FakeUrl);
        A.CallTo(() => htmlAnalyzeService.GetJobName(A<HtmlDocument>.Ignored)).Returns(null);
        var service = GetService("fake data");

        var result = await service.GetJobInfo<JobDto>(getJobInfoDto);

        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task GetJobInfo_HttpResponse_GetJobTitle()
    {
        A.CallTo(() => parameterService.JobInfoUrl(A<GetJobInfoDto>.Ignored)).Returns(FakeUrl);
        A.CallTo(() => htmlAnalyzeService.GetJobName(A<HtmlDocument>.Ignored)).Returns(JobId);
        var service = GetService("fake data");

        var result = await service.GetJobInfo<JobDto>(getJobInfoDto);

        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.EqualTo(getJobInfoDto.JobId));
            Assert.That(result.CompanyId, Is.EqualTo(getJobInfoDto.CompanyId));
        });
    }

    [Test]
    public async Task GetJobInfo_CardContentNodesGetNull()
    {
        A.CallTo(() => parameterService.JobInfoUrl(A<GetJobInfoDto>.Ignored)).Returns(FakeUrl);
        A.CallTo(() => htmlAnalyzeService.GetJobName(A<HtmlDocument>.Ignored)).Returns(JobId);
        A.CallTo(() => htmlAnalyzeService.GetJobCardContentNodes(A<HtmlDocument>.Ignored)).Returns(null);
        var service = GetService("fake data");

        var result = await service.GetJobInfo<JobDto>(getJobInfoDto);

        Assert.That(result, Is.Not.Null);
    }
}

