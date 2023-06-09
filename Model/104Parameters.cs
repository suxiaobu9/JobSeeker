using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Model;

public static class _104Parameters
{
    private static readonly string[] JobAreas = new string[]
    {
        "6001001001", "6001001002",
        "6001001003", "6001001004",
        "6001001005", "6001001006",
        "6001001007", "6001001008",
        "6001001009", "6001001010",
        "6001001011", "6001001012",
        "6001002001", "6001002002",
        "6001002003", "6001002004",
        "6001002005", "6001002006",
        "6001002007", "6001002008",
        "6001002009", "6001002010",
        "6001002011", "6001002012",
        "6001002013", "6001002014",
        "6001002015", "6001002016",
        "6001002017", "6001002018",
        "6001002019", "6001002020",
        "6001002021", "6001002022",
        "6001002023", "6001002024",
        "6001002025", "6001002026",
        "6001002027", "6001002028",
        "6001002029"
    };

    private static readonly string[] Keywords = new string[]
    {
        ".net",
        "C#"
    };

    public static (string Area, string Keyword)[] AreaAndKeywords => JobAreas.SelectMany(x => Keywords.Select(y => (x, y))).ToArray();

    public static readonly string Referer = @"https://www.104.com.tw";
    public static string Get104JobUrl(string keyword, string jobArea, int page)
    {
        keyword = HttpUtility.UrlEncode(keyword);

        return $@"{Referer}/jobs/search/list?ro=1&kwop=7&keyword={keyword}&area={jobArea}&order=15&asc=0&page={page}&mode=l&jobsource=2018indexpoc&searchTempExclude=2&langFlag=0&langStatus=0&recommendJob=1&hotJob=1";
    }

    public static readonly string DataDir = "./data";
    public static readonly string JobListDir = Path.Combine(DataDir, "JobList");
    public static readonly string JobInfoDir = Path.Combine(DataDir, "JobInfos");
}
