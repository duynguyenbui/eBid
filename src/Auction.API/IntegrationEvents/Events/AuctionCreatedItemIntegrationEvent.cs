namespace eBid.Auction.API.IntegrationEvents.Events;

public record AuctionCreatedIntegrationEvent(AuctionItemData ItemData) : IntegrationEvent;