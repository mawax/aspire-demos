using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace AspireApp.FunctionApp;

public class ServiceBusFunction
{
    private readonly ILogger<ServiceBusFunction> _logger;

    public ServiceBusFunction(ILogger<ServiceBusFunction> logger)
    {
        _logger = logger;
    }

    [Function(nameof(ServiceBusFunction))]
    public async Task Run(
        [ServiceBusTrigger("myqueue", Connection = "myservicebus")]
        ServiceBusReceivedMessage message,
        ServiceBusMessageActions messageActions)
    {
        _logger.LogInformation("Successfully received message with body: {Body}", message.Body);

        // Complete the message
        await messageActions.CompleteMessageAsync(message);
    }
}
