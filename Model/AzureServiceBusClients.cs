using Azure.Messaging.ServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model;

public class AzureServiceBusClients
{
    public readonly ServiceBusClient JobQueue;
    public readonly ServiceBusClient CompanyQueue;

    public AzureServiceBusClients(ServiceBusClient jobQueue, ServiceBusClient companyQueue)
    {
        this.JobQueue = jobQueue;
        this.CompanyQueue = companyQueue;
    }
}
