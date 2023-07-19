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

    public override string CompanyInfoUrl(CompanyDto dto)
    {
        return Parameters104.Get104CompanyInfoUrl(dto.Id);
    }

    public override string CompanyPageUrl(CompanyDto dto)
    {
        return Parameters104.Get104CompanyPageUrl(dto.Id);
    }

    public override string JobInfoUrl(JobDto dto)
    {
        return Parameters104.Get104JobInfoUrl(dto.Id);
    }

    public override string JobPageUrl(JobDto dto)
    {
        return Parameters104.Get104JobPageUrl(dto.Id);
    }
}
