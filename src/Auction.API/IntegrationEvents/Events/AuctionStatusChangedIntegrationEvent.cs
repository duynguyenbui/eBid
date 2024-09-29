namespace eBid.Auction.API.IntegrationEvents.Events;

public record AuctionStatusChangedIntegrationEvent(string Status) : IntegrationEvent;