using AspireApp1.AppHost.Extensions;

var builder = DistributedApplication.CreateBuilder(args);

var serviceBus = builder
    .AddAzureServiceBus("myservicebus")
    .RunAsEmulator(c => c
        .WithLifetime(ContainerLifetime.Persistent));

serviceBus
    .AddServiceBusQueue("myqueue")
    .WithTestCommands();

builder.AddAzureFunctionsProject<Projects.AspireApp_FunctionApp>("functionapp")
    .WithReference(serviceBus)
    .WaitFor(serviceBus);

builder.Build().Run();
