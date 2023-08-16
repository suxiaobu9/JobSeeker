using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using Model;
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

    public abstract Task CompanyMessageHandler<T>(T args);

    public abstract Task JobInfoMessageHandler<T>(T args);

    public Task ProcessMessageFromMq<T>(string queueName, Func<T, Task> messageHandler)
    {
        logger.LogInformation("ProcessMessageFromMq start.");

        if (messageHandler is not Func<BasicDeliverEventArgs, Task> basicDeliverEventArgs)
        {
            logger.BeginScope("ProcessMessageFromMq messageHandler is not Func<BasicDeliverEventArgs, Task>.");
            return Task.CompletedTask;
        }

        var channel = connection.CreateModel();
        channel.BasicQos(0, 10, false);

        channel.ExchangeDeclare(exchange: Parameters.RabbitMqExchangeName, type: ExchangeType.Direct, durable: false, autoDelete: false, arguments: null);

        channel.QueueBind(queue: channel.QueueDeclare().QueueName, exchange: "direct_exchange", routingKey: queueName);

        var consumer = new AsyncEventingBasicConsumer(channel);

        logger.LogInformation($"ProcessMessageFromMq listen to mq.");

        consumer.Received += async (sender, ea) =>
        {
            await basicDeliverEventArgs(ea);
            //var body = ea.Body.ToArray();
            //var message = Encoding.UTF8.GetString(body);

            //logger.LogInformation($"Received message from mq: {message}");

            //await messageHandler(JsonConvert.DeserializeObject<T>(message));
            
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
            channel.ExchangeDeclare(exchange: Parameters.RabbitMqExchangeName, type: ExchangeType.Direct, durable: false, autoDelete: false, arguments: null);

            channel.BasicPublish(exchange: Parameters.RabbitMqExchangeName, routingKey: queueName, basicProperties: null, body: body);
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
