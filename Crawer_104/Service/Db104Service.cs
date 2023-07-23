using Model.Dto;
using Model.Dto104;
using Model.JobSeekerDb;
using Service.Db;
using StackExchange.Redis;

namespace Crawer_104.Service;

public class Db104Service : DbService
{
    public Db104Service(ILogger<DbService> logger, postgresContext postgresContext, IDatabase redisDb) : base(logger, postgresContext, redisDb)
    {
    }

    /// <summary>
    /// 公司資訊的網址
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    public override string CompanyInfoUrl(CompanyDto dto)
    {
        return Parameters104.Get104CompanyInfoUrl(dto.Id);
    }

    /// <summary>
    /// 公司網頁的網址
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    public override string CompanyPageUrl(CompanyDto dto)
    {
        return Parameters104.Get104CompanyPageUrl(dto.Id);
    }

    /// <summary>
    /// 公司資訊的網址
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    public override string JobInfoUrl(JobDto dto)
    {
        return Parameters104.Get104JobInfoUrl(dto.Id);
    }

    /// <summary>
    /// 公司網頁的網址
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    public override string JobPageUrl(JobDto dto)
    {
        return Parameters104.Get104JobPageUrl(dto.Id);
    }
}
