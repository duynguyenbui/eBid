namespace eBid.Search.API.Extensions;

public static class Extensions
{
    public static void AddApplicationServices(this IHostApplicationBuilder builder)
    {
        builder.AddRedisClient("redis");

        builder.AddElasticsearchClient("elasticsearch");

        builder.Services.AddSingleton<RedisService>();

        builder.Services.AddScoped<IElasticSearchRepository<AuctionItemData>>(provider =>
        {
            var elasticsearch = provider.GetRequiredService<ElasticsearchClient>();
            var redisService = provider.GetRequiredService<RedisService>();

            return new CachingElasticSearchRepository(new ElasticSearchRepository(elasticsearch), redisService);
        });

        builder.AddRabbitMqEventBus("eventbus")
            .AddSubscription<AuctionCreatedIntegrationEvent, AuctionCreatedIntegrationEventHandler>()
            .AddSubscription<AuctionDeletedIntegrationEvent, AuctionDeletedIntegrationEventHandler>()
            .AddSubscription<AuctionChangedToBeSoldIntegrationEvent, AuctionChangedToBeSoldIntegrationEventHandler>()
            .AddSubscription<AuctionUpdatedIntegrationEvent, AuctionUpdatedIntegrationEventHandler>()
            .AddSubscription<AuctionImageUpdatedIntegrationEvent, AuctionImageUpdatedIntegrationEventHandler>();
    }
}