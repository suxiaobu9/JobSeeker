using NUnit.Framework;
using Service.Delay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Service;

public class TaskDelayServiceTest
{
    private readonly ITaskDelayService taskDelayService = new TaskDelayService();

    [Test]
    public void GetWaitTime_6_18_AM5取_等待1小時()
    {
        var datetime = new DateTime(2023, 1, 1, 5, 0, 0);
        var expect = new TimeSpan(1, 0, 0);
        var result = taskDelayService.GetWaitTime_6_18(datetime);
        Assert.That(result, Is.EqualTo(expect));
    }

    [Test]
    public void GetWaitTime_6_18_AM7取_等待11小時()
    {
        var datetime = new DateTime(2023, 1, 1, 7, 0, 0);
        var expect = new TimeSpan(11, 0, 0);
        var result = taskDelayService.GetWaitTime_6_18(datetime);
        Assert.That(result, Is.EqualTo(expect));
    }

    [Test]
    public void GetWaitTime_6_18_PM19點取_等待11小時_測試跨日()
    {
        var datetime = new DateTime(2023, 1, 1, 19, 0, 0);
        var expect = new TimeSpan(11, 0, 0);
        var result = taskDelayService.GetWaitTime_6_18(datetime);
        Assert.That(result, Is.EqualTo(expect));
    }
}
