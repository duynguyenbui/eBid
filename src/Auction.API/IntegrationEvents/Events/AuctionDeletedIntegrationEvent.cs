namespace eBid.Auction.API.IntegrationEvents.Events;

public record AuctionDeletedIntegrationEvent(int AuctionId) : IntegrationEvent;