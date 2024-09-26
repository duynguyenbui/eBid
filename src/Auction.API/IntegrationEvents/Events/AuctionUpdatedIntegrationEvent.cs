namespace eBid.Auction.API.IntegrationEvents.Events;

public record AuctionUpdatedIntegrationEvent(AuctionItemData ItemData) : IntegrationEvent;