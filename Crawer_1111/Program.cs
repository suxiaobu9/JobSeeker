using Crawer_1111.Workers;
using Service;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddSerilog(hostContext);
        services.AddDbContext(hostContext);
        services.AddRedis(hostContext);
        services.AddRabbitMq(hostContext);

        services.AddHttpClient();

        services.AddHostedService<FourfoldOneWorker>();

        services.DbMigration();
    }).ConfigureAppConfiguration(config =>
    {
        config.NacosConfig();
    }).Build();

await host.RunAsync();
