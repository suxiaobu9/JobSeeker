using Crawer_Yourator.Service;
using Model.Dto;
using Model.Dto104;

namespace Test.Yourator;

public class ParameterYouratorServiceTest
{
    private readonly string _companyId = "TestCompanyId";
    private readonly string _jobId = "TestJobId";
    private readonly ParameterYouratorService _service = new();

    [Test]
    public void GetCompanyInfoUrl_合格的Dto_回傳預期中的網址()
    {
        var dto = new GetCompanyInfoDto
        {
            CompanyId = _companyId
        };

        var expected = $"https://www.yourator.co/companies/{dto.CompanyId}";

        var result = _service.CompanyInfoUrl(dto);

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void GetCompanyPageUrl_合格的Dto_回傳預期中的網址()
    {
        var dto = new GetCompanyInfoDto
        {
            CompanyId = _companyId
        };

        var expected = $"https://www.yourator.co/companies/{dto.CompanyId}";

        var result = _service.CompanyPageUrl(dto);

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void GetJobInfoUrl_合格的Dto_回傳預期中的網址()
    {
        var dto = new GetJobInfoDto
        {
            CompanyId = _companyId,
            JobId = _jobId
        };

        var expected = $"https://www.yourator.co/companies/{dto.CompanyId}/jobs/{dto.JobId}";

        var result = _service.JobInfoUrl(dto);

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void GetJobPageUrl_合格的Dto_回傳預期中的網址()
    {
        var dto = new GetJobInfoDto
        {
            CompanyId = _companyId,
            JobId = _jobId
        };

        var expected = $"https://www.yourator.co/companies/{dto.CompanyId}/jobs/{dto.JobId}";

        var result = _service.JobPageUrl(dto);

        Assert.That(result, Is.EqualTo(expected));
    }
}
