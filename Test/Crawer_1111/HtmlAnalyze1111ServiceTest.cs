using Crawer_1111.Service;
using HtmlAgilityPack;
using Model.Dto1111;

namespace Test.Crawer_1111;

public class HtmlAnalyze1111ServiceTest
{
    private readonly ILogger<HtmlAnalyze1111Service> logger = A.Fake<ILogger<HtmlAnalyze1111Service>>();
    private readonly string TestUrl = "https://www.example.com";

    [Test]
    public void GetJobListCardContentNode_HtmlDoc_NotContainTargetHtmlTage()
    {
        var html = "<div></div>";
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(html);

        var service = new HtmlAnalyze1111Service(logger);

        var result = service.GetJobListCardContentNode(htmlDoc);

        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetJobListCardContentNode_HtmlDoc_ContainCompanyAndJobHref()
    {
        var html = $"<div class='{Parameters1111.JobListCardDivClassName}'><div class='{Parameters1111.JobListJobDivClassName}'><a href='{TestUrl}'></a><div class='{Parameters1111.JobListCompanyDivClassName}'><a href='{TestUrl}'></a></div></div></div>";
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(html);

        var service = new HtmlAnalyze1111Service(logger);

        var result = service.GetJobListCardContentNode(htmlDoc);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Has.Count.EqualTo(1));
    }

    [Test]
    public void GetJobListCardContentNode_HtmlDoc_NoJobHref()
    {
        var html = $"<div class='{Parameters1111.JobListCardDivClassName}'><div class='{Parameters1111.JobListJobDivClassName}'><a></a><div class='{Parameters1111.JobListCompanyDivClassName}'><a href='{TestUrl}'></a></div></div></div>";
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(html);

        var service = new HtmlAnalyze1111Service(logger);

        var result = service.GetJobListCardContentNode(htmlDoc);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Has.Count.EqualTo(0));
    }

    [Test]
    public void GetJobListCardContentNode_HtmlDoc_NoCompanyHref()
    {
        var html = $"<div class='{Parameters1111.JobListCardDivClassName}'><div class='{Parameters1111.JobListJobDivClassName}'><a href='{TestUrl}'></a><div class='{Parameters1111.JobListCompanyDivClassName}'><a></a></div></div></div>";
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(html);

        var service = new HtmlAnalyze1111Service(logger);

        var result = service.GetJobListCardContentNode(htmlDoc);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Has.Count.EqualTo(0));
    }

    [Test]
    public void GetJobListCardContentNode_HtmlDoc_NoJobDiv()
    {
        var html = $"<div class='{Parameters1111.JobListCardDivClassName}'><div class='{Parameters1111.JobListCompanyDivClassName}'><a href='{TestUrl}'></a></div></div>";
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(html);

        var service = new HtmlAnalyze1111Service(logger);

        var result = service.GetJobListCardContentNode(htmlDoc);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Has.Count.EqualTo(0));
    }

    [Test]
    public void GetJobListCardContentNode_HtmlDoc_NoCompanyDiv()
    {
        var html = $"<div class='{Parameters1111.JobListCardDivClassName}'><div class='{Parameters1111.JobListJobDivClassName}'><a href='{TestUrl}'></a></div></div></div>";
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(html);

        var service = new HtmlAnalyze1111Service(logger);

        var result = service.GetJobListCardContentNode(htmlDoc);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Has.Count.EqualTo(0));
    }

    [Test]
    public void GetJobListJobNode_NotContainTargetHtml()
    {
        var html = "<div><a></a></div>";

        var htmlNode = HtmlNode.CreateNode(html);

        var service = new HtmlAnalyze1111Service(logger);

        var result = service.GetJobListJobNode(htmlNode);

        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetJobListJobNode_ContainTargetHtml()
    {
        var html = $"<div><a class='{Parameters1111.JobListJobAClass}'></a></div>";

        var htmlNode = HtmlNode.CreateNode(html);

        var service = new HtmlAnalyze1111Service(logger);

        var result = service.GetJobListJobNode(htmlNode);

        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void GetCompanyListJobNode_NotContainTargetHtml()
    {
        var html = "<div><a></a></div>";

        var htmlNode = HtmlNode.CreateNode(html);

        var service = new HtmlAnalyze1111Service(logger);

        var result = service.GetJobListCompanyNode(htmlNode);

        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetJobListCompanyNode_ContainTargetHtml()
    {
        var html = $"<div><a class='{Parameters1111.JobListCompanyAClass}'></a></div>";

        var htmlNode = HtmlNode.CreateNode(html);

        var service = new HtmlAnalyze1111Service(logger);

        var result = service.GetJobListCompanyNode(htmlNode);

        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void GetCompanyName_NoTargetHtml()
    {
        var html = $"<div></div>";

        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(html);

        var service = new HtmlAnalyze1111Service(logger);

        var result = service.GetCompanyName(htmlDoc);

        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetCompanyName_NotValidClassName()
    {
        var content = "fake data";
        var html = $"<div class='fake_class'><h1>{content}</h1></div>";

        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(html);

        var service = new HtmlAnalyze1111Service(logger);

        var result = service.GetCompanyName(htmlDoc);

        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetCompanyName_HasTargetHtml()
    {
        var content = "fake data";
        var html = $"<div class='{Parameters1111.CompanyNameDivName}'><h1>{content}</h1></div>";

        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(html);

        var service = new HtmlAnalyze1111Service(logger);

        var result = service.GetCompanyName(htmlDoc);

        Assert.That(result, Is.EqualTo(content));
    }

    [Test]
    public void GetCompanyCardContentNodes_NoCardContentDivName()
    {
        var html = $"<div></div>";
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(html);

        var service = new HtmlAnalyze1111Service(logger);

        var result = service.GetCompanyCardContentNodes(htmlDoc);

        Assert.That(result, Is.Null);
    }

    private string RealyWorld = "<div class=\"corp_body\"> <h2 class=\"spy_item\" id=\"crop_info\">公司資訊</h2> <div class=\"corp_detail\"> <div class=\"row\"> <div class=\"col-sm-8\"> <ul class=\"UI_list_dots\"> <li> <span class=\"column_title\">產業類別</span> : <a href=\"/job-bank/job-index.asp?t0=100103\" target=\"_blank\" rel=\"noopener noreferrer\"> 網路相關 </a> <a href=\"/job-bank/job-index.asp?t0=0\" target=\"_blank\" rel=\"noopener noreferrer\"> </a> <a href=\"/job-bank/job-index.asp?t0=0\" target=\"_blank\" rel=\"noopener noreferrer\"> </a> </li><p></p><li> <span class=\"column_title\">產業說明</span> : <span class=\"column_value\">資訊軟體服務</span> </li><li> <span class=\"column_title\">公司位置</span> : <a href=\"https://maps.google.com.tw/maps?q=25.0438225,121.5229528\" target=\"_blank\" rel=\"noopener noreferrer\"> 台北市大同區承德路一段70之1號15樓 </a> </li><li class=\"txtFont\"> <span class=\"column_title\">公司電話</span> : <span class=\"column_value\">  </span> </li><li class=\"txtFont\"> <span class=\"column_title\">公司傳真</span> : <span class=\"column_value\">  </span> </li><li> <span class=\"column_title\">資本額 </span> : <span class=\"column_value\"> 1億 </span> </li><li> <span class=\"column_title\">負責人</span> : <span class=\"column_value\"> 耿維德 先生 </span> </li><li> <span class=\"column_title\">公司人數</span> : <span class=\"column_value\"> 220 </span> </li><li><span class=\"column_title\">公司網址</span> : <a href=\"http://www.frog-jump.com/\" target=\"_blank\" rel=\"noreferrer noopener\">http://www.frog-jump.com/</a></li></ul> </div><div class=\"col-sm-4 ml-auto corp_other\"> <h4 data-toggle=\"collapse\" href=\"#collapseOther\">其他查詢資源：</h4> <div class=\"collapse show\" id=\"collapseOther\"> <ul> <li><a href=\"https://findbiz.nat.gov.tw/fts/query/QueryBar/queryInit.do\" target=\"_blank\" rel=\"noopener noreferrer\">經濟部商業司登記 <i class=\"UI_icon UI_icon_share\"></i></a></li><li><a href=\"https://www.etax.nat.gov.tw/etwmain/etw113w1/ban/query\" target=\"_blank\" rel=\"noopener noreferrer\">財政部營業(稅籍)登記 <i class=\"UI_icon UI_icon_share\"></i></a></li><li><a href=\"https://www.google.com.tw/search?q=德義資訊股份有限公司+PTT\" target=\"_blank\" rel=\"noopener noreferrer\">Ptt查公司名稱 <i class=\"UI_icon UI_icon_share\"></i></a></li><li><a href=\"https://www.google.com.tw/search?q=德義資訊股份有限公司\" target=\"_blank\" rel=\"noopener noreferrer\">Google查公司名稱 <i class=\"UI_icon UI_icon_share\"></i></a></li></ul> </div></div></div></div><h2 class=\"spy_item\" id=\"crop_detail\">公司簡介</h2> <div class=\"corp_info\"> 德義資訊以累積多年的資訊系統專業技術與完整豐富的軟體開發實力，提供企業用戶軟體高品質服務，是台灣地區無論是金融、電信、工業製造以及政府部門等領域，高階軟體開發服務供應鏈的領導廠商。 　　我們專精微軟ASP.NET及Oracle JAVA等技術，依照各技術領域需求分派開發工程師、測試工程師及團隊經理，透過短時間內迅速的調派作業，成功建立團隊協助企業達成預期目標。不論是單一程式開發工程師或多人開發團隊，皆能提供符合現有企業架構的需求人力。此外，更注重同仁在技術及職涯的發展，根據實務現況及新技術資訊，每年規劃完善教育訓練課程及邀請 JAVA 及微軟、資料庫等名師舉辦技術講座。 　　藉由專業人才及各種產業相關知識上深厚能力的緊密結合，德義充分協助了客戶縮短產品上市時間在關鍵任務及知識密集的系統上降低軟體開發過程中的風險。並在「用心」、「專業」及「創新」的經營理念下，創造員工、客戶及公司的三方皆贏的永續經營。<br>經營理念<br>◎「用心」 － 對人對事以用心為第一要務，是我們最真誠的態度。 ◎「專業」 － 用高品質的程式技術，達成目標。 ◎「創新」 － 用成熟而不陳腐的創意，加速完成專案任務。 德義秉持著三大理念，創造員工、客戶及公司所有人都滿意而三方皆贏的永續經營。 </div><h2 class=\"spy_item\" id=\"crop_service\">產品/服務</h2> <div class=\"corp_info corp_info_service\"> <div>＊軟體開發服務</div><div>包含系統建置、系統整合及顧問服務</div><br><div>＊專案開發</div><div>德義資訊專案團隊憑藉在網際網路、應用軟體開發等多方面業務的綜合實力和豐富經驗，為使用者提供全面的專案軟體服務。</div><br><div>＊專業技術顧問服務</div><div>－駐點程式設計服務</div><div>－駐點軟體測試服務</div><div>－應用軟體發展專業委外</div><br><div>＊.雲端計算應用轉換工具及相關技術服務</div><div>＊SOA資訊技術服務</div><div>＊Web&nbsp;2.0資訊技術服務</div><div>＊代理並整合知名國際品牌之軟體系統</div></div><h2 class=\"spy_item\" id=\"crop_rule\">公司福利</h2> <div class=\"corp_info\"> <h3>法定項目：</h3> <p class=\"mb-5\">勞保、健保、加班費、週休二日、陪產檢及陪產假、育嬰假、生理假、特別休假、產假、員工體檢、勞退提撥金、職災保險 </p><h3>福利制度：</h3> <p class=\"mb-5\"> <span class=\"text-secondary\">獎金類：</span>年終獎金、三節獎金、禮品、激勵獎金、績效獎金 <br><span class=\"text-secondary\">保險類：</span>員工團保 <br><span class=\"text-secondary\">娛樂類：</span>員工電影、國內旅遊、國外旅遊、員工聚餐、慶生會、尾牙、春酒、下午茶、家庭日 <br><span class=\"text-secondary\">補助類：</span>員工結婚補助、生育補助、員工國內、外進修補助、員工購置電腦及其他相關設備之低利貸款或補助、員工及眷屬喪葬補助 <br><span class=\"text-secondary\">其他類：</span>員工在職教育訓練、良好升遷制度 </p><h3 class=\"mb-3\">更多說明</h3> <div class=\"corp_more mb-3\">＊週休二日 自主管理<br>－人性化管理、彈性上下班、自由開放的工作環境<br>＊優於勞基法的休假制度<br>＊三節獎金或禮品<br>＊定期員工健檢<br>＊尾牙及春酒等餐會<br>＊電影包場，運動會(Gocar、農場)，聚餐Party..等活動<br>＊員工旅遊<br>＊員工保險<br>－享勞保、健保、團保<br>＊績效導向的薪資報酬<br>－以績效為導向的薪資報酬設計，結合個人目標與組織目標，引導員工專注目標發展。<br>＊全方位的職涯發展規劃及員工教育訓練<br>－高度融入性的引導教育訓練、專業內部訓練及高額補助的外部訓練提供同仁自我成長<br>＊員工購買電腦設備貸款</div><div class=\"UI_note\"> <i class=\"UI_icon UI_icon_notice UI_icon_size_25\"></i> <div> <h4>注意！</h4> <p>本區全部福利項目可能依不同職缺有所不同，實際職缺福利請依面試時與公司面談結果為準</p></div></div></div><div id=\"jobs-anchor\" class=\"anchor-nav\"></div><h2 class=\"spy_item\" id=\"crop_jobs\">工作機會</h2><div id=\"job-listings\" class=\"card_wrap position-relative\"><div class=\"content container\"><form method=\"post\" id=\"jobListForm\"><input data-val=\"true\" data-val-number=\"欄位 organNo 必須是數字。\" data-val-required=\"organNo 欄位是必要項。\" id=\"organNo\" name=\"organNo\" type=\"hidden\" value=\"69191119\"><input data-val=\"true\" data-val-number=\"欄位 pageIndex 必須是數字。\" data-val-required=\"pageIndex 欄位是必要項。\" id=\"pageIndex\" name=\"pageIndex\" type=\"hidden\" value=\"\"><input data-val=\"true\" data-val-required=\"listDisplay 欄位是必要項。\" id=\"listDisplay\" name=\"listDisplay\" type=\"hidden\" value=\"False\"><input id=\"Empkey\" name=\"Empkey\" type=\"hidden\" value=\"\"><div class=\"job-list-item\"><div class=\"data_nav d-flex justify-content-between align-items-center corp_job_list\"><div class=\"nav_item job_count col-12\">81<span>個職缺</span></div><ul class=\"job_list_filter col-12 \"><li class=\"select_box col-4\"><div class=\"dropdown bootstrap-select selectBox\"><select class=\"selectBox selectpicker\" data-val=\"true\" data-val-number=\"欄位 role 必須是數字。\" data-val-required=\"role 欄位是必要項。\" id=\"role\" name=\"role\" tabindex=\"-98\"><option selected=\"selected\" value=\"0\">工作性質(不拘)</option><option value=\"1\">全職</option></select><button type=\"button\" class=\"btn dropdown-toggle btn-light\" data-toggle=\"dropdown\" role=\"combobox\" aria-owns=\"bs-select-1\" aria-haspopup=\"listbox\" aria-expanded=\"false\" data-id=\"role\" title=\"工作性質(不拘)\"><div class=\"filter-option\"><div class=\"filter-option-inner\"><div class=\"filter-option-inner-inner\">工作性質(不拘)</div></div></div></button><div class=\"dropdown-menu \"><div class=\"inner show\" role=\"listbox\" id=\"bs-select-1\" tabindex=\"-1\"><ul class=\"dropdown-menu inner show\" role=\"presentation\"></ul></div></div></div></li><li class=\"select_box col-4\"><div class=\"dropdown bootstrap-select selectBox\"><select class=\"selectBox selectpicker\" data-val=\"true\" data-val-number=\"欄位 duty 必須是數字。\" data-val-required=\"duty 欄位是必要項。\" id=\"duty\" name=\"duty\" tabindex=\"-98\"><option selected=\"selected\" value=\"0\">職務類別(不拘)</option><option value=\"120203\">國內業務人員</option><option value=\"130204\">市場調查／分析人員</option><option value=\"140202\">軟體工程師</option><option value=\"140203\">通訊軟體工程師</option><option value=\"140205\">韌體工程師</option><option value=\"140206\">軟／韌體測試人員</option><option value=\"140213\">網站程式設計師</option><option value=\"140302\">系統分析師</option><option value=\"140303\">系統維護員／操作員</option><option value=\"140402\">MIS工程師</option><option value=\"140403\">網路管理工程師</option><option value=\"140405\">資訊助理人員</option><option value=\"140509\">演算法開發工程師</option><option value=\"140515\">資料庫管理人員</option><option value=\"220307\">影片製作技術人員</option><option value=\"230106\">網頁設計</option></select><button type=\"button\" class=\"btn dropdown-toggle btn-light\" data-toggle=\"dropdown\" role=\"combobox\" aria-owns=\"bs-select-2\" aria-haspopup=\"listbox\" aria-expanded=\"false\" data-id=\"duty\" title=\"職務類別(不拘)\"><div class=\"filter-option\"><div class=\"filter-option-inner\"><div class=\"filter-option-inner-inner\">職務類別(不拘)</div></div></div></button><div class=\"dropdown-menu \"><div class=\"inner show\" role=\"listbox\" id=\"bs-select-2\" tabindex=\"-1\"><ul class=\"dropdown-menu inner show\" role=\"presentation\"></ul></div></div></div></li><li class=\"select_box col-4\"><div class=\"dropdown bootstrap-select selectBox\"><select class=\"selectBox selectpicker\" data-val=\"true\" data-val-number=\"欄位 city 必須是數字。\" data-val-required=\"city 欄位是必要項。\" id=\"city\" name=\"city\" tabindex=\"-98\"><option selected=\"selected\" value=\"0\">工作地區(不拘)</option><option value=\"100100\">台北市_不拘</option><option value=\"100101\">台北市_中正區</option><option value=\"100102\">台北市_大同區</option><option value=\"100104\">台北市_松山區</option><option value=\"100105\">台北市_大安區</option><option value=\"100107\">台北市_信義區</option><option value=\"100110\">台北市_內湖區</option><option value=\"100111\">台北市_南港區</option></select><button type=\"button\" class=\"btn dropdown-toggle btn-light\" data-toggle=\"dropdown\" role=\"combobox\" aria-owns=\"bs-select-3\" aria-haspopup=\"listbox\" aria-expanded=\"false\" data-id=\"city\" title=\"工作地區(不拘)\"><div class=\"filter-option\"><div class=\"filter-option-inner\"><div class=\"filter-option-inner-inner\">工作地區(不拘)</div></div></div></button><div class=\"dropdown-menu \"><div class=\"inner show\" role=\"listbox\" id=\"bs-select-3\" tabindex=\"-1\"><ul class=\"dropdown-menu inner show\" role=\"presentation\"></ul></div></div></div></li></ul></div><div id=\"jobListPage\"><div class=\"none-touch-mode\"><div id=\"jobsListContent\" class=\"job_items_wapper\" style=\"margin-top: 47px;\"><div class=\"job_item fade-on-show\"> <div class=\"card-hothit\"></div><div class=\"card-body\"> <div class=\"body-wrapper\"> <div class=\"job_item_info\"> <div class=\"job_item_badge_mobile_show\"> <div class=\"badge_group\"> </div></div><a href=\"/job/76811880/\" target=\"_blank\"> <h5 class=\"card-title title_6\"> Asp.Net ,C# 程式設計師 </h5> </a> <div class=\"card-subtitle mb-4 text-muted happiness-hidd\"> <a href=\"#\"> <h6 class=\"job_item_company mb-1 digit_5 body_3\">德義資訊股份有限公司</h6> <div class=\"job_item_detail d-flex body_4\"> <div class=\"job_item_detail_location mr-3 position-relative\">台北市大同區</div><div class=\"job_item_detail_salary ml-3 font-weight-style digit_6\">面議（經常性薪資達 4 萬元或以上）</div></div></a> </div><div class=\"job_item_data_mobile_show\"> <div class=\"job_item_data\"> <div class=\"applicants mb-3 d-flex align-items-center\"> <span>應徵人數：</span> <span class=\"applicants_data\">1 ~ 5人</span> <span class=\"item_icon_cursor_default item_icon_fire UI_icon_fire4 \"></span> <a href=\"/Job/CompetitorAnalysis?employeeNo=76811880\" target=\"_blank\" class=\"applicants_data_analyze\"></a> </div></div><ul class=\"job_item_data d-flex item_list\"> <li>日班</li><li>2年以上工作經驗</li><li>學歷不拘</li></ul> </div><p class=\"card-text job_item_description body_4\"> 1.建置與開發網站前後台2.資料庫建置與規劃3.程式開發與維護4.熟悉.net(C#、VB) C#尤佳，及MS SQL5.熟悉Ajax、Java Script、CSS等相關網頁開發技術6.熟MVC開發架構佳7.具備三層式架構，N-Tier及OOP經驗，若有Team Foundation 經驗佳 </p></div><div class=\"item_data\"> <div class=\"job_item_data_mobile_hidden\"> <small class=\"text-muted job_item_date\">2023/08/28</small> <div class=\"badge_group mt-3\"> </div><div class=\"item_group mt-3 body_4\"> <div class=\"job_item_data\"> <div class=\"applicants mb-3 d-flex align-items-center\"> <span>應徵人數：</span> <span class=\"applicants_data\">1 ~ 5人</span> <span class=\"item_icon_cursor_default item_icon_fire UI_icon_fire4 \"></span> <a href=\"/Job/CompetitorAnalysis?employeeNo=76811880\" target=\"_blank\" class=\"applicants_data_analyze\"></a> </div></div><ul class=\"job_item_data d-flex item_list\"> <li>日班</li><li>2年以上工作經驗</li><li>學歷不拘</li></ul> </div></div><div class=\"job_item_data_mobile_show\"><small class=\"text-muted job_item_date\">2023/08/28</small></div><div class=\"item_group mt-5 d-flex align-items-center justify-content-between\"> <div> <a href=\"#\" class=\"btnCollectJob\" data-eno=\"76811880\"> <span class=\"item_icon icon_add icon_love_add \"></span> </a> </div><button class=\"btn btn_secondary_2 btn_size_5 applicants_btn\" onclick=\"normalApply('76811880')\">應徵</button> </div></div></div></div></div><div class=\"job_item fade-on-show\"> <div class=\"card-hothit\"></div><div class=\"card-body\"> <div class=\"body-wrapper\"> <div class=\"job_item_info\"> <div class=\"job_item_badge_mobile_show\"> <div class=\"badge_group\"> </div></div><a href=\"/job/76812021/\" target=\"_blank\"> <h5 class=\"card-title title_6\"> Java J2EE 軟體工程師 </h5> </a> <div class=\"card-subtitle mb-4 text-muted happiness-hidd\"> <a href=\"#\"> <h6 class=\"job_item_company mb-1 digit_5 body_3\">德義資訊股份有限公司</h6> <div class=\"job_item_detail d-flex body_4\"> <div class=\"job_item_detail_location mr-3 position-relative\">台北市大同區</div><div class=\"job_item_detail_salary ml-3 font-weight-style digit_6\">月薪 50,000元</div></div></a> </div><div class=\"job_item_data_mobile_show\"> <div class=\"job_item_data\"> <div class=\"applicants mb-3 d-flex align-items-center\"> <span>應徵人數：</span> <span class=\"applicants_data\">1 ~ 5人</span> <span class=\"item_icon_cursor_default item_icon_fire UI_icon_fire4 \"></span> <a href=\"/Job/CompetitorAnalysis?employeeNo=76812021\" target=\"_blank\" class=\"applicants_data_analyze\"></a> </div></div><ul class=\"job_item_data d-flex item_list\"> <li>日班</li><li>需外派</li><li>1年以上工作經驗</li><li>專科以上</li></ul> </div><p class=\"card-text job_item_description body_4\"> 工作內容 :Java web-based application 程式開發、測試及系統建置使用技術：基本: jsp, servlet, java, SQL基本操作具下列技能更佳:JavaScript, Ajax, MVC, JSTL, EL, POI 等技術 </p></div><div class=\"item_data\"> <div class=\"job_item_data_mobile_hidden\"> <small class=\"text-muted job_item_date\">2023/08/28</small> <div class=\"badge_group mt-3\"> </div><div class=\"item_group mt-3 body_4\"> <div class=\"job_item_data\"> <div class=\"applicants mb-3 d-flex align-items-center\"> <span>應徵人數：</span> <span class=\"applicants_data\">1 ~ 5人</span> <span class=\"item_icon_cursor_default item_icon_fire UI_icon_fire4 \"></span> <a href=\"/Job/CompetitorAnalysis?employeeNo=76812021\" target=\"_blank\" class=\"applicants_data_analyze\"></a> </div></div><ul class=\"job_item_data d-flex item_list\"> <li>日班</li><li>需外派</li><li>1年以上工作經驗</li><li>專科以上</li></ul> </div></div><div class=\"job_item_data_mobile_show\"><small class=\"text-muted job_item_date\">2023/08/28</small></div><div class=\"item_group mt-5 d-flex align-items-center justify-content-between\"> <div> <a href=\"#\" class=\"btnCollectJob\" data-eno=\"76812021\"> <span class=\"item_icon icon_add icon_love_add \"></span> </a> </div><button class=\"btn btn_secondary_2 btn_size_5 applicants_btn\" onclick=\"normalApply('76812021')\">應徵</button> </div></div></div></div></div><div class=\"job_item fade-on-show\"> <div class=\"card-hothit\"></div><div class=\"card-body\"> <div class=\"body-wrapper\"> <div class=\"job_item_info\"> <div class=\"job_item_badge_mobile_show\"> <div class=\"badge_group\"> </div></div><a href=\"/job/91238246/\" target=\"_blank\"> <h5 class=\"card-title title_6\"> 前端網頁工程師 （Front End Developer) </h5> </a> <div class=\"card-subtitle mb-4 text-muted happiness-hidd\"> <a href=\"#\"> <h6 class=\"job_item_company mb-1 digit_5 body_3\">德義資訊股份有限公司</h6> <div class=\"job_item_detail d-flex body_4\"> <div class=\"job_item_detail_location mr-3 position-relative\">台北市大安區</div><div class=\"job_item_detail_salary ml-3 font-weight-style digit_6\">面議（經常性薪資達 4 萬元或以上）</div></div></a> </div><div class=\"job_item_data_mobile_show\"> <div class=\"job_item_data\"> <div class=\"applicants mb-3 d-flex align-items-center\"> <span>應徵人數：</span> <span class=\"applicants_data\">1 ~ 5人</span> <span class=\"item_icon_cursor_default item_icon_fire UI_icon_fire4 \"></span> <a href=\"/Job/CompetitorAnalysis?employeeNo=91238246\" target=\"_blank\" class=\"applicants_data_analyze\"></a> </div></div><ul class=\"job_item_data d-flex item_list\"> <li>日班</li><li>1年以上工作經驗</li><li>專科以上</li></ul> </div><p class=\"card-text job_item_description body_4\"> 1. 前端網頁程式設計，具整合後台.Net經驗2. 需 JQuery, JavaScript, HTML5 經驗3. AngularJS尤佳4. 需基本英文能力 </p></div><div class=\"item_data\"> <div class=\"job_item_data_mobile_hidden\"> <small class=\"text-muted job_item_date\">2023/08/28</small> <div class=\"badge_group mt-3\"> </div><div class=\"item_group mt-3 body_4\"> <div class=\"job_item_data\"> <div class=\"applicants mb-3 d-flex align-items-center\"> <span>應徵人數：</span> <span class=\"applicants_data\">1 ~ 5人</span> <span class=\"item_icon_cursor_default item_icon_fire UI_icon_fire4 \"></span> <a href=\"/Job/CompetitorAnalysis?employeeNo=91238246\" target=\"_blank\" class=\"applicants_data_analyze\"></a> </div></div><ul class=\"job_item_data d-flex item_list\"> <li>日班</li><li>1年以上工作經驗</li><li>專科以上</li></ul> </div></div><div class=\"job_item_data_mobile_show\"><small class=\"text-muted job_item_date\">2023/08/28</small></div><div class=\"item_group mt-5 d-flex align-items-center justify-content-between\"> <div> <a href=\"#\" class=\"btnCollectJob\" data-eno=\"91238246\"> <span class=\"item_icon icon_add icon_love_add \"></span> </a> </div><button class=\"btn btn_secondary_2 btn_size_5 applicants_btn\" onclick=\"normalApply('91238246')\">應徵</button> </div></div></div></div></div><div class=\"job_item fade-on-show\"> <div class=\"card-hothit\"></div><div class=\"card-body\"> <div class=\"body-wrapper\"> <div class=\"job_item_info\"> <div class=\"job_item_badge_mobile_show\"> <div class=\"badge_group\"> <span class=\"badge badge-normal badge-practice\">研替</span> </div></div><a href=\"/job/91238158/\" target=\"_blank\"> <h5 class=\"card-title title_6\"> 【112年度研發替代役】ASP.Net 軟體工程師 </h5> </a> <div class=\"card-subtitle mb-4 text-muted happiness-hidd\"> <a href=\"#\"> <h6 class=\"job_item_company mb-1 digit_5 body_3\">德義資訊股份有限公司</h6> <div class=\"job_item_detail d-flex body_4\"> <div class=\"job_item_detail_location mr-3 position-relative\">台北市內湖區</div><div class=\"job_item_detail_salary ml-3 font-weight-style digit_6\">面議（經常性薪資達 4 萬元或以上）</div></div></a> </div><div class=\"job_item_data_mobile_show\"> <div class=\"job_item_data\"> <div class=\"applicants mb-3 d-flex align-items-center\"> <span>應徵人數：</span> <span class=\"applicants_data\">1 ~ 5人</span> <span class=\"item_icon_cursor_default item_icon_fire UI_icon_fire4 \"></span> <a href=\"/Job/CompetitorAnalysis?employeeNo=91238158\" target=\"_blank\" class=\"applicants_data_analyze\"></a> </div></div><ul class=\"job_item_data d-flex item_list\"> <li>日班</li><li>1年以上工作經驗</li><li>碩士以上</li></ul> </div><p class=\"card-text job_item_description body_4\"> 1.碩士學歷，資訊管理、資訊工程、系統設計、電算機應用相關科系2.研發設計ASP.Net 系統 開發模組3.懂VB.NET或C#之開發工具及SQL資料庫4.熟MVC尤佳 </p></div><div class=\"item_data\"> <div class=\"job_item_data_mobile_hidden\"> <small class=\"text-muted job_item_date\">2023/08/28</small> <div class=\"badge_group mt-3\"> <span class=\"badge badge-normal badge-practice\">研替</span> </div><div class=\"item_group mt-3 body_4\"> <div class=\"job_item_data\"> <div class=\"applicants mb-3 d-flex align-items-center\"> <span>應徵人數：</span> <span class=\"applicants_data\">1 ~ 5人</span> <span class=\"item_icon_cursor_default item_icon_fire UI_icon_fire4 \"></span> <a href=\"/Job/CompetitorAnalysis?employeeNo=91238158\" target=\"_blank\" class=\"applicants_data_analyze\"></a> </div></div><ul class=\"job_item_data d-flex item_list\"> <li>日班</li><li>1年以上工作經驗</li><li>碩士以上</li></ul> </div></div><div class=\"job_item_data_mobile_show\"><small class=\"text-muted job_item_date\">2023/08/28</small></div><div class=\"item_group mt-5 d-flex align-items-center justify-content-between\"> <div> <a href=\"#\" class=\"btnCollectJob\" data-eno=\"91238158\"> <span class=\"item_icon icon_add icon_love_add \"></span> </a> </div><button class=\"btn btn_secondary_2 btn_size_5 applicants_btn\" onclick=\"normalApply('91238158')\">應徵</button> </div></div></div></div></div><div class=\"job_item fade-on-show\"> <div class=\"card-hothit\"></div><div class=\"card-body\"> <div class=\"body-wrapper\"> <div class=\"job_item_info\"> <div class=\"job_item_badge_mobile_show\"> <div class=\"badge_group\"> <span class=\"badge badge-normal badge-practice\">研替</span> </div></div><a href=\"/job/91238491/\" target=\"_blank\"> <h5 class=\"card-title title_6\"> 【112年度研發替代役】Java J2EE 軟體工程師 </h5> </a> <div class=\"card-subtitle mb-4 text-muted happiness-hidd\"> <a href=\"#\"> <h6 class=\"job_item_company mb-1 digit_5 body_3\">德義資訊股份有限公司</h6> <div class=\"job_item_detail d-flex body_4\"> <div class=\"job_item_detail_location mr-3 position-relative\">台北市內湖區</div><div class=\"job_item_detail_salary ml-3 font-weight-style digit_6\">面議（經常性薪資達 4 萬元或以上）</div></div></a> </div><div class=\"job_item_data_mobile_show\"> <div class=\"job_item_data\"> <div class=\"applicants mb-3 d-flex align-items-center\"> <span>應徵人數：</span> <span class=\"applicants_data\">1 ~ 5人</span> <span class=\"item_icon_cursor_default item_icon_fire UI_icon_fire4 \"></span> <a href=\"/Job/CompetitorAnalysis?employeeNo=91238491\" target=\"_blank\" class=\"applicants_data_analyze\"></a> </div></div><ul class=\"job_item_data d-flex item_list\"> <li>日班</li><li>工作經驗不拘</li><li>碩士</li></ul> </div><p class=\"card-text job_item_description body_4\"> 1. 熟 Java SE/ Java EE 程式設計與開發2. 熟 Java、JSP、Servlets3. 熟 OOAD, UML4. 會WebSphere／WebLogic／JBoss Application Server尤佳5. 會Struts, Spring, Hibernate, JQuery 等 Frameworks </p></div><div class=\"item_data\"> <div class=\"job_item_data_mobile_hidden\"> <small class=\"text-muted job_item_date\">2023/08/28</small> <div class=\"badge_group mt-3\"> <span class=\"badge badge-normal badge-practice\">研替</span> </div><div class=\"item_group mt-3 body_4\"> <div class=\"job_item_data\"> <div class=\"applicants mb-3 d-flex align-items-center\"> <span>應徵人數：</span> <span class=\"applicants_data\">1 ~ 5人</span> <span class=\"item_icon_cursor_default item_icon_fire UI_icon_fire4 \"></span> <a href=\"/Job/CompetitorAnalysis?employeeNo=91238491\" target=\"_blank\" class=\"applicants_data_analyze\"></a> </div></div><ul class=\"job_item_data d-flex item_list\"> <li>日班</li><li>工作經驗不拘</li><li>碩士</li></ul> </div></div><div class=\"job_item_data_mobile_show\"><small class=\"text-muted job_item_date\">2023/08/28</small></div><div class=\"item_group mt-5 d-flex align-items-center justify-content-between\"> <div> <a href=\"#\" class=\"btnCollectJob\" data-eno=\"91238491\"> <span class=\"item_icon icon_add icon_love_add \"></span> </a> </div><button class=\"btn btn_secondary_2 btn_size_5 applicants_btn\" onclick=\"normalApply('91238491')\">應徵</button> </div></div></div></div></div><div class=\"job_item fade-on-show\"> <div class=\"card-body\"> <div class=\"body-wrapper\"> <div class=\"job_item_info\"> <div class=\"job_item_badge_mobile_show\"> <div class=\"badge_group\"> </div></div><a href=\"/job/76811882/\" target=\"_blank\"> <h5 class=\"card-title title_6\"> Java EE 顧問工程師（SD） </h5> </a> <div class=\"card-subtitle mb-4 text-muted happiness-hidd\"> <a href=\"#\"> <h6 class=\"job_item_company mb-1 digit_5 body_3\">德義資訊股份有限公司</h6> <div class=\"job_item_detail d-flex body_4\"> <div class=\"job_item_detail_location mr-3 position-relative\">台北市大同區</div><div class=\"job_item_detail_salary ml-3 font-weight-style digit_6\">面議（經常性薪資達 4 萬元或以上）</div></div></a> </div><div class=\"job_item_data_mobile_show\"> <div class=\"job_item_data\"> <div class=\"applicants mb-3 d-flex align-items-center\"> <span>應徵人數：</span> <span class=\"applicants_data\">1 ~ 5人</span> <span class=\"item_icon_cursor_default item_icon_fire UI_icon_fire4 \"></span> <a href=\"/Job/CompetitorAnalysis?employeeNo=76811882\" target=\"_blank\" class=\"applicants_data_analyze\"></a> </div></div><ul class=\"job_item_data d-flex item_list\"> <li>週休二日</li><li>日班</li><li>需外派</li><li>3年以上工作經驗</li><li>專科以上</li></ul> </div><p class=\"card-text job_item_description body_4\"> 1. 熟悉 Java SE/ Java EE程式設計與開發 (3年以上實務專案開發經驗)2. 熟 Java、JSP、Java EE、Servlets3. 熟 OOAD, UML4. WebSphere／WebLogic／JBoss Application Server5. Struts, Spring, Hibernate, JSF 等 Java Frameworks </p></div><div class=\"item_data\"> <div class=\"job_item_data_mobile_hidden\"> <small class=\"text-muted job_item_date\">2023/08/28</small> <div class=\"badge_group mt-3\"> </div><div class=\"item_group mt-3 body_4\"> <div class=\"job_item_data\"> <div class=\"applicants mb-3 d-flex align-items-center\"> <span>應徵人數：</span> <span class=\"applicants_data\">1 ~ 5人</span> <span class=\"item_icon_cursor_default item_icon_fire UI_icon_fire4 \"></span> <a href=\"/Job/CompetitorAnalysis?employeeNo=76811882\" target=\"_blank\" class=\"applicants_data_analyze\"></a> </div></div><ul class=\"job_item_data d-flex item_list\"> <li>週休二日</li><li>日班</li><li>需外派</li><li>3年以上工作經驗</li><li>專科以上</li></ul> </div></div><div class=\"job_item_data_mobile_show\"><small class=\"text-muted job_item_date\">2023/08/28</small></div><div class=\"item_group mt-5 d-flex align-items-center justify-content-between\"> <div> <a href=\"#\" class=\"btnCollectJob\" data-eno=\"76811882\"> <span class=\"item_icon icon_add icon_love_add \"></span> </a> </div><button class=\"btn btn_secondary_2 btn_size_5 applicants_btn\" onclick=\"normalApply('76811882')\">應徵</button> </div></div></div></div></div><div class=\"job_item fade-on-show\"> <div class=\"card-body\"> <div class=\"body-wrapper\"> <div class=\"job_item_info\"> <div class=\"job_item_badge_mobile_show\"> <div class=\"badge_group\"> </div></div><a href=\"/job/76811890/\" target=\"_blank\"> <h5 class=\"card-title title_6\"> 資深.NET程式設計師 (SD) </h5> </a> <div class=\"card-subtitle mb-4 text-muted happiness-hidd\"> <a href=\"#\"> <h6 class=\"job_item_company mb-1 digit_5 body_3\">德義資訊股份有限公司</h6> <div class=\"job_item_detail d-flex body_4\"> <div class=\"job_item_detail_location mr-3 position-relative\">台北市大同區</div><div class=\"job_item_detail_salary ml-3 font-weight-style digit_6\">面議（經常性薪資達 4 萬元或以上）</div></div></a> </div><div class=\"job_item_data_mobile_show\"> <div class=\"job_item_data\"> <div class=\"applicants mb-3 d-flex align-items-center\"> <span>應徵人數：</span> <span class=\"applicants_data\">1 ~ 5人</span> <span class=\"item_icon_cursor_default item_icon_fire UI_icon_fire4 \"></span> <a href=\"/Job/CompetitorAnalysis?employeeNo=76811890\" target=\"_blank\" class=\"applicants_data_analyze\"></a> </div></div><ul class=\"job_item_data d-flex item_list\"> <li>週休二日</li><li>日班</li><li>4年以上工作經驗</li><li>專科以上</li></ul> </div><p class=\"card-text job_item_description body_4\"> 1. 運用UML規劃與設計文件2. 撰寫程式及執行單元測試 (Unit Test)3. 執行產品及系統維護與保固作業4. 客戶服務與支援5. 撰寫使用手冊與操作手冊 </p></div><div class=\"item_data\"> <div class=\"job_item_data_mobile_hidden\"> <small class=\"text-muted job_item_date\">2023/08/28</small> <div class=\"badge_group mt-3\"> </div><div class=\"item_group mt-3 body_4\"> <div class=\"job_item_data\"> <div class=\"applicants mb-3 d-flex align-items-center\"> <span>應徵人數：</span> <span class=\"applicants_data\">1 ~ 5人</span> <span class=\"item_icon_cursor_default item_icon_fire UI_icon_fire4 \"></span> <a href=\"/Job/CompetitorAnalysis?employeeNo=76811890\" target=\"_blank\" class=\"applicants_data_analyze\"></a> </div></div><ul class=\"job_item_data d-flex item_list\"> <li>週休二日</li><li>日班</li><li>4年以上工作經驗</li><li>專科以上</li></ul> </div></div><div class=\"job_item_data_mobile_show\"><small class=\"text-muted job_item_date\">2023/08/28</small></div><div class=\"item_group mt-5 d-flex align-items-center justify-content-between\"> <div> <a href=\"#\" class=\"btnCollectJob\" data-eno=\"76811890\"> <span class=\"item_icon icon_add icon_love_add \"></span> </a> </div><button class=\"btn btn_secondary_2 btn_size_5 applicants_btn\" onclick=\"normalApply('76811890')\">應徵</button> </div></div></div></div></div><div class=\"job_item fade-on-show\"> <div class=\"card-body\"> <div class=\"body-wrapper\"> <div class=\"job_item_info\"> <div class=\"job_item_badge_mobile_show\"> <div class=\"badge_group\"> </div></div><a href=\"/job/76811892/\" target=\"_blank\"> <h5 class=\"card-title title_6\"> ASP.NET,C#程式設計師（PG）金融業 </h5> </a> <div class=\"card-subtitle mb-4 text-muted happiness-hidd\"> <a href=\"#\"> <h6 class=\"job_item_company mb-1 digit_5 body_3\">德義資訊股份有限公司</h6> <div class=\"job_item_detail d-flex body_4\"> <div class=\"job_item_detail_location mr-3 position-relative\">台北市大同區</div><div class=\"job_item_detail_salary ml-3 font-weight-style digit_6\">面議（經常性薪資達 4 萬元或以上）</div></div></a> </div><div class=\"job_item_data_mobile_show\"> <div class=\"job_item_data\"> <div class=\"applicants mb-3 d-flex align-items-center\"> <span>應徵人數：</span> <span class=\"applicants_data\">1 ~ 5人</span> <span class=\"item_icon_cursor_default item_icon_fire UI_icon_fire4 \"></span> <a href=\"/Job/CompetitorAnalysis?employeeNo=76811892\" target=\"_blank\" class=\"applicants_data_analyze\"></a> </div></div><ul class=\"job_item_data d-flex item_list\"> <li>週休二日</li><li>日班</li><li>3年以上工作經驗</li><li>專科以上</li></ul> </div><p class=\"card-text job_item_description body_4\"> 1. ASP.NET 2.0或以上，ASP.NET 1.1經驗2年以上亦可，若有ASP.NET 3.5經驗尤佳2. 開發語言VB或C#皆可，C#尤佳3. 需具備網頁開發相關技術，Ajax、JavaScript、CSS等4. 需具備三層式架構，N-Tier及OOP經驗，若有Team Foundation 經驗尤佳5. UML 觀念6. Object-relational mapping，LINQ、ADO.NET Entity Framework7. 良好的溝通與傾聽能力8. 對程式開發及軟體工程具有熱情，願意嘗試新技術及架構9. 願意以負責任的態度，對工作及所產出之程式進行品質管理10. 可獨立或與團隊成員合作11. Microsoft 相關認證 </p></div><div class=\"item_data\"> <div class=\"job_item_data_mobile_hidden\"> <small class=\"text-muted job_item_date\">2023/08/28</small> <div class=\"badge_group mt-3\"> </div><div class=\"item_group mt-3 body_4\"> <div class=\"job_item_data\"> <div class=\"applicants mb-3 d-flex align-items-center\"> <span>應徵人數：</span> <span class=\"applicants_data\">1 ~ 5人</span> <span class=\"item_icon_cursor_default item_icon_fire UI_icon_fire4 \"></span> <a href=\"/Job/CompetitorAnalysis?employeeNo=76811892\" target=\"_blank\" class=\"applicants_data_analyze\"></a> </div></div><ul class=\"job_item_data d-flex item_list\"> <li>週休二日</li><li>日班</li><li>3年以上工作經驗</li><li>專科以上</li></ul> </div></div><div class=\"job_item_data_mobile_show\"><small class=\"text-muted job_item_date\">2023/08/28</small></div><div class=\"item_group mt-5 d-flex align-items-center justify-content-between\"> <div> <a href=\"#\" class=\"btnCollectJob\" data-eno=\"76811892\"> <span class=\"item_icon icon_add icon_love_add \"></span> </a> </div><button class=\"btn btn_secondary_2 btn_size_5 applicants_btn\" onclick=\"normalApply('76811892')\">應徵</button> </div></div></div></div></div><div class=\"job_item fade-on-show\"> <div class=\"card-body\"> <div class=\"body-wrapper\"> <div class=\"job_item_info\"> <div class=\"job_item_badge_mobile_show\"> <div class=\"badge_group\"> </div></div><a href=\"/job/76811901/\" target=\"_blank\"> <h5 class=\"card-title title_6\"> 資深 JAVA 工程師（PG＋SD） </h5> </a> <div class=\"card-subtitle mb-4 text-muted happiness-hidd\"> <a href=\"#\"> <h6 class=\"job_item_company mb-1 digit_5 body_3\">德義資訊股份有限公司</h6> <div class=\"job_item_detail d-flex body_4\"> <div class=\"job_item_detail_location mr-3 position-relative\">台北市大同區</div><div class=\"job_item_detail_salary ml-3 font-weight-style digit_6\">面議（經常性薪資達 4 萬元或以上）</div></div></a> </div><div class=\"job_item_data_mobile_show\"> <div class=\"job_item_data\"> <div class=\"applicants mb-3 d-flex align-items-center\"> <span>應徵人數：</span> <span class=\"applicants_data\">1 ~ 5人</span> <span class=\"item_icon_cursor_default item_icon_fire UI_icon_fire4 \"></span> <a href=\"/Job/CompetitorAnalysis?employeeNo=76811901\" target=\"_blank\" class=\"applicants_data_analyze\"></a> </div></div><ul class=\"job_item_data d-flex item_list\"> <li>週休二日</li><li>日班</li><li>3年以上工作經驗</li><li>專科以上</li></ul> </div><p class=\"card-text job_item_description body_4\"> 1.熟 Java 相關技術2.熟悉DB：Oracle, DB2, Postgre3.Java IDE：Eclipse4.Java Frameworks：Struts、Hibernate、Spring 開發5.具備 Sun Certified Java Programer(OCP/JP) </p></div><div class=\"item_data\"> <div class=\"job_item_data_mobile_hidden\"> <small class=\"text-muted job_item_date\">2023/08/28</small> <div class=\"badge_group mt-3\"> </div><div class=\"item_group mt-3 body_4\"> <div class=\"job_item_data\"> <div class=\"applicants mb-3 d-flex align-items-center\"> <span>應徵人數：</span> <span class=\"applicants_data\">1 ~ 5人</span> <span class=\"item_icon_cursor_default item_icon_fire UI_icon_fire4 \"></span> <a href=\"/Job/CompetitorAnalysis?employeeNo=76811901\" target=\"_blank\" class=\"applicants_data_analyze\"></a> </div></div><ul class=\"job_item_data d-flex item_list\"> <li>週休二日</li><li>日班</li><li>3年以上工作經驗</li><li>專科以上</li></ul> </div></div><div class=\"job_item_data_mobile_show\"><small class=\"text-muted job_item_date\">2023/08/28</small></div><div class=\"item_group mt-5 d-flex align-items-center justify-content-between\"> <div> <a href=\"#\" class=\"btnCollectJob\" data-eno=\"76811901\"> <span class=\"item_icon icon_add icon_love_add \"></span> </a> </div><button class=\"btn btn_secondary_2 btn_size_5 applicants_btn\" onclick=\"normalApply('76811901')\">應徵</button> </div></div></div></div></div><div class=\"job_item fade-on-show\"> <div class=\"card-body\"> <div class=\"body-wrapper\"> <div class=\"job_item_info\"> <div class=\"job_item_badge_mobile_show\"> <div class=\"badge_group\"> </div></div><a href=\"/job/76811906/\" target=\"_blank\"> <h5 class=\"card-title title_6\"> Java 程式設計師（PG） </h5> </a> <div class=\"card-subtitle mb-4 text-muted happiness-hidd\"> <a href=\"#\"> <h6 class=\"job_item_company mb-1 digit_5 body_3\">德義資訊股份有限公司</h6> <div class=\"job_item_detail d-flex body_4\"> <div class=\"job_item_detail_location mr-3 position-relative\">台北市大同區</div><div class=\"job_item_detail_salary ml-3 font-weight-style digit_6\">面議（經常性薪資達 4 萬元或以上）</div></div></a> </div><div class=\"job_item_data_mobile_show\"> <div class=\"job_item_data\"> <div class=\"applicants mb-3 d-flex align-items-center\"> <span>應徵人數：</span> <span class=\"applicants_data\">1 ~ 5人</span> <span class=\"item_icon_cursor_default item_icon_fire UI_icon_fire4 \"></span> <a href=\"/Job/CompetitorAnalysis?employeeNo=76811906\" target=\"_blank\" class=\"applicants_data_analyze\"></a> </div></div><ul class=\"job_item_data d-flex item_list\"> <li>週休二日</li><li>日班</li><li>2年以上工作經驗</li><li>專科以上</li></ul> </div><p class=\"card-text job_item_description body_4\"> 1 Familiar with Java, Java Script, JSP tech.2 Familiar with SQL statement, Store Procedure3 Familiar with Java EE architecture </p></div><div class=\"item_data\"> <div class=\"job_item_data_mobile_hidden\"> <small class=\"text-muted job_item_date\">2023/08/28</small> <div class=\"badge_group mt-3\"> </div><div class=\"item_group mt-3 body_4\"> <div class=\"job_item_data\"> <div class=\"applicants mb-3 d-flex align-items-center\"> <span>應徵人數：</span> <span class=\"applicants_data\">1 ~ 5人</span> <span class=\"item_icon_cursor_default item_icon_fire UI_icon_fire4 \"></span> <a href=\"/Job/CompetitorAnalysis?employeeNo=76811906\" target=\"_blank\" class=\"applicants_data_analyze\"></a> </div></div><ul class=\"job_item_data d-flex item_list\"> <li>週休二日</li><li>日班</li><li>2年以上工作經驗</li><li>專科以上</li></ul> </div></div><div class=\"job_item_data_mobile_show\"><small class=\"text-muted job_item_date\">2023/08/28</small></div><div class=\"item_group mt-5 d-flex align-items-center justify-content-between\"> <div> <a href=\"#\" class=\"btnCollectJob\" data-eno=\"76811906\"> <span class=\"item_icon icon_add icon_love_add \"></span> </a> </div><button class=\"btn btn_secondary_2 btn_size_5 applicants_btn\" onclick=\"normalApply('76811906')\">應徵</button> </div></div></div></div></div><div class=\"UI_pagination\" id=\"pageIndexDiv\"> <div class=\"UI_pagination_content\"> <a href=\"javascript:;\" class=\"dropdown-toggle\" data-toggle=\"dropdown\" aria-expanded=\"true\"> <span><u>1</u> / 9</span> </a> <div class=\"dropdown-menu dropdown_corp\"> <ul> <li><a href=\"javascript:;\" onclick=\"changePage(1)\">01</a></li><li><a href=\"javascript:;\" onclick=\"changePage(2)\">02</a></li><li><a href=\"javascript:;\" onclick=\"changePage(3)\">03</a></li><li><a href=\"javascript:;\" onclick=\"changePage(4)\">04</a></li><li><a href=\"javascript:;\" onclick=\"changePage(5)\">05</a></li><li><a href=\"javascript:;\" onclick=\"changePage(6)\">06</a></li><li><a href=\"javascript:;\" onclick=\"changePage(7)\">07</a></li><li><a href=\"javascript:;\" onclick=\"changePage(8)\">08</a></li><li><a href=\"javascript:;\" onclick=\"changePage(9)\">09</a></li></ul> </div></div><div class=\"UI_pagination_btn next\"> <a href=\"javascript:;\" onclick=\"changePage(2)\">next</a> </div></div></div></div><div class=\"touch-mode\"><div id=\"jobsMobileContent\" class=\"job_items_wapper\" style=\"margin-top: 47px;\"><div class=\"job_item job-listing-box fade-on-show\" data-page=\"1\"><div class=\"card-hothit\"></div><div class=\"card-body\"><div class=\"body-wrapper\"><div class=\"job_item_info\"><div class=\"job_item_badge_mobile_show\"><div class=\"badge_group\"></div></div><a href=\"/job/76811880\" target=\"_blank\"><h5 class=\"card-title title_6\">Asp.Net ,C# 程式設計師 </h5></a><div class=\"card-subtitle mb-4 text-muted happiness-hidd\"><a href=\"#\"><h6 class=\"job_item_company mb-1 digit_5 body_3\">德義資訊股份有限公司</h6><div class=\"job_item_detail d-flex body_4\"><div class=\"job_item_detail_location mr-3 position-relative\">台北市大同區</div><div class=\"job_item_detail_salary ml-3 font-weight-style digit_6\">面議（經常性薪資達 4 萬元或以上）</div></div></a></div><div class=\"job_item_data_mobile_show\"><div class=\"job_item_data\"><div class=\"applicants mb-3 d-flex align-items-center\"><span>應徵人數：</span><span class=\"applicants_data\">1 ~ 5人</span><span class=\"item_icon_cursor_default item_icon_fire UI_icon_fire4\"></span><a href=\"/Job/CompetitorAnalysis?employeeNo=76811880\" target=\"_blank\" class=\"applicants_data_analyze\"></a></div></div><ul class=\"job_item_data d-flex item_list\"><li>日班</li><li>2年以上工作經驗</li><li>學歷不拘</li></ul></div><p class=\"card-text job_item_description body_4\">1.建置與開發網站前後台2.資料庫建置與規劃3.程式開發與維護4.熟悉.net(C#、VB) C#尤佳，及MS SQL5.熟悉Ajax、Java Script、CSS等相關網頁開發技術6.熟MVC開發架構佳7.具備三層式架構，N-Tier及OOP經驗，若有Team Foundation 經驗佳</p></div><div class=\"item_data\"><div class=\"job_item_data_mobile_hidden\"><small class=\"text-muted job_item_date\">2023/08/28</small><div class=\"badge_group mt-3\"></div><div class=\"item_group mt-3 body_4\"><div class=\"job_item_data\"><div class=\"applicants mb-3 d-flex align-items-center\"><span>應徵人數：</span><span class=\"applicants_data\">1 ~ 5人</span><span class=\"item_icon_cursor_default item_icon_fire UI_icon_fire4 \"></span><a href=\"/Job/CompetitorAnalysis?employeeNo=76811880\" target=\"_blank\" class=\"applicants_data_analyze\"></a></div></div><ul class=\"job_item_data d-flex item_list\"><li>日班</li><li>2年以上工作經驗</li><li>學歷不拘</li></ul></div></div><div class=\"job_item_data_mobile_show\"><small class=\"text-muted job_item_date\">2023/08/28</small></div><div class=\"item_group mt-5 d-flex align-items-center justify-content-between\"><div><a href=\"#\" class=\"btnCollectJob\" data-eno=\"76811880\"><span class=\"item_icon icon_add icon_love_add \"></span></a></div><button class=\"btn btn_secondary_2 btn_size_5 applicants_btn\" onclick=\"normalApply('76811880')\">應徵</button></div></div></div></div></div><div class=\"job_item job-listing-box fade-on-show\" data-page=\"1\"><div class=\"card-hothit\"></div><div class=\"card-body\"><div class=\"body-wrapper\"><div class=\"job_item_info\"><div class=\"job_item_badge_mobile_show\"><div class=\"badge_group\"></div></div><a href=\"/job/76812021\" target=\"_blank\"><h5 class=\"card-title title_6\">Java J2EE 軟體工程師 </h5></a><div class=\"card-subtitle mb-4 text-muted happiness-hidd\"><a href=\"#\"><h6 class=\"job_item_company mb-1 digit_5 body_3\">德義資訊股份有限公司</h6><div class=\"job_item_detail d-flex body_4\"><div class=\"job_item_detail_location mr-3 position-relative\">台北市大同區</div><div class=\"job_item_detail_salary ml-3 font-weight-style digit_6\">月薪 50,000元</div></div></a></div><div class=\"job_item_data_mobile_show\"><div class=\"job_item_data\"><div class=\"applicants mb-3 d-flex align-items-center\"><span>應徵人數：</span><span class=\"applicants_data\">1 ~ 5人</span><span class=\"item_icon_cursor_default item_icon_fire UI_icon_fire4\"></span><a href=\"/Job/CompetitorAnalysis?employeeNo=76812021\" target=\"_blank\" class=\"applicants_data_analyze\"></a></div></div><ul class=\"job_item_data d-flex item_list\"><li>日班</li><li>需外派</li><li>1年以上工作經驗</li><li>專科以上</li></ul></div><p class=\"card-text job_item_description body_4\">工作內容 :Java web-based application 程式開發、測試及系統建置使用技術：基本: jsp, servlet, java, SQL基本操作具下列技能更佳:JavaScript, Ajax, MVC, JSTL, EL, POI 等技術</p></div><div class=\"item_data\"><div class=\"job_item_data_mobile_hidden\"><small class=\"text-muted job_item_date\">2023/08/28</small><div class=\"badge_group mt-3\"></div><div class=\"item_group mt-3 body_4\"><div class=\"job_item_data\"><div class=\"applicants mb-3 d-flex align-items-center\"><span>應徵人數：</span><span class=\"applicants_data\">1 ~ 5人</span><span class=\"item_icon_cursor_default item_icon_fire UI_icon_fire4 \"></span><a href=\"/Job/CompetitorAnalysis?employeeNo=76812021\" target=\"_blank\" class=\"applicants_data_analyze\"></a></div></div><ul class=\"job_item_data d-flex item_list\"><li>日班</li><li>需外派</li><li>1年以上工作經驗</li><li>專科以上</li></ul></div></div><div class=\"job_item_data_mobile_show\"><small class=\"text-muted job_item_date\">2023/08/28</small></div><div class=\"item_group mt-5 d-flex align-items-center justify-content-between\"><div><a href=\"#\" class=\"btnCollectJob\" data-eno=\"76812021\"><span class=\"item_icon icon_add icon_love_add \"></span></a></div><button class=\"btn btn_secondary_2 btn_size_5 applicants_btn\" onclick=\"normalApply('76812021')\">應徵</button></div></div></div></div></div><div class=\"job_item job-listing-box fade-on-show\" data-page=\"1\"><div class=\"card-hothit\"></div><div class=\"card-body\"><div class=\"body-wrapper\"><div class=\"job_item_info\"><div class=\"job_item_badge_mobile_show\"><div class=\"badge_group\"></div></div><a href=\"/job/91238246\" target=\"_blank\"><h5 class=\"card-title title_6\">前端網頁工程師 （Front End Developer)</h5></a><div class=\"card-subtitle mb-4 text-muted happiness-hidd\"><a href=\"#\"><h6 class=\"job_item_company mb-1 digit_5 body_3\">德義資訊股份有限公司</h6><div class=\"job_item_detail d-flex body_4\"><div class=\"job_item_detail_location mr-3 position-relative\">台北市大安區</div><div class=\"job_item_detail_salary ml-3 font-weight-style digit_6\">面議（經常性薪資達 4 萬元或以上）</div></div></a></div><div class=\"job_item_data_mobile_show\"><div class=\"job_item_data\"><div class=\"applicants mb-3 d-flex align-items-center\"><span>應徵人數：</span><span class=\"applicants_data\">1 ~ 5人</span><span class=\"item_icon_cursor_default item_icon_fire UI_icon_fire4\"></span><a href=\"/Job/CompetitorAnalysis?employeeNo=91238246\" target=\"_blank\" class=\"applicants_data_analyze\"></a></div></div><ul class=\"job_item_data d-flex item_list\"><li>日班</li><li>1年以上工作經驗</li><li>專科以上</li></ul></div><p class=\"card-text job_item_description body_4\">1. 前端網頁程式設計，具整合後台.Net經驗2. 需 JQuery, JavaScript, HTML5 經驗3. AngularJS尤佳4. 需基本英文能力</p></div><div class=\"item_data\"><div class=\"job_item_data_mobile_hidden\"><small class=\"text-muted job_item_date\">2023/08/28</small><div class=\"badge_group mt-3\"></div><div class=\"item_group mt-3 body_4\"><div class=\"job_item_data\"><div class=\"applicants mb-3 d-flex align-items-center\"><span>應徵人數：</span><span class=\"applicants_data\">1 ~ 5人</span><span class=\"item_icon_cursor_default item_icon_fire UI_icon_fire4 \"></span><a href=\"/Job/CompetitorAnalysis?employeeNo=91238246\" target=\"_blank\" class=\"applicants_data_analyze\"></a></div></div><ul class=\"job_item_data d-flex item_list\"><li>日班</li><li>1年以上工作經驗</li><li>專科以上</li></ul></div></div><div class=\"job_item_data_mobile_show\"><small class=\"text-muted job_item_date\">2023/08/28</small></div><div class=\"item_group mt-5 d-flex align-items-center justify-content-between\"><div><a href=\"#\" class=\"btnCollectJob\" data-eno=\"91238246\"><span class=\"item_icon icon_add icon_love_add \"></span></a></div><button class=\"btn btn_secondary_2 btn_size_5 applicants_btn\" onclick=\"normalApply('91238246')\">應徵</button></div></div></div></div></div><div class=\"job_item job-listing-box fade-on-show\" data-page=\"1\"><div class=\"card-hothit\"></div><div class=\"card-body\"><div class=\"body-wrapper\"><div class=\"job_item_info\"><div class=\"job_item_badge_mobile_show\"><div class=\"badge_group\"><span class=\"badge badge-normal badge-practice\">研替</span></div></div><a href=\"/job/91238158\" target=\"_blank\"><h5 class=\"card-title title_6\">【112年度研發替代役】ASP.Net 軟體工程師</h5></a><div class=\"card-subtitle mb-4 text-muted happiness-hidd\"><a href=\"#\"><h6 class=\"job_item_company mb-1 digit_5 body_3\">德義資訊股份有限公司</h6><div class=\"job_item_detail d-flex body_4\"><div class=\"job_item_detail_location mr-3 position-relative\">台北市內湖區</div><div class=\"job_item_detail_salary ml-3 font-weight-style digit_6\">面議（經常性薪資達 4 萬元或以上）</div></div></a></div><div class=\"job_item_data_mobile_show\"><div class=\"job_item_data\"><div class=\"applicants mb-3 d-flex align-items-center\"><span>應徵人數：</span><span class=\"applicants_data\">1 ~ 5人</span><span class=\"item_icon_cursor_default item_icon_fire UI_icon_fire4\"></span><a href=\"/Job/CompetitorAnalysis?employeeNo=91238158\" target=\"_blank\" class=\"applicants_data_analyze\"></a></div></div><ul class=\"job_item_data d-flex item_list\"><li>日班</li><li>1年以上工作經驗</li><li>碩士以上</li></ul></div><p class=\"card-text job_item_description body_4\">1.碩士學歷，資訊管理、資訊工程、系統設計、電算機應用相關科系2.研發設計ASP.Net 系統 開發模組3.懂VB.NET或C#之開發工具及SQL資料庫4.熟MVC尤佳</p></div><div class=\"item_data\"><div class=\"job_item_data_mobile_hidden\"><small class=\"text-muted job_item_date\">2023/08/28</small><div class=\"badge_group mt-3\"><span class=\"badge badge-normal badge-practice\">研替</span></div><div class=\"item_group mt-3 body_4\"><div class=\"job_item_data\"><div class=\"applicants mb-3 d-flex align-items-center\"><span>應徵人數：</span><span class=\"applicants_data\">1 ~ 5人</span><span class=\"item_icon_cursor_default item_icon_fire UI_icon_fire4 \"></span><a href=\"/Job/CompetitorAnalysis?employeeNo=91238158\" target=\"_blank\" class=\"applicants_data_analyze\"></a></div></div><ul class=\"job_item_data d-flex item_list\"><li>日班</li><li>1年以上工作經驗</li><li>碩士以上</li></ul></div></div><div class=\"job_item_data_mobile_show\"><small class=\"text-muted job_item_date\">2023/08/28</small></div><div class=\"item_group mt-5 d-flex align-items-center justify-content-between\"><div><a href=\"#\" class=\"btnCollectJob\" data-eno=\"91238158\"><span class=\"item_icon icon_add icon_love_add \"></span></a></div><button class=\"btn btn_secondary_2 btn_size_5 applicants_btn\" onclick=\"normalApply('91238158')\">應徵</button></div></div></div></div></div><div class=\"job_item job-listing-box fade-on-show\" data-page=\"1\"><div class=\"card-hothit\"></div><div class=\"card-body\"><div class=\"body-wrapper\"><div class=\"job_item_info\"><div class=\"job_item_badge_mobile_show\"><div class=\"badge_group\"><span class=\"badge badge-normal badge-practice\">研替</span></div></div><a href=\"/job/91238491\" target=\"_blank\"><h5 class=\"card-title title_6\">【112年度研發替代役】Java J2EE 軟體工程師</h5></a><div class=\"card-subtitle mb-4 text-muted happiness-hidd\"><a href=\"#\"><h6 class=\"job_item_company mb-1 digit_5 body_3\">德義資訊股份有限公司</h6><div class=\"job_item_detail d-flex body_4\"><div class=\"job_item_detail_location mr-3 position-relative\">台北市內湖區</div><div class=\"job_item_detail_salary ml-3 font-weight-style digit_6\">面議（經常性薪資達 4 萬元或以上）</div></div></a></div><div class=\"job_item_data_mobile_show\"><div class=\"job_item_data\"><div class=\"applicants mb-3 d-flex align-items-center\"><span>應徵人數：</span><span class=\"applicants_data\">1 ~ 5人</span><span class=\"item_icon_cursor_default item_icon_fire UI_icon_fire4\"></span><a href=\"/Job/CompetitorAnalysis?employeeNo=91238491\" target=\"_blank\" class=\"applicants_data_analyze\"></a></div></div><ul class=\"job_item_data d-flex item_list\"><li>日班</li><li>工作經驗不拘</li><li>碩士</li></ul></div><p class=\"card-text job_item_description body_4\">1. 熟 Java SE/ Java EE 程式設計與開發2. 熟 Java、JSP、Servlets3. 熟 OOAD, UML4. 會WebSphere／WebLogic／JBoss Application Server尤佳5. 會Struts, Spring, Hibernate, JQuery 等 Frameworks</p></div><div class=\"item_data\"><div class=\"job_item_data_mobile_hidden\"><small class=\"text-muted job_item_date\">2023/08/28</small><div class=\"badge_group mt-3\"><span class=\"badge badge-normal badge-practice\">研替</span></div><div class=\"item_group mt-3 body_4\"><div class=\"job_item_data\"><div class=\"applicants mb-3 d-flex align-items-center\"><span>應徵人數：</span><span class=\"applicants_data\">1 ~ 5人</span><span class=\"item_icon_cursor_default item_icon_fire UI_icon_fire4 \"></span><a href=\"/Job/CompetitorAnalysis?employeeNo=91238491\" target=\"_blank\" class=\"applicants_data_analyze\"></a></div></div><ul class=\"job_item_data d-flex item_list\"><li>日班</li><li>工作經驗不拘</li><li>碩士</li></ul></div></div><div class=\"job_item_data_mobile_show\"><small class=\"text-muted job_item_date\">2023/08/28</small></div><div class=\"item_group mt-5 d-flex align-items-center justify-content-between\"><div><a href=\"#\" class=\"btnCollectJob\" data-eno=\"91238491\"><span class=\"item_icon icon_add icon_love_add \"></span></a></div><button class=\"btn btn_secondary_2 btn_size_5 applicants_btn\" onclick=\"normalApply('91238491')\">應徵</button></div></div></div></div></div><div class=\"job_item job-listing-box fade-on-show\" data-page=\"1\"><div class=\"card-body\"><div class=\"body-wrapper\"><div class=\"job_item_info\"><div class=\"job_item_badge_mobile_show\"><div class=\"badge_group\"></div></div><a href=\"/job/76811882\" target=\"_blank\"><h5 class=\"card-title title_6\">Java EE 顧問工程師（SD） </h5></a><div class=\"card-subtitle mb-4 text-muted happiness-hidd\"><a href=\"#\"><h6 class=\"job_item_company mb-1 digit_5 body_3\">德義資訊股份有限公司</h6><div class=\"job_item_detail d-flex body_4\"><div class=\"job_item_detail_location mr-3 position-relative\">台北市大同區</div><div class=\"job_item_detail_salary ml-3 font-weight-style digit_6\">面議（經常性薪資達 4 萬元或以上）</div></div></a></div><div class=\"job_item_data_mobile_show\"><div class=\"job_item_data\"><div class=\"applicants mb-3 d-flex align-items-center\"><span>應徵人數：</span><span class=\"applicants_data\">1 ~ 5人</span><span class=\"item_icon_cursor_default item_icon_fire UI_icon_fire4\"></span><a href=\"/Job/CompetitorAnalysis?employeeNo=76811882\" target=\"_blank\" class=\"applicants_data_analyze\"></a></div></div><ul class=\"job_item_data d-flex item_list\"><li>週休二日</li><li>日班</li><li>需外派</li><li>3年以上工作經驗</li><li>專科以上</li></ul></div><p class=\"card-text job_item_description body_4\">1. 熟悉 Java SE/ Java EE程式設計與開發 (3年以上實務專案開發經驗)2. 熟 Java、JSP、Java EE、Servlets3. 熟 OOAD, UML4. WebSphere／WebLogic／JBoss Application Server5. Struts, Spring, Hibernate, JSF 等 Java Frameworks</p></div><div class=\"item_data\"><div class=\"job_item_data_mobile_hidden\"><small class=\"text-muted job_item_date\">2023/08/28</small><div class=\"badge_group mt-3\"></div><div class=\"item_group mt-3 body_4\"><div class=\"job_item_data\"><div class=\"applicants mb-3 d-flex align-items-center\"><span>應徵人數：</span><span class=\"applicants_data\">1 ~ 5人</span><span class=\"item_icon_cursor_default item_icon_fire UI_icon_fire4 \"></span><a href=\"/Job/CompetitorAnalysis?employeeNo=76811882\" target=\"_blank\" class=\"applicants_data_analyze\"></a></div></div><ul class=\"job_item_data d-flex item_list\"><li>週休二日</li><li>日班</li><li>需外派</li><li>3年以上工作經驗</li><li>專科以上</li></ul></div></div><div class=\"job_item_data_mobile_show\"><small class=\"text-muted job_item_date\">2023/08/28</small></div><div class=\"item_group mt-5 d-flex align-items-center justify-content-between\"><div><a href=\"#\" class=\"btnCollectJob\" data-eno=\"76811882\"><span class=\"item_icon icon_add icon_love_add \"></span></a></div><button class=\"btn btn_secondary_2 btn_size_5 applicants_btn\" onclick=\"normalApply('76811882')\">應徵</button></div></div></div></div></div><div class=\"job_item job-listing-box fade-on-show\" data-page=\"1\"><div class=\"card-body\"><div class=\"body-wrapper\"><div class=\"job_item_info\"><div class=\"job_item_badge_mobile_show\"><div class=\"badge_group\"></div></div><a href=\"/job/76811890\" target=\"_blank\"><h5 class=\"card-title title_6\">資深.NET程式設計師 (SD) </h5></a><div class=\"card-subtitle mb-4 text-muted happiness-hidd\"><a href=\"#\"><h6 class=\"job_item_company mb-1 digit_5 body_3\">德義資訊股份有限公司</h6><div class=\"job_item_detail d-flex body_4\"><div class=\"job_item_detail_location mr-3 position-relative\">台北市大同區</div><div class=\"job_item_detail_salary ml-3 font-weight-style digit_6\">面議（經常性薪資達 4 萬元或以上）</div></div></a></div><div class=\"job_item_data_mobile_show\"><div class=\"job_item_data\"><div class=\"applicants mb-3 d-flex align-items-center\"><span>應徵人數：</span><span class=\"applicants_data\">1 ~ 5人</span><span class=\"item_icon_cursor_default item_icon_fire UI_icon_fire4\"></span><a href=\"/Job/CompetitorAnalysis?employeeNo=76811890\" target=\"_blank\" class=\"applicants_data_analyze\"></a></div></div><ul class=\"job_item_data d-flex item_list\"><li>週休二日</li><li>日班</li><li>4年以上工作經驗</li><li>專科以上</li></ul></div><p class=\"card-text job_item_description body_4\">1. 運用UML規劃與設計文件2. 撰寫程式及執行單元測試 (Unit Test)3. 執行產品及系統維護與保固作業4. 客戶服務與支援5. 撰寫使用手冊與操作手冊</p></div><div class=\"item_data\"><div class=\"job_item_data_mobile_hidden\"><small class=\"text-muted job_item_date\">2023/08/28</small><div class=\"badge_group mt-3\"></div><div class=\"item_group mt-3 body_4\"><div class=\"job_item_data\"><div class=\"applicants mb-3 d-flex align-items-center\"><span>應徵人數：</span><span class=\"applicants_data\">1 ~ 5人</span><span class=\"item_icon_cursor_default item_icon_fire UI_icon_fire4 \"></span><a href=\"/Job/CompetitorAnalysis?employeeNo=76811890\" target=\"_blank\" class=\"applicants_data_analyze\"></a></div></div><ul class=\"job_item_data d-flex item_list\"><li>週休二日</li><li>日班</li><li>4年以上工作經驗</li><li>專科以上</li></ul></div></div><div class=\"job_item_data_mobile_show\"><small class=\"text-muted job_item_date\">2023/08/28</small></div><div class=\"item_group mt-5 d-flex align-items-center justify-content-between\"><div><a href=\"#\" class=\"btnCollectJob\" data-eno=\"76811890\"><span class=\"item_icon icon_add icon_love_add \"></span></a></div><button class=\"btn btn_secondary_2 btn_size_5 applicants_btn\" onclick=\"normalApply('76811890')\">應徵</button></div></div></div></div></div><div class=\"job_item job-listing-box fade-on-show\" data-page=\"1\"><div class=\"card-body\"><div class=\"body-wrapper\"><div class=\"job_item_info\"><div class=\"job_item_badge_mobile_show\"><div class=\"badge_group\"></div></div><a href=\"/job/76811892\" target=\"_blank\"><h5 class=\"card-title title_6\">ASP.NET,C#程式設計師（PG）金融業 </h5></a><div class=\"card-subtitle mb-4 text-muted happiness-hidd\"><a href=\"#\"><h6 class=\"job_item_company mb-1 digit_5 body_3\">德義資訊股份有限公司</h6><div class=\"job_item_detail d-flex body_4\"><div class=\"job_item_detail_location mr-3 position-relative\">台北市大同區</div><div class=\"job_item_detail_salary ml-3 font-weight-style digit_6\">面議（經常性薪資達 4 萬元或以上）</div></div></a></div><div class=\"job_item_data_mobile_show\"><div class=\"job_item_data\"><div class=\"applicants mb-3 d-flex align-items-center\"><span>應徵人數：</span><span class=\"applicants_data\">1 ~ 5人</span><span class=\"item_icon_cursor_default item_icon_fire UI_icon_fire4\"></span><a href=\"/Job/CompetitorAnalysis?employeeNo=76811892\" target=\"_blank\" class=\"applicants_data_analyze\"></a></div></div><ul class=\"job_item_data d-flex item_list\"><li>週休二日</li><li>日班</li><li>3年以上工作經驗</li><li>專科以上</li></ul></div><p class=\"card-text job_item_description body_4\">1. ASP.NET 2.0或以上，ASP.NET 1.1經驗2年以上亦可，若有ASP.NET 3.5經驗尤佳2. 開發語言VB或C#皆可，C#尤佳3. 需具備網頁開發相關技術，Ajax、JavaScript、CSS等4. 需具備三層式架構，N-Tier及OOP經驗，若有Team Foundation 經驗尤佳5. UML 觀念6. Object-relational mapping，LINQ、ADO.NET Entity Framework7. 良好的溝通與傾聽能力8. 對程式開發及軟體工程具有熱情，願意嘗試新技術及架構9. 願意以負責任的態度，對工作及所產出之程式進行品質管理10. 可獨立或與團隊成員合作11. Microsoft 相關認證</p></div><div class=\"item_data\"><div class=\"job_item_data_mobile_hidden\"><small class=\"text-muted job_item_date\">2023/08/28</small><div class=\"badge_group mt-3\"></div><div class=\"item_group mt-3 body_4\"><div class=\"job_item_data\"><div class=\"applicants mb-3 d-flex align-items-center\"><span>應徵人數：</span><span class=\"applicants_data\">1 ~ 5人</span><span class=\"item_icon_cursor_default item_icon_fire UI_icon_fire4 \"></span><a href=\"/Job/CompetitorAnalysis?employeeNo=76811892\" target=\"_blank\" class=\"applicants_data_analyze\"></a></div></div><ul class=\"job_item_data d-flex item_list\"><li>週休二日</li><li>日班</li><li>3年以上工作經驗</li><li>專科以上</li></ul></div></div><div class=\"job_item_data_mobile_show\"><small class=\"text-muted job_item_date\">2023/08/28</small></div><div class=\"item_group mt-5 d-flex align-items-center justify-content-between\"><div><a href=\"#\" class=\"btnCollectJob\" data-eno=\"76811892\"><span class=\"item_icon icon_add icon_love_add \"></span></a></div><button class=\"btn btn_secondary_2 btn_size_5 applicants_btn\" onclick=\"normalApply('76811892')\">應徵</button></div></div></div></div></div><div class=\"job_item job-listing-box fade-on-show\" data-page=\"1\"><div class=\"card-body\"><div class=\"body-wrapper\"><div class=\"job_item_info\"><div class=\"job_item_badge_mobile_show\"><div class=\"badge_group\"></div></div><a href=\"/job/76811901\" target=\"_blank\"><h5 class=\"card-title title_6\">資深 JAVA 工程師（PG＋SD） </h5></a><div class=\"card-subtitle mb-4 text-muted happiness-hidd\"><a href=\"#\"><h6 class=\"job_item_company mb-1 digit_5 body_3\">德義資訊股份有限公司</h6><div class=\"job_item_detail d-flex body_4\"><div class=\"job_item_detail_location mr-3 position-relative\">台北市大同區</div><div class=\"job_item_detail_salary ml-3 font-weight-style digit_6\">面議（經常性薪資達 4 萬元或以上）</div></div></a></div><div class=\"job_item_data_mobile_show\"><div class=\"job_item_data\"><div class=\"applicants mb-3 d-flex align-items-center\"><span>應徵人數：</span><span class=\"applicants_data\">1 ~ 5人</span><span class=\"item_icon_cursor_default item_icon_fire UI_icon_fire4\"></span><a href=\"/Job/CompetitorAnalysis?employeeNo=76811901\" target=\"_blank\" class=\"applicants_data_analyze\"></a></div></div><ul class=\"job_item_data d-flex item_list\"><li>週休二日</li><li>日班</li><li>3年以上工作經驗</li><li>專科以上</li></ul></div><p class=\"card-text job_item_description body_4\">1.熟 Java 相關技術2.熟悉DB：Oracle, DB2, Postgre3.Java IDE：Eclipse4.Java Frameworks：Struts、Hibernate、Spring 開發5.具備 Sun Certified Java Programer(OCP/JP)</p></div><div class=\"item_data\"><div class=\"job_item_data_mobile_hidden\"><small class=\"text-muted job_item_date\">2023/08/28</small><div class=\"badge_group mt-3\"></div><div class=\"item_group mt-3 body_4\"><div class=\"job_item_data\"><div class=\"applicants mb-3 d-flex align-items-center\"><span>應徵人數：</span><span class=\"applicants_data\">1 ~ 5人</span><span class=\"item_icon_cursor_default item_icon_fire UI_icon_fire4 \"></span><a href=\"/Job/CompetitorAnalysis?employeeNo=76811901\" target=\"_blank\" class=\"applicants_data_analyze\"></a></div></div><ul class=\"job_item_data d-flex item_list\"><li>週休二日</li><li>日班</li><li>3年以上工作經驗</li><li>專科以上</li></ul></div></div><div class=\"job_item_data_mobile_show\"><small class=\"text-muted job_item_date\">2023/08/28</small></div><div class=\"item_group mt-5 d-flex align-items-center justify-content-between\"><div><a href=\"#\" class=\"btnCollectJob\" data-eno=\"76811901\"><span class=\"item_icon icon_add icon_love_add \"></span></a></div><button class=\"btn btn_secondary_2 btn_size_5 applicants_btn\" onclick=\"normalApply('76811901')\">應徵</button></div></div></div></div></div><div class=\"job_item job-listing-box fade-on-show\" data-page=\"1\"><div class=\"card-body\"><div class=\"body-wrapper\"><div class=\"job_item_info\"><div class=\"job_item_badge_mobile_show\"><div class=\"badge_group\"></div></div><a href=\"/job/76811906\" target=\"_blank\"><h5 class=\"card-title title_6\">Java 程式設計師（PG）</h5></a><div class=\"card-subtitle mb-4 text-muted happiness-hidd\"><a href=\"#\"><h6 class=\"job_item_company mb-1 digit_5 body_3\">德義資訊股份有限公司</h6><div class=\"job_item_detail d-flex body_4\"><div class=\"job_item_detail_location mr-3 position-relative\">台北市大同區</div><div class=\"job_item_detail_salary ml-3 font-weight-style digit_6\">面議（經常性薪資達 4 萬元或以上）</div></div></a></div><div class=\"job_item_data_mobile_show\"><div class=\"job_item_data\"><div class=\"applicants mb-3 d-flex align-items-center\"><span>應徵人數：</span><span class=\"applicants_data\">1 ~ 5人</span><span class=\"item_icon_cursor_default item_icon_fire UI_icon_fire4\"></span><a href=\"/Job/CompetitorAnalysis?employeeNo=76811906\" target=\"_blank\" class=\"applicants_data_analyze\"></a></div></div><ul class=\"job_item_data d-flex item_list\"><li>週休二日</li><li>日班</li><li>2年以上工作經驗</li><li>專科以上</li></ul></div><p class=\"card-text job_item_description body_4\">1 Familiar with Java, Java Script, JSP tech.2 Familiar with SQL statement, Store Procedure3 Familiar with Java EE architecture</p></div><div class=\"item_data\"><div class=\"job_item_data_mobile_hidden\"><small class=\"text-muted job_item_date\">2023/08/28</small><div class=\"badge_group mt-3\"></div><div class=\"item_group mt-3 body_4\"><div class=\"job_item_data\"><div class=\"applicants mb-3 d-flex align-items-center\"><span>應徵人數：</span><span class=\"applicants_data\">1 ~ 5人</span><span class=\"item_icon_cursor_default item_icon_fire UI_icon_fire4 \"></span><a href=\"/Job/CompetitorAnalysis?employeeNo=76811906\" target=\"_blank\" class=\"applicants_data_analyze\"></a></div></div><ul class=\"job_item_data d-flex item_list\"><li>週休二日</li><li>日班</li><li>2年以上工作經驗</li><li>專科以上</li></ul></div></div><div class=\"job_item_data_mobile_show\"><small class=\"text-muted job_item_date\">2023/08/28</small></div><div class=\"item_group mt-5 d-flex align-items-center justify-content-between\"><div><a href=\"#\" class=\"btnCollectJob\" data-eno=\"76811906\"><span class=\"item_icon icon_add icon_love_add \"></span></a></div><button class=\"btn btn_secondary_2 btn_size_5 applicants_btn\" onclick=\"normalApply('76811906')\">應徵</button></div></div></div></div></div><div class=\"job_item job-listing-box fade-on-show\" data-page=\"1\"><div class=\"card-body\"><div class=\"body-wrapper\"><div class=\"job_item_info\"><div class=\"job_item_badge_mobile_show\"><div class=\"badge_group\"></div></div><a href=\"/job/76811909\" target=\"_blank\"><h5 class=\"card-title title_6\">Crystal reports及資料庫工程師</h5></a><div class=\"card-subtitle mb-4 text-muted happiness-hidd\"><a href=\"#\"><h6 class=\"job_item_company mb-1 digit_5 body_3\">德義資訊股份有限公司</h6><div class=\"job_item_detail d-flex body_4\"><div class=\"job_item_detail_location mr-3 position-relative\">台北市大安區</div><div class=\"job_item_detail_salary ml-3 font-weight-style digit_6\">面議（經常性薪資達 4 萬元或以上）</div></div></a></div><div class=\"job_item_data_mobile_show\"><div class=\"job_item_data\"><div class=\"applicants mb-3 d-flex align-items-center\"><span>應徵人數：</span><span class=\"applicants_data\">1 ~ 5人</span><span class=\"item_icon_cursor_default item_icon_fire UI_icon_fire4\"></span><a href=\"/Job/CompetitorAnalysis?employeeNo=76811909\" target=\"_blank\" class=\"applicants_data_analyze\"></a></div></div><ul class=\"job_item_data d-flex item_list\"><li>週休二日</li><li>日班</li><li>3年以上工作經驗</li><li>專科以上</li></ul></div><p class=\"card-text job_item_description body_4\">1. 建置、管理與維護報表系統。2. 開發Crystal Reports，以符合企業需求，需撰寫Oracle P/L SQL。3. 撰寫Java程式，以執行報表。</p></div><div class=\"item_data\"><div class=\"job_item_data_mobile_hidden\"><small class=\"text-muted job_item_date\">2023/08/28</small><div class=\"badge_group mt-3\"></div><div class=\"item_group mt-3 body_4\"><div class=\"job_item_data\"><div class=\"applicants mb-3 d-flex align-items-center\"><span>應徵人數：</span><span class=\"applicants_data\">1 ~ 5人</span><span class=\"item_icon_cursor_default item_icon_fire UI_icon_fire4 \"></span><a href=\"/Job/CompetitorAnalysis?employeeNo=76811909\" target=\"_blank\" class=\"applicants_data_analyze\"></a></div></div><ul class=\"job_item_data d-flex item_list\"><li>週休二日</li><li>日班</li><li>3年以上工作經驗</li><li>專科以上</li></ul></div></div><div class=\"job_item_data_mobile_show\"><small class=\"text-muted job_item_date\">2023/08/28</small></div><div class=\"item_group mt-5 d-flex align-items-center justify-content-between\"><div><a href=\"#\" class=\"btnCollectJob\" data-eno=\"76811909\"><span class=\"item_icon icon_add icon_love_add \"></span></a></div><button class=\"btn btn_secondary_2 btn_size_5 applicants_btn\" onclick=\"normalApply('76811909')\">應徵</button></div></div></div></div></div><div class=\"job_item job-listing-box fade-on-show\" data-page=\"1\"><div class=\"card-body\"><div class=\"body-wrapper\"><div class=\"job_item_info\"><div class=\"job_item_badge_mobile_show\"><div class=\"badge_group\"></div></div><a href=\"/job/76811915\" target=\"_blank\"><h5 class=\"card-title title_6\">C# Web Programmer</h5></a><div class=\"card-subtitle mb-4 text-muted happiness-hidd\"><a href=\"#\"><h6 class=\"job_item_company mb-1 digit_5 body_3\">德義資訊股份有限公司</h6><div class=\"job_item_detail d-flex body_4\"><div class=\"job_item_detail_location mr-3 position-relative\">台北市大同區</div><div class=\"job_item_detail_salary ml-3 font-weight-style digit_6\">面議（經常性薪資達 4 萬元或以上）</div></div></a></div><div class=\"job_item_data_mobile_show\"><div class=\"job_item_data\"><div class=\"applicants mb-3 d-flex align-items-center\"><span>應徵人數：</span><span class=\"applicants_data\">1 ~ 5人</span><span class=\"item_icon_cursor_default item_icon_fire UI_icon_fire4\"></span><a href=\"/Job/CompetitorAnalysis?employeeNo=76811915\" target=\"_blank\" class=\"applicants_data_analyze\"></a></div></div><ul class=\"job_item_data d-flex item_list\"><li>週休二日</li><li>日班</li><li>3年以上工作經驗</li><li>專科以上</li></ul></div><p class=\"card-text job_item_description body_4\">1. ASP.NET 2.0或以上，ASP.NET 1.1經驗2年以上亦可，若有ASP.NET 3.5經驗尤佳2. 開發語言VB或C#皆可，C#尤佳3. 需具備網頁開發相關技術，Ajax、Java Scr廿金ipt、CSS等4. 需具備三層式架構，N-Tier及OOP經驗，若有Team Foundation 經驗尤佳5. UML 觀念6. Object-relational mapping，LINQ、ADO.NET Entity Framework7. 良好的溝通與傾聽能力8. 對程式開發及軟體工程具有熱情，願意嘗試新技術及架構9. 願意以負責任的態度，對工作及所產出之程式進行品質管理10. 可獨立或與團隊成員合作11. Microsoft 相關認證</p></div><div class=\"item_data\"><div class=\"job_item_data_mobile_hidden\"><small class=\"text-muted job_item_date\">2023/08/28</small><div class=\"badge_group mt-3\"></div><div class=\"item_group mt-3 body_4\"><div class=\"job_item_data\"><div class=\"applicants mb-3 d-flex align-items-center\"><span>應徵人數：</span><span class=\"applicants_data\">1 ~ 5人</span><span class=\"item_icon_cursor_default item_icon_fire UI_icon_fire4 \"></span><a href=\"/Job/CompetitorAnalysis?employeeNo=76811915\" target=\"_blank\" class=\"applicants_data_analyze\"></a></div></div><ul class=\"job_item_data d-flex item_list\"><li>週休二日</li><li>日班</li><li>3年以上工作經驗</li><li>專科以上</li></ul></div></div><div class=\"job_item_data_mobile_show\"><small class=\"text-muted job_item_date\">2023/08/28</small></div><div class=\"item_group mt-5 d-flex align-items-center justify-content-between\"><div><a href=\"#\" class=\"btnCollectJob\" data-eno=\"76811915\"><span class=\"item_icon icon_add icon_love_add \"></span></a></div><button class=\"btn btn_secondary_2 btn_size_5 applicants_btn\" onclick=\"normalApply('76811915')\">應徵</button></div></div></div></div></div><div class=\"job_item job-listing-box fade-on-show\" data-page=\"1\"><div class=\"card-body\"><div class=\"body-wrapper\"><div class=\"job_item_info\"><div class=\"job_item_badge_mobile_show\"><div class=\"badge_group\"></div></div><a href=\"/job/76811924\" target=\"_blank\"><h5 class=\"card-title title_6\">(Taipei) C# Web Programmer</h5></a><div class=\"card-subtitle mb-4 text-muted happiness-hidd\"><a href=\"#\"><h6 class=\"job_item_company mb-1 digit_5 body_3\">德義資訊股份有限公司</h6><div class=\"job_item_detail d-flex body_4\"><div class=\"job_item_detail_location mr-3 position-relative\">台北市大同區</div><div class=\"job_item_detail_salary ml-3 font-weight-style digit_6\">面議（經常性薪資達 4 萬元或以上）</div></div></a></div><div class=\"job_item_data_mobile_show\"><div class=\"job_item_data\"><div class=\"applicants mb-3 d-flex align-items-center\"><span>應徵人數：</span><span class=\"applicants_data\">1 ~ 5人</span><span class=\"item_icon_cursor_default item_icon_fire UI_icon_fire4\"></span><a href=\"/Job/CompetitorAnalysis?employeeNo=76811924\" target=\"_blank\" class=\"applicants_data_analyze\"></a></div></div><ul class=\"job_item_data d-flex item_list\"><li>週休二日</li><li>日班</li><li>3年以上工作經驗</li><li>專科以上</li></ul></div><p class=\"card-text job_item_description body_4\">1. ASP.NET 2.0或以上，ASP.NET 1.1經驗2年以上亦可，若有ASP.NET 3.5經驗尤佳2. 開發語言VB或C#皆可，C#尤佳3. 需具備網頁開發相關技術，Ajax、Java Scr廿金ipt、CSS等4. 需具備三層式架構，N-Tier及OOP經驗，若有Team Foundation 經驗尤佳5. UML 觀念6. Object-relational mapping，LINQ、ADO.NET Entity Framework7. 良好的溝通與傾聽能力8. 對程式開發及軟體工程具有熱情，願意嘗試新技術及架構9. 願意以負責任的態度，對工作及所產出之程式進行品質管理10. 可獨立或與團隊成員合作11. Microsoft 相關認證</p></div><div class=\"item_data\"><div class=\"job_item_data_mobile_hidden\"><small class=\"text-muted job_item_date\">2023/08/28</small><div class=\"badge_group mt-3\"></div><div class=\"item_group mt-3 body_4\"><div class=\"job_item_data\"><div class=\"applicants mb-3 d-flex align-items-center\"><span>應徵人數：</span><span class=\"applicants_data\">1 ~ 5人</span><span class=\"item_icon_cursor_default item_icon_fire UI_icon_fire4 \"></span><a href=\"/Job/CompetitorAnalysis?employeeNo=76811924\" target=\"_blank\" class=\"applicants_data_analyze\"></a></div></div><ul class=\"job_item_data d-flex item_list\"><li>週休二日</li><li>日班</li><li>3年以上工作經驗</li><li>專科以上</li></ul></div></div><div class=\"job_item_data_mobile_show\"><small class=\"text-muted job_item_date\">2023/08/28</small></div><div class=\"item_group mt-5 d-flex align-items-center justify-content-between\"><div><a href=\"#\" class=\"btnCollectJob\" data-eno=\"76811924\"><span class=\"item_icon icon_add icon_love_add \"></span></a></div><button class=\"btn btn_secondary_2 btn_size_5 applicants_btn\" onclick=\"normalApply('76811924')\">應徵</button></div></div></div></div></div><div class=\"job_item job-listing-box fade-on-show\" data-page=\"1\"><div class=\"card-body\"><div class=\"body-wrapper\"><div class=\"job_item_info\"><div class=\"job_item_badge_mobile_show\"><div class=\"badge_group\"></div></div><a href=\"/job/76811952\" target=\"_blank\"><h5 class=\"card-title title_6\">Sr. Asp.Net 軟體工程師 </h5></a><div class=\"card-subtitle mb-4 text-muted happiness-hidd\"><a href=\"#\"><h6 class=\"job_item_company mb-1 digit_5 body_3\">德義資訊股份有限公司</h6><div class=\"job_item_detail d-flex body_4\"><div class=\"job_item_detail_location mr-3 position-relative\">台北市大同區</div><div class=\"job_item_detail_salary ml-3 font-weight-style digit_6\">面議（經常性薪資達 4 萬元或以上）</div></div></a></div><div class=\"job_item_data_mobile_show\"><div class=\"job_item_data\"><div class=\"applicants mb-3 d-flex align-items-center\"><span>應徵人數：</span><span class=\"applicants_data\">1 ~ 5人</span><span class=\"item_icon_cursor_default item_icon_fire UI_icon_fire4\"></span><a href=\"/Job/CompetitorAnalysis?employeeNo=76811952\" target=\"_blank\" class=\"applicants_data_analyze\"></a></div></div><ul class=\"job_item_data d-flex item_list\"><li>日班</li><li>2年以上工作經驗</li><li>學歷不拘</li></ul></div><p class=\"card-text job_item_description body_4\">1.建置與開發網站前後台2.資料庫建置與規劃3.程式開發與維護4.熟悉.net(C#、VB) C#尤佳，及MS SQL5.熟悉Ajax、Java Script、CSS等相關網頁開發技術6.熟MVC開發架構佳7.具備三層式架構，N-Tier及OOP經驗，若有Team Foundation 經驗佳</p></div><div class=\"item_data\"><div class=\"job_item_data_mobile_hidden\"><small class=\"text-muted job_item_date\">2023/08/28</small><div class=\"badge_group mt-3\"></div><div class=\"item_group mt-3 body_4\"><div class=\"job_item_data\"><div class=\"applicants mb-3 d-flex align-items-center\"><span>應徵人數：</span><span class=\"applicants_data\">1 ~ 5人</span><span class=\"item_icon_cursor_default item_icon_fire UI_icon_fire4 \"></span><a href=\"/Job/CompetitorAnalysis?employeeNo=76811952\" target=\"_blank\" class=\"applicants_data_analyze\"></a></div></div><ul class=\"job_item_data d-flex item_list\"><li>日班</li><li>2年以上工作經驗</li><li>學歷不拘</li></ul></div></div><div class=\"job_item_data_mobile_show\"><small class=\"text-muted job_item_date\">2023/08/28</small></div><div class=\"item_group mt-5 d-flex align-items-center justify-content-between\"><div><a href=\"#\" class=\"btnCollectJob\" data-eno=\"76811952\"><span class=\"item_icon icon_add icon_love_add \"></span></a></div><button class=\"btn btn_secondary_2 btn_size_5 applicants_btn\" onclick=\"normalApply('76811952')\">應徵</button></div></div></div></div></div><div class=\"job_item job-listing-box fade-on-show\" data-page=\"1\"><div class=\"card-body\"><div class=\"body-wrapper\"><div class=\"job_item_info\"><div class=\"job_item_badge_mobile_show\"><div class=\"badge_group\"></div></div><a href=\"/job/76811955\" target=\"_blank\"><h5 class=\"card-title title_6\">(Taipei) Asp.Net 軟體工程師</h5></a><div class=\"card-subtitle mb-4 text-muted happiness-hidd\"><a href=\"#\"><h6 class=\"job_item_company mb-1 digit_5 body_3\">德義資訊股份有限公司</h6><div class=\"job_item_detail d-flex body_4\"><div class=\"job_item_detail_location mr-3 position-relative\">台北市大同區</div><div class=\"job_item_detail_salary ml-3 font-weight-style digit_6\">面議（經常性薪資達 4 萬元或以上）</div></div></a></div><div class=\"job_item_data_mobile_show\"><div class=\"job_item_data\"><div class=\"applicants mb-3 d-flex align-items-center\"><span>應徵人數：</span><span class=\"applicants_data\">1 ~ 5人</span><span class=\"item_icon_cursor_default item_icon_fire UI_icon_fire4\"></span><a href=\"/Job/CompetitorAnalysis?employeeNo=76811955\" target=\"_blank\" class=\"applicants_data_analyze\"></a></div></div><ul class=\"job_item_data d-flex item_list\"><li>日班</li><li>2年以上工作經驗</li><li>學歷不拘</li></ul></div><p class=\"card-text job_item_description body_4\">1.建置與開發網站前後台2.資料庫建置與規劃3.程式開發與維護4.熟悉.net(C#、VB) C#尤佳，及MS SQL5.熟悉Ajax、Java Script、CSS等相關網頁開發技術6.熟MVC開發架構佳7.具備三層式架構，N-Tier及OOP經驗，若有Team Foundation 經驗佳</p></div><div class=\"item_data\"><div class=\"job_item_data_mobile_hidden\"><small class=\"text-muted job_item_date\">2023/08/28</small><div class=\"badge_group mt-3\"></div><div class=\"item_group mt-3 body_4\"><div class=\"job_item_data\"><div class=\"applicants mb-3 d-flex align-items-center\"><span>應徵人數：</span><span class=\"applicants_data\">1 ~ 5人</span><span class=\"item_icon_cursor_default item_icon_fire UI_icon_fire4 \"></span><a href=\"/Job/CompetitorAnalysis?employeeNo=76811955\" target=\"_blank\" class=\"applicants_data_analyze\"></a></div></div><ul class=\"job_item_data d-flex item_list\"><li>日班</li><li>2年以上工作經驗</li><li>學歷不拘</li></ul></div></div><div class=\"job_item_data_mobile_show\"><small class=\"text-muted job_item_date\">2023/08/28</small></div><div class=\"item_group mt-5 d-flex align-items-center justify-content-between\"><div><a href=\"#\" class=\"btnCollectJob\" data-eno=\"76811955\"><span class=\"item_icon icon_add icon_love_add \"></span></a></div><button class=\"btn btn_secondary_2 btn_size_5 applicants_btn\" onclick=\"normalApply('76811955')\">應徵</button></div></div></div></div></div><div class=\"job_item job-listing-box fade-on-show\" data-page=\"1\"><div class=\"card-body\"><div class=\"body-wrapper\"><div class=\"job_item_info\"><div class=\"job_item_badge_mobile_show\"><div class=\"badge_group\"></div></div><a href=\"/job/76811956\" target=\"_blank\"><h5 class=\"card-title title_6\">Sr. Asp.Net Internet工程師 </h5></a><div class=\"card-subtitle mb-4 text-muted happiness-hidd\"><a href=\"#\"><h6 class=\"job_item_company mb-1 digit_5 body_3\">德義資訊股份有限公司</h6><div class=\"job_item_detail d-flex body_4\"><div class=\"job_item_detail_location mr-3 position-relative\">台北市大同區</div><div class=\"job_item_detail_salary ml-3 font-weight-style digit_6\">面議（經常性薪資達 4 萬元或以上）</div></div></a></div><div class=\"job_item_data_mobile_show\"><div class=\"job_item_data\"><div class=\"applicants mb-3 d-flex align-items-center\"><span>應徵人數：</span><span class=\"applicants_data\">1 ~ 5人</span><span class=\"item_icon_cursor_default item_icon_fire UI_icon_fire4\"></span><a href=\"/Job/CompetitorAnalysis?employeeNo=76811956\" target=\"_blank\" class=\"applicants_data_analyze\"></a></div></div><ul class=\"job_item_data d-flex item_list\"><li>日班</li><li>2年以上工作經驗</li><li>學歷不拘</li></ul></div><p class=\"card-text job_item_description body_4\">1.建置與開發網站前後台2.資料庫建置與規劃3.程式開發與維護4.熟悉.net(C#、VB) C#尤佳，及MS SQL5.熟悉Ajax、Java Script、CSS等相關網頁開發技術6.熟MVC開發架構佳7.具備三層式架構，N-Tier及OOP經驗，若有Team Foundation 經驗佳</p></div><div class=\"item_data\"><div class=\"job_item_data_mobile_hidden\"><small class=\"text-muted job_item_date\">2023/08/28</small><div class=\"badge_group mt-3\"></div><div class=\"item_group mt-3 body_4\"><div class=\"job_item_data\"><div class=\"applicants mb-3 d-flex align-items-center\"><span>應徵人數：</span><span class=\"applicants_data\">1 ~ 5人</span><span class=\"item_icon_cursor_default item_icon_fire UI_icon_fire4 \"></span><a href=\"/Job/CompetitorAnalysis?employeeNo=76811956\" target=\"_blank\" class=\"applicants_data_analyze\"></a></div></div><ul class=\"job_item_data d-flex item_list\"><li>日班</li><li>2年以上工作經驗</li><li>學歷不拘</li></ul></div></div><div class=\"job_item_data_mobile_show\"><small class=\"text-muted job_item_date\">2023/08/28</small></div><div class=\"item_group mt-5 d-flex align-items-center justify-content-between\"><div><a href=\"#\" class=\"btnCollectJob\" data-eno=\"76811956\"><span class=\"item_icon icon_add icon_love_add \"></span></a></div><button class=\"btn btn_secondary_2 btn_size_5 applicants_btn\" onclick=\"normalApply('76811956')\">應徵</button></div></div></div></div></div><div class=\"job_item job-listing-box fade-on-show\" data-page=\"1\"><div class=\"card-body\"><div class=\"body-wrapper\"><div class=\"job_item_info\"><div class=\"job_item_badge_mobile_show\"><div class=\"badge_group\"></div></div><a href=\"/job/76811958\" target=\"_blank\"><h5 class=\"card-title title_6\">Asp.Net Internet工程師 </h5></a><div class=\"card-subtitle mb-4 text-muted happiness-hidd\"><a href=\"#\"><h6 class=\"job_item_company mb-1 digit_5 body_3\">德義資訊股份有限公司</h6><div class=\"job_item_detail d-flex body_4\"><div class=\"job_item_detail_location mr-3 position-relative\">台北市大同區</div><div class=\"job_item_detail_salary ml-3 font-weight-style digit_6\">面議（經常性薪資達 4 萬元或以上）</div></div></a></div><div class=\"job_item_data_mobile_show\"><div class=\"job_item_data\"><div class=\"applicants mb-3 d-flex align-items-center\"><span>應徵人數：</span><span class=\"applicants_data\">1 ~ 5人</span><span class=\"item_icon_cursor_default item_icon_fire UI_icon_fire4\"></span><a href=\"/Job/CompetitorAnalysis?employeeNo=76811958\" target=\"_blank\" class=\"applicants_data_analyze\"></a></div></div><ul class=\"job_item_data d-flex item_list\"><li>日班</li><li>2年以上工作經驗</li><li>學歷不拘</li></ul></div><p class=\"card-text job_item_description body_4\">1.建置與開發網站前後台2.資料庫建置與規劃3.程式開發與維護4.熟悉.net(C#、VB) C#尤佳，及MS SQL5.熟悉Ajax、Java Script、CSS等相關網頁開發技術6.熟MVC開發架構佳7.具備三層式架構，N-Tier及OOP經驗，若有Team Foundation 經驗佳</p></div><div class=\"item_data\"><div class=\"job_item_data_mobile_hidden\"><small class=\"text-muted job_item_date\">2023/08/28</small><div class=\"badge_group mt-3\"></div><div class=\"item_group mt-3 body_4\"><div class=\"job_item_data\"><div class=\"applicants mb-3 d-flex align-items-center\"><span>應徵人數：</span><span class=\"applicants_data\">1 ~ 5人</span><span class=\"item_icon_cursor_default item_icon_fire UI_icon_fire4 \"></span><a href=\"/Job/CompetitorAnalysis?employeeNo=76811958\" target=\"_blank\" class=\"applicants_data_analyze\"></a></div></div><ul class=\"job_item_data d-flex item_list\"><li>日班</li><li>2年以上工作經驗</li><li>學歷不拘</li></ul></div></div><div class=\"job_item_data_mobile_show\"><small class=\"text-muted job_item_date\">2023/08/28</small></div><div class=\"item_group mt-5 d-flex align-items-center justify-content-between\"><div><a href=\"#\" class=\"btnCollectJob\" data-eno=\"76811958\"><span class=\"item_icon icon_add icon_love_add \"></span></a></div><button class=\"btn btn_secondary_2 btn_size_5 applicants_btn\" onclick=\"normalApply('76811958')\">應徵</button></div></div></div></div></div><div class=\"job_item job-listing-box fade-on-show\" data-page=\"1\"><div class=\"card-body\"><div class=\"body-wrapper\"><div class=\"job_item_info\"><div class=\"job_item_badge_mobile_show\"><div class=\"badge_group\"></div></div><a href=\"/job/76811962\" target=\"_blank\"><h5 class=\"card-title title_6\">(Taipei) Asp.Net Internet工程師 </h5></a><div class=\"card-subtitle mb-4 text-muted happiness-hidd\"><a href=\"#\"><h6 class=\"job_item_company mb-1 digit_5 body_3\">德義資訊股份有限公司</h6><div class=\"job_item_detail d-flex body_4\"><div class=\"job_item_detail_location mr-3 position-relative\">台北市大同區</div><div class=\"job_item_detail_salary ml-3 font-weight-style digit_6\">面議（經常性薪資達 4 萬元或以上）</div></div></a></div><div class=\"job_item_data_mobile_show\"><div class=\"job_item_data\"><div class=\"applicants mb-3 d-flex align-items-center\"><span>應徵人數：</span><span class=\"applicants_data\">1 ~ 5人</span><span class=\"item_icon_cursor_default item_icon_fire UI_icon_fire4\"></span><a href=\"/Job/CompetitorAnalysis?employeeNo=76811962\" target=\"_blank\" class=\"applicants_data_analyze\"></a></div></div><ul class=\"job_item_data d-flex item_list\"><li>日班</li><li>2年以上工作經驗</li><li>學歷不拘</li></ul></div><p class=\"card-text job_item_description body_4\">1.建置與開發網站前後台2.資料庫建置與規劃3.程式開發與維護4.熟悉.net(C#、VB) C#尤佳，及MS SQL5.熟悉Ajax、Java Script、CSS等相關網頁開發技術6.熟MVC開發架構佳7.具備三層式架構，N-Tier及OOP經驗，若有Team Foundation 經驗佳</p></div><div class=\"item_data\"><div class=\"job_item_data_mobile_hidden\"><small class=\"text-muted job_item_date\">2023/08/28</small><div class=\"badge_group mt-3\"></div><div class=\"item_group mt-3 body_4\"><div class=\"job_item_data\"><div class=\"applicants mb-3 d-flex align-items-center\"><span>應徵人數：</span><span class=\"applicants_data\">1 ~ 5人</span><span class=\"item_icon_cursor_default item_icon_fire UI_icon_fire4 \"></span><a href=\"/Job/CompetitorAnalysis?employeeNo=76811962\" target=\"_blank\" class=\"applicants_data_analyze\"></a></div></div><ul class=\"job_item_data d-flex item_list\"><li>日班</li><li>2年以上工作經驗</li><li>學歷不拘</li></ul></div></div><div class=\"job_item_data_mobile_show\"><small class=\"text-muted job_item_date\">2023/08/28</small></div><div class=\"item_group mt-5 d-flex align-items-center justify-content-between\"><div><a href=\"#\" class=\"btnCollectJob\" data-eno=\"76811962\"><span class=\"item_icon icon_add icon_love_add \"></span></a></div><button class=\"btn btn_secondary_2 btn_size_5 applicants_btn\" onclick=\"normalApply('76811962')\">應徵</button></div></div></div></div></div><div class=\"job_item job-listing-box fade-on-show\" data-page=\"1\"><div class=\"card-body\"><div class=\"body-wrapper\"><div class=\"job_item_info\"><div class=\"job_item_badge_mobile_show\"><div class=\"badge_group\"></div></div><a href=\"/job/76811989\" target=\"_blank\"><h5 class=\"card-title title_6\">系統及資料庫工程師 </h5></a><div class=\"card-subtitle mb-4 text-muted happiness-hidd\"><a href=\"#\"><h6 class=\"job_item_company mb-1 digit_5 body_3\">德義資訊股份有限公司</h6><div class=\"job_item_detail d-flex body_4\"><div class=\"job_item_detail_location mr-3 position-relative\">台北市內湖區</div><div class=\"job_item_detail_salary ml-3 font-weight-style digit_6\">面議（經常性薪資達 4 萬元或以上）</div></div></a></div><div class=\"job_item_data_mobile_show\"><div class=\"job_item_data\"><div class=\"applicants mb-3 d-flex align-items-center\"><span>應徵人數：</span><span class=\"applicants_data\">1 ~ 5人</span><span class=\"item_icon_cursor_default item_icon_fire UI_icon_fire4\"></span><a href=\"/Job/CompetitorAnalysis?employeeNo=76811989\" target=\"_blank\" class=\"applicants_data_analyze\"></a></div></div><ul class=\"job_item_data d-flex item_list\"><li>日班</li><li>工作經驗不拘</li><li>高中職以上</li></ul></div><p class=\"card-text job_item_description body_4\">1.建置、管理與維護公司系統。2.開發MIS系統的功能，以符合企業需求，需撰寫Oracle PL/ SQL。3.產出公司報表。</p></div><div class=\"item_data\"><div class=\"job_item_data_mobile_hidden\"><small class=\"text-muted job_item_date\">2023/08/28</small><div class=\"badge_group mt-3\"></div><div class=\"item_group mt-3 body_4\"><div class=\"job_item_data\"><div class=\"applicants mb-3 d-flex align-items-center\"><span>應徵人數：</span><span class=\"applicants_data\">1 ~ 5人</span><span class=\"item_icon_cursor_default item_icon_fire UI_icon_fire4 \"></span><a href=\"/Job/CompetitorAnalysis?employeeNo=76811989\" target=\"_blank\" class=\"applicants_data_analyze\"></a></div></div><ul class=\"job_item_data d-flex item_list\"><li>日班</li><li>工作經驗不拘</li><li>高中職以上</li></ul></div></div><div class=\"job_item_data_mobile_show\"><small class=\"text-muted job_item_date\">2023/08/28</small></div><div class=\"item_group mt-5 d-flex align-items-center justify-content-between\"><div><a href=\"#\" class=\"btnCollectJob\" data-eno=\"76811989\"><span class=\"item_icon icon_add icon_love_add \"></span></a></div><button class=\"btn btn_secondary_2 btn_size_5 applicants_btn\" onclick=\"normalApply('76811989')\">應徵</button></div></div></div></div></div><div class=\"job_item job-listing-box fade-on-show\" data-page=\"1\"><div class=\"card-body\"><div class=\"body-wrapper\"><div class=\"job_item_info\"><div class=\"job_item_badge_mobile_show\"><div class=\"badge_group\"></div></div><a href=\"/job/76811991\" target=\"_blank\"><h5 class=\"card-title title_6\">國際化WEB測試工程師 </h5></a><div class=\"card-subtitle mb-4 text-muted happiness-hidd\"><a href=\"#\"><h6 class=\"job_item_company mb-1 digit_5 body_3\">德義資訊股份有限公司</h6><div class=\"job_item_detail d-flex body_4\"><div class=\"job_item_detail_location mr-3 position-relative\">台北市大安區</div><div class=\"job_item_detail_salary ml-3 font-weight-style digit_6\">面議（經常性薪資達 4 萬元或以上）</div></div></a></div><div class=\"job_item_data_mobile_show\"><div class=\"job_item_data\"><div class=\"applicants mb-3 d-flex align-items-center\"><span>應徵人數：</span><span class=\"applicants_data\">1 ~ 5人</span><span class=\"item_icon_cursor_default item_icon_fire UI_icon_fire4\"></span><a href=\"/Job/CompetitorAnalysis?employeeNo=76811991\" target=\"_blank\" class=\"applicants_data_analyze\"></a></div></div><ul class=\"job_item_data d-flex item_list\"><li>週休二日</li><li>日班</li><li>工作經驗不拘</li><li>專科以上</li></ul></div><p class=\"card-text job_item_description body_4\">Web相關測試經驗撰寫Test Case／Plan</p></div><div class=\"item_data\"><div class=\"job_item_data_mobile_hidden\"><small class=\"text-muted job_item_date\">2023/08/28</small><div class=\"badge_group mt-3\"></div><div class=\"item_group mt-3 body_4\"><div class=\"job_item_data\"><div class=\"applicants mb-3 d-flex align-items-center\"><span>應徵人數：</span><span class=\"applicants_data\">1 ~ 5人</span><span class=\"item_icon_cursor_default item_icon_fire UI_icon_fire4 \"></span><a href=\"/Job/CompetitorAnalysis?employeeNo=76811991\" target=\"_blank\" class=\"applicants_data_analyze\"></a></div></div><ul class=\"job_item_data d-flex item_list\"><li>週休二日</li><li>日班</li><li>工作經驗不拘</li><li>專科以上</li></ul></div></div><div class=\"job_item_data_mobile_show\"><small class=\"text-muted job_item_date\">2023/08/28</small></div><div class=\"item_group mt-5 d-flex align-items-center justify-content-between\"><div><a href=\"#\" class=\"btnCollectJob\" data-eno=\"76811991\"><span class=\"item_icon icon_add icon_love_add \"></span></a></div><button class=\"btn btn_secondary_2 btn_size_5 applicants_btn\" onclick=\"normalApply('76811991')\">應徵</button></div></div></div></div></div></div><div id=\"job-loader\" class=\"job-loading\"><div class=\"bar-loader\"><div class=\"bar1111\"></div><div class=\"bar1111\"></div><div class=\"bar1111\"></div><div class=\"bar1111\"></div></div></div></div></div></div></form></div></div><hr> <a href=\"https://www.1111.com.tw/help/service.asp?oNo=69191119\" target=\"_blank\" class=\"link_sm\" rel=\"noopener noreferrer\" style=\"color:darkred\"><i class=\"UI_icon UI_icon_flag\"></i> 防詐檢舉/反應不實</a> </div>";

    [Test]
    public void GetCompanyCardContentNodes_NoH2()
    {
        var html = $"<div><div class='{Parameters1111.CompanyCardContentDivClassName}'><div class='corp1'></div></div></div>";
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(html);

        var service = new HtmlAnalyze1111Service(logger);

        var result = service.GetCompanyCardContentNodes(htmlDoc);

        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetCompanyCardContentNodes_NoDivWithClassName()
    {
        var html = $"<div><div class='{Parameters1111.CompanyCardContentDivClassName}'><h2>fake data</h2></div></div>";
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(html);

        var service = new HtmlAnalyze1111Service(logger);

        var result = service.GetCompanyCardContentNodes(htmlDoc);

        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetCompanyCardContentNodes_HasData()
    {
        var html = $"<div><div class='{Parameters1111.CompanyCardContentDivClassName}'><h2>fake data</h2><div class='corp1'><p>faek content</p></div></div></div>";
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(html);

        var service = new HtmlAnalyze1111Service(logger);

        var result = service.GetCompanyCardContentNodes(htmlDoc);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Has.Count.EqualTo(1));
    }

    [Test]
    public void GetCompanyCardContent_NoData()
    {
        var html = $"<article></article>";
        var htmlDoc = HtmlNode.CreateNode(html);

        var service = new HtmlAnalyze1111Service(logger);

        var result = service.GetCompanyCardContent(htmlDoc);

        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetCompanyCardContent_NoH2()
    {
        var html = $"<div></div>";
        var htmlDoc = HtmlNode.CreateNode(html);

        var service = new HtmlAnalyze1111Service(logger);

        var result = service.GetCompanyCardContent(htmlDoc);

        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetCompanyCardContent_NoH2TextNotInFilter()
    {
        var html = $"<div><h2>fake</h2></div>";
        var htmlDoc = HtmlNode.CreateNode(html);

        var service = new HtmlAnalyze1111Service(logger);

        var result = service.GetCompanyCardContent(htmlDoc);

        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetCompanyCardContent_NoH2TextFilterPass_NoContent()
    {
        var html = $"<div><h2>{Parameters1111.ProductsOrServicesHtmlTitle[0]}</h2></div>";
        var htmlDoc = HtmlNode.CreateNode(html);

        var service = new HtmlAnalyze1111Service(logger);

        var result = service.GetCompanyCardContent(htmlDoc);

        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetCompanyCardContent_NoH2TextFilterPass_HasContent()
    {
        var html = $"<div><h2>{Parameters1111.ProductsOrServicesHtmlTitle[0]}</h2><div>{TestValue.JustRandomComment}</div></div>";
        var htmlDoc = HtmlNode.CreateNode(html);

        var service = new HtmlAnalyze1111Service(logger);

        var result = service.GetCompanyCardContent(htmlDoc);

        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.Value.Key, Is.EqualTo(Parameters1111.CompanyContentFilter.First().Key));
            Assert.That(result.Value.Value, Is.EqualTo(TestValue.JustRandomComment));
        });
    }

    [Test]
    public void GetJobName_NoData()
    {
        var html = $"<div></div>";
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(html);

        var service = new HtmlAnalyze1111Service(logger);

        var result = service.GetJobName(htmlDoc);

        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetJobName_GetData()
    {
        var html = $"<h1>{TestValue.JustRandomComment}</h1>";
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(html);

        var service = new HtmlAnalyze1111Service(logger);

        var result = service.GetJobName(htmlDoc);

        Assert.That(result, Is.EqualTo(TestValue.JustRandomComment));
    }

    [Test]
    public void GetWorkContent_NoData()
    {
        var html = $"<div></div>";
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(html);

        var service = new HtmlAnalyze1111Service(logger);

        var result = service.GetWorkContent(htmlDoc);

        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetWorkContent_GetData()
    {
        var html = $"<div class='{Parameters1111.JobWorkContentDivClass}'><div>{TestValue.JustRandomComment}</div></div>";
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(html);

        var service = new HtmlAnalyze1111Service(logger);

        var result = service.GetWorkContent(htmlDoc);

        Assert.That(result, Is.EqualTo(TestValue.JustRandomComment));
    }

    [Test]
    public void GetJobPlace_NoData()
    {
        var html = $"<div></div>";
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(html);

        var service = new HtmlAnalyze1111Service(logger);

        var result = service.GetJobPlace(htmlDoc);

        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetJobPlace_NoSpanContent()
    {
        var html = $"<div><div class='{Parameters1111.JobPlaceIconDivClass}'></div></div>";
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(html);

        var service = new HtmlAnalyze1111Service(logger);

        var result = service.GetJobPlace(htmlDoc);

        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetJobPlace_GetContent()
    {
        var html = $"<div><div class='{Parameters1111.JobPlaceIconDivClass}'></div><span class='{Parameters1111.JobPlaceSpanClass}'>{TestValue.JustRandomComment}</span></div>";
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(html);

        var service = new HtmlAnalyze1111Service(logger);

        var result = service.GetJobPlace(htmlDoc);

        Assert.That(result, Is.EqualTo(TestValue.JustRandomComment));
    }

    [Test]
    public void GetOtherRequirement_NoData()
    {
        var html = $"<div></div>";
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(html);

        var service = new HtmlAnalyze1111Service(logger);

        var result = service.GetOtherRequirement(htmlDoc);

        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetOtherRequirement_GetData()
    {
        var html = $"<div class='{Parameters1111.JobOtherRequirementDivClass}'>{TestValue.JustRandomComment}</div>";
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(html);

        var service = new HtmlAnalyze1111Service(logger);

        var result = service.GetOtherRequirement(htmlDoc);

        Assert.That(result, Is.EqualTo(TestValue.JustRandomComment));
    }

    [Test]
    public void GetSalary_NoData()
    {
        var html = $"<div></div>";
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(html);

        var service = new HtmlAnalyze1111Service(logger);

        var result = service.GetSalary(htmlDoc);

        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetSalary_NoSpan()
    {
        var html = $"<div class='{Parameters1111.JobSalaryRegionDivClass}'></div>";
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(html);

        var service = new HtmlAnalyze1111Service(logger);

        var result = service.GetSalary(htmlDoc);

        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetSalary_GetData()
    {
        var html = $"<div class='{Parameters1111.JobSalaryRegionDivClass}'><span class='{Parameters1111.JobSalarySpanClass}'>{TestValue.JustRandomComment}</span></div>";
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(html);

        var service = new HtmlAnalyze1111Service(logger);

        var result = service.GetSalary(htmlDoc);

        Assert.That(result, Is.EqualTo(TestValue.JustRandomComment));
    }

    [Test]
    public void GetJobLastUpdateTime_NoData()
    {
        var html = $"<div></div>";
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(html);

        var service = new HtmlAnalyze1111Service(logger);

        var result = service.GetJobLastUpdateTime(htmlDoc);

        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetJobLastUpdateTime_GetData()
    {
        var html = $"<small class='{Parameters1111.JobLastUpdateTimeSmallClass}'>{TestValue.JustRandomComment}</small>";
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(html);

        var service = new HtmlAnalyze1111Service(logger);

        var result = service.GetJobLastUpdateTime(htmlDoc);

        Assert.That(result, Is.EqualTo(TestValue.JustRandomComment));
    }
}
