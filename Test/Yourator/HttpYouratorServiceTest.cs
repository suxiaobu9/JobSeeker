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

namespace Test.Yourator;

public class HttpYouratorServiceTest
{
    private readonly HttpClient httpClient = A.Fake<HttpClient>();
    private readonly ILogger<BaseHttpService> logger = A.Fake<ILogger<BaseHttpService>>();
    private static string GetJobListUrl => "https://www.joblist.com.tw";
    private static string JobListJson => "{\"payload\":{\"jobs\":[{\"id\":100,\"company\":{\"path\":\"/companies/comp1\"}},{\"id\":200,\"company\":{\"path\":\"/companies/comp2\"}}]}}";
    private static string JobListJson_JobsIsZero => "{\"payload\":{\"jobs\":[]}}";
    private static string JobListJson_NoJobs => "{\"payload\":{}}";

    private HttpYouratorService GetService(string content)
    {
        var httpMessageHandler = new FakeHttpMessageHandler(new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(content)
        });
        var httpClient = new HttpClient(httpMessageHandler);

        return new HttpYouratorService(httpClient, logger);
    }


    [Test]
    public async Task GetJobList_HttpResponse_GetStringEmpty()
    {
        var service = GetService("");

        var result = await service.GetJobList<JobListDto<SimpleJobInfoDto>>(GetJobListUrl);

        Assert.That(result, Is.Null);
    }


    [Test]
    public void GetJobList_HttpResponse_取得非Json資料()
    {
        var service = GetService("fake data");

        Assert.ThrowsAsync<JsonException>(async () => await service.GetJobList<JobListDto<SimpleJobInfoDto>>(GetJobListUrl));
    }

    [Test]
    public async Task GetJobList_HttpResponse_取得Json資料_非dto格式()
    {
        var service = GetService(TestValue.NotValidJsonContent);

        var result = await service.GetJobList<JobListDto<SimpleJobInfoDto>>(GetJobListUrl);

        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task GetJobList_HttpResponse_取得Json資料_無職缺資訊()
    {
        var service = GetService(JobListJson_NoJobs);

        var result = await service.GetJobList<JobListDto<SimpleJobInfoDto>>(GetJobListUrl);

        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task GetJobList_HttpResponse_取得Json資料_職缺資訊為0()
    {
        var service = GetService(JobListJson_JobsIsZero);

        var result = await service.GetJobList<JobListDto<SimpleJobInfoDto>>(GetJobListUrl);

        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task GetJobList_HttpResponse_取得Json資料()
    {
        var service = GetService(JobListJson);

        var result = await service.GetJobList<JobListDto<SimpleJobInfoDto>>(GetJobListUrl);

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
}

