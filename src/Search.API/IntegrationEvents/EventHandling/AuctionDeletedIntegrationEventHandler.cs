namespace eBid.Search.API.IntegrationEvents.EventHandling;

public class AuctionDeletedIntegrationEventHandler(
    ElasticsearchClient client,
    ILogger<AuctionDeletedIntegrationEventHandler> logger)
    : IIntegrationEventHandler<AuctionDeletedIntegrationEvent>
{
    public async Task Handle(AuctionDeletedIntegrationEvent @event)
    {
        var response = await client.DeleteAsync<object>(@event.AuctionId, d => d
            .Index("auctions")
        );

        if (response.IsValidResponse)
        {
            logger.LogInformation("Successfully deleted auction with ID: {AuctionId}", @event.AuctionId);
        }
        else
        {
            logger.LogError("Failed to delete auction with ID: {AuctionId}. Reason: {Reason}", @event.AuctionId,
                response.ElasticsearchServerError?.Error?.Reason);
        }

        logger.LogInformation("Handled integration event: {Event}", @event);
    }
}