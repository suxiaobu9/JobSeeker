using Crawer_1111.Service;
using Crawer_1111.Workers;
using Service;
using Service.Cache;
using Service.Data;
using Service.Db;
using Service.Delay;
using Service.HtmlAnalyze;
using Service.Http;
using Service.Mq;
using Service.Parameter;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddSerilog(hostContext);
        services.AddDbContext(hostContext);
        services.AddRedis(hostContext);
        services.AddRabbitMqConnection(hostContext);

        services.AddHttpClient();

        services.AddSingleton<ICacheService, Redis1111Service>();
        services.AddSingleton<IDbService, DbService>();
        services.AddSingleton<IHttpService, Http1111Service>();
        services.AddSingleton<IHtmlAnalyzeService, HtmlAnalyze1111Service>();
        services.AddSingleton<ICacheService, Redis1111Service>();
        services.AddSingleton<IParameterService, Parameter1111Service>();
        services.AddSingleton<IMqService, RabbitMq1111Service>();
        services.AddSingleton<ITaskDelayService, TaskDelayService>();
        services.AddSingleton<IDataService, DataService>();

        services.AddHostedService<FourfoldOneWorker>();

        services.DbMigration();
    }).ConfigureAppConfiguration(config =>
    {
        config.NacosConfig();
    }).Build();

await host.RunAsync();
