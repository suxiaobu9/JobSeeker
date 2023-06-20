namespace Service.Mq;

public interface IMqService
{
    public delegate Task MessageReceivedProcesserAsync(string message);

    /// <summary>
    /// 送訊息到 Mq 中
    /// </summary>
    /// <param name="queueName"></param>
    /// <param name="message"></param>
    public void SendMessageToMq(string queueName, string message);

    /// <summary>
    /// 送訊息到 Mq 中
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="queueName"></param>
    /// <param name="model"></param>
    public void SendMessageToMq<T>(string queueName, T model);

    /// <summary>
    /// 處理  MQ 來的訊息
    /// </summary>
    /// <param name="queueName"></param>
    /// <param name="message"></param>
    public void ProcessMessageFromMq(string queueName, MessageReceivedProcesserAsync processer, ushort? prefetchCount = null);

}
