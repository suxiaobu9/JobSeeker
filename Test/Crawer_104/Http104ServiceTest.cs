using Crawer_104.Service;
using Model.Dto;
using Model.Dto104;
using Service.Http;
using Service.Parameter;
using System;
using System.Net;
using System.Text.Json;

namespace Test.Crawer_104;

public class Http104ServiceTest
{
    private static string CompanyId => "TestCompanyId";
    private static string JobId => "TestJobId";
    private static string GetCompanyInfoUrl => "https://www.company.com.tw";
    private static string GetJobInfoUrl => "https://www.job.com.tw";
    private readonly ILogger<BaseHttpService> logger = A.Fake<ILogger<BaseHttpService>>();
    private readonly IHttpClientFactory httpClientFactory = A.Fake<IHttpClientFactory>();
    private readonly IParameterService parameterService = A.Fake<IParameterService>();
    private readonly string ValidCompanyJsonContent = "{\"data\":{\"custSwitch\":\"on\",\"custName\":\"科技股份有限公司\",\"custNo\":13035663000,\"industryDesc\":\"網際網路相關業\",\"indcat\":\"網際網路相關業\",\"empNo\":\"10人\",\"capital\":\"00萬元\",\"address\":\"台北市大安區敦化南路\",\"industryArea\":null,\"custLink\":\"http://www.example.com.tw\",\"profile\":\"股份有限公司\",\"product\":\"網頁平台製作站建置\",\"welfare\":\"彈性上下班時間\",\"management\":\"品質創新卓越\",\"phone\":\"02-12345678\",\"fax\":\"02-12345678\",\"hrName\":\"人資\",\"lat\":\"22.028723\",\"lon\":\"122.548946\",\"logo\":\"//static.104.com.tw/b_profile\",\"news\":\"強力募集\",\"newsLink\":\"\",\"zone\":{},\"linkMore\":{},\"corpImage2\":\"\",\"corpImage1\":\"\",\"corpImage3\":\"\",\"corpLink2\":\"\",\"corpLink1\":\"\",\"corpLink3\":\"\",\"productPictures\":[{\"auto_no\":890,\"custno\":\"3000\",\"picture_sort\":1,\"file\":\"47422.jpg\",\"description\":\"\",\"is_cover\":0,\"type\":1,\"input_date\":\"2021-10-26 12:16:06\",\"link_l\":\"//static.104.com.tw/b_profile\",\"link_s\":\"//static.104.com.tw\"}],\"envPictures\":[{\"auto_no\":9330200,\"custno\":3000,\"file\":\"asd.jpg\",\"description\":\"門口\",\"is_cover\":0,\"type\":2,\"input_date\":\"2022-08-19 19:12:22\",\"update_date\":\"2022-08-19 19:12:23\",\"picture_sort\":1,\"link_l\":\"//static.104.com.tw/b_profile\",\"link_s\":\"//static.104.com.tw/\"},{\"auto_no\":742400,\"custno\":63000,\"file\":\"742453.jpg\",\"description\":\"辦公區域\",\"is_cover\":0,\"type\":2,\"input_date\":\"2022-08-19 19:12:22\",\"update_date\":\"2023-03-02 12:02:46\",\"picture_sort\":2,\"link_l\":\"//static.104.com.tw/b_profile/\",\"link_s\":\"//static.104.com.tw/b_profile/\"},{\"auto_no\":42400,\"custno\":3000,\"file\":\"2454.jpg\",\"description\":\"A區辦公室走道\",\"is_cover\":0,\"type\":2,\"input_date\":\"2022-08-19 19:12:22\",\"update_date\":\"2022-08-19 19:12:23\",\"picture_sort\":3,\"link_l\":\"//static.104.com.tw/b_profile/\",\"link_s\":\"//static.104.com.tw/b_profile/\"},{\"auto_no\":2400,\"custno\":13035663000,\"file\":\"42455.jpg\",\"description\":\"B區辦公室走道\",\"is_cover\":0,\"type\":2,\"input_date\":\"2022-08-19 19:12:22\",\"update_date\":\"2022-08-19 19:12:23\",\"picture_sort\":4,\"link_l\":\"//static.104.com.tw/b_profile/\",\"link_s\":\"//static.104.com.tw/b_profile\"},{\"auto_no\":6742400,\"custno\":13035663000,\"file\":\"42456.jpg\",\"description\":\"訓練教室\",\"is_cover\":0,\"type\":2,\"input_date\":\"2022-08-19 19:12:22\",\"update_date\":\"2023-03-02 12:02:46\",\"picture_sort\":5,\"link_l\":\"//static.104.com.tw/b_profile/\",\"link_s\":\"//static.104.com.tw/b_profile\"},{\"auto_no\":964488071839330200,\"custno\":3000,\"file\":\"30197.jpg\",\"description\":\"VIP會議室\",\"is_cover\":0,\"type\":2,\"input_date\":\"2022-08-19 19:12:22\",\"update_date\":\"2023-03-02 12:02:46\",\"picture_sort\":6,\"link_l\":\"//static.104.com.tw/b_profile\",\"link_s\":\"//static.104.com.tw/b_profile\"},{\"auto_no\":26742400,\"custno\":13035663000,\"file\":\"26742457.jpg\",\"description\":\"開放空間討論區\",\"is_cover\":0,\"type\":2,\"input_date\":\"2022-08-19 19:12:22\",\"update_date\":\"2023-03-02 12:02:46\",\"picture_sort\":7,\"link_l\":\"//static.104.com.tw/b_profile/\",\"link_s\":\"//static.104.com.tw/b_profile\"},{\"auto_no\":181300,\"custno\":63000,\"file\":\"1346.jpg\",\"description\":\"開放空間討論區\",\"is_cover\":0,\"type\":2,\"input_date\":\"2021-10-26 14:55:46\",\"update_date\":\"2023-03-02 12:02:46\",\"picture_sort\":8,\"link_l\":\"//static.104.com.tw/b_profile\",\"link_s\":\"//static.104.com.tw/b_profile/\"},{\"auto_no\":103300,\"custno\":\"3000\",\"picture_sort\":9,\"file\":\"3335.jpg\",\"description\":\"直播錄音室\",\"is_cover\":0,\"type\":2,\"input_date\":\"2023-03-02 12:06:18\",\"link_l\":\"//static.104.com.tw/b_profile\",\"link_s\":\"//static.104.com.tw/b_profile/\"}],\"tagNames\":[\"年終獎金\",\"三節獎金/禮品\",\"咖啡吧\",\"結婚禮金\",\"生育津貼\",\"國內旅遊\",\"國外旅遊\"],\"legalTagNames\":[\"週休二日\",\"勞保\",\"健保\",\"特別休假\",\"員工體檢\",\"職災保險\"],\"historys\":[],\"isSaved\":false,\"isTracked\":false,\"addrNoDesc\":\"台北市大安區\",\"reportUrl\":\"//www.104.com.tw/feedback?\",\"postalCode\":106},\"metadata\":{\"seo\":{\"noindex\":false}}}";
    private readonly string ValidJobJsonContent = "{\"data\":{\"corpImageRight\":{\"corpImageRight\":{\"imageUrl\":\"\",\"link\":\"\"}},\"header\":{\"corpImageTop\":{\"imageUrl\":\"\",\"link\":\"\"},\"jobName\":\"工程師\",\"appearDate\":\"2023/08/15\",\"custName\":\"股份有限公司\",\"custUrl\":\"https://www.com.tw/company\",\"applyDate\":\"\",\"analysisType\":2,\"analysisUrl\":\"//www.com.tw/jobs/apply/analysis/\",\"isSaved\":false,\"isApplied\":false},\"contact\":{\"hrName\":\"先生\",\"email\":\"\",\"visit\":\"\",\"phone\":[],\"other\":\"\",\"reply\":\"\",\"suggestExam\":false},\"environmentPic\":{\"environmentPic\":[{\"thumbnailLink\":\"https://static.104.com.tw/b_profile/\",\"link\":\"https://static.104.com.tw/b_profile/\",\"description\":\"出入口\"},{\"thumbnailLink\":\"https://static.104.com.tw/b_profile/\",\"link\":\"https://static.104.com.tw/b_profile/\",\"description\":\"辦公空間\"},{\"thumbnailLink\":\"https://static.104.com.tw/b_profile/\",\"link\":\"https://static.104.com.tw/b_profile/\",\"description\":\"室開會更舒適\"},{\"thumbnailLink\":\"https://static.104.com.tw/b_profile/\",\"link\":\"https://static.104.com.tw/b_profile/\",\"description\":\"每天供應\"},{\"thumbnailLink\":\"https://static.104.com.tw/b_profile/\",\"link\":\"https://static.104.com.tw/b_profile/\",\"description\":\"2存戰\"},{\"thumbnailLink\":\"https://static.104.com.tw/b_profile/\",\"link\":\"https://static.104.com.tw/b_profile/\",\"description\":\"2晚宴\"},{\"thumbnailLink\":\"https://static.104.com.tw/b_profile/\",\"link\":\"https://static.104.com.tw/b_profile/\",\"description\":\"202館\"},{\"thumbnailLink\":\"https://static.104.com.tw/b_profile/\",\"link\":\"https://static.104.com.tw/b_profile/\",\"description\":\"20旅遊\"},{\"thumbnailLink\":\"https://static.104.com.tw/b_profile/\",\"link\":\"https://static.104.com.tw/b_profile/\",\"description\":\"201禮物\"},{\"thumbnailLink\":\"https://static.104.com.tw/b_profile/\",\"link\":\"https://static.104.com.tw/b_profile/\",\"description\":\"20踢\"}],\"corpImageBottom\":{\"imageUrl\":\"\",\"link\":\"\"}},\"condition\":{\"acceptRole\":{\"role\":[{\"code\":2,\"description\":\"應屆畢業生\"},{\"code\":64,\"description\":\"原住民\"}],\"disRole\":{\"needHandicapCompendium\":false,\"disability\":[]}},\"workExp\":\"不拘\",\"edu\":\"專科以上\",\"major\":[\"資訊管理相關\",\"資訊工程相關\"],\"language\":[{\"code\":1,\"language\":\"英文\",\"ability\":\"聽 /略懂、說 /略懂、讀 /略懂、寫 /略懂\"}],\"localLanguage\":[],\"specialty\":[{\"code\":\"12001003006\",\"description\":\"ASP.NET\"},{\"code\":\"12001003009\",\"description\":\"C#\"},{\"code\":\"12001004031\",\"description\":\"MS SQL\"},{\"code\":\"12001006017\",\"description\":\"JavaScript\"},{\"code\":\"12001006030\",\"description\":\"jQuery\"},{\"code\":\"12001006036\",\"description\":\"ReactJS\"},{\"code\":\"12001006044\",\"description\":\"Angular\"}],\"skill\":[{\"code\":\"11009005001\",\"description\":\"軟體程式設計\"}],\"certificate\":[],\"driverLicense\":[],\"other\":\"1.完成學員投遞履歷\\n\\n\"},\"welfare\":{\"tag\":[\"年終獎金\",\"三節獎金/禮品\",\"零食櫃\",\"員工舒壓按摩\",\"生日假\",\"不扣薪病假\",\"國內旅遊\",\"國外旅遊\",\"優於勞基法特休\",\"員工團體保險\"],\"welfare\":\"員工團體優於國外旅遊勞基法特休保險\",\"legalTag\":[\"週休二日\",\"勞保\",\"健保\",\"陪產假\",\"產假\",\"特別休假\",\"育嬰留停\",\"女性生理假\",\"勞退提撥金\",\"安胎假\",\"產檢假\",\"防疫照顧假\",\"員工體檢\",\"職災保險\"]},\"jobDetail\":{\"jobDescription\":\"系統建置工作\\n\",\"jobCategory\":[{\"code\":\"2007001004\",\"description\":\"軟體工程師\"},{\"code\":\"2007001006\",\"description\":\"Internet程式設計師\"},{\"code\":\"2007002003\",\"description\":\"MIS程式設計師\"}],\"salary\":\"月薪300~400元\",\"salaryMin\":300,\"salaryMax\":400,\"salaryType\":50,\"jobType\":1,\"workType\":[],\"addressNo\":\"6001001004\",\"addressRegion\":\"台北市松山區\",\"addressArea\":\"台北市\",\"addressDetail\":\"南京東路\",\"industryArea\":\"\",\"longitude\":\"122.5553512\",\"latitude\":\"21.3514056\",\"manageResp\":\"不需負擔管理責任\",\"businessTrip\":\"無需出差外派\",\"workPeriod\":\"日班\",\"vacationPolicy\":\"週休二日\",\"startWorkingDay\":\"不限\",\"hireType\":0,\"delegatedRecruit\":\"\",\"needEmp\":\"2~4人\",\"landmark\":\"距捷運台北小巨蛋站約350公尺\",\"remoteWork\":null},\"switch\":\"on\",\"custLogo\":\"https://static.104.com.tw/b_profile/\",\"postalCode\":\"105\",\"closeDate\":\"\",\"industry\":\"其它軟體及網路相關業\",\"custNo\":\"28458252000\",\"reportUrl\":\"https://static.104.com.tw/b_profile/\",\"industryNo\":\"1001\",\"employees\":\"140人\",\"chinaCorp\":false},\"metadata\":{\"enableHTML\":false,\"hiddenBanner\":false,\"seo\":{\"noindex\":false}}}";

    private readonly GetJobInfoDto getJobInfoDto = new()
    {
        CompanyId = CompanyId,
        JobId = JobId
    };

    private readonly GetCompanyInfoDto getCompanyInfoDto = new()
    {
        CompanyId = CompanyId
    };

    private Http104Service GetService(string content)
    {
        var httpMessageHandler = new FakeHttpMessageHandler(new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(content)
        });

        var httpClient = new HttpClient(httpMessageHandler);

        A.CallTo(() => httpClientFactory.CreateClient(A<string>.Ignored)).Returns(httpClient);

        return new Http104Service(logger, parameterService, httpClientFactory);
    }

    [SetUp]
    public void Setup()
    {
        A.CallTo(() => parameterService.CompanyInfoUrl(A<GetCompanyInfoDto>.Ignored)).Returns(GetCompanyInfoUrl);
        A.CallTo(() => parameterService.JobInfoUrl(A<GetJobInfoDto>.Ignored)).Returns(GetJobInfoUrl);
    }

    [Test]
    public async Task GetCompanyInfo_從HttpResponse取回空字串_回傳null()
    {
        var service = GetService("");

        var result = await service.GetCompanyInfo<CompanyDto>(getCompanyInfoDto);

        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task GetJobInfo_從HttpResponse取回空字串_回傳null()
    {
        var service = GetService("");

        var result = await service.GetJobInfo<JobDto>(getJobInfoDto);

        Assert.That(result, Is.Null);
    }

    [Test]
    public Task GetCompanyInfo_從HttpResponse取回html_拋出例外錯誤()
    {
        var service = GetService("<html></html>");
        Assert.ThrowsAsync<JsonException>(async () => await service.GetCompanyInfo<CompanyDto>(getCompanyInfoDto));
        return Task.CompletedTask;
    }

    [Test]
    public Task GetJobInfo_從HttpResponse取回html_拋出例外錯誤()
    {
        var service = GetService("<html></html>");
        Assert.ThrowsAsync<JsonException>(async () => await service.GetJobInfo<JobDto>(getJobInfoDto));
        return Task.CompletedTask;
    }

    [Test]
    public async Task GetCompanyInfo_從HttpResponse取回Json字串_轉型變null()
    {
        var service = GetService("{\"TestProperty\":\"TestValue\"}");

        var result = await service.GetCompanyInfo<CompanyDto>(getCompanyInfoDto);

        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task GetJobInfo_從HttpResponse取回Json字串_轉型變null()
    {
        var service = GetService("{\"TestProperty\":\"TestValue\"}");

        var result = await service.GetJobInfo<JobDto>(getJobInfoDto);

        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task GetCompanyInfo_從HttpResponse取回Json字串_轉型變成功()
    {
        var service = GetService(ValidCompanyJsonContent);

        var result = await service.GetCompanyInfo<CompanyDto>(getCompanyInfoDto);

        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.EqualTo("TestCompanyId"));
            Assert.That(result.Name, Is.EqualTo("科技股份有限公司"));
            Assert.That(result.Product, Is.EqualTo("網頁平台製作站建置"));
            Assert.That(result.Profile, Is.EqualTo("股份有限公司"));
            Assert.That(result.SourceFrom, Is.EqualTo(Parameters104.SourceFrom));
            Assert.That(result.Welfare, Is.EqualTo("彈性上下班時間"));
        });
    }

    [Test]
    public async Task GetJobInfo_從HttpResponse取回Json字串_轉型變成功()
    {
        var service = GetService(ValidJobJsonContent);

        var result = await service.GetJobInfo<JobDto>(getJobInfoDto);

        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.EqualTo("TestJobId"));
            Assert.That(result.Name, Is.EqualTo("工程師"));
            Assert.That(result.CompanyId, Is.EqualTo("TestCompanyId"));
            Assert.That(result.JobPlace, Is.EqualTo("台北市松山區"));
            Assert.That(result.OtherRequirement, Is.EqualTo("1.完成學員投遞履歷\n\n"));
            Assert.That(result.Salary, Is.EqualTo("月薪300~400元"));
            Assert.That(result.WorkContent, Is.EqualTo("系統建置工作\n"));
            Assert.That(result.LatestUpdateDate, Is.EqualTo("2023/08/15"));
        });
    }
}

public class FakeHttpMessageHandler : HttpMessageHandler
{
    private readonly HttpResponseMessage _response;

    public FakeHttpMessageHandler(HttpResponseMessage response)
    {
        _response = response;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        return Task.FromResult(_response);
    }
}
