using Microsoft.Extensions.Logging;
using Model;
using Model.Dto104;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Service.Mq;

public abstract class RabbitMqService : IMqService
{
    private readonly ILogger<RabbitMqService> logger;
    private readonly IConnection connection;

    protected RabbitMqService(ILogger<RabbitMqService> logger,
        IConnection connection)
    {
        this.logger = logger;
        this.connection = connection;
    }

    public abstract Task<ReturnStatus> CompanyMessageHandler<T>(T args);

    public abstract Task<ReturnStatus> JobInfoMessageHandler<T>(T args);

    public Task ProcessMessageFromMq<T>(string queueName, Func<T, Task<ReturnStatus>> messageHandler)
    {
        logger.LogInformation("ProcessMessageFromMq start.");

        if (messageHandler is not Func<BasicDeliverEventArgs, Task<ReturnStatus>> basicDeliverEventArgs)
        {
            logger.BeginScope("ProcessMessageFromMq messageHandler is not Func<BasicDeliverEventArgs, Task>.");
            return Task.CompletedTask;
        }

        var channel = connection.CreateModel();
        channel.BasicQos(0, 1, false);

        channel.ExchangeDeclare(exchange: Parameters104.RabbitMq104ExchangeName, type: ExchangeType.Direct);

        channel.QueueDeclare(queueName, false, false, false, null);

        channel.QueueBind(queue: queueName, exchange: Parameters104.RabbitMq104ExchangeName, routingKey: queueName);

        var consumer = new AsyncEventingBasicConsumer(channel);

        logger.LogInformation($"ProcessMessageFromMq listen to mq.");

        consumer.Received += async (sender, ea) =>
        {
            var result = await basicDeliverEventArgs(ea);

            if (result == ReturnStatus.Retry)
            {
                var properties = channel.CreateBasicProperties();
                var retryCount = 0;

                if (ea.BasicProperties.Headers != null &&
                    ea.BasicProperties.Headers.ContainsKey("retry-count"))
                    retryCount = (int)ea.BasicProperties.Headers["retry-count"];

                retryCount++;

                if (retryCount < 10)
                {
                    // 設置新的重試計數並重新發送消息
                    properties.Headers = new Dictionary<string, object> { { "retry-count", retryCount } };
                    channel.BasicPublish(Parameters104.RabbitMq104ExchangeName, queueName, properties, ea.Body);
                }

            }

            channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
        };

        channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);

        return Task.CompletedTask;
    }

    /// <summary>
    /// 送訊息到 Mq 
    /// </summary>
    /// <param name="queueName"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    public Task SendMessageToMq(string queueName, string message)
    {
        try
        {
            var body = Encoding.UTF8.GetBytes(message);
            using var channel = connection.CreateModel();
            channel.ExchangeDeclare(exchange: Parameters104.RabbitMq104ExchangeName, type: ExchangeType.Direct, durable: false, autoDelete: false, arguments: null);

            channel.BasicPublish(exchange: Parameters104.RabbitMq104ExchangeName, routingKey: queueName, basicProperties: null, body: body);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Send message to mq failed");
        }

        return Task.CompletedTask;
    }

    /// <summary>
    /// 送訊息到 Mq 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="queueName"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    public Task SendMessageToMq<T>(string queueName, T message)
    {
        return SendMessageToMq(queueName, JsonConvert.SerializeObject(message));
    }
}
