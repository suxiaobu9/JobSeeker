using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crawer_104.Workers
{
    public class CompanyToDbWorker : BackgroundService
    {
        public CompanyToDbWorker()
        {
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //todo: get mq (comp_id_for_104)
            //todo: check redis (company id
            //todo: get company dto
            //todo: company dto to entity
            //todo: save to db

            throw new NotImplementedException();
        }
    }
}
