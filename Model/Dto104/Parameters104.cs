namespace Model.Dto104;

public static class Parameters104
{
    private static readonly string[] JobAreas = new string[]
    {
        "6001001001", "6001001002",
        "6001001003", "6001001004",
        "6001001005", "6001001006",
        "6001001007", "6001001008",
        "6001001009", "6001001010",
        "6001001011", "6001001012",
        "6001002001", "6001002002",
        "6001002003", "6001002004",
        "6001002005", "6001002006",
        "6001002007", "6001002008",
        "6001002009", "6001002010",
        "6001002011", "6001002012",
        "6001002013", "6001002014",
        "6001002015", "6001002016",
        "6001002017", "6001002018",
        "6001002019", "6001002020",
        "6001002021", "6001002022",
        "6001002023", "6001002024",
        "6001002025", "6001002026",
        "6001002027", "6001002028",
        "6001002029"
    };

    public static (string Area, string Keyword)[] AreaAndKeywords => JobAreas.SelectMany(x => Parameters.Keywords.Select(y => (x, y))).ToArray();

    public static readonly string Referer = @"https://www.104.com.tw";
    public static string Get104JobListUrl(string keyword, string jobArea, int page)
    {
        return $@"{Referer}/jobs/search/list?ro=1&jobcat=2007001000&kwop=7&keyword={keyword}&area={jobArea}&order=15&asc=0&page={page}&mode=l&jobsource=2018indexpoc&searchTempExclude=2&langFlag=0&langStatus=0&recommendJob=1&hotJob=1";
    }

    /// <summary>
    /// queue_company_id_for_104
    /// </summary>
    public static string QueueNameForCompanyId => "queue_company_id_for_104";

    /// <summary>
    /// queue_job_id_for_104
    /// </summary>
    public static string QueueNameForJobId => "queue_job_id_for_104";

    /// <summary>
    /// company_id_send_to_queue_104
    /// </summary>
    public static string RedisKeyForCompanyIdSendToQueue => "company_id_send_to_queue_104";

    /// <summary>
    /// job_id_send_to_queue_104
    /// </summary>
    public static string RedisKeyForJobIdSendToQueue => "job_id_send_to_queue_104";

    /// <summary>
    /// company_id_updated_104
    /// </summary>
    public static string RedisKeyForCompanyUpdated => "company_id_updated_104";

    /// <summary>
    /// 104
    /// </summary>
    public static string SourceFrom => "104";

    public static string RabbitMq104ExchangeName => "rabbit_mq_104_exchange";

}
