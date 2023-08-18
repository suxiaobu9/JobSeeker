using Crawer_104.Service;
using Model.Dto;
using Model.Dto104;

namespace Test.Crawer_104;

public class Parameter104ServiceTest
{
    private readonly string _companyId = "TestCompanyId";
    private readonly string _jobId = "TestJobId";
    private readonly Parameter104Service _service = new();

    [Test]
    public void GetCompanyInfoUrl_合格的Dto_回傳預期中的網址()
    {
        var dto = new GetCompanyInfoDto
        {
            CompanyId = _companyId
        };

        var expected = $"{Parameters104.Referer}/company/ajax/content/{_companyId}";

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

        var expected = $"{Parameters104.Referer}/company/{_companyId}";

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

        var expected = $"{Parameters104.Referer}/job/ajax/content/{_jobId}";

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

        var expected = $"{Parameters104.Referer}/job/{_jobId}";

        var result = _service.JobPageUrl(dto);

        Assert.That(result, Is.EqualTo(expected));
    }
}
