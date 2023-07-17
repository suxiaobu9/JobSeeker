using Crawer_104.Service;
using Crawer_104.Workers;
using Microsoft.EntityFrameworkCore;
using Model.Dto104;
using Model.JobSeekerDb;
using Serilog;
using Service.Cache;
using Service.Http;
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
            contextLifetime: ServiceLifetime.Singleton);

        // redis
        string redisConnectionString = hostContext.Configuration.GetSection("redis:Host").Value;
        string redisSecret = hostContext.Configuration.GetSection("redis:Secret").Value;
        services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConnectionString + ",password=" + redisSecret));
        services.AddSingleton<IDatabase>(sp => sp.GetRequiredService<IConnectionMultiplexer>().GetDatabase(0));

        services.AddSingleton<IHttpService, Http104Service>();
        services.AddSingleton<ICacheService, Redis104Service>();

        services.AddHostedService<GetCompanyAndJobWorker>();

        // db migration
        using var scope = services.BuildServiceProvider().CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<postgresContext>();
        context.Database.Migrate();

    }).ConfigureAppConfiguration(config =>
    {
        // nacos
        // configration
        var nacosConfig = config.Build().GetSection("NacosConfig");
        config.AddNacosV2Configuration(nacosConfig);
    }).Build();

await host.RunAsync();
