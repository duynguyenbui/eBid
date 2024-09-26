namespace eBid.Search.API.IntegrationEvents.Events;

public record AuctionUpdatedIntegrationEvent(AuctionItemData ItemData) : IntegrationEvent;