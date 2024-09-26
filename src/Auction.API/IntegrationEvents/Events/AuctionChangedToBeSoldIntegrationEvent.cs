namespace eBid.Auction.API.IntegrationEvents.Events;

public record AuctionChangedToBeSoldIntegrationEvent(int AuctionItemId) : IntegrationEvent;