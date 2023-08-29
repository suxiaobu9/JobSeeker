using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dto1111;

public static class Parameters1111
{
    /// <summary>
    /// 1111
    /// </summary>
    public const string SourceFrom = "1111";

    /// <summary>
    /// queue_company_id_for_1111
    /// </summary>
    public const string QueueNameForCompanyId = "queue_company_id_for_1111";

    /// <summary>
    /// queue_job_id_for_1111
    /// </summary>
    public const string QueueNameForJobId = "queue_job_id_for_1111";

    /// <summary>
    /// company_id_send_to_queue_1111
    /// </summary>
    public const string RedisKeyForCompanyIdSendToQueue = "company_id_send_to_queue_1111";

    /// <summary>
    /// job_id_send_to_queue_1111
    /// </summary>
    public const string RedisKeyForJobIdSendToQueue = "job_id_send_to_queue_1111";

    /// <summary>
    /// company_id_updated_1111
    /// </summary>
    public const string RedisKeyForCompanyUpdated = "company_id_updated_1111";

    public static string GetJobListUrl(string keyword, int page) => $"https://www.1111.com.tw/search/job?c0=100100%2C100200&ks={keyword}&page={page}&act=load_page";

    public const string JobListCardDivClassName = "job_item_profile";
    public const string JobListJobDivClassName = "job_item_info";
    public const string JobListCompanyDivClassName = "subtitle";
    public const string JobListJobAClass = "job";
    public const string JobListCompanyAClass = "company";
}
