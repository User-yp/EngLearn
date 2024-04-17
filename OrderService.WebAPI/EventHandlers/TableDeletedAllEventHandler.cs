using EventBus;
using Order.Domain.Entities;

namespace OrderService.WebAPI.EventHandlers;

[EventName("OrderService.Table.DeletedAll")]
public class TableDeletedAllEventHandler : JsonIntegrationEventHandler<TableDeletedEvent>
{
    private readonly IEventBus eventcBus;
    private readonly IOrderRepository repository;

    public TableDeletedAllEventHandler(IEventBus eventcBus, IOrderRepository repository)
    {
        this.eventcBus = eventcBus;
        this.repository = repository;
    }
    public override async Task HandleJson(string eventName, TableDeletedEvent? eventData)
    {
        var orders = await repository.GetOrdersByTableIdAsync(eventData.Id);
        if (orders != null)
        {
            foreach (var order in orders)
            {
                eventcBus.Publish("OrderTable.Deleted", new { OrderId = order.Id });
            } 
        }
    }
}
