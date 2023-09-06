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
    public void GetWaitTime_5AM開始且時間區間是6AM與6PM_等待1小時()
    {
        var datetime = new DateTime(2023, 1, 1, 5, 0, 0);
        var expect = new TimeSpan(1, 0, 0);
        var result = taskDelayService.GetWaitTime(datetime, 6, 18);
        Assert.That(result, Is.EqualTo(expect));
    }

    [Test]
    public void GetWaitTime_7AM開始且時間區間是6AM與6PM_等待11小時()
    {
        var datetime = new DateTime(2023, 1, 1, 7, 0, 0);
        var expect = new TimeSpan(11, 0, 0);
        var result = taskDelayService.GetWaitTime(datetime, 6, 18);
        Assert.That(result, Is.EqualTo(expect));
    }

    [Test]
    public void GetWaitTime_7PM開始且時間區間是6AM與6PM_等待11小時()
    {
        var datetime = new DateTime(2023, 1, 1, 19, 0, 0);
        var expect = new TimeSpan(11, 0, 0);
        var result = taskDelayService.GetWaitTime(datetime, 6, 18);
        Assert.That(result, Is.EqualTo(expect));
    }

    [Test]
    public void GetWaitTime_5AM開始且時間區間是12PM與6PM_等待7小時()
    {
        var datetime = new DateTime(2023, 1, 1, 5, 0, 0);
        var expect = new TimeSpan(7, 0, 0);
        var result = taskDelayService.GetWaitTime(datetime, 12, 18);
        Assert.That(result, Is.EqualTo(expect));
    }

    [Test]
    public void GetWaitTime_7AM開始且時間區間是12PM與6PM_等待5小時()
    {
        var datetime = new DateTime(2023, 1, 1, 7, 0, 0);
        var expect = new TimeSpan(5, 0, 0);
        var result = taskDelayService.GetWaitTime(datetime, 12, 18);
        Assert.That(result, Is.EqualTo(expect));
    }

    [Test]
    public void GetWaitTime_7PM開始且時間區間是12PM與6PM_等待17小時()
    {
        var datetime = new DateTime(2023, 1, 1, 19, 0, 0);
        var expect = new TimeSpan(17, 0, 0);
        var result = taskDelayService.GetWaitTime(datetime, 12, 18);
        Assert.That(result, Is.EqualTo(expect));
    }
}
