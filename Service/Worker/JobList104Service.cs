using Microsoft.Extensions.Logging;
using Model;
using Serilog;
using Service.FileSystem;
using Service.Http;
using Service.Mq;

namespace Service.Worker;

public class JobList104Service : IWorker
{
    private readonly ILogger<JobList104Service> logger;
    private readonly IHttpService get104JobService;
    private readonly IMqService mqService;
    private readonly IFileSystemService fileSystemService;

    public JobList104Service(ILogger<JobList104Service> logger,
        IHttpService get104JobService,
        IFileSystemService fileSystemService,
        IMqService mqService)
    {
        this.logger = logger;
        this.get104JobService = get104JobService;
        this.mqService = mqService;
        this.fileSystemService = fileSystemService;
    }

    public async Task StartAsync()
    {
        logger.LogInformation($"{nameof(JobList104Service)} Start.");
        while (true)
        {
            foreach ((string jobArea, string keyword) in _104Parameters.AreaAndKeywords)
            {
                try
                {
                    await GetJobList_SaveToFileSystem_SendMessageToMqAsync(keyword, jobArea);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, $"{nameof(JobList104Service)} get job list get exception.");
                    continue;
                }
            }

            await Task.Delay(TimeSpan.FromHours(1));
        }
    }

    /// <summary>
    /// 取得職缺列表、儲存至檔案系統、發送訊息至MQ
    /// </summary>
    /// <param name="keyword"></param>
    /// <param name="jobArea"></param>
    /// <returns></returns>
    private async Task GetJobList_SaveToFileSystem_SendMessageToMqAsync(string keyword, string jobArea)
    {
        var getJobListUrl = string.Format(_104Parameters.Get104JobListUrl(keyword, jobArea, 1));

        logger.LogInformation($"{nameof(JobList104Service)} Get initial Data.{{url}}", getJobListUrl);

        var jobList = await get104JobService.GetJobListAsync<_104JobListModel>(getJobListUrl);

        if (jobList == null)
        {
            logger.LogWarning($"{nameof(JobList104Service)} Get initial job list fail.{{getJobListUrl}}", getJobListUrl);
            return;
        }

        var totalPage = jobList.Data.TotalPage;

        for (var i = 1; i <= totalPage; i++)
        {
            getJobListUrl = string.Format(_104Parameters.Get104JobListUrl(keyword, jobArea, i));
            jobList = await get104JobService.GetJobListAsync<_104JobListModel>(getJobListUrl);

            if (jobList == null)
            {
                logger.LogWarning($"{nameof(JobList104Service)} Get initial job list fail.{{getJobListUrl}}", getJobListUrl);
                continue;
            }

            var fileNameArgs = new string[] { keyword, jobArea, i.ToString() };
            var fileName = await fileSystemService.SaveFileToFileSystemAsync(jobList, fileNameArgs);

            if (string.IsNullOrWhiteSpace(fileName))
                continue;

            mqService.SendMessageToMq(_104Parameters._104JobListQueueName, fileName);
        }
    }
}
