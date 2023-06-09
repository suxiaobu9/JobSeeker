using Microsoft.AspNetCore.Components.RenderTree;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Model;
using Serilog;
using Serilog.Events;
using System;
using System.Text.Json;

string currentTag = "Crawer";

IConfiguration config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build();

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Seq(config.GetSection("seqLogServerAddress").Value)
    .WriteTo.Console()
    .CreateLogger();

Log.Information($"{{tag}} start.", currentTag);

var services = new ServiceCollection();
services.AddSingleton<HttpClient>();

using var serviceProvider = services.BuildServiceProvider();
using var httpClient = serviceProvider.GetRequiredService<HttpClient>();

httpClient.DefaultRequestHeaders.Add("Referer", _104Parameters.Referer);

if (!Directory.Exists(_104Parameters.JobListDir))
    Directory.CreateDirectory(_104Parameters.JobListDir);

foreach ((string jobArea, string keyword) in _104Parameters.AreaAndKeywords)
{
    try
    {
        var jobList = await GetJobListInfo(keyword, jobArea, 1);

        if (jobList == null)
            continue;

        for (var i = 1; i <= jobList.Data.TotalPage; i++)
        {
            await SaveJobListToFileSystem(keyword, jobArea, i);
        }
    }
    catch (Exception ex)
    {
        Log.Error(ex, $"{{tag}} get job list get exception.", currentTag);
        continue;
    }
}

Log.Information($"{{tag}} end.", currentTag);

Log.CloseAndFlush();

/// <summary>
/// 從 104 上將職缺列表抓下來
/// </summary>
async Task<_104JobListModel?> GetJobListInfo(string keyword, string jobArea, int pageNo)
{
    var url = string.Format(_104Parameters.Get104JobUrl(keyword, jobArea, pageNo));

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
async Task<string> SaveJobListToFileSystem(string keyword, string jobArea, int pageNo)
{
    var fileName = $"{DateTime.UtcNow:yyyyMMddHHmmssfff}-{keyword}-{jobArea}-{pageNo}.json";

    var jobList = await GetJobListInfo(keyword, jobArea, pageNo);

    var filePath = Path.Combine(_104Parameters.JobListDir, fileName);

    await File.AppendAllTextAsync(filePath, JsonSerializer.Serialize(jobList));

    return fileName;
}