namespace eBid.Bidding.API.IntegrationEvents.Events;

public record AuctionPlacedBidIntegrationEvent(int auctionId, string buyerId, decimal amount) : IntegrationEvent;