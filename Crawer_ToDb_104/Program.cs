using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Model;
using Model.JobSeekerDb;
using RabbitMQ.Client.Events;
using Serilog;
using Service;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.ComponentModel.Design;
using System.Globalization;
using System;
using Nacos.V2.Naming.Dtos;
using RabbitMQ.Client;

string currentTag = "Crawer_ToDb_104";

var factory = ConfigService.RabbitMqFactoryCreater();

string seqLogServerAddress = ConfigService.Configuration.GetSection("SeqLogServerAddress").Value;
Log.Logger = ConfigService.SeqLogCreater(seqLogServerAddress);

Log.Information($"{{tag}} start.", currentTag);

var serviceCollection = new ServiceCollection();

serviceCollection.AddDbContext<postgresContext>(option => option.UseNpgsql(ConfigService.Configuration.GetConnectionString("NpgsqlConnection")), contextLifetime: ServiceLifetime.Transient);
serviceCollection.AddSingleton<HttpClient>();
serviceCollection.AddTransient<_104CompanyService>();
using var serviceProvider = serviceCollection.BuildServiceProvider();

using var httpClient = serviceProvider.GetRequiredService<HttpClient>();
httpClient.DefaultRequestHeaders.Add("Referer", _104Parameters.Referer);


var existCompany = new HashSet<string>();

var connection = factory.CreateConnection();

var infoChannel = connection.CreateModel();

infoChannel.QueueDeclare(queue: _104Parameters._104JobInfoQueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

var consumer = new EventingBasicConsumer(infoChannel);

consumer.Received += async (sender, args) =>
{
    try
    {
        var service = serviceProvider.GetRequiredService<_104CompanyService>();

        await service.AddJobInfoToDb(args);
    }
    catch (Exception ex)
    {
        Log.Error(ex, $"{{tag}} jobInfo to db get exception.", currentTag);
    }
};

infoChannel.BasicConsume(queue: _104Parameters._104JobInfoQueueName, autoAck: true, consumer: consumer);

while (true)
    await Task.Delay(TimeSpan.FromHours(1));

