using eBid.EventBus.Events;

namespace eBid.EventBus.Abstractions;

public interface IEventBus
{
    Task PublishAsync(IntegrationEvent @event);
}