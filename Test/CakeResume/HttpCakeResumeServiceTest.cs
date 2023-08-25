using Crawer_CakeResume.Service;
using HtmlAgilityPack;
using Model.Dto;
using Model.DtoCakeResume;
using Service.Delay;
using Service.HtmlAnalyze;
using Service.Http;
using Service.Parameter;
using System.Net;

namespace Test.CakeResume;

public class HttpCakeResumeServiceTest
{
    private static string CompanyId => "TestCompanyId";
    private static string JobId => "TestJobId";
    private static string GetCompanyInfoUrl => "https://www.company.com.tw";
    private static string GetJobInfoUrl => "https://www.job.com.tw";
    private static string GetJobListUrl => "https://www.joblist.com.tw";

    private readonly GetJobInfoDto getJobInfoDto = new()
    {
        CompanyId = CompanyId,
        JobId = JobId
    };

    private readonly GetCompanyInfoDto getCompanyInfoDto = new()
    {
        CompanyId = CompanyId
    };

    private readonly ILogger<BaseHttpService> logger = A.Fake<ILogger<BaseHttpService>>();
    private readonly IParameterService parameterService = A.Fake<IParameterService>();
    private readonly ITaskDelayService taskDelayService = A.Fake<ITaskDelayService>();
    private readonly IHtmlAnalyzeService htmlAnalyzeService = A.Fake<IHtmlAnalyzeService>();

    private static string CompanyName => "公司名稱";
    private static string JobName => "職缺名稱";

    [SetUp]
    public void Setup()
    {
        A.CallTo(() => parameterService.CompanyInfoUrl(A<GetCompanyInfoDto>.Ignored)).Returns(GetCompanyInfoUrl);
        A.CallTo(() => parameterService.JobInfoUrl(A<GetJobInfoDto>.Ignored)).Returns(GetJobInfoUrl);
        A.CallTo(() => taskDelayService.WorkerWaiting()).Returns(Task.CompletedTask);
        A.CallTo(() => taskDelayService.Delay(A<TimeSpan>.Ignored)).Returns(Task.CompletedTask);
    }

    private HttpCakeResumeService GetService(string content)
    {
        var httpMessageHandler = new FakeHttpMessageHandler(new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(content)
        });
        var httpClient = new HttpClient(httpMessageHandler);

        return new HttpCakeResumeService(httpClient, parameterService, taskDelayService, htmlAnalyzeService, logger);
    }

    [Test]
    public async Task GetCompanyInfo_從HttpResponse取回空字串_回傳null()
    {
        var service = GetService("");
        var result = await service.GetCompanyInfo<CompanyDto>(getCompanyInfoDto);
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task GetCompanyInfo_從HttpResponse取回json_回傳null()
    {
        var service = GetService(TestValue.NotValidJsonContent);
        var result = await service.GetCompanyInfo<CompanyDto>(getCompanyInfoDto);
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task GetCompanyInfo_取得空的公司名稱_回傳null()
    {
        var service = GetService("fake data");
        A.CallTo(() => htmlAnalyzeService.GetCompanyName(A<HtmlDocument>.Ignored)).Returns("");
        var result = await service.GetCompanyInfo<CompanyDto>(getCompanyInfoDto);
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task GetCompanyInfo_取得公司名稱_無公司介紹_回傳dto()
    {
        var service = GetService("fake data");
        A.CallTo(() => htmlAnalyzeService.GetCompanyName(A<HtmlDocument>.Ignored)).Returns(CompanyName);
        A.CallTo(() => htmlAnalyzeService.GetCompanyCardContentNodes(A<HtmlDocument>.Ignored)).Returns(null);
        var result = await service.GetCompanyInfo<CompanyDto>(getCompanyInfoDto);

        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.EqualTo(CompanyId));
            Assert.That(result.Name, Is.EqualTo(CompanyName));
            Assert.That(result.SourceFrom, Is.EqualTo(ParametersCakeResume.SourceFrom));
            Assert.That(result.Profile, Is.EqualTo("N/A"));
            Assert.That(result.Product, Is.EqualTo("N/A"));
            Assert.That(result.Welfare, Is.EqualTo("N/A"));
        });
    }

    [Test]
    public async Task GetCompanyInfo_取得公司名稱_公司介紹取回null_回傳dto()
    {
        var fakeHtmlNodes = new HtmlNodeCollection(null);

        HtmlNode node1 = HtmlNode.CreateNode("<div>Content 1</div>");
        fakeHtmlNodes.Add(node1);

        var service = GetService("fake data");
        A.CallTo(() => htmlAnalyzeService.GetCompanyName(A<HtmlDocument>.Ignored)).Returns(CompanyName);
        A.CallTo(() => htmlAnalyzeService.GetCompanyCardContentNodes(A<HtmlDocument>.Ignored)).Returns(fakeHtmlNodes);
        A.CallTo(() => htmlAnalyzeService.GetCompanyCardContent(A<HtmlNode>.Ignored)).Returns(null);
        var result = await service.GetCompanyInfo<CompanyDto>(getCompanyInfoDto);

        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.EqualTo(CompanyId));
            Assert.That(result.Name, Is.EqualTo(CompanyName));
            Assert.That(result.SourceFrom, Is.EqualTo(ParametersCakeResume.SourceFrom));
            Assert.That(result.Product, Is.EqualTo("N/A"));
        });
    }

    [Test]
    public async Task GetCompanyInfo_取得公司名稱_公司介紹取回錯誤的Key_回傳dto()
    {
        var content = "fake content";

        var fakeHtmlNodes = new HtmlNodeCollection(null);

        HtmlNode node1 = HtmlNode.CreateNode("<div>Content 1</div>");
        fakeHtmlNodes.Add(node1);

        var service = GetService("fake data");
        A.CallTo(() => htmlAnalyzeService.GetCompanyName(A<HtmlDocument>.Ignored)).Returns(CompanyName);
        A.CallTo(() => htmlAnalyzeService.GetCompanyCardContentNodes(A<HtmlDocument>.Ignored)).Returns(fakeHtmlNodes);
        A.CallTo(() => htmlAnalyzeService.GetCompanyCardContent(A<HtmlNode>.Ignored)).Returns(new KeyValuePair<string, string>("fake key", content));
        var result = await service.GetCompanyInfo<CompanyDto>(getCompanyInfoDto);

        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.EqualTo(CompanyId));
            Assert.That(result.Name, Is.EqualTo(CompanyName));
            Assert.That(result.SourceFrom, Is.EqualTo(ParametersCakeResume.SourceFrom));
            Assert.That(result.Product, Is.EqualTo("N/A"));
        });
    }

    [Test]
    public async Task GetCompanyInfo_取得公司名稱_有公司介紹_回傳dto()
    {
        var content = "fake content";
        var fakeHtmlNodes = new HtmlNodeCollection(null);

        HtmlNode node1 = HtmlNode.CreateNode("<div>Content 1</div>");
        fakeHtmlNodes.Add(node1);

        var service = GetService("fake data");
        A.CallTo(() => htmlAnalyzeService.GetCompanyName(A<HtmlDocument>.Ignored)).Returns(CompanyName);
        A.CallTo(() => htmlAnalyzeService.GetCompanyCardContentNodes(A<HtmlDocument>.Ignored)).Returns(fakeHtmlNodes);
        A.CallTo(() => htmlAnalyzeService.GetCompanyCardContent(A<HtmlNode>.Ignored)).Returns(new KeyValuePair<string, string>(ParametersCakeResume.CompanyContentFilter.Keys.First(), content));
        var result = await service.GetCompanyInfo<CompanyDto>(getCompanyInfoDto);

        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.EqualTo(CompanyId));
            Assert.That(result.Name, Is.EqualTo(CompanyName));
            Assert.That(result.SourceFrom, Is.EqualTo(ParametersCakeResume.SourceFrom));
            Assert.That(result.Product, Is.EqualTo(content));
        });
    }

    [Test]
    public async Task GetJobInfo_從HttpResponse取回空字串_回傳null()
    {
        var service = GetService("");
        var result = await service.GetJobInfo<JobDto>(getJobInfoDto);
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task GetJobInfo_從HttpResponse取回Json_回傳null()
    {
        var service = GetService(TestValue.NotValidJsonContent);
        var result = await service.GetJobInfo<JobDto>(getJobInfoDto);
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task GetJobInfo_職缺取得空字串()
    {
        var service = GetService("fake data");
        A.CallTo(() => htmlAnalyzeService.GetJobName(A<HtmlDocument>.Ignored)).Returns("");
        var result = await service.GetJobInfo<JobDto>(getJobInfoDto);
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task GetJobInfo_職缺取得資訊()
    {
        var service = GetService("fake data");
        A.CallTo(() => htmlAnalyzeService.GetJobName(A<HtmlDocument>.Ignored)).Returns(JobName);
        A.CallTo(() => htmlAnalyzeService.GetJobCardContentNodes(A<HtmlDocument>.Ignored)).Returns(null);
        A.CallTo(() => htmlAnalyzeService.GetJobPlace(A<HtmlDocument>.Ignored)).Returns(null);
        A.CallTo(() => htmlAnalyzeService.GetSalary(A<HtmlDocument>.Ignored)).Returns(null);
        A.CallTo(() => htmlAnalyzeService.GetJobLastUpdateTime(A<HtmlDocument>.Ignored)).Returns(null);
        var result = await service.GetJobInfo<JobDto>(getJobInfoDto);
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.EqualTo(JobId));
            Assert.That(result.Name, Is.EqualTo(JobName));
            Assert.That(result.CompanyId, Is.EqualTo(CompanyId));
            Assert.That(result.WorkContent, Is.EqualTo("N/A"));
            Assert.That(result.JobPlace, Is.EqualTo("N/A"));
            Assert.That(result.OtherRequirement, Is.EqualTo("N/A"));
            Assert.That(result.Salary, Is.EqualTo("N/A"));
            Assert.That(result.LatestUpdateDate, Is.EqualTo("N/A"));
        });
    }

    [Test]
    public async Task GetJobInfo_職缺取得資訊_取得工作地點_取得薪資_取得最後更新時間()
    {
        var service = GetService("fake data");
        A.CallTo(() => htmlAnalyzeService.GetJobName(A<HtmlDocument>.Ignored)).Returns(JobName);
        A.CallTo(() => htmlAnalyzeService.GetJobCardContentNodes(A<HtmlDocument>.Ignored)).Returns(null);
        A.CallTo(() => htmlAnalyzeService.GetJobPlace(A<HtmlDocument>.Ignored)).Returns(nameof(htmlAnalyzeService.GetJobPlace));
        A.CallTo(() => htmlAnalyzeService.GetSalary(A<HtmlDocument>.Ignored)).Returns(nameof(htmlAnalyzeService.GetSalary));
        A.CallTo(() => htmlAnalyzeService.GetJobLastUpdateTime(A<HtmlDocument>.Ignored)).Returns(nameof(htmlAnalyzeService.GetJobLastUpdateTime));
        var result = await service.GetJobInfo<JobDto>(getJobInfoDto);
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.EqualTo(JobId));
            Assert.That(result.Name, Is.EqualTo(JobName));
            Assert.That(result.CompanyId, Is.EqualTo(CompanyId));
            Assert.That(result.WorkContent, Is.EqualTo("N/A"));
            Assert.That(result.JobPlace, Is.EqualTo(nameof(htmlAnalyzeService.GetJobPlace)));
            Assert.That(result.OtherRequirement, Is.EqualTo("N/A"));
            Assert.That(result.Salary, Is.EqualTo(nameof(htmlAnalyzeService.GetSalary)));
            Assert.That(result.LatestUpdateDate, Is.EqualTo(nameof(htmlAnalyzeService.GetJobLastUpdateTime)));
        });
    }

    [Test]
    public async Task GetJobInfo_職缺取得資訊_取得工作內容為null()
    {
        var fakeHtmlNodes = new HtmlNodeCollection(null);

        HtmlNode node1 = HtmlNode.CreateNode("<div>Content 1</div>");
        fakeHtmlNodes.Add(node1);

        var service = GetService("fake data");
        A.CallTo(() => htmlAnalyzeService.GetJobName(A<HtmlDocument>.Ignored)).Returns(JobName);
        A.CallTo(() => htmlAnalyzeService.GetJobCardContentNodes(A<HtmlDocument>.Ignored)).Returns(fakeHtmlNodes);

        A.CallTo(() => htmlAnalyzeService.GetJobCardTitle(A<HtmlNode>.Ignored)).Returns(null);
        var result = await service.GetJobInfo<JobDto>(getJobInfoDto);
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.EqualTo(JobId));
            Assert.That(result.Name, Is.EqualTo(JobName));
            Assert.That(result.CompanyId, Is.EqualTo(CompanyId));
            Assert.That(result.WorkContent, Is.EqualTo("N/A"));
            Assert.That(result.OtherRequirement, Is.EqualTo("N/A"));
        });
    }

    [Test]
    public async Task GetJobInfo_職缺取得資訊_取得工作內容_取回錯誤的CardTitle()
    {
        var fakeHtmlNodes = new HtmlNodeCollection(null);

        HtmlNode node1 = HtmlNode.CreateNode("<div>Content 1</div>");
        fakeHtmlNodes.Add(node1);

        var service = GetService("fake data");
        A.CallTo(() => htmlAnalyzeService.GetJobName(A<HtmlDocument>.Ignored)).Returns(JobName);
        A.CallTo(() => htmlAnalyzeService.GetJobCardContentNodes(A<HtmlDocument>.Ignored)).Returns(fakeHtmlNodes);

        A.CallTo(() => htmlAnalyzeService.GetJobCardTitle(A<HtmlNode>.Ignored)).Returns("fake title");
        var result = await service.GetJobInfo<JobDto>(getJobInfoDto);
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.EqualTo(JobId));
            Assert.That(result.Name, Is.EqualTo(JobName));
            Assert.That(result.CompanyId, Is.EqualTo(CompanyId));
            Assert.That(result.WorkContent, Is.EqualTo("N/A"));
            Assert.That(result.OtherRequirement, Is.EqualTo("N/A"));
        });
    }

    [Test]
    public async Task GetJobInfo_職缺取得資訊_取得工作內容_取回正確的CardTitle_取得職務內容()
    {
        var jobContent = "fake job content";
        var fakeHtmlNodes = new HtmlNodeCollection(null);

        HtmlNode node1 = HtmlNode.CreateNode("<div>Content 1</div>");
        fakeHtmlNodes.Add(node1);

        var service = GetService("fake data");
        A.CallTo(() => htmlAnalyzeService.GetJobName(A<HtmlDocument>.Ignored)).Returns(JobName);
        A.CallTo(() => htmlAnalyzeService.GetJobCardContentNodes(A<HtmlDocument>.Ignored)).Returns(fakeHtmlNodes);

        A.CallTo(() => htmlAnalyzeService.GetJobCardTitle(A<HtmlNode>.Ignored)).Returns(ParametersCakeResume.JobContentFilter.Values.First()[0]);
        A.CallTo(() => htmlAnalyzeService.GetJobCardContent(A<HtmlNode>.Ignored)).Returns(jobContent);
        var result = await service.GetJobInfo<JobDto>(getJobInfoDto);
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.EqualTo(JobId));
            Assert.That(result.Name, Is.EqualTo(JobName));
            Assert.That(result.CompanyId, Is.EqualTo(CompanyId));
            Assert.That(result.WorkContent, Is.EqualTo(jobContent));
            Assert.That(result.OtherRequirement, Is.EqualTo("N/A"));
        });
    }

    [Test]
    public async Task GetJobList_從HttpResponse取回空字串_回傳null()
    {
        var service = GetService("");
        var result = await service.GetJobList<JobListDto<SimpleJobInfoDto>>(GetJobListUrl);
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task GetJobList_從HttpResponse取回Json_回傳null()
    {
        var service = GetService(TestValue.NotValidJsonContent);
        var result = await service.GetJobList<JobListDto<SimpleJobInfoDto>>(GetJobListUrl);
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task GetJobList_職缺內容的HtmlNodeCollection為null()
    {
        var service = GetService("<div>fake data</div>");
        A.CallTo(() => htmlAnalyzeService.GetJobListCardContentNode(A<HtmlDocument>.Ignored)).Returns(null);
        var result = await service.GetJobList<JobListDto<SimpleJobInfoDto>>(GetJobListUrl);
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task GetJobList_職缺內容的HtmlNodeCollection為有值_職缺資訊數量為0()
    {
        var fakeHtmlNodes = new HtmlNodeCollection(null);

        HtmlNode node1 = HtmlNode.CreateNode("<div>Content 1</div>");
        fakeHtmlNodes.Add(node1);

        var service = GetService("<div>fake data</div>");
        A.CallTo(() => htmlAnalyzeService.GetJobListCardContentNode(A<HtmlDocument>.Ignored)).Returns(fakeHtmlNodes);
        A.CallTo(() => htmlAnalyzeService.GetJobListJobNode(A<HtmlNode>.Ignored)).Returns(null);
        A.CallTo(() => htmlAnalyzeService.GetJobListCompanyNode(A<HtmlNode>.Ignored)).Returns(node1);

        var result = await service.GetJobList<JobListDto<SimpleJobInfoDto>>(GetJobListUrl);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.JobList, Is.Not.Null);
        Assert.That(result.JobList.Count, Is.EqualTo(0));
    }

    [Test]
    public async Task GetJobList_職缺內容的HtmlNodeCollection為有值_職缺資訊數量不為0()
    {
        var fakeHtmlNodes = new HtmlNodeCollection(null);

        HtmlNode companyNode = HtmlNode.CreateNode($"<a href='{GetCompanyInfoUrl}/{CompanyId}'>Content 1</div>");
        HtmlNode jobNode = HtmlNode.CreateNode($"<a href='{GetJobInfoUrl}/{JobId}'>Content 1</div>");
        fakeHtmlNodes.Add(companyNode);

        var service = GetService("<div>fake data</div>");
        A.CallTo(() => htmlAnalyzeService.GetJobListCardContentNode(A<HtmlDocument>.Ignored)).Returns(fakeHtmlNodes);
        A.CallTo(() => htmlAnalyzeService.GetJobListJobNode(A<HtmlNode>.Ignored)).Returns(jobNode);
        A.CallTo(() => htmlAnalyzeService.GetJobListCompanyNode(A<HtmlNode>.Ignored)).Returns(companyNode);

        var result = await service.GetJobList<JobListDto<SimpleJobInfoDto>>(GetJobListUrl);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.JobList, Is.Not.Null);
        Assert.That(result.JobList.Count, Is.Not.EqualTo(0));
        Assert.Multiple(() =>
        {
            Assert.That(result.JobList.First().JobId, Is.EqualTo(JobId));
            Assert.That(result.JobList.First().CompanyId, Is.EqualTo(CompanyId));
        });
    }
}
