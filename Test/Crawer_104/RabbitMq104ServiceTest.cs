using Crawer_104.Service;
using Model;
using Model.Dto;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Service.Cache;
using Service.Data;
using Service.Db;
using Service.Http;
using Service.Mq;
using System.Text;
using System.Text.Json;

namespace Test.Crawer_104;

public class RabbitMq104ServiceTest
{
    private readonly ILogger<RabbitMqService> logger = A.Fake<ILogger<RabbitMqService>>();
    private readonly IConnection connection = A.Fake<IConnection>();
    private readonly IHttpService httpService = A.Fake<IHttpService>();
    private readonly ICacheService cacheService = A.Fake<ICacheService>();
    private readonly IDbService dbService = A.Fake<IDbService>();
    private readonly IDataService dataService = A.Fake<IDataService>();

    [Test]
    public async Task CompanyMessageHandler_BasicDeliverEventArgs為null()
    {
        var service = new RabbitMq104Service(logger, connection, httpService, cacheService, dbService, dataService);

        BasicDeliverEventArgs? args = null;

        var result = await service.CompanyMessageHandler(args);

        Assert.That(result, Is.EqualTo(ReturnStatus.Fail));
    }

    [Test]
    public async Task CompanyMessageHandler_BasicDeliverEventArgs轉型失敗()
    {
        var service = new RabbitMq104Service(logger, connection, httpService, cacheService, dbService, dataService);

        JobDto args = new();

        var result = await service.CompanyMessageHandler(args);

        Assert.That(result, Is.EqualTo(ReturnStatus.Fail));
    }

    [Test]
    public async Task CompanyMessageHandler_BasicDeliverEventArgs的Body為空()
    {
        var service = new RabbitMq104Service(logger, connection, httpService, cacheService, dbService, dataService);

        BasicDeliverEventArgs args = new()
        {
            Body = Encoding.UTF8.GetBytes("")
        };

        var result = await service.CompanyMessageHandler(args);

        Assert.That(result, Is.EqualTo(ReturnStatus.Fail));
    }

    [Test]
    public async Task CompanyMessageHandler_BasicDeliverEventArgs的Body有CompanyId_Update成功()
    {
        A.CallTo(() => dataService.GetCompanyDataAndUpsert(A<GetCompanyInfoDto>._)).Returns(ReturnStatus.Success);
        var service = new RabbitMq104Service(logger, connection, httpService, cacheService, dbService, dataService);

        BasicDeliverEventArgs args = new()
        {
            Body = Encoding.UTF8.GetBytes("fake company id")
        };

        var result = await service.CompanyMessageHandler(args);

        Assert.That(result, Is.EqualTo(ReturnStatus.Success));
    }

    [Test]
    public async Task CompanyMessageHandler_BasicDeliverEventArgs的Body有CompanyId_Update失敗()
    {
        A.CallTo(() => dataService.GetCompanyDataAndUpsert(A<GetCompanyInfoDto>._)).Returns(ReturnStatus.Fail);
        var service = new RabbitMq104Service(logger, connection, httpService, cacheService, dbService, dataService);

        BasicDeliverEventArgs args = new()
        {
            Body = Encoding.UTF8.GetBytes("fake company id")
        };

        var result = await service.CompanyMessageHandler(args);

        Assert.That(result, Is.EqualTo(ReturnStatus.Fail));
    }

    [Test]
    public async Task JobInfoMessageHandler_BasicDeliverEventArgs為null()
    {
        var service = new RabbitMq104Service(logger, connection, httpService, cacheService, dbService, dataService);

        BasicDeliverEventArgs? args = null;

        var result = await service.JobInfoMessageHandler(args);

        Assert.That(result, Is.EqualTo(ReturnStatus.Fail));
    }

    [Test]
    public async Task JobInfoMessageHandler_BasicDeliverEventArgs轉型失敗()
    {
        var service = new RabbitMq104Service(logger, connection, httpService, cacheService, dbService, dataService);

        JobDto args = new();

        var result = await service.JobInfoMessageHandler(args);

        Assert.That(result, Is.EqualTo(ReturnStatus.Fail));
    }

    [Test]
    public async Task JobInfoMessageHandler_BasicDeliverEventArgs的Body為空()
    {
        var service = new RabbitMq104Service(logger, connection, httpService, cacheService, dbService, dataService);

        BasicDeliverEventArgs args = new()
        {
            Body = Encoding.UTF8.GetBytes("")
        };

        var result = await service.JobInfoMessageHandler(args);

        Assert.That(result, Is.EqualTo(ReturnStatus.Fail));
    }

    [Test]
    public void JobInfoMessageHandler_BasicDeliverEventArgs的Body有值但不是Json()
    {
        var service = new RabbitMq104Service(logger, connection, httpService, cacheService, dbService, dataService);

        BasicDeliverEventArgs args = new()
        {
            Body = Encoding.UTF8.GetBytes("fake data")
        };

        Assert.ThrowsAsync<JsonException>(async () => await service.JobInfoMessageHandler(args));
    }

    [Test]
    public async Task JobInfoMessageHandler_BasicDeliverEventArgs的Body為Json但非正確的型別()
    {
        var service = new RabbitMq104Service(logger, connection, httpService, cacheService, dbService, dataService);

        BasicDeliverEventArgs args = new()
        {
            Body = Encoding.UTF8.GetBytes("{\"TestProperty\":\"TestValue\"}")
        };

        var result = await service.JobInfoMessageHandler(args);

        Assert.That(result, Is.EqualTo(ReturnStatus.Fail));
    }

    [Test]
    public async Task JobInfoMessageHandler_BasicDeliverEventArgs的Body為Json的正確的型別_Update失敗()
    {
        A.CallTo(() => dataService.GetJobDataAndUpsert(A<GetJobInfoDto>._)).Returns(ReturnStatus.Fail);
        var service = new RabbitMq104Service(logger, connection, httpService, cacheService, dbService, dataService);
        var sampleModel = new SimpleJobInfoDto
        {
            CompanyId = "fake company id",
            JobId = "fake job id",
        };

        BasicDeliverEventArgs args = new()
        {
            Body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(sampleModel))
        };

        var result = await service.JobInfoMessageHandler(args);

        Assert.That(result, Is.EqualTo(ReturnStatus.Fail));
    }

    [Test]
    public async Task JobInfoMessageHandler_BasicDeliverEventArgs的Body為Json的正確的型別_Update成功()
    {
        A.CallTo(() => dataService.GetJobDataAndUpsert(A<GetJobInfoDto>._)).Returns(ReturnStatus.Success);
        var service = new RabbitMq104Service(logger, connection, httpService, cacheService, dbService, dataService);
        var sampleModel = new SimpleJobInfoDto
        {
            CompanyId = "fake company id",
            JobId = "fake job id",
        };

        BasicDeliverEventArgs args = new()
        {
            Body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(sampleModel))
        };

        var result = await service.JobInfoMessageHandler(args);

        Assert.That(result, Is.EqualTo(ReturnStatus.Success));
    }

    [Test]
    public async Task JobInfoMessageHandler_BasicDeliverEventArgs的Body為Json的正確的型別_Update失敗_重試()
    {
        A.CallTo(() => dataService.GetJobDataAndUpsert(A<GetJobInfoDto>._)).Returns(ReturnStatus.Retry);
        var service = new RabbitMq104Service(logger, connection, httpService, cacheService, dbService, dataService);
        var sampleModel = new SimpleJobInfoDto
        {
            CompanyId = "fake company id",
            JobId = "fake job id",
        };

        BasicDeliverEventArgs args = new()
        {
            Body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(sampleModel))
        };

        var result = await service.JobInfoMessageHandler(args);

        Assert.That(result, Is.EqualTo(ReturnStatus.Retry));
    }
}
