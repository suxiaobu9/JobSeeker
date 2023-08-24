using Model;
using RabbitMQ.Client;
using Service.Mq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crawer_Yourator.Service
{
    internal class RabbitMqYouratorService : RabbitMqService
    {
        public RabbitMqYouratorService(ILogger<RabbitMqService> logger, IConnection connection) : base(logger, connection)
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
