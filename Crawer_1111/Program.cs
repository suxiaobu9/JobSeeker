using Crawer_1111.Workers;
using Microsoft.EntityFrameworkCore;
using Model.JobSeekerDb;
using RabbitMQ.Client;
using Serilog;
using StackExchange.Redis;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        var configuration = hostContext.Configuration;

        // serilog
        services.AddLogging(configure =>
        {
            configure.ClearProviders();
            configure.AddSerilog(new LoggerConfiguration()
                    .WriteTo.Seq(configuration.GetSection("SeqLogServerAddress").Value)
                    .WriteTo.Console()
                    .CreateLogger());
        });

        services.AddHttpClient();

        // db connection
        services.AddDbContext<postgresContext>(option =>
                   option.UseNpgsql(configuration.GetConnectionString("NpgsqlConnection")),
                             contextLifetime: ServiceLifetime.Transient, optionsLifetime: ServiceLifetime.Transient);

        // redis
        string redisConnectionString = configuration.GetSection("redis:Host").Value;
        string redisSecret = configuration.GetSection("redis:Secret").Value;
        services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConnectionString + ",password=" + redisSecret));
        services.AddSingleton(sp => sp.GetRequiredService<IConnectionMultiplexer>().GetDatabase(0));



        services.AddHostedService<FourfoldOneWorker>();

        // RabbitMq
        services.AddSingleton(serviceProvider =>
        {
            return new ConnectionFactory
            {
                HostName = configuration.GetSection("RabbitMq:Host").Value,
                UserName = configuration.GetSection("RabbitMq:Name").Value,
                Password = configuration.GetSection("RabbitMq:Password").Value,
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
