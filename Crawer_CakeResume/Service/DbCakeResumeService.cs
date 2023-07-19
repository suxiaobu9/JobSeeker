using Model.Dto;
using Model.DtoCakeResume;
using Model.JobSeekerDb;
using Service.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crawer_CakeResume.Service;

public class DbCakeResumeService : DbService
{
    public DbCakeResumeService(ILogger<DbService> logger, postgresContext postgresContext) : base(logger, postgresContext)
    {
    }

    public override string CompanyInfoUrl(CompanyDto dto)
    {
        return ParametersCakeResume.GetCompanyUrl(dto.Id);
    }

    public override string CompanyPageUrl(CompanyDto dto)
    {
        return ParametersCakeResume.GetCompanyUrl(dto.Id);
    }

    public override string JobInfoUrl(JobDto dto)
    {
        return ParametersCakeResume.GetJobUrl(dto.CompanyId,dto.Id);
    }

    public override string JobPageUrl(JobDto dto)
    {
        return ParametersCakeResume.GetJobUrl(dto.CompanyId,dto.Id);
    }
}
