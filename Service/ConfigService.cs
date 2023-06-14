using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using Serilog;
using Serilog.Events;

namespace Service;

public static class ConfigService
{
    public static Serilog.Core.Logger SeqLogCreater(string seqLogServerAddress)
        => new LoggerConfiguration()
                    .MinimumLevel.Information()
                    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                    .Enrich.FromLogContext()
                    .WriteTo.Seq(seqLogServerAddress)
                    .WriteTo.Console()
                    .CreateLogger();

    private static IConfigurationRoot ConfigurationRoot()
    {

        var config = new ConfigurationBuilder()
                        .AddJsonFile("appsettings.json")
                        .AddEnvironmentVariables()
                        .Build();

        return new ConfigurationBuilder()
                    .AddNacosV2Configuration(config.GetSection("NacosConfig"))
                    .Build();

    }

    public static readonly IConfiguration Configuration = ConfigurationRoot();

    public static ConnectionFactory RabbitMqFactoryCreater()
    {
        string Host = Configuration.GetSection("RabbitMq:Host").Value;
        string Name = Configuration.GetSection("RabbitMq:Name").Value;
        string Password = Configuration.GetSection("RabbitMq:Password").Value;

        return new ConnectionFactory
        {
            HostName = Host,
            UserName = Name,
            Password = Password
        };

    }
}
