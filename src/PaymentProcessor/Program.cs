var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddOptions<PaymentOptions>()
    .BindConfiguration(nameof(PaymentOptions));

builder.AddRabbitMqEventBus("eventbus")
    .AddSubscription<OrderChangedToPaymentConfirmedIntegrationEvent,
        OrderChangedToPaymentConfirmedIntegrationEventHandler>();

var app = builder.Build();

app.MapDefaultEndpoints();

await app.RunAsync();