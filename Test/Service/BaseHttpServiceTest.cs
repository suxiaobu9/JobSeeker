using Microsoft.Extensions.Http;
using Service.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Test.Service;

public class BaseHttpServiceTest
{
    private readonly ILogger<BaseHttpService> logger = A.Fake<ILogger<BaseHttpService>>();
    private readonly string Url = "https://www.com.tw/";

    private static HttpClient GetHttpClient(HttpStatusCode code, string content)
    {
        var httpMessageHandler = new FakeHttpMessageHandler(new HttpResponseMessage
        {
            StatusCode = code,
            Content = new StringContent(content)
        });

        return new HttpClient(httpMessageHandler);
    }

    [Test]
    public async Task GetDataFromHttpRequest_HttpResponse_IsSuccessStatusCode為False()
    {
        var httpClient = GetHttpClient(HttpStatusCode.InternalServerError, "Fake Data");

        var service = new BaseHttpService(httpClient, logger);

        var result = await service.GetDataFromHttpRequest(Url);

        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task GetDataFromHttpRequest_HttpResponse_IsSuccessStatusCode為True_Content有字串()
    {
        var content = "Fake Data";
        var httpClient = GetHttpClient(HttpStatusCode.OK, content);

        var service = new BaseHttpService(httpClient, logger);

        var result = await service.GetDataFromHttpRequest(Url);

        Assert.That(result, Is.EqualTo(content));
    }

    [Test]
    public async Task GetDataFromHttpRequest_HttpResponse_IsSuccessStatusCode為True_Content為空字串()
    {
        var content = "";
        var httpClient = GetHttpClient(HttpStatusCode.OK, content);

        var service = new BaseHttpService(httpClient, logger);

        var result = await service.GetDataFromHttpRequest(Url);

        Assert.That(result, Is.EqualTo(content));
    }
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
