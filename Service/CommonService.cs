namespace Service;

public class CommonService
{
    public static async Task WorkerWaiting()
    {
#if DEBUG
        return;
#endif
        var delay = GetWaitTime_6_18();

        await Task.Delay(delay);
    }

    private static TimeSpan GetWaitTime_6_18()
    {
        var now = DateTime.UtcNow.AddHours(8);

        var timeTo6 = new TimeSpan(6, 0, 0) - now.TimeOfDay;
        var timeTo18 = new TimeSpan(18, 0, 0) - now.TimeOfDay;

        if (timeTo6 < TimeSpan.Zero || timeTo18 < TimeSpan.Zero)
            return timeTo6 > TimeSpan.Zero ? timeTo6 : timeTo18;

        return timeTo6 < timeTo18 ? timeTo6 : timeTo18;
    }
}
