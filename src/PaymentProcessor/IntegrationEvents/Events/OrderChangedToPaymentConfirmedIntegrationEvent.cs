namespace eBid.PaymentProcessor.IntegrationEvents.Events;

public record OrderChangedToPaymentConfirmedIntegrationEvent(int OrderId) : IntegrationEvent;