namespace Service;

public class CommonService
{
    public static async Task FirstWait()
    {
#if DEBUG
        return;
#endif

        var twNow = DateTime.UtcNow.AddHours(8);
        var midnight = twNow.Date.AddDays(1);

        var delay = midnight - twNow;

        await Task.Delay(delay);
    }

}
