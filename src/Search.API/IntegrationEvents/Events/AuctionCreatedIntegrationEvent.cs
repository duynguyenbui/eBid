namespace eBid.Search.API.IntegrationEvents.Events;

public record AuctionCreatedIntegrationEvent(AuctionItemData ItemData) : IntegrationEvent;