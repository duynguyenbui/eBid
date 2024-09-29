namespace eBid.Auction.API.IntegrationEvents.EventHandling;

public class AuctionStatusChangedEventHandler : IIntegrationEventHandler<AuctionStatusChangedIntegrationEvent>
{
    public Task Handle(AuctionStatusChangedIntegrationEvent @event)
    {
        throw new NotImplementedException();
    }
}