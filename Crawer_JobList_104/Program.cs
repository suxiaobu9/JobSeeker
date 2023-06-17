using Microsoft.Extensions.DependencyInjection;
using Model;
using Serilog;
using Service;
using Service.FileSystem;
using Service.Http;
using Service.Mq;
using Service.Worker;

string seqLogServerAddress = ConfigService.Configuration.GetSection("SeqLogServerAddress").Value;

var services = new ServiceCollection();
services.AddLogging(configure => configure.AddSerilog(ConfigService.SeqLogCreater(seqLogServerAddress)));
services.AddSingleton<IMqService, RabbitMqService>();
services.AddSingleton<IFileSystemService, LocalFileSystemService>();
services.AddSingleton<IHttpService, Get104JobService>();
services.AddSingleton<IWorker, JobList104Service>();
services.AddSingleton(ConfigService.Configuration);
services.AddHttpClient(_104Parameters.Referer,client => 
{
    client.DefaultRequestHeaders.Add("Referer", _104Parameters.Referer);
});

var serviceProvider = services.BuildServiceProvider();

var worker = serviceProvider.GetRequiredService<IWorker>();

await worker.StartAsync();