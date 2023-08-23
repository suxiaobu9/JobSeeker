namespace Service.Delay;

public class TaskDelayService : ITaskDelayService
{
    public Task Delay(TimeSpan delayTime)
    {
        return Task.Delay(delayTime);
    }

    public Task WorkerWaiting()
    {
        var now = DateTime.UtcNow.AddHours(8);
        return Delay(GetWaitTime_6_18(now));
    }

    /// <summary>
    /// 取得等待時間，等到 6 點或 18 點
    /// </summary>
    /// <returns></returns>
    public TimeSpan GetWaitTime_6_18(DateTime dateTime)
    {
        var timeTo6 = new TimeSpan(6, 0, 0) - dateTime.TimeOfDay;
        var timeTo18 = new TimeSpan(18, 0, 0) - dateTime.TimeOfDay;

        // 處理跨日的情況
        if (timeTo6 < TimeSpan.Zero && timeTo18 < TimeSpan.Zero)
            return timeTo18 + TimeSpan.FromHours(12);

        if (timeTo6 < TimeSpan.Zero)
            return timeTo18;

        return timeTo6;
    }
}
