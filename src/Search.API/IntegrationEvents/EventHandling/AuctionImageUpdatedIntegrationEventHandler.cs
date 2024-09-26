namespace eBid.Search.API.IntegrationEvents.EventHandling;

public class AuctionImageUpdatedIntegrationEventHandler(
    ElasticsearchClient client,
    ILogger<AuctionImageUpdatedIntegrationEventHandler> logger)
    : IIntegrationEventHandler<AuctionImageUpdatedIntegrationEvent>
{
    public async Task Handle(AuctionImageUpdatedIntegrationEvent @event)
    {
        logger.LogInformation("Handling integration event: {event}", @event);

        var response = await client.UpdateAsync<AuctionItemData, AuctionItemDataWithPictureUrl>("auctions",
            @event.AuctionId,
            descriptor =>
            {
                descriptor.Doc(new AuctionItemDataWithPictureUrl(@event.PictureUrl));
            });

        if (response.IsValidResponse)
        {
            logger.LogInformation("Successfully updated auction item with ID: {AuctionId}", @event.AuctionId);
        }
        else
        {
            logger.LogError("Failed to update auction item with ID: {AuctionId}. Error: {Error}",
                @event.AuctionId, response.DebugInformation);
        }
    }
}

public record AuctionItemDataWithPictureUrl(string PictureUrl);