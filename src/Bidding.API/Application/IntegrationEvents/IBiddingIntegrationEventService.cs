namespace eBid.Bidding.API.Application.IntegrationEvents;

public interface IBiddingIntegrationEventService
{
    Task PublishEventsThroughEventBusAsync(Guid transactionId);
    Task AddAndSaveEventAsync(IntegrationEvent evt);
}