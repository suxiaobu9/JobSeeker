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
            Password = password,
            DispatchConsumersAsync = true,
        }.CreateConnection();
    }

    /// <summary>
    /// 處理  MQ 來的訊息
    /// </summary>
    /// <param name="queueName"></param>
    /// <param name="processer"></param>
    /// <exception cref="NotImplementedException"></exception>
    public void ProcessMessageFromMq(string queueName, MessageReceivedProcesserAsync processer, ushort? prefetchCount = null)
    {
        var currentMethod = "RabbitMqService.ProcessMessageFromMq";
        logger.LogInformation($"{{currentMethod}} start.", currentMethod);

        var channel = Connection.Value.CreateModel();
        var autoAck = true;

        if (prefetchCount != null)
        {
            channel.BasicQos(0, prefetchCount.Value, true);
            autoAck = false;
        }

        channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
        var consumer = new AsyncEventingBasicConsumer(channel);

        logger.LogInformation($"{{currentMethod}} listen to mq.", currentMethod);

        consumer.Received += async (sender, args) =>
        {
            var body = args.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            logger.LogInformation($"{{currentMethod}} receive message.", currentMethod);
            await processer(message);

            if (!autoAck)
                channel.BasicAck(args.DeliveryTag, false);
        };

        channel.BasicConsume(queue: queueName, autoAck: autoAck, consumer: consumer);
    }

    /// <summary>
    /// 送訊息到 MQ 中
    /// </summary>
    /// <param name="queueName"></param>
    /// <param name="message"></param>
    public void SendMessageToMq(string queueName, string message)
    {
        var currentMethod = "RabbitMqService.SendMessageToMq";
        try
        {
            var body = Encoding.UTF8.GetBytes(message);
            using var channel = Connection.Value.CreateModel();
            channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

            channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);

            logger.LogInformation($"{{currentMethod}} send message finish.", currentMethod);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"{{currentMethod}} send message error.{{message}}", currentMethod, message);
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
