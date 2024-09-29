namespace eBid.Bidding.API.IntegrationEvents.Events;

public record AuctionStatusChangedIntegrationEvent(string Status) : IntegrationEvent;