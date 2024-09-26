namespace eBid.Search.API.IntegrationEvents.EventHandling;

public class AuctionChangedToBeSoldIntegrationEventHandler(
    ElasticsearchClient client,
    ILogger<AuctionChangedToBeSoldIntegrationEventHandler> logger)
    : IIntegrationEventHandler<AuctionChangedToBeSoldIntegrationEvent>
{
    public async Task Handle(AuctionChangedToBeSoldIntegrationEvent @event)
    {
        logger.LogInformation("Handling integration event: {event}", @event);

        var response = await client.UpdateAsync<AuctionItemData, AuctionItemDataToChangedToSell>("auctions",
            @event.AuctionItemId,
            descriptor =>
            {
                descriptor.Doc(new AuctionItemDataToChangedToSell(true));
            });

        if (response.IsValidResponse)
        {
            logger.LogInformation("Successfully updated auction item with ID: {AuctionItemId}", @event.AuctionItemId);
        }
        else
        {
            logger.LogError("Failed to update auction item with ID: {AuctionItemId}. Error: {Error}",
                @event.AuctionItemId, response.DebugInformation);
        }
    }
}

public record AuctionItemDataToChangedToSell(bool OnSell);