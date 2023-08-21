using HtmlAgilityPack;

namespace Crawer_CakeResume.Service.Interface
{
    public interface IHtmlAnalyzeService
    {
        /// <summary>
        /// 取得公司介紹卡片內容
        /// </summary>
        /// <param name="htmlNode"></param>
        /// <returns></returns>
        KeyValuePair<string, string>? GetCompanyCardContent(HtmlNode htmlNode);

        /// <summary>
        /// 取得公司介紹卡片節點
        /// </summary>
        /// <param name="htmlDoc"></param>
        /// <returns></returns>
        HtmlNodeCollection? GetCompanyCardContentNodes(HtmlDocument htmlDoc);

        /// <summary>
        /// 取得公司名稱
        /// </summary>
        /// <param name="htmlDoc"></param>
        /// <returns></returns>
        string? GetCompanyName(HtmlDocument htmlDoc);

        /// <summary>
        /// 取得職缺卡片內文
        /// </summary>
        /// <param name="htmlNode"></param>
        /// <returns></returns>
        string? GetJobCardContent(HtmlNode htmlNode);

        /// <summary>
        /// 取得職缺內容節點
        /// </summary>
        /// <param name="htmlDoc"></param>
        /// <returns></returns>
        HtmlNodeCollection? GetJobCardContentNodes(HtmlDocument htmlDoc);

        /// <summary>
        /// 取得職缺卡片內文標題
        /// </summary>
        /// <param name="htmlNode"></param>
        /// <returns></returns>
        string? GetJobCardTitle(HtmlNode htmlNode);

        /// <summary>
        /// 取得最後更新時間
        /// </summary>
        /// <param name="htmlDoc"></param>
        /// <returns></returns>
        string? GetJobLastUpdateTime(HtmlDocument htmlDoc);

        /// <summary>
        /// 取得職缺清單內容傑點
        /// </summary>
        /// <param name="htmlDoc"></param>
        /// <returns></returns>
        HtmlNodeCollection? GetJobListCardContentNode(HtmlDocument htmlDoc);

        /// <summary>
        /// 取得職缺清單公司傑點
        /// </summary>
        /// <param name="htmlNode"></param>
        /// <returns></returns>
        HtmlNode? GetJobListCompanyNode(HtmlNode htmlNode);

        /// <summary>
        /// 取得職缺清單職缺傑點
        /// </summary>
        /// <param name="htmlNode"></param>
        /// <returns></returns>
        HtmlNode? GetJobListJobNode(HtmlNode htmlNode);

        /// <summary>
        /// 取得職缺名稱
        /// </summary>
        /// <param name="htmlDoc"></param>
        /// <returns></returns>
        string? GetJobName(HtmlDocument htmlDoc);

        /// <summary>
        /// 取得工作地點
        /// </summary>
        /// <param name="htmlDoc"></param>
        /// <returns></returns>
        string? GetJobPlace(HtmlDocument htmlDoc);

        /// <summary>
        /// 取得薪水
        /// </summary>
        /// <param name="htmlDoc"></param>
        /// <returns></returns>
        string? GetSalary(HtmlDocument htmlDoc);
    }
}