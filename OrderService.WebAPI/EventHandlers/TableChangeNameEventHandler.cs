using EventBus;
using Order.Domain.Entities;

namespace OrderService.WebAPI.EventHandlers;

[EventName("OrderService.Table.ChangeName")]
public class TableChangeNameEventHandler : JsonIntegrationEventHandler<TableChangeNameEvent>
{
    private readonly IEventBus eventBus;
    private readonly IOrderRepository repository;

    public TableChangeNameEventHandler(IEventBus eventBus,IOrderRepository repository)
    {
        this.eventBus = eventBus;
        this.repository = repository;
    }

    public override async Task HandleJson(string eventName, TableChangeNameEvent? eventData)
    {
        var orders = await repository.GetOrdersByTableIdAsync(eventData.Id);
        if (orders != null)
        {
            foreach (var order in orders)
            {
                eventBus.Publish("SearchService.Table.ChangeName", new { OrderId = order.Id, eventData.TableName });
            }
        }
    }
}
