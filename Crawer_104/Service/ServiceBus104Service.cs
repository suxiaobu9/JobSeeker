using Azure.Messaging.ServiceBus;
using Model;
using Model.Dto;
using Service.Cache;
using Service.Data;
using Service.Db;
using Service.Http;
using Service.Mq;
using System.Text.Json;

namespace Crawer_104.Service;

public class ServiceBus104Service : ServiceBusService
{
    private readonly ILogger<ServiceBusService> logger;
    private readonly ICacheService cacheService;
    private readonly IHttpService httpService;
    private readonly IDbService dbService;
    private readonly IDataService dataService;

    public ServiceBus104Service(ILogger<ServiceBusService> logger,
        ServiceBusClient mqClient,
        Dictionary<string, ServiceBusSender> diSenders,
        ICacheService cacheService,
        IHttpService httpService,
        IDbService dbService,
        IDataService dataService) : base(logger, mqClient, diSenders)
    {
        this.logger = logger;
        this.cacheService = cacheService;
        this.httpService = httpService;
        this.dbService = dbService;
        this.dataService = dataService;
    }

    /// <summary>
    /// 處理 Company mq 訊息
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="args"></param>
    /// <returns></returns>
    public override async Task<ReturnStatus> CompanyMessageHandler<T>(T args)
    {
        logger.LogInformation($"{nameof(ServiceBus104Service)} CompanyMessageHandler start.");

        if (args is not ProcessMessageEventArgs processMessageEventArgs)
        {
            logger.LogError($"{nameof(ServiceBus104Service)} CompanyMessageHandler processMessageEventArgs is null.");
            return ReturnStatus.Fail;
        }

        try
        {
            // get mq (comp_id_for_104)
            string? companyId = processMessageEventArgs.Message.Body.ToString();
            if (string.IsNullOrWhiteSpace(companyId))
            {
                logger.LogError($"{nameof(ServiceBus104Service)} CompanyMessageHandler companyId is null.");
                return ReturnStatus.Fail;
            }

            var getCompanyInfoDto = new GetCompanyInfoDto
            {
                CompanyId= companyId,
            };

            await dataService.GetCompanyDataAndUpsert(getCompanyInfoDto);

        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"{nameof(ServiceBus104Service)} CompanyMessageHandler error.");
            return ReturnStatus.Exception;
        }

        await processMessageEventArgs.CompleteMessageAsync(processMessageEventArgs.Message);

        return ReturnStatus.Success;

    }

    /// <summary>
    /// 處理 Jobinfo mq 訊息
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="args"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public override async Task<ReturnStatus> JobInfoMessageHandler<T>(T args)
    {
        logger.LogInformation($"{nameof(ServiceBus104Service)} JobInfoMessageHandler start.");

        if (args is not ProcessMessageEventArgs processMessageEventArgs)
        {
            logger.LogError($"{nameof(ServiceBus104Service)} CompanyMessageHandler processMessageEventArgs is null.");
            return ReturnStatus.Fail;
        }

        try
        {
            string? message = processMessageEventArgs.Message.Body.ToString();

            if (string.IsNullOrWhiteSpace(message))
            {
                logger.LogError($"{nameof(ServiceBus104Service)} JobInfoMessageHandler message is null.");
                return ReturnStatus.Fail;
            }

            var simpleJobInfo = JsonSerializer.Deserialize<SimpleJobInfoDto>(message);

            if(simpleJobInfo == null)
            {
                logger.LogError($"{nameof(ServiceBus104Service)} JobInfoMessageHandler SimpleJobInfoDto is null.");
                return ReturnStatus.Fail;
            }

            var getJobInfoDto = new GetJobInfoDto
            {
                CompanyId = simpleJobInfo.CompanyId,
                JobId = simpleJobInfo.JobId,
            };

            var result = await dataService.GetJobDataAndUpsert(getJobInfoDto);

        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"{nameof(ServiceBus104Service)} JobInfoMessageHandler error.");
            return ReturnStatus.Fail;
        }

        await processMessageEventArgs.CompleteMessageAsync(processMessageEventArgs.Message);
        return ReturnStatus.Success;
    }
}
