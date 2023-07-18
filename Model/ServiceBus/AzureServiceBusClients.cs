using Azure.Messaging.ServiceBus;

namespace Model.ServiceBus;

public class AzureServiceBusClients
{
    public readonly ServiceBusClient JobQueue;
    public readonly ServiceBusClient CompanyQueue;

    public AzureServiceBusClients(ServiceBusClient jobQueue, ServiceBusClient companyQueue)
    {
        JobQueue = jobQueue;
        CompanyQueue = companyQueue;
    }
}
