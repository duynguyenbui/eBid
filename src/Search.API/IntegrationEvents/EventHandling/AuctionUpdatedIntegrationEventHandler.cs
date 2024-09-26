namespace eBid.Search.API.IntegrationEvents.EventHandling;

public class AuctionUpdatedIntegrationEventHandler(
    ElasticsearchClient client,
    ILogger<AuctionUpdatedIntegrationEventHandler> logger) : IIntegrationEventHandler<AuctionUpdatedIntegrationEvent>
{
    public async Task Handle(AuctionUpdatedIntegrationEvent @event)
    {
        logger.LogInformation("Handling integration event: {event}", @event);
        var response = await client.UpdateAsync<AuctionItemData, AuctionItemData>("auctions", @event.ItemData.Id,
            descriptor =>
            {
                descriptor.Doc(@event.ItemData);
            });

        if (response.IsValidResponse)
        {
            logger.LogInformation("Successfully updated auction item with ID: {AuctionItemId}", @event.ItemData.Id);
        }
        else
        {
            logger.LogError("Failed to update auction item with ID: {AuctionItemId}. Error: {Error}",
                @event.ItemData.Id, response.DebugInformation);
        }
    }
}