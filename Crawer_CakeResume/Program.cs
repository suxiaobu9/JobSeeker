using Crawer_CakeResume.Service;
using Crawer_CakeResume.Workers;
using Microsoft.EntityFrameworkCore;
using Model.JobSeekerDb;
using Serilog;
using Service.Cache;
using Service.Data;
using Service.Db;
using Service.Delay;
using Service.HtmlAnalyze;
using Service.Http;
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

        services.AddHttpClient();

        // db connection
        services.AddDbContext<postgresContext>(option =>
            option.UseNpgsql(hostContext.Configuration.GetConnectionString("NpgsqlConnection")),
           contextLifetime: ServiceLifetime.Transient, optionsLifetime: ServiceLifetime.Transient);

        // redis
        string redisConnectionString = hostContext.Configuration.GetSection("redis:Host").Value;
        string redisSecret = hostContext.Configuration.GetSection("redis:Secret").Value;
        services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConnectionString + ",password=" + redisSecret));
        services.AddSingleton(sp => sp.GetRequiredService<IConnectionMultiplexer>().GetDatabase(0));

        services.AddSingleton<IHttpService, HttpCakeResumeService>();
        services.AddSingleton<ICacheService, RedisCakeResumeService>();
        services.AddSingleton<IDbService, DbService>();
        services.AddSingleton<IDataService, DataService>();
        services.AddSingleton<IParameterService, ParameterCakeResumeService>();
        services.AddSingleton<ITaskDelayService, TaskDelayService>();
        services.AddSingleton<IHtmlAnalyzeService, HtmlAnalyzeCakeResumeService>();

        services.AddHostedService<CakeResumeWorker>();

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
