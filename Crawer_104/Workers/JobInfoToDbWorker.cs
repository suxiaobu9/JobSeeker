using Model;
using Service.Db;
using Service.Http;
using Service.Mq;

namespace Crawer_104.Workers;

public class JobInfoToDbWorker : BackgroundService
{
    private readonly ILogger<JobInfoToDbWorker> logger;
    private readonly IMqService mqService;
    private readonly IDbService dbService;
    private readonly IHttpService get104JobService;

    public JobInfoToDbWorker(ILogger<JobInfoToDbWorker> logger,
        IMqService mqService,
        IDbService dbService,
        IHttpService get104JobService)
    {
        this.logger = logger;
        this.mqService = mqService;
        this.dbService = dbService;
        this.get104JobService = get104JobService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var currentMethod = "JobInfoToDbWorker.ExecuteAsync";
        logger.LogInformation($"{{currentMethod}} running at: {{time}}", currentMethod, DateTimeOffset.Now);

        mqService.ProcessMessageFromMq(_104Parameters._104JobInfoQueueName, GetJobInfoAndSaveToDb, 10);


        while (!stoppingToken.IsCancellationRequested)
            await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
    }

    private async Task GetJobInfoAndSaveToDb(string jobInfoData)
    {
        var currentMethod = "JobInfoToDbWorker.GetJobInfoAndSaveToDb";
        try
        {
            var jobInfo = dbService.TransJobInfoToDbEntity(jobInfoData);

            if (jobInfo == null)
            {
                logger.LogWarning($"{{currentMethod}} get job info from job list get null.", currentMethod);
                return;
            }

            var companyNo = jobInfo.CompanyId;

            if (!await dbService.CompanyExists(companyNo))
            {
                var companyInfo = await get104JobService.GetCompanyInfo(companyNo);

                if (companyInfo == null)
                {
                    logger.LogWarning($"{{currentMethod}} get company info get null.", currentMethod);
                    return;
                }

                var companyEntity = dbService.TransCompanyInfoToDbEntity(companyInfo);

                if (companyEntity == null)
                {
                    logger.LogWarning($"{{currentMethod}} get company entity get null", currentMethod);
                    return;
                }

                await dbService.UpsertCompany(companyEntity);
            }

            await dbService.UpsertJob(jobInfo);

        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"{{currentMethod}} get exception.{{jobInfoData}}", currentMethod, jobInfoData);

        }
    }
}
