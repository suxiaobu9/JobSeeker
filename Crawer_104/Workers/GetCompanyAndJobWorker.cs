namespace Crawer_104.Workers;

public class GetCompanyAndJobWorker : BackgroundService
{
    public GetCompanyAndJobWorker()
    {
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        //todo: delete all job and company redis key
        //todo: get job list
        //todo: get companies
        //todo: send company id to mq
        //todo: get jobs
        //todo: send job id to mq

        //todo delay 6 hours
        throw new NotImplementedException();
    }
}
