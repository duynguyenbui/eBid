using eBid.EventBus.Abstractions;

namespace eBid.PaymentProcessor.IntegrationEvents.EventHandling;

public class OrderChangedToPaymentConfirmedIntegrationEventHandler : IIntegrationEventHandler<OrderChangedToPaymentConfirmedIntegrationEvent>
{
    public Task Handle(OrderChangedToPaymentConfirmedIntegrationEvent @event)
    {
        throw new NotImplementedException();
    }
}