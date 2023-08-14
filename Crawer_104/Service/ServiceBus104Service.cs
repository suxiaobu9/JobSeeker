using Azure.Messaging.ServiceBus;
using Model.Dto;
using Model.Dto104;
using Service.Cache;
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

    public ServiceBus104Service(ILogger<ServiceBusService> logger,
        ServiceBusClient mqClient,
        Dictionary<string, ServiceBusSender> diSenders,
        ICacheService cacheService,
        IHttpService httpService,
        IDbService dbService) : base(logger, mqClient, diSenders)
    {
        this.logger = logger;
        this.cacheService = cacheService;
        this.httpService = httpService;
        this.dbService = dbService;
    }

    /// <summary>
    /// 處理 Company mq 訊息
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="args"></param>
    /// <returns></returns>
    public override async Task CompanyMessageHandler<T>(T args)
    {
        logger.LogInformation($"CompanyMessageHandler start.");

        if (args is not ProcessMessageEventArgs processMessageEventArgs)
        {
            logger.LogError($"CompanyMessageHandler processMessageEventArgs is null.");
            return;
        }

        try
        {
            // get mq (comp_id_for_104)
            string? companyId = processMessageEventArgs.Message.Body.ToString();
            if (string.IsNullOrWhiteSpace(companyId))
            {
                logger.LogError($"CompanyMessageHandler companyId is null.");
                return;
            }

            // get company dto
            var companyDto = await httpService.GetCompanyInfo<CompanyDto>(companyId, Parameters104.Get104CompanyInfoUrl(companyId));

            if (companyDto == null)
            {
                logger.LogWarning($"CompanyMessageHandler get null companyDto.{{companyId}}", companyId);
                await processMessageEventArgs.CompleteMessageAsync(processMessageEventArgs.Message);
                return;
            }

            // save to db
            await dbService.UpsertCompany(companyDto);

        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"CompanyMessageHandler error.");
        }

        await processMessageEventArgs.CompleteMessageAsync(processMessageEventArgs.Message);

    }

    /// <summary>
    /// 處理 Jobinfo mq 訊息
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="args"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public override async Task JobInfoMessageHandler<T>(T args)
    {
        logger.LogInformation($"JobInfoMessageHandler start.");

        if (args is not ProcessMessageEventArgs processMessageEventArgs)
        {
            logger.LogError($"CompanyMessageHandler processMessageEventArgs is null.");
            return;
        }

        try
        {
            string? message = processMessageEventArgs.Message.Body.ToString();

            if (string.IsNullOrWhiteSpace(message))
            {
                logger.LogError($"JobInfoMessageHandler message is null.");
                return;
            }

            var simpleJobInfo = JsonSerializer.Deserialize<SimpleJobInfoDto>(message);

            if (simpleJobInfo == null || string.IsNullOrWhiteSpace(simpleJobInfo.JobId) || string.IsNullOrWhiteSpace(simpleJobInfo.CompanyId))
            {
                logger.LogWarning($"JobInfoMessageHandler MessageHandler get null simpleJobInfo.{{message}}", message);
                await processMessageEventArgs.DeadLetterMessageAsync(processMessageEventArgs.Message);
                return;
            }

            // 確定公司資訊是否新增了
            if (!await cacheService.CompanyExist(Parameters104.RedisKeyForCompanyUpdated, simpleJobInfo.CompanyId))
            {
                logger.LogInformation($"JobInfoMessageHandler company not exist, renew message.{{message}}", message);

                await Task.Delay(TimeSpan.FromSeconds(10));

                // 把 Message 推回 MQ
                await processMessageEventArgs.AbandonMessageAsync(processMessageEventArgs.Message);

                return;
            }

            // get job dto
            var jobDto = await httpService.GetJobInfo<JobDto>(simpleJobInfo.JobId, simpleJobInfo.CompanyId, Parameters104.Get104JobInfoUrl(simpleJobInfo.JobId));

            if (jobDto == null)
            {
                logger.LogWarning($"JobInfoMessageHandler MessageHandler get null jobDto.{{jobId}}", simpleJobInfo.JobId);
                await processMessageEventArgs.CompleteMessageAsync(processMessageEventArgs.Message);
                return;
            }

            if (!jobDto.FilterPassed)
            {
                //logger.LogWarning($"JobInfoMessageHandler JobDto filter failed.{{jobId}}", jobId);
                await processMessageEventArgs.CompleteMessageAsync(processMessageEventArgs.Message);
                return;
            }

            // save to db
            await dbService.UpsertJob(jobDto);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"CompanyMessageHandler error.");
        }

        await processMessageEventArgs.CompleteMessageAsync(processMessageEventArgs.Message);
    }
}
