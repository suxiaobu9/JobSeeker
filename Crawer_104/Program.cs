using Crawer_104.Workers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Http;
using Model;
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
        services.AddLogging(configure =>
        {
            configure.ClearProviders();
            configure.AddSerilog(new LoggerConfiguration()
                    .WriteTo.Seq(hostContext.Configuration.GetSection("SeqLogServerAddress").Value)
                    .WriteTo.Console()
                    .CreateLogger());
        });

        services.AddHttpClient(_104Parameters.Referer, client => client.DefaultRequestHeaders.Add("Referer", _104Parameters.Referer));
        // httpclient ¤£¬ö¿ý Log
        services.RemoveAll<IHttpMessageHandlerBuilderFilter>();

        services.AddDbContext<postgresContext>(option =>
            option.UseNpgsql(hostContext.Configuration.GetConnectionString("NpgsqlConnection")),
            contextLifetime: ServiceLifetime.Singleton);

        string redisConnectionString = hostContext.Configuration.GetSection("redis:Host").Value;
        string redisSecret= hostContext.Configuration.GetSection("redis:Secret").Value;


        services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConnectionString + ",password=" + redisSecret));
        services.AddSingleton<IDatabase>(sp => sp.GetRequiredService<IConnectionMultiplexer>().GetDatabase(0));

        services.AddSingleton<IHttpService, Http104JobService>();
        services.AddSingleton<IMqService, RabbitMqService>();
        services.AddSingleton<ICacheService, RedisService>();
        services.AddTransient<IDbService, Db104JobService>();

        services.AddHostedService<JobListWorker>();
        services.AddHostedService<JobInfoWorker>();
        services.AddHostedService<JobInfoToDbWorker>();

        using var scope = services.BuildServiceProvider().CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<postgresContext>();
        context.Database.Migrate();

    }).ConfigureAppConfiguration(config =>
    {
        var nacosConfig = config.Build().GetSection("NacosConfig");
        config.AddNacosV2Configuration(nacosConfig);
    }).Build();

await host.RunAsync();
