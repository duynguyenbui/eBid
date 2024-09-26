namespace eBid.Search.API.IntegrationEvents.Events;

public record AuctionDeletedIntegrationEvent(int AuctionId) : IntegrationEvent;