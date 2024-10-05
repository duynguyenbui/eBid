namespace eBid.Search.API.IntegrationEvents.EventHandling;

public class AuctionCreatedIntegrationEventHandler(
    ElasticsearchClient client,
    ILogger<AuctionCreatedIntegrationEventHandler> logger)
    : IIntegrationEventHandler<AuctionCreatedIntegrationEvent>
{
    public async Task Handle(AuctionCreatedIntegrationEvent @event)
    {
        logger.LogInformation("Handling integration event: {event}", @event);
        var idxResponse = await client.IndexAsync(@event.ItemData,
            req => req.Index(ElasticSearchConstants.AuctionsElasticSearchConstants));

        if (!idxResponse.IsValidResponse)
        {
            logger.LogInformation("Indexing document failed: {item}", @event.ItemData);
        }

        logger.LogInformation("Handled integration event: {event}", @event);
    }
}