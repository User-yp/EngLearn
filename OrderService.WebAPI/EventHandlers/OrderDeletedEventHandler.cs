using EventBus;
using OrderService.WebAPI.EventHandlers;

namespace Order.Domain.Event;

[EventName("OrderService.Order.Deleted")]
public class OrderDeletedEventHandler : JsonIntegrationEventHandler<OrderDeletedEvent>
{
    private readonly IEventBus eventBus;

    public OrderDeletedEventHandler(IEventBus eventBus)
    {
        this.eventBus = eventBus;
    }
    public override Task HandleJson(string eventName, OrderDeletedEvent? eventData)
    {
        eventBus.Publish("OrderTable.Deleted", new { eventData.Id});
        return Task.CompletedTask;
    }
}
