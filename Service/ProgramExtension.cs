using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Model.JobSeekerDb;
using RabbitMQ.Client;
using Serilog;
using StackExchange.Redis;

namespace Service;

public static class ProgramExtension
{
    public static void AddSerilog(this IServiceCollection services, HostBuilderContext hostBuilderContext)
    {
        var configuration = hostBuilderContext.Configuration;

        // serilog
        services.AddLogging(configure =>
        {
            configure.ClearProviders();
            configure.AddSerilog(new LoggerConfiguration()
                    .WriteTo.Seq(configuration.GetSection("SeqLogServerAddress").Value)
                    .WriteTo.Console()
                    .CreateLogger());
        });
    }

    public static void AddDbContext(this IServiceCollection services, HostBuilderContext hostBuilderContext)
    {
        var configuration = hostBuilderContext.Configuration;

        // db connection
        services.AddDbContext<postgresContext>(option =>
                   option.UseNpgsql(configuration.GetConnectionString("NpgsqlConnection")),
                             contextLifetime: ServiceLifetime.Transient, optionsLifetime: ServiceLifetime.Transient);
    }

    public static void AddRedis(this IServiceCollection services, HostBuilderContext hostBuilderContext)
    {
        var configuration = hostBuilderContext.Configuration;

        // redis
        string redisConnectionString = configuration.GetSection("redis:Host").Value;
        string redisSecret = configuration.GetSection("redis:Secret").Value;
        services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConnectionString + ",password=" + redisSecret));
        services.AddSingleton(sp => sp.GetRequiredService<IConnectionMultiplexer>().GetDatabase(0));
    }

    public static void AddRabbitMq(this IServiceCollection services, HostBuilderContext hostBuilderContext)
    {
        var configuration = hostBuilderContext.Configuration;

        // RabbitMq
        services.AddSingleton(serviceProvider =>
        {
            var HostName = configuration.GetSection("RabbitMq:Host").Value;
            var UserName = configuration.GetSection("RabbitMq:Name").Value;
            var Password = configuration.GetSection("RabbitMq:Password").Value;

            return new ConnectionFactory
            {
                HostName = configuration.GetSection("RabbitMq:Host").Value,
                UserName = configuration.GetSection("RabbitMq:Name").Value,
                Password = configuration.GetSection("RabbitMq:Password").Value,
                DispatchConsumersAsync = true,
            }.CreateConnection();
        });
    }

    public static void DbMigration(this IServiceCollection services)
    {
        // db migration
        using var scope = services.BuildServiceProvider().CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<postgresContext>();
        context.Database.Migrate();
    }

    public static void NacosConfig(this IConfigurationBuilder config)
    {
        // nacos
        // configration
        var nacosConfig = config.Build().GetSection("NacosConfig");
        config.AddNacosV2Configuration(nacosConfig);
    }
}
