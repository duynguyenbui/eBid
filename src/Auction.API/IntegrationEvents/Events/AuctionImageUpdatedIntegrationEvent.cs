namespace eBid.Auction.API.IntegrationEvents.Events;

public record AuctionImageUpdatedIntegrationEvent(int AuctionId, string PictureUrl) : IntegrationEvent;