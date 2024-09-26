namespace eBid.Search.API.IntegrationEvents.Events;

public record AuctionChangedToBeSoldIntegrationEvent(int AuctionItemId) : IntegrationEvent;