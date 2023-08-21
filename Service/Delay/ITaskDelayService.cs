namespace Service.Delay;

public interface ITaskDelayService
{
    /// <summary>
    /// Worker 等待
    /// </summary>
    /// <returns></returns>
    public Task WorkerWaiting();

    /// <summary>
    /// 延遲時間
    /// </summary>
    /// <param name="delayTime"></param>
    /// <returns></returns>
    public Task Delay(TimeSpan delayTime);

}
