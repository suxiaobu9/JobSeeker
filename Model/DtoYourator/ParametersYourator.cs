using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DtoYourator;

public class ParametersYourator
{
    public static string SourceFrom => "Yourator";

    public static string GetJobListUrl(string keyword, int page) => $"https://www.yourator.co/api/v4/jobs?area[]=TPE&area[]=NWT&page={page}&sort=most_related&term[]={keyword}";

    /// <summary>
    /// company_id_send_to_queue_Yourator
    /// </summary>
    public static string RedisKeyForCompanyIdSendToQueue => "company_id_send_to_queue_Yourator";

    /// <summary>
    /// job_id_send_to_queue_Yourator
    /// </summary>
    public static string RedisKeyForJobIdSendToQueue => "job_id_send_to_queue_Yourator";

    /// <summary>
    /// queue_company_id_for_Yourator
    /// </summary>
    public static string QueueNameForCompanyId => "queue_company_id_for_Yourator";

    /// <summary>
    /// queue_job_id_for_Yourator
    /// </summary>
    public static string QueueNameForJobId => "queue_job_id_for_Yourator";
}
