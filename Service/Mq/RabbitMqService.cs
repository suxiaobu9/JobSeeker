using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using static Service.Mq.IMqService;

namespace Service.Mq;

public class RabbitMqService : IMqService
{
    private readonly ILogger<RabbitMqService> logger;
    private readonly IConfiguration configuration;
    private readonly Lazy<IConnection> Connection;

    public RabbitMqService(ILogger<RabbitMqService> logger,
        IConfiguration configuration)
    {
        this.logger = logger;
        this.configuration = configuration;
        Connection = new Lazy<IConnection>(() => CreateRabbitMqConnect());
    }

    /// <summary>
    /// 建立 Rabbit MQ 的連線
    /// </summary>
    /// <returns></returns>
    private IConnection CreateRabbitMqConnect()
    {
        string host = configuration.GetSection("RabbitMq:Host").Value,
                name = configuration.GetSection("RabbitMq:Name").Value,
                password = configuration.GetSection("RabbitMq:Password").Value;

        return new ConnectionFactory
        {
            HostName = host,
            UserName = name,
            Password = password
        }.CreateConnection();
    }

    /// <summary>
    /// 處理  MQ 來的訊息
    /// </summary>
    /// <param name="queueName"></param>
    /// <param name="processer"></param>
    /// <exception cref="NotImplementedException"></exception>
    public void ProcessMessageFromMq(string queueName, MessageReceivedProcesser processer)
    {
        logger.LogInformation($"{nameof(RabbitMQ)} {nameof(ProcessMessageFromMq)} start.");

        var channel = Connection.Value.CreateModel();
        channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
        var consumer = new EventingBasicConsumer(channel);

        logger.LogInformation($"{nameof(RabbitMQ)} {nameof(ProcessMessageFromMq)} listen to mq.");

        consumer.Received += (sender, args) =>
        {
            var body = args.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            logger.LogInformation($"{nameof(RabbitMQ)} {nameof(ProcessMessageFromMq)} receive message.{{message}}", message);
            processer(message);
        };

        channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);

        throw new NotImplementedException();
    }

    /// <summary>
    /// 送訊息到 MQ 中
    /// </summary>
    /// <param name="queueName"></param>
    /// <param name="message"></param>
    public void SendMessageToMq(string queueName, string message)
    {
        try
        {
            var body = Encoding.UTF8.GetBytes(message);
            var channel = Connection.Value.CreateModel();
            channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

            channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);

            logger.LogInformation($"{nameof(RabbitMQ)} {nameof(SendMessageToMq)} send message finish.{{message}}", message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"{nameof(RabbitMQ)} {nameof(SendMessageToMq)} send message error.{{message}}", message);
            throw;
        }

    }

    /// <summary>
    /// 送訊息到 MQ 中
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="queueName"></param>
    /// <param name="model"></param>
    public void SendMessageToMq<T>(string queueName, T model)
    {
        SendMessageToMq(queueName, JsonSerializer.Serialize(model));
    }
}
