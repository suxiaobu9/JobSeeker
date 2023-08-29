using Model;
using RabbitMQ.Client;
using Service.Mq;

namespace Crawer_1111.Service
{
    public class RabbitMq1111Service : RabbitMqService
    {
        public RabbitMq1111Service(ILogger<RabbitMqService> logger, IConnection connection) : base(logger, connection)
        {
        }

        public override Task<ReturnStatus> CompanyMessageHandler<T>(T args)
        {
            throw new NotImplementedException();
        }

        public override Task<ReturnStatus> JobInfoMessageHandler<T>(T args)
        {
            throw new NotImplementedException();
        }
    }
}
