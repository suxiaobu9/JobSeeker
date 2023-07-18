using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Service.Mq;

public class ServiceBusService : IMqService
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
    /// 處理 MQ 來的訊息
    /// </summary>
    /// <param name="queueName"></param>
    /// <param name="message"></param>
    public async Task ProcessMessageFromMq(string queueName, Func<ProcessMessageEventArgs, Task> messageHandler, Func<ProcessErrorEventArgs, Task> errorHandler)
    {
        var processor = mqClient.CreateProcessor(queueName, new ServiceBusProcessorOptions());

        processor.ProcessMessageAsync += messageHandler;

        processor.ProcessErrorAsync += errorHandler;

        await processor.StartProcessingAsync();

        logger.LogInformation($"{nameof(ServiceBusClient)} ProcessMessageFromMq get start.");

        while (true)
            await Task.Delay(TimeSpan.FromDays(1));
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
}
