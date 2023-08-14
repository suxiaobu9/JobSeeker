using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Service.Mq;

public abstract class ServiceBusService : IMqService
{
    private readonly ILogger<ServiceBusService> logger;
    private readonly ServiceBusClient mqClient;
    private readonly Dictionary<string, ServiceBusSender> diSenders;

    public ServiceBusService(ILogger<ServiceBusService> logger,
        ServiceBusClient mqClient,
        Dictionary<string, ServiceBusSender> diSenders)
    {
        this.logger = logger;
        this.mqClient = mqClient;
        this.diSenders = diSenders;
    }

    /// <summary>
    /// 處理 Company mq 訊息
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="args"></param>
    /// <returns></returns>
    public abstract Task CompanyMessageHandler<T>(T args);

    /// <summary>
    /// 處理 Jobinfo mq 訊息
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="args"></param>
    /// <returns></returns>
    public abstract Task JobInfoMessageHandler<T>(T args);

    /// <summary>
    /// 處理 MQ 來的訊息
    /// </summary>
    /// <param name="queueName"></param>
    /// <param name="message"></param>
    public async Task ProcessMessageFromMq<T>(string queueName, Func<T, Task> messageHandler)
    {
        var processor = mqClient.CreateProcessor(queueName, new ServiceBusProcessorOptions
        {
            AutoCompleteMessages = false,
        });

        processor.ProcessMessageAsync += messageHandler as Func<ProcessMessageEventArgs, Task>;

        processor.ProcessErrorAsync += BasicErrorHandler;

        await processor.StartProcessingAsync();

        logger.LogInformation($"{nameof(ServiceBusClient)} ProcessMessageFromMq get start.");
    }

    /// <summary>
    /// 送訊息到 Mq 
    /// </summary>
    /// <param name="queueName"></param>
    /// <param name="message"></param>
    public async Task SendMessageToMq(string queueName, string message)
    {
        ServiceBusSender sender =
            diSenders.ContainsKey(queueName) ?
            diSenders[queueName] :
            mqClient.CreateSender(queueName);

        var serviceBusMessage = new ServiceBusMessage(message);

        await sender.SendMessageAsync(serviceBusMessage);
    }

    /// <summary>
    /// 送訊息到 Mq 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="queueName"></param>
    /// <param name="message"></param>
    public async Task SendMessageToMq<T>(string queueName, T message)
    {
        await SendMessageToMq(queueName, JsonSerializer.Serialize(message));
    }

    /// <summary>
    /// 基礎的處理訊息錯誤的 handle
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    private Task BasicErrorHandler(ProcessErrorEventArgs args)
    {
        logger.LogError(args.Exception, $"{nameof(ServiceBusService)} BasicErrorHandler get exception.");
        return Task.CompletedTask;
    }
}
