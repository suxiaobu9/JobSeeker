using Crawer_104.Service;
using Crawer_104.Workers;
using Microsoft.EntityFrameworkCore;
using Model.Dto104;
using Model.JobSeekerDb;
using RabbitMQ.Client;
using Serilog;
using Service.Cache;
using Service.Data;
using Service.Db;
using Service.Delay;
using Service.Http;
using Service.Mq;
using Service.Parameter;
using StackExchange.Redis;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        // serilog
        services.AddLogging(configure =>
        {
            configure.ClearProviders();
            configure.AddSerilog(new LoggerConfiguration()
                    .WriteTo.Seq(hostContext.Configuration.GetSection("SeqLogServerAddress").Value)
                    .WriteTo.Console()
                    .CreateLogger());
        });

        services.AddHttpClient(Parameters104.Referer, client => client.DefaultRequestHeaders.Add("Referer", Parameters104.Referer));

        // db connection
        services.AddDbContext<postgresContext>(option =>
            option.UseNpgsql(hostContext.Configuration.GetConnectionString("NpgsqlConnection")),
            contextLifetime: ServiceLifetime.Transient, optionsLifetime: ServiceLifetime.Transient);

        // redis
        string redisConnectionString = hostContext.Configuration.GetSection("redis:Host").Value;
        string redisSecret = hostContext.Configuration.GetSection("redis:Secret").Value;
        services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConnectionString + ",password=" + redisSecret));
        services.AddSingleton(sp => sp.GetRequiredService<IConnectionMultiplexer>().GetDatabase(0));

        services.AddSingleton<IHttpService, Http104Service>();
        services.AddSingleton<ICacheService, Redis104Service>();
        services.AddSingleton<IMqService, RabbitMq104Service>();
        services.AddSingleton<IDbService, DbService>();
        services.AddSingleton<IDataService, DataService>();
        services.AddSingleton<IParameterService, Parameter104Service>();
        services.AddSingleton<ITaskDelayService, TaskDelayService>();

        services.AddHostedService<OneZeroFourWorker>();


        // RabbitMq
        services.AddSingleton(serviceProvider =>
        {
            return new ConnectionFactory
            {
                HostName = hostContext.Configuration.GetSection("RabbitMq:Host").Value,
                UserName = hostContext.Configuration.GetSection("RabbitMq:Name").Value,
                Password = hostContext.Configuration.GetSection("RabbitMq:Password").Value,
                DispatchConsumersAsync = true,
            }.CreateConnection();
        });

        // db migration
        using var scope = services.BuildServiceProvider().CreateScope();
        using var context = scope.ServiceProvider.GetRequiredService<postgresContext>();
        context.Database.Migrate();

    }).ConfigureAppConfiguration(config =>
    {
        // nacos
        // configration
        var nacosConfig = config.Build().GetSection("NacosConfig");
        config.AddNacosV2Configuration(nacosConfig);
    }).Build();

await host.RunAsync();
