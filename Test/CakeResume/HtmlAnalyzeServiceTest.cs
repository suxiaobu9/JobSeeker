using Crawer_CakeResume.Service;
using HtmlAgilityPack;
using Model.DtoCakeResume;
using System.Net.Mime;
using System.Text;

namespace Test.CakeResume;

public class HtmlAnalyzeServiceTest
{
    private readonly ILogger<HtmlAnalyzeService> logger = A.Fake<ILogger<HtmlAnalyzeService>>();

    [Test]
    public void GetCompanyName_Content為空字串()
    {
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml("");

        var service = new HtmlAnalyzeService(logger);

        var result = service.GetCompanyName(htmlDoc);

        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetCompanyName_Content取得Json()
    {
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(TestValue.NotValidJsonContent);

        var service = new HtmlAnalyzeService(logger);

        var result = service.GetCompanyName(htmlDoc);

        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetCompanyName_Content不包含正確的DivClassName()
    {
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml("<div class='FakeClassName'>Fake Data</div>");

        var service = new HtmlAnalyzeService(logger);

        var result = service.GetCompanyName(htmlDoc);

        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetCompanyName_Content包含正確的DivClassName()
    {
        var content = "Fake Data";
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml($"<div class='{ParametersCakeResume.CompanyNameDivClass}'>{content}</div>");

        var service = new HtmlAnalyzeService(logger);

        var result = service.GetCompanyName(htmlDoc);

        Assert.That(result, Is.EqualTo(content));
    }

    [Test]
    public void GetCompanyCardContentNodes_Content為空字串()
    {
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml("");

        var service = new HtmlAnalyzeService(logger);

        var result = service.GetCompanyCardContentNodes(htmlDoc);

        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetCompanyCardContentNodes_Content取得Json()
    {
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(TestValue.NotValidJsonContent);

        var service = new HtmlAnalyzeService(logger);

        var result = service.GetCompanyCardContentNodes(htmlDoc);

        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetCompanyCardContentNodes_Content不包含正確的DivClassName()
    {
        var content = new StringBuilder();
        content.Append($"<div class='fake_class_name'>Fake Data</div>");
        content.Append($"<div class='fake_class_name'>Fake Data</div>");
        content.Append($"<div class='fake_class_name'>Fake Data</div>");

        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(content.ToString());

        var service = new HtmlAnalyzeService(logger);

        var result = service.GetCompanyCardContentNodes(htmlDoc);

        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetCompanyCardContentNodes_Content包含正確的DivClassName()
    {
        var content = new StringBuilder();
        content.Append($"<div class='{ParametersCakeResume.CompanyInfoDivClass}'>Fake Data</div>");
        content.Append($"<div class='{ParametersCakeResume.CompanyInfoDivClass}'>Fake Data</div>");
        content.Append($"<div class='{ParametersCakeResume.CompanyInfoDivClass}'>Fake Data</div>");

        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(content.ToString());

        var service = new HtmlAnalyzeService(logger);

        var result = service.GetCompanyCardContentNodes(htmlDoc);

        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void GetCompanyCardContent_Content為空字串()
    {
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml("<div></div>");

        var htmlNode = htmlDoc.DocumentNode.SelectNodes("//div")[0];
        var service = new HtmlAnalyzeService(logger);

        var result = service.GetCompanyCardContent(htmlNode);

        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetCompanyCardContent_Content不包含正確的H2ClassName()
    {
        var content = new StringBuilder();
        content.Append($"<div><h2 class='fake_class_name'>Fake Data</h2></div>");

        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(content.ToString());

        var htmlNode = htmlDoc.DocumentNode.SelectNodes("//div")[0];

        var service = new HtmlAnalyzeService(logger);

        var result = service.GetCompanyCardContent(htmlNode);

        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetCompanyCardContentNodes_Content包含正確的DivClassName_不含正確的TitleKey()
    {
        var content = new StringBuilder();
        content.Append($"<div><h2 class='{ParametersCakeResume.CompanyCardH2Class}'>Fake Data</h2></div>");

        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(content.ToString());
        var htmlNode = htmlDoc.DocumentNode.SelectNodes("//div")[0];

        var service = new HtmlAnalyzeService(logger);

        var result = service.GetCompanyCardContent(htmlNode);

        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetCompanyCardContentNodes_Content包含正確的DivClassName_含正確的TitleKey_不包含詳細內容Div()
    {
        var content = new StringBuilder();
        content.Append($"<div><h2 class='{ParametersCakeResume.CompanyCardH2Class}'>{ParametersCakeResume.ProductsOrServicesHtmlTitle[0]}</h2></div>");

        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(content.ToString());
        var htmlNode = htmlDoc.DocumentNode.SelectNodes("//div")[0];

        var service = new HtmlAnalyzeService(logger);

        var result = service.GetCompanyCardContent(htmlNode);

        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetCompanyCardContentNodes_Content包含正確的DivClassName_含正確的TitleKey_包含詳細內容Div()
    {
        var value = ParametersCakeResume.CompanyContentFilter.Values.First()[0];
        var key = ParametersCakeResume.CompanyContentFilter.Keys.First();
        var divContent = "Fake Data";
        var content = new StringBuilder();
        content.Append($"<div class='fake_class'>");
        content.Append($"<h2 class='{ParametersCakeResume.CompanyCardH2Class}'>{value}</h2>");
        content.Append($"<div class='{ParametersCakeResume.CompanyCardDivClass}'>{divContent}</div>");
        content.Append($"</div>");

        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(content.ToString());
        var htmlNode = htmlDoc.DocumentNode.SelectNodes(".//div[contains(@class, 'fake_class')]")[0];

        var service = new HtmlAnalyzeService(logger);

        var result = service.GetCompanyCardContent(htmlNode);

        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.Value.Key, Is.EqualTo(key));
            Assert.That(result.Value.Value, Is.EqualTo(divContent));
        });
    }

    [Test]
    public void GetJobName_Content為空字串()
    {
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml("");

        var service = new HtmlAnalyzeService(logger);

        var result = service.GetJobName(htmlDoc);

        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetJobName_Content為Json()
    {
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(TestValue.NotValidJsonContent);

        var service = new HtmlAnalyzeService(logger);

        var result = service.GetJobName(htmlDoc);

        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetJobName_Content不包含正確的H2_Class()
    {
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml("<h2 class='fake_h2_class'>Fake Data</h2>");

        var service = new HtmlAnalyzeService(logger);

        var result = service.GetJobName(htmlDoc);

        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetJobName_Content包含正確的H2_Class()
    {
        var content = "Fake Data";
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml($"<h2 class='{ParametersCakeResume.JobNameDivClass}'>{content}</h2>");

        var service = new HtmlAnalyzeService(logger);

        var result = service.GetJobName(htmlDoc);

        Assert.That(result, Is.EqualTo(content));
    }

    [Test]
    public void GetJobPlace_Content為空字串()
    {
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml("");

        var service = new HtmlAnalyzeService(logger);

        var result = service.GetJobPlace(htmlDoc);

        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetJobPlace_Content為Json()
    {
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(TestValue.NotValidJsonContent);

        var service = new HtmlAnalyzeService(logger);

        var result = service.GetJobPlace(htmlDoc);

        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetJobPlace_Content不包含正確的a_Class()
    {
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml("<a class='fake_h2_class'>Fake Data</a>");

        var service = new HtmlAnalyzeService(logger);

        var result = service.GetJobPlace(htmlDoc);

        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetJobPlace_Content包含正確的a_Class()
    {
        var content = "Fake Data";
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml($"<a class='{ParametersCakeResume.JobPlaceAClass}'>{content}</a>");

        var service = new HtmlAnalyzeService(logger);

        var result = service.GetJobPlace(htmlDoc);

        Assert.That(result, Is.EqualTo(content));
    }

    [Test]
    public void GetSalary_Content為空字串()
    {
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml("");

        var service = new HtmlAnalyzeService(logger);

        var result = service.GetSalary(htmlDoc);

        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetSalary_Content為Json()
    {
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(TestValue.NotValidJsonContent);

        var service = new HtmlAnalyzeService(logger);

        var result = service.GetSalary(htmlDoc);

        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetSalary_Content不包含正確的div_Class()
    {
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml("<div class='fake_h2_class'>Fake Data</div>");

        var service = new HtmlAnalyzeService(logger);

        var result = service.GetSalary(htmlDoc);

        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetSalary_Content包含正確的div_Class()
    {
        var content = "Fake Data";
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml($"<div class='{ParametersCakeResume.SalaryDivClass}'>{content}</div>");

        var service = new HtmlAnalyzeService(logger);

        var result = service.GetSalary(htmlDoc);

        Assert.That(result, Is.EqualTo(content));
    }

    [Test]
    public void GetJobLastUpdateTime_Content為空字串()
    {
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml("");

        var service = new HtmlAnalyzeService(logger);

        var result = service.GetJobLastUpdateTime(htmlDoc);

        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetJobLastUpdateTime_Content為Json()
    {
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(TestValue.NotValidJsonContent);

        var service = new HtmlAnalyzeService(logger);

        var result = service.GetJobLastUpdateTime(htmlDoc);

        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetJobLastUpdateTime_Content不包含正確的div_Class()
    {
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml("<div class='fake_h2_class'>Fake Data</div>");

        var service = new HtmlAnalyzeService(logger);

        var result = service.GetJobLastUpdateTime(htmlDoc);

        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetJobLastUpdateTime_Content包含正確的div_Class()
    {
        var content = "Fake Data";
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml($"<div class='{ParametersCakeResume.LatestUpdateDateOuterDivClass}'><div class='{ParametersCakeResume.LatestUpdateDateDivClass}'>{content}</div></div>");

        var service = new HtmlAnalyzeService(logger);

        var result = service.GetJobLastUpdateTime(htmlDoc);

        Assert.That(result, Is.EqualTo(content));
    }

    [Test]
    public void GetJobCardContentNodes_Content為空字串()
    {
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml("");

        var service = new HtmlAnalyzeService(logger);

        var result = service.GetJobCardContentNodes(htmlDoc);

        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetJobCardContentNodes_Content為Json()
    {
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(TestValue.NotValidJsonContent);

        var service = new HtmlAnalyzeService(logger);

        var result = service.GetJobCardContentNodes(htmlDoc);

        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetJobCardContentNodes_Content不包含正確的div_Class()
    {
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml("<div class='fake_h2_class'>Fake Data</div>");

        var service = new HtmlAnalyzeService(logger);

        var result = service.GetJobCardContentNodes(htmlDoc);

        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetJobCardContentNodes_Content包含正確的div_Class()
    {
        var content = "Fake Data";
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml($"<div class='{ParametersCakeResume.JobCardContentDivClass}'>{content}</div>");

        var service = new HtmlAnalyzeService(logger);

        var result = service.GetJobCardContentNodes(htmlDoc);

        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void GetJobListCardContentNode_Content為空字串()
    {
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml("");

        var service = new HtmlAnalyzeService(logger);

        var result = service.GetJobListCardContentNode(htmlDoc);

        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetJobListCardContentNode_Content為Json()
    {
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(TestValue.NotValidJsonContent);

        var service = new HtmlAnalyzeService(logger);

        var result = service.GetJobListCardContentNode(htmlDoc);

        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetJobListCardContentNode_Content不包含正確的div_Class()
    {
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml("<div class='fake_h2_class'>Fake Data</div>");

        var service = new HtmlAnalyzeService(logger);

        var result = service.GetJobListCardContentNode(htmlDoc);

        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetJobListCardContentNode_Content包含正確的div_Class()
    {
        var content = "Fake Data";
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml($"<div class='{ParametersCakeResume.JobListCardContentDivClassName}'>{content}</div>");

        var service = new HtmlAnalyzeService(logger);

        var result = service.GetJobListCardContentNode(htmlDoc);

        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void GetJobCardTitle_Content為空字串()
    {
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml("<div></div>");

        var htmlNode = htmlDoc.DocumentNode.SelectNodes("//div")[0];
        var service = new HtmlAnalyzeService(logger);

        var result = service.GetJobCardTitle(htmlNode);

        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetJobCardTitle_Content不包含正確的H3ClassName()
    {
        var content = new StringBuilder();
        content.Append($"<div><h3 class='fake_class_name'>Fake Data</h3></div>");

        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(content.ToString());

        var htmlNode = htmlDoc.DocumentNode.SelectNodes("//div")[0];

        var service = new HtmlAnalyzeService(logger);

        var result = service.GetJobCardTitle(htmlNode);

        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetJobCardTitle_Content包含正確的H3ClassName()
    {
        var innerText = "Fake Data";
        var content = new StringBuilder();
        content.Append($"<div><h3 class='{ParametersCakeResume.JobCardInnerTitleH3ClassName}'>{innerText}</h3></div>");

        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(content.ToString());

        var htmlNode = htmlDoc.DocumentNode.SelectNodes("//div")[0];

        var service = new HtmlAnalyzeService(logger);

        var result = service.GetJobCardTitle(htmlNode);

        Assert.That(result, Is.EqualTo(innerText));
    }

    [Test]
    public void GetJobCardContent_Content為空字串()
    {
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml("<div></div>");

        var htmlNode = htmlDoc.DocumentNode.SelectNodes("//div")[0];
        var service = new HtmlAnalyzeService(logger);

        var result = service.GetJobCardContent(htmlNode);

        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetJobCardContent_Content不包含正確的DivClassName()
    {
        var content = new StringBuilder();
        content.Append($"<div class='fake'><div class='fake_class_name'>Fake Data</div></div>");

        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(content.ToString());

        var htmlNode = htmlDoc.DocumentNode.SelectNodes("//div[@class='fake']")[0];

        var service = new HtmlAnalyzeService(logger);

        var result = service.GetJobCardContent(htmlNode);

        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetJobCardContent_Content包含正確的DivClassName()
    {
        var innerText = "Fake Data";
        var content = new StringBuilder();
        content.Append($"<div class='fake'><div class='{ParametersCakeResume.JobCardInnerContentDivClass}'>{innerText}</div></div>");

        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(content.ToString());

        var htmlNode = htmlDoc.DocumentNode.SelectNodes("//div[@class='fake']")[0];

        var service = new HtmlAnalyzeService(logger);

        var result = service.GetJobCardContent(htmlNode);

        Assert.That(result, Is.EqualTo(innerText));
    }

    [Test]
    public void GetJobListCompanyNode_Content為空字串()
    {
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml("<div></div>");

        var htmlNode = htmlDoc.DocumentNode.SelectNodes("//div")[0];
        var service = new HtmlAnalyzeService(logger);

        var result = service.GetJobListCompanyNode(htmlNode);

        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetJobListCompanyNode_Content不包含正確的DivClassName()
    {
        var content = new StringBuilder();
        content.Append($"<div class='fake'><a class='fake_class_name'>Fake Data</a></div>");

        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(content.ToString());

        var htmlNode = htmlDoc.DocumentNode.SelectNodes("//div[@class='fake']")[0];

        var service = new HtmlAnalyzeService(logger);

        var result = service.GetJobListCompanyNode(htmlNode);

        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetJobListCompanyNode_Content包含正確的DivClassName()
    {
        var content = new StringBuilder();
        content.Append($"<div class='fake'><a class='{ParametersCakeResume.JobListCompanyNodeAClassName}'><p>Fake data</p></a></div>");

        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(content.ToString());

        var htmlNode = htmlDoc.DocumentNode.SelectNodes("//div[@class='fake']")[0];

        var service = new HtmlAnalyzeService(logger);

        var result = service.GetJobListCompanyNode(htmlNode);

        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void GetJobListJobNode_Content為空字串()
    {
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml("<div></div>");

        var htmlNode = htmlDoc.DocumentNode.SelectNodes("//div")[0];
        var service = new HtmlAnalyzeService(logger);

        var result = service.GetJobListJobNode(htmlNode);

        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetJobListJobNode_Content不包含正確的DivClassName()
    {
        var content = new StringBuilder();
        content.Append($"<div class='fake'><a class='fake_class_name'>Fake Data</a></div>");

        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(content.ToString());

        var htmlNode = htmlDoc.DocumentNode.SelectNodes("//div[@class='fake']")[0];

        var service = new HtmlAnalyzeService(logger);

        var result = service.GetJobListJobNode(htmlNode);

        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetJobListJobNode_Content包含正確的DivClassName()
    {
        var content = new StringBuilder();
        content.Append($"<div class='fake'><a class='{ParametersCakeResume.JobListJobNodeAClassName}'><p>Fake data</p></a></div>");

        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(content.ToString());

        var htmlNode = htmlDoc.DocumentNode.SelectNodes("//div[@class='fake']")[0];

        var service = new HtmlAnalyzeService(logger);

        var result = service.GetJobListJobNode(htmlNode);

        Assert.That(result, Is.Not.Null);
    }

    /*
GetJobListCompanyNode
GetJobListJobNode
*/
}
