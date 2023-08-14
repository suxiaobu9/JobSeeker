using Azure.Messaging.ServiceBus;
using Crawer_104.Service;
using Crawer_104.Workers;
using Microsoft.EntityFrameworkCore;
using Model.Dto104;
using Model.JobSeekerDb;
using Serilog;
using Service.Cache;
using Service.Db;
using Service.Http;
using Service.Mq;
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
        services.AddSingleton<IDatabase>(sp => sp.GetRequiredService<IConnectionMultiplexer>().GetDatabase(0));

        services.AddSingleton<IHttpService, Http104Service>();
        services.AddSingleton<ICacheService, Redis104Service>();
        services.AddSingleton<IMqService, ServiceBusService>();
        services.AddSingleton<IDbService, Db104Service>();

        services.AddHostedService<OneZeroFourWorker>();

        // ServiceBusClient 
        string serviceBusConnectionString = hostContext.Configuration.GetSection("AzureServiceBus:ConnectionString").Value;
        services.AddSingleton(serviceProvider =>
        {
            return new ServiceBusClient(serviceBusConnectionString);
        });
        services.AddSingleton(serviceProvider =>
        {
            var scope = services.BuildServiceProvider().CreateScope();
            var serviceBusClient = scope.ServiceProvider.GetRequiredService<ServiceBusClient>();
            var companySender = serviceBusClient.CreateSender(Parameters104.QueueNameForCompanyId);
            var jobSender = serviceBusClient.CreateSender(Parameters104.QueueNameForJobId);

            return new Dictionary<string, ServiceBusSender>
            {
                { Parameters104.QueueNameForCompanyId, companySender },
                { Parameters104.QueueNameForJobId, jobSender },
            };
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
