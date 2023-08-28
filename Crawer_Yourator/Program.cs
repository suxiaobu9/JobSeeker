using Crawer_Yourator.Service;
using Crawer_Yourator.Workers;
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

        services.AddSerilog(hostContext );
        services.AddDbContext(hostContext );
        services.AddRedis(hostContext );
        services.AddRabbitMq(hostContext );

        services.AddHttpClient();

        services.AddSingleton<ICacheService, RedisYouratorService>();
        services.AddSingleton<ITaskDelayService, TaskDelayService>();
        services.AddSingleton<IDbService, DbService>();
        services.AddSingleton<IParameterService, ParameterYouratorService>();
        services.AddSingleton<IHttpService, HttpYouratorService>();
        services.AddSingleton<IMqService, RabbitMqYouratorService>();
        services.AddSingleton<IHtmlAnalyzeService, HtmlAnalyzeYouratorService>();
        services.AddSingleton<IDataService, DataService>();

        services.AddHostedService<YouratorWorker>();

        services.DbMigration();
    }).ConfigureAppConfiguration(config =>
    {
        config.NacosConfig();
    }).Build();

await host.RunAsync();
