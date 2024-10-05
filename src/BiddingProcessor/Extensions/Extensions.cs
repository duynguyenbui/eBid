namespace eBid.BiddingProcessor.Extensions;

public static class Extensions
{
    public static void AddApplicationServices(this IHostApplicationBuilder builder)
    {
        builder.AddRabbitMqEventBus("eventbus")
            .ConfigureJsonOptions(options => options.TypeInfoResolverChain.Add(IntegrationEventContext.Default));

        builder.AddNpgsqlDataSource("biddingdb");

        builder.Services.AddOptions<BackgroundTaskOptions>()
            .BindConfiguration(nameof(BackgroundTaskOptions));

        builder.Services.AddHostedService<Worker>();
    }
}

[JsonSerializable(typeof(AuctionStatusChangedToFinished))]
partial class IntegrationEventContext : JsonSerializerContext
{
}