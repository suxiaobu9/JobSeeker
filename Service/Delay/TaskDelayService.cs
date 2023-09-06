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
        return Delay(GetWaitTime(now, 12, 18));
    }

    /// <summary>
    /// 取得等待時間
    /// </summary>
    /// <returns></returns>
    public TimeSpan GetWaitTime(DateTime dateTime, params int[] hours)
    {
        if (hours.Length == 0)
            throw new ArgumentNullException($"{nameof(hours)}");

        var timeAry = hours.Select(x => new TimeSpan(x, 0, 0)).ToArray();

        var timeDiff = timeAry
            .Select(x => x - dateTime.TimeOfDay)
            .Where(x => x > TimeSpan.Zero)
            .ToArray();

        if (timeDiff.Any())
            return timeDiff.Min();

        // 處理跨日的情況
        return timeAry.Min() + TimeSpan.FromDays(1) - dateTime.TimeOfDay;
    }
}
