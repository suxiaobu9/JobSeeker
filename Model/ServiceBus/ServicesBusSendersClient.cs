namespace Model.ServiceBus;

public class ServicesBusSendersClient
{
    public readonly Dictionary<string, ServicesBusSendersClient> ServicesBusSenders;

    public ServicesBusSendersClient(Dictionary<string , ServicesBusSendersClient> servicesBusSenders)
    {
        this.ServicesBusSenders = servicesBusSenders;
    }
}
