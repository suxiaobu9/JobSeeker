using Crawer_104.Service;
using Crawer_104.Workers;
using Model.Dto104;
using Service;
using Service.Cache;
using Service.Data;
using Service.Db;
using Service.Delay;
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

        services.AddHttpClient(Parameters104.Referer, client => client.DefaultRequestHeaders.Add("Referer", Parameters104.Referer));

        services.AddSingleton<IHttpService, Http104Service>();
        services.AddSingleton<ICacheService, Redis104Service>();
        services.AddSingleton<IMqService, RabbitMq104Service>();
        services.AddSingleton<IDbService, DbService>();
        services.AddSingleton<IDataService, DataService>();
        services.AddSingleton<IParameterService, Parameter104Service>();
        services.AddSingleton<ITaskDelayService, TaskDelayService>();

        services.AddHostedService<OneZeroFourWorker>();

        services.DbMigration();
    }).ConfigureAppConfiguration(config =>
    {
        config.NacosConfig();
    }).Build();

await host.RunAsync();
