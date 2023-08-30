using Crawer_CakeResume.Service;
using Crawer_CakeResume.Workers;
using Service;
using Service.Cache;
using Service.Data;
using Service.Db;
using Service.Delay;
using Service.HtmlAnalyze;
using Service.Http;
using Service.Parameter;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddSerilog(hostContext);
        services.AddDbContext(hostContext);
        services.AddRedis(hostContext);

        services.AddHttpClient();

        services.AddSingleton<IHttpService, HttpCakeResumeService>();
        services.AddSingleton<ICacheService, RedisCakeResumeService>();
        services.AddSingleton<IDbService, DbService>();
        services.AddSingleton<IDataService, DataService>();
        services.AddSingleton<IParameterService, ParameterCakeResumeService>();
        services.AddSingleton<ITaskDelayService, TaskDelayService>();
        services.AddSingleton<IHtmlAnalyzeService, HtmlAnalyzeCakeResumeService>();

        services.AddHostedService<CakeResumeWorker>();

        services.DbMigration();
    }).ConfigureAppConfiguration(config =>
    {
        config.NacosConfig();
    }).Build();

await host.RunAsync();
