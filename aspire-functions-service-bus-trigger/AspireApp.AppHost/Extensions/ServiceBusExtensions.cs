using Aspire.Hosting.Azure;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.DependencyInjection;

namespace AspireApp1.AppHost.Extensions;

public static class ServiceBusExtensions
{
    public static IResourceBuilder<AzureServiceBusQueueResource> WithTestCommands(
        this IResourceBuilder<AzureServiceBusQueueResource> builder)
    {
        builder.ApplicationBuilder.Services.AddSingleton<ServiceBusClient>(provider =>
        {
            var connectionString = builder.Resource.Parent.ConnectionStringExpression
                .GetValueAsync(CancellationToken.None).GetAwaiter().GetResult();
            return new ServiceBusClient(connectionString);
        });

        builder.WithCommand("SendSbMessage", "Send Service Bus message", executeCommand: async (c) =>
        {
            var sbClient = c.ServiceProvider.GetRequiredService<ServiceBusClient>();
            await sbClient.CreateSender(builder.Resource.QueueName)
                .SendMessageAsync(new ServiceBusMessage("Hello, world!"));

            return new ExecuteCommandResult { Success = true };
        });

        return builder;
    }
}
