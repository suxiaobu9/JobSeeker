namespace Service.Delay;

public class TaskDelayService : ITaskDelayService
{
    public Task Delay(TimeSpan delayTime)
    {
        return Task.Delay(delayTime);
    }

    public Task WorkerWaiting()
    {
        return Delay(GetWaitTime_6_18());
    }

    private static TimeSpan GetWaitTime_6_18()
    {
        var now = DateTime.UtcNow.AddHours(8);

        var timeTo6 = new TimeSpan(6, 0, 0) - now.TimeOfDay;
        var timeTo18 = new TimeSpan(18, 0, 0) - now.TimeOfDay;

        // 處理跨日的情況
        if (timeTo6 < TimeSpan.Zero && timeTo18 < TimeSpan.Zero)
            return timeTo18 + TimeSpan.FromHours(12);

        if (timeTo6 < TimeSpan.Zero)
            return timeTo18;

        return timeTo18;
    }
}
