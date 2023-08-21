using Crawer_CakeResume.Service;
using Model.Dto;
using Model.DtoCakeResume;
using Service.Delay;
using Service.Http;
using Service.Parameter;
using System.Net;
using System.Text;

namespace Test.CakeResume;

public class HttpCakeResumeServiceTest
{
    //private static string CompanyId => "TestCompanyId";
    //private static string JobId => "TestJobId";
    //private static string GetCompanyInfoUrl => "https://www.company.com.tw";
    //private static string GetJobInfoUrl => "https://www.job.com.tw";

    //private readonly GetJobInfoDto getJobInfoDto = new()
    //{
    //    CompanyId = CompanyId,
    //    JobId = JobId
    //};

    //private readonly GetCompanyInfoDto getCompanyInfoDto = new()
    //{
    //    CompanyId = CompanyId
    //};

    //private readonly ILogger<BaseHttpService> logger = A.Fake<ILogger<BaseHttpService>>();
    //private readonly IParameterService parameterService = A.Fake<IParameterService>();
    //private readonly ITaskDelayService taskDelayService = A.Fake<ITaskDelayService>();

    //private static string CompanyName => "公司名稱";

    //[SetUp]
    //public void Setup()
    //{
    //    A.CallTo(() => parameterService.CompanyInfoUrl(A<GetCompanyInfoDto>.Ignored)).Returns(GetCompanyInfoUrl);
    //    A.CallTo(() => parameterService.JobInfoUrl(A<GetJobInfoDto>.Ignored)).Returns(GetJobInfoUrl);
    //    A.CallTo(() => taskDelayService.WorkerWaiting()).Returns(Task.CompletedTask);
    //}

    //private HttpCakeResumeService GetService(string content)
    //{
    //    var httpMessageHandler = new FakeHttpMessageHandler(new HttpResponseMessage
    //    {
    //        StatusCode = HttpStatusCode.OK,
    //        Content = new StringContent(content)
    //    });
    //    var httpClient = new HttpClient(httpMessageHandler);

    //    return new HttpCakeResumeService(httpClient, parameterService, taskDelayService, logger);
    //}

    //[Test]
    //public async Task GetCompanyInfo_從HttpResponse取回空字串_回傳null()
    //{
    //    var service = GetService("");
    //    var result = await service.GetCompanyInfo<CompanyDto>(getCompanyInfoDto);
    //    Assert.That(result, Is.Null);
    //}

    //[Test]
    //public async Task GetCompanyInfo_從HttpResponse取回json_回傳null()
    //{
    //    var service = GetService("{\"TestProperty\":\"TestValue\"}");
    //    var result = await service.GetCompanyInfo<CompanyDto>(getCompanyInfoDto);
    //    Assert.That(result, Is.Null);
    //}

    
}

public class FakeHttpMessageHandler : HttpMessageHandler
{
    private readonly HttpResponseMessage _response;

    public FakeHttpMessageHandler(HttpResponseMessage response)
    {
        _response = response;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        return Task.FromResult(_response);
    }
}
