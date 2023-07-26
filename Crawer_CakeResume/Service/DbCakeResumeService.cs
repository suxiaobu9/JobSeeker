using Model.Dto;
using Model.DtoCakeResume;
using Model.JobSeekerDb;
using Service.Db;
using StackExchange.Redis;

namespace Crawer_CakeResume.Service;

public class DbCakeResumeService : DbService
{
    public DbCakeResumeService(ILogger<DbService> logger, IServiceScopeFactory serviceScopeFactory, IDatabase redisDb) : base(logger, serviceScopeFactory, redisDb)
    {
    }

    /// <summary>
    /// 公司資訊的網址
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    public override string CompanyInfoUrl(CompanyDto dto)
    {
        return ParametersCakeResume.GetCompanyUrl(dto.Id);
    }

    /// <summary>
    /// 公司網頁的網址
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    public override string CompanyPageUrl(CompanyDto dto)
    {
        return ParametersCakeResume.GetCompanyUrl(dto.Id);
    }

    /// <summary>
    /// 職缺資訊的網址
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    public override string JobInfoUrl(JobDto dto)
    {
        return ParametersCakeResume.GetJobUrl(dto.CompanyId, dto.Id);
    }

    /// <summary>
    /// 職缺網頁的網址
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    public override string JobPageUrl(JobDto dto)
    {
        return ParametersCakeResume.GetJobUrl(dto.CompanyId, dto.Id);
    }
}
