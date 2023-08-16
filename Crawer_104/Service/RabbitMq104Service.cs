using Azure.Messaging.ServiceBus;
using Model.Dto;
using Model.Dto104;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Service.Cache;
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

    public RabbitMq104Service(ILogger<RabbitMqService> logger,
        IConnection connection,
        IHttpService httpService,
        ICacheService cacheService,
        IDbService dbService) : base(logger, connection)
    {
        this.logger = logger;
        this.connection = connection;
        this.httpService = httpService;
        this.cacheService = cacheService;
        this.dbService = dbService;
    }

    public override async Task CompanyMessageHandler<T>(T args)
    {
        logger.LogInformation($"{nameof(RabbitMq104Service)} CompanyMessageHandler start.");

        if (args is not BasicDeliverEventArgs basicDeliverEventArgs)
        {
            logger.LogError($"{nameof(RabbitMq104Service)} CompanyMessageHandler basicDeliverEventArgs is null.");
            return;
        }

        try
        {
            // get mq (comp_id_for_104)
            var body = basicDeliverEventArgs.Body.ToArray();
            var companyId = Encoding.UTF8.GetString(body);

            if (string.IsNullOrWhiteSpace(companyId))
            {
                logger.LogError($"{nameof(ServiceBus104Service)} CompanyMessageHandler companyId is null.");
                return;
            }

            // get company dto
            var companyDto = await httpService.GetCompanyInfo<CompanyDto>(companyId, Parameters104.Get104CompanyInfoUrl(companyId));

            if (companyDto == null)
            {
                logger.LogWarning($"{nameof(ServiceBus104Service)} CompanyMessageHandler get null companyDto.{{companyId}}", companyId);
                return;
            }

            // save to db
            await dbService.UpsertCompany(companyDto);

        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"{nameof(RabbitMq104Service)} CompanyMessageHandler error.");
        }

    }

    public override async Task JobInfoMessageHandler<T>(T args)
    {

        logger.LogInformation($"{nameof(RabbitMq104Service)} JobInfoMessageHandler start.");

        if (args is not BasicDeliverEventArgs basicDeliverEventArgs)
        {
            logger.LogError($"{nameof(RabbitMq104Service)} JobInfoMessageHandler basicDeliverEventArgs is null.");
            return;
        }

        try
        {
            var body = basicDeliverEventArgs.Body.ToArray();

            if (body.Length == 0)
            {
                logger.LogError($"{nameof(ServiceBus104Service)} JobInfoMessageHandler body is null.");
                return;
            }

            var message = Encoding.UTF8.GetString(body);

            var simpleJobInfo = JsonSerializer.Deserialize<SimpleJobInfoDto>(message);

            if (simpleJobInfo == null || string.IsNullOrWhiteSpace(simpleJobInfo.JobId) || string.IsNullOrWhiteSpace(simpleJobInfo.CompanyId))
            {
                logger.LogWarning($"{nameof(ServiceBus104Service)} JobInfoMessageHandler MessageHandler get null simpleJobInfo.{{message}}", message);
                return;
            }

            // 確定公司資訊是否新增了
            if (!await cacheService.CompanyExist(Parameters104.RedisKeyForCompanyUpdated, simpleJobInfo.CompanyId))
            {
                logger.LogInformation($"{nameof(ServiceBus104Service)} JobInfoMessageHandler company not exist, renew message.{{message}}", message);

                //await Task.Delay(TimeSpan.FromSeconds(10));

                // todo: 把 Message 推回 MQ retry 10 次
               // await processMessageEventArgs.AbandonMessageAsync(processMessageEventArgs.Message);
                
                return;
            }

            // get job dto
            var jobDto = await httpService.GetJobInfo<JobDto>(simpleJobInfo.JobId, simpleJobInfo.CompanyId, Parameters104.Get104JobInfoUrl(simpleJobInfo.JobId));

            if (jobDto == null)
            {
                logger.LogWarning($"{nameof(ServiceBus104Service)} JobInfoMessageHandler MessageHandler get null jobDto.{{jobId}}", simpleJobInfo.JobId);
                return;
            }

            if (!jobDto.FilterPassed)
            {
                //logger.LogWarning($"{nameof(ServiceBus104Service)} JobInfoMessageHandler JobDto filter failed.{{jobId}}", jobId);
                return;
            }

            // save to db
            await dbService.UpsertJob(jobDto);


        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"{nameof(RabbitMq104Service)} JobInfoMessageHandler error.");
        }
    }
}
