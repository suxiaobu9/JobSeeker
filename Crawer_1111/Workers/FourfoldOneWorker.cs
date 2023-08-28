using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crawer_1111.Workers;

public class FourfoldOneWorker : BackgroundService
{
    private readonly ILogger<FourfoldOneWorker> logger;

    public FourfoldOneWorker(ILogger<FourfoldOneWorker> logger)
    {
        this.logger = logger;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        throw new NotImplementedException();
    }
}
