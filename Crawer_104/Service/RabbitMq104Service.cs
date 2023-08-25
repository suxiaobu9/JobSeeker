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

namespace Crawer_104.Service;

public class RabbitMq104Service : RabbitMqService
{
    private readonly ILogger<RabbitMqService> logger;
    private readonly IConnection connection;
    private readonly IHttpService httpService;
    private readonly ICacheService cacheService;
    private readonly IDbService dbService;
    private readonly IDataService dataService;

    public RabbitMq104Service(ILogger<RabbitMqService> logger,
        IConnection connection,
        IHttpService httpService,
        ICacheService cacheService,
        IDbService dbService,
        IDataService dataService) : base(logger, connection)
    {
        this.logger = logger;
        this.connection = connection;
        this.httpService = httpService;
        this.cacheService = cacheService;
        this.dbService = dbService;
        this.dataService = dataService;
    }

    public override async Task<ReturnStatus> CompanyMessageHandler<T>(T args)
    {
        logger.LogInformation($"{nameof(RabbitMq104Service)} CompanyMessageHandler start.");

        if (args is not BasicDeliverEventArgs basicDeliverEventArgs)
        {
            logger.LogError($"{nameof(RabbitMq104Service)} CompanyMessageHandler basicDeliverEventArgs is null.");
            return ReturnStatus.Fail;
        }

        try
        {
            // get mq (comp_id_for_104)
            var body = basicDeliverEventArgs.Body.ToArray();
            var companyId = Encoding.UTF8.GetString(body);

            if (string.IsNullOrWhiteSpace(companyId))
            {
                logger.LogError($"{nameof(ServiceBus104Service)} CompanyMessageHandler companyId is null.");
                return ReturnStatus.Fail;
            }

            var getCompanyInfoDto = new GetCompanyInfoDto
            {
                CompanyId = companyId,
            };

            return await dataService.GetCompanyDataAndUpsert(getCompanyInfoDto);

        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"{nameof(RabbitMq104Service)} CompanyMessageHandler error.");
            throw;
        }

    }

    public override async Task<ReturnStatus> JobInfoMessageHandler<T>(T args)
    {

        logger.LogInformation($"{nameof(RabbitMq104Service)} JobInfoMessageHandler start.");

        if (args is not BasicDeliverEventArgs basicDeliverEventArgs)
        {
            logger.LogError($"{nameof(RabbitMq104Service)} JobInfoMessageHandler basicDeliverEventArgs is null.");
            return ReturnStatus.Fail;
        }

        try
        {
            var body = basicDeliverEventArgs.Body.ToArray();

            if (body.Length == 0)
            {
                logger.LogError($"{nameof(RabbitMq104Service)} JobInfoMessageHandler body is null.");
                return ReturnStatus.Fail;
            }

            var message = Encoding.UTF8.GetString(body);

            var simpleJobInfo = JsonSerializer.Deserialize<SimpleJobInfoDto>(message);

            if (simpleJobInfo == null || string.IsNullOrWhiteSpace(simpleJobInfo.CompanyId) || string.IsNullOrWhiteSpace(simpleJobInfo.JobId))
            {
                logger.LogError($"{nameof(RabbitMq104Service)} JobInfoMessageHandler SimpleJobInfoDto is null.");
                return ReturnStatus.Fail;
            }

            var getJobInfoDto = new GetJobInfoDto
            {
                CompanyId = simpleJobInfo.CompanyId,
                JobId = simpleJobInfo.JobId,
            };

            return await dataService.GetJobDataAndUpsert(getJobInfoDto);

        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"{nameof(RabbitMq104Service)} JobInfoMessageHandler error.");
            throw;
        }
    }
}
