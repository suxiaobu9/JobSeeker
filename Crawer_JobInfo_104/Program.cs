using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Model;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Serilog;
using Service;
using System.Text;
using System.Text.Json;

string currentTag = "Crawer_JobInfo";

string seqLogServerAddress = ConfigService.Configuration.GetSection("SeqLogServerAddress").Value;
Log.Logger = ConfigService.SeqLogCreater(seqLogServerAddress);

Log.Information($"{{tag}} start.", currentTag);

var services = new ServiceCollection();
services.AddSingleton<HttpClient>();
using var serviceProvider = services.BuildServiceProvider();
using var httpClient = serviceProvider.GetRequiredService<HttpClient>();

httpClient.DefaultRequestHeaders.Add("Referer", _104Parameters.Referer);

if (!Directory.Exists(_104Parameters.JobInfoDir))
    Directory.CreateDirectory(_104Parameters.JobInfoDir);

var factory = ConfigService.RabbitMqFactoryCreater();
using var connection = factory.CreateConnection();

AnalyzeJobList(connection);

while (true)
    await Task.Delay(TimeSpan.FromDays(1));


/// <summary>
/// 解析職缺清單
/// </summary>
void AnalyzeJobList(IConnection connection)
{
    try
    {
        var listChannel = connection.CreateModel();
        var infoChannel = connection.CreateModel();

        listChannel.QueueDeclare(queue: _104Parameters._104JobListQueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
        infoChannel.QueueDeclare(queue: _104Parameters._104JobInfoQueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

        var consumer = new EventingBasicConsumer(listChannel);

        Log.Information($"{{tag}} listening mq.", currentTag);

        consumer.Received += async (sender, args) =>
        {
            var jobInfoFileNames = await AnalyzeMqMessage(args);

            if (jobInfoFileNames == null)
                return;
            foreach (var fileName in jobInfoFileNames)
                SendMessageToRabbitMq(infoChannel, fileName);
        };

        listChannel.BasicConsume(queue: _104Parameters._104JobListQueueName, autoAck: true, consumer: consumer);
        Log.Information($"{{tag}} waiting mq.", currentTag);

    }
    catch (Exception ex)
    {
        Log.Error(ex, $"{{tag}} Analyze job list get exception.", currentTag);
        return;
    }
}

/// <summary>
/// 解析職缺清單的 mq 訊息
/// </summary>
async Task<List<string>?> AnalyzeMqMessage(BasicDeliverEventArgs args)
{
    try
    {
        var body = args.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);

        Log.Information($"{{tag}} get mq message. {{message}}", currentTag, message);

        var jobListFileName = Path.Combine(_104Parameters.JobListDir, message);

        if (!File.Exists(jobListFileName))
        {
            Log.Error($"{{tag}} job list file not exist. {{fileName}}", currentTag, jobListFileName);
            return null;
        }

        var jobList = JsonSerializer.Deserialize<_104JobListModel>(File.ReadAllText(jobListFileName));

        if (jobList == null)
        {
            Log.Error($"{{tag}} job list deserialize fail. {{fileName}}", currentTag, jobListFileName);
            return null;
        }

        var result = new List<string>();

        foreach (var job in jobList.Data.List)
        {
            var link = @"https:" + job.Link.Job;

            var jobId = new Uri(link).Segments.LastOrDefault();

            if (jobId == null)
            {
                Log.Error($"{{tag}} get null job id. {link}", currentTag, link);
                continue;
            }

            Log.Information($"{{tag}} get job info. {{jobid}}", currentTag, jobId);

            var jobInfo = await GetJobInfo(jobId);

            if (jobInfo == null)
                continue;

            var jobInfoFileName = Path.Combine(_104Parameters.JobInfoDir, $"{DateTime.UtcNow:yyyyMMddHHmmss}-{jobId}.json");

            File.WriteAllText(jobInfoFileName, JsonSerializer.Serialize(jobInfo));

            result.Add(jobInfoFileName);
        }

        File.Delete(jobListFileName);
        return result;
    }
    catch (Exception ex)
    {
        Log.Error(ex, $"{{tag}} get job info get exception.", currentTag);
        return null;
    }
}

///<summary>
/// 取得職缺內容
/// </summary>
async Task<_104JobInfoModel?> GetJobInfo(string jobId)
{
    try
    {
        var url = _104Parameters.Get104JobInfoUrl(jobId);

        var response = await httpClient.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            Log.Error($"{{tag}} get job info fail. {{jobNo}} {{content}}", currentTag, jobId, content);
            return null;
        }

        return JsonSerializer.Deserialize<_104JobInfoModel>(content);
    }
    catch (Exception ex)
    {
        Log.Error(ex, $"{{tag}} get job info get exception.", currentTag);
        return null;
    }
}

/// <summary>
/// 傳送訊息到 Rabbit MQ
/// </summary>
void SendMessageToRabbitMq(IModel channel, string fileName)
{
    var body = Encoding.UTF8.GetBytes(fileName);
    channel.BasicPublish(exchange: "", routingKey: _104Parameters._104JobInfoQueueName, basicProperties: null, body: body);

    Log.Information($"{{tag}}. Send job info message to rabbit mq. {{fileName}}", currentTag, fileName);
}