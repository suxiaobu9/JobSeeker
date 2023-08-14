namespace Service.Mq;

public interface IMqService
{
    /// <summary>
    /// 送訊息到 Mq 
    /// </summary>
    /// <param name="queueName"></param>
    /// <param name="message"></param>
    public Task SendMessageToMq(string queueName, string message);

    /// <summary>
    /// 送訊息到 Mq 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="queueName"></param>
    /// <param name="message"></param>
    public Task SendMessageToMq<T>(string queueName, T message);

    /// <summary>
    /// 處理 MQ 來的訊息
    /// </summary>
    /// <param name="queueName"></param>
    /// <param name="message"></param>
    public Task ProcessMessageFromMq<T>(string queueName, Func<T, Task> messageHandler);

    /// <summary>
    /// 處理 Company mq 訊息
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="args"></param>
    /// <returns></returns>
    public abstract Task CompanyMessageHandler<T>(T args);

    /// <summary>
    /// 處理 Jobinfo mq 訊息
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="args"></param>
    /// <returns></returns>
    public abstract Task JobInfoMessageHandler<T>(T args);
}
