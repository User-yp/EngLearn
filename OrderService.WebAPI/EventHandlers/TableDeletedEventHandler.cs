using EventBus;
using Order.Domain.Entities;

namespace OrderService.WebAPI.EventHandlers;

[EventName("OrderService.Table.DeletedAllClosedOrder")]
public class TableDeletedEventHandler : JsonIntegrationEventHandler<TableDeletedEvent>
{
    private readonly IEventBus eventBus;
    private readonly IOrderRepository repository;

    public TableDeletedEventHandler(IEventBus eventBus,IOrderRepository repository)
    {
        this.eventBus = eventBus;
        this.repository = repository;
    }
    public override async Task HandleJson(string eventName, TableDeletedEvent? eventData)
    {
        var orders = await repository.GetAllClosedOrderBytableIdAsync(eventData.Id);
        foreach (var order in orders)
        {
            eventBus.Publish("OrderTable.Deleted", new { OrderId=order.Id });
        }
    }
}
