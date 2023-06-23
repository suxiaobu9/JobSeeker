using Crawer_104.Workers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Http;
using Model;
using Model.JobSeekerDb;
using Serilog;
using Service;
using Service.Db;
using Service.Http;
using Service.Mq;


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

        services.AddSingleton<IHttpService, Http104JobService>();
        services.AddSingleton<IMqService, RabbitMqService>();
        services.AddTransient<IDbService, Db104JobService>();

        services.AddHostedService<JobListWorker>();
        services.AddHostedService<JobInfoWorker>();
        services.AddHostedService<JobInfoToDbWorker>();

        if (!hostContext.HostingEnvironment.IsDevelopment())
        {
            using var scope = services.BuildServiceProvider().CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<postgresContext>();
            context.Database.Migrate();
        }

    }).ConfigureAppConfiguration(config =>
    {
        var nacosConfig = config.Build().GetSection("NacosConfig");
        config.AddNacosV2Configuration(nacosConfig);
    }).Build();

await host.RunAsync();
