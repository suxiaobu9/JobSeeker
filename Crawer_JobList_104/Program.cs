using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Model;
using RabbitMQ.Client;
using Serilog;
using Service;
using System.Text;
using System.Text.Json;

string currentTag = "Crawer_JobList";
var factory = ConfigService.RabbitMqFactoryCreater();

string seqLogServerAddress = ConfigService.Configuration.GetSection("SeqLogServerAddress").Value;
Log.Logger = ConfigService.SeqLogCreater(seqLogServerAddress);

Log.Information($"{{tag}} start.", currentTag);

var services = new ServiceCollection();
services.AddSingleton<HttpClient>();
using var serviceProvider = services.BuildServiceProvider();
using var httpClient = serviceProvider.GetRequiredService<HttpClient>();

httpClient.DefaultRequestHeaders.Add("Referer", _104Parameters.Referer);

if (!Directory.Exists(_104Parameters.JobListDir))
    Directory.CreateDirectory(_104Parameters.JobListDir);

while (true)
{
    using var connection = factory.CreateConnection();
    using var channel = connection.CreateModel();
    channel.QueueDeclare(queue: _104Parameters._104JobListQueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

    foreach ((string jobArea, string keyword) in _104Parameters.AreaAndKeywords)
    {
        try
        {
            Log.Information($"{{tag}}. Get initial job list.", currentTag);

            var jobList = await GetJobListInfo(keyword, jobArea, 1);

            if (jobList == null)
                continue;

            for (var i = 1; i <= jobList.Data.TotalPage; i++)
            {
                var fileName = await SaveJobListToFileSystem(keyword, jobArea, i);

                if (string.IsNullOrWhiteSpace(fileName))
                    continue;

                SendMessageToRabbitMq(channel, fileName);
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, $"{{tag}} get job list get exception.", currentTag);
            continue;
        }
    }
    await Task.Delay(TimeSpan.FromHours(1));
}

/// <summary>
/// 從 104 上將職缺列表抓下來
/// </summary>
async Task<_104JobListModel?> GetJobListInfo(string keyword, string jobArea, int pageNo)
{
    var url = string.Format(_104Parameters.Get104JobListUrl(keyword, jobArea, pageNo));

    Log.Information($"{{tag}}. {nameof(url)}: {{url}}.", currentTag, url);
    var response = await httpClient.GetAsync(url);

    if (!response.IsSuccessStatusCode)
    {
        Log.Warning($"{{tag}}. Get response fail. {{httpStatus}}, {{content}}", currentTag, response.StatusCode, await response.Content.ReadAsStringAsync());
        return null;
    }

    var content = await response.Content.ReadAsStringAsync();

    var jobList = JsonSerializer.Deserialize<_104JobListModel>(content);

    if (jobList == null || jobList.Data.TotalCount == 0)
    {
        Log.Warning($"{{tag}}. Get data count = 0.", currentTag);
        return null;
    }

    return jobList;
}

/// <summary>
/// 將檔資料存到檔案系統
/// </summary>
async Task<string?> SaveJobListToFileSystem(string keyword, string jobArea, int pageNo)
{
    var fileName = $"{DateTime.UtcNow:yyyyMMddHHmmssfff}-{keyword}-{jobArea}-{pageNo}.json";

    var jobList = await GetJobListInfo(keyword, jobArea, pageNo);

    if (jobList == null)
        return null;

    var filePath = Path.Combine(_104Parameters.JobListDir, fileName);

    Log.Information($"{{tag}}. Write file. {{fileName}}", currentTag, fileName);

    await File.AppendAllTextAsync(filePath, JsonSerializer.Serialize(jobList));

    await Task.Delay(TimeSpan.FromSeconds(1));

    return fileName;
}

/// <summary>
/// 傳送訊息到 Rabbit MQ
/// </summary>
void SendMessageToRabbitMq(IModel channel, string fileName)
{
    var body = Encoding.UTF8.GetBytes(fileName);
    channel.BasicPublish(exchange: "", routingKey: _104Parameters._104JobListQueueName, basicProperties: null, body: body);

    Log.Information($"{{tag}}. Send job list message to rabbit mq. {{fileName}}", currentTag, fileName);
}