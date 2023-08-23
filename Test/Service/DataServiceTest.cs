using Model;
using Model.Dto;
using Model.Dto104;
using Service.Cache;
using Service.Data;
using Service.Db;
using Service.Http;
using Service.Parameter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Service;

public class DataServiceTest
{
    private readonly ILogger<DataService> logger = A.Fake<ILogger<DataService>>();
    private readonly IHttpService httpService = A.Fake<IHttpService>();
    private readonly ICacheService cacheService = A.Fake<ICacheService>();
    private readonly IParameterService parameterService = A.Fake<IParameterService>();
    private readonly IDbService dbService = A.Fake<IDbService>();

    [Test]
    public async Task GetCompanyDataAndUpsert_CompanyDtoGetNull()
    {
        CompanyDto? companyDto = null;
        A.CallTo(() => httpService.GetCompanyInfo<CompanyDto>(A<GetCompanyInfoDto>.Ignored)).Returns(companyDto);

        var service = new DataService(logger, httpService, cacheService, parameterService, dbService);

        var result = await service.GetCompanyDataAndUpsert(new GetCompanyInfoDto());

        Assert.That(result, Is.EqualTo(ReturnStatus.Fail));
    }

    [Test]
    public async Task GetCompanyDataAndUpsert_CompanyDtoGetObject()
    {
        CompanyDto? companyDto = new();
        A.CallTo(() => httpService.GetCompanyInfo<CompanyDto>(A<GetCompanyInfoDto>.Ignored)).Returns(companyDto);

        var service = new DataService(logger, httpService, cacheService, parameterService, dbService);

        var result = await service.GetCompanyDataAndUpsert(new GetCompanyInfoDto());

        Assert.That(result, Is.EqualTo(ReturnStatus.Success));
    }

    [Test]
    public async Task GetJobDataAndUpsert_CompanyNotExists_ReturnRetry()
    {
        A.CallTo(() => cacheService.CompanyExist(A<string>.Ignored, A<string>.Ignored)).Returns(false);

        var service = new DataService(logger, httpService, cacheService, parameterService, dbService);

        var result = await service.GetJobDataAndUpsert(new GetJobInfoDto());

        Assert.That(result, Is.EqualTo(ReturnStatus.Retry));
    }

    [Test]
    public async Task GetJobDataAndUpsert_JobDtoGetNull_ReturnFail()
    {
        JobDto? jobDto = null;
        A.CallTo(() => cacheService.CompanyExist(A<string>.Ignored, A<string>.Ignored)).Returns(true);
        A.CallTo(() => cacheService.CompanyExist(A<string>.Ignored, A<string>.Ignored)).Returns(true);
        A.CallTo(() => httpService.GetJobInfo<JobDto>(A<GetJobInfoDto>.Ignored)).Returns(jobDto);

        var service = new DataService(logger, httpService, cacheService, parameterService, dbService);

        var result = await service.GetJobDataAndUpsert(new GetJobInfoDto());

        Assert.That(result, Is.EqualTo(ReturnStatus.Fail));
    }

    [Test]
    public async Task GetJobDataAndUpsert_JobDtoFilterNotPass_ReturnSuccess()
    {
        JobDto jobDto = new()
        {
            Name = "fake data"
        };

        A.CallTo(() => cacheService.CompanyExist(A<string>.Ignored, A<string>.Ignored)).Returns(true);
        A.CallTo(() => cacheService.CompanyExist(A<string>.Ignored, A<string>.Ignored)).Returns(true);
        A.CallTo(() => httpService.GetJobInfo<JobDto>(A<GetJobInfoDto>.Ignored)).Returns(jobDto);

        var service = new DataService(logger, httpService, cacheService, parameterService, dbService);

        var result = await service.GetJobDataAndUpsert(new GetJobInfoDto());

        Assert.That(result, Is.EqualTo(ReturnStatus.Success));
    }

    [Test]
    public async Task GetJobDataAndUpsert_JobDtoFilterPass_ReturnSuccess()
    {
        JobDto jobDto = new()
        {
            Name = Parameters.KeywordsFilters[0]
        };

        A.CallTo(() => cacheService.CompanyExist(A<string>.Ignored, A<string>.Ignored)).Returns(true);
        A.CallTo(() => cacheService.CompanyExist(A<string>.Ignored, A<string>.Ignored)).Returns(true);
        A.CallTo(() => httpService.GetJobInfo<JobDto>(A<GetJobInfoDto>.Ignored)).Returns(jobDto);

        var service = new DataService(logger, httpService, cacheService, parameterService, dbService);

        var result = await service.GetJobDataAndUpsert(new GetJobInfoDto());

        Assert.That(result, Is.EqualTo(ReturnStatus.Success));
    }
}
