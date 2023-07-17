namespace Crawer_104.Workers;

public class JobInfoToDbWorker : BackgroundService
{
    public JobInfoToDbWorker()
    {
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        //todo: get mq (job_id_for_104)
        //todo: check redis (job id
        //todo: get job dto
        //todo: job dto to entity
        //todo: save to db

        throw new NotImplementedException();
    }
}
