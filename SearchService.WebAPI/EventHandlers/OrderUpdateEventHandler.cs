using EventBus;
using SearchService.Domain;
using SearchService.Domain.Entities;

namespace SearchService.WebAPI.EventHandlers;

[EventName("OrderService.Order.Update")]
public class OrderUpdateEventHandler : DynamicIntegrationEventHandler
{
    private readonly IOrderSearchRepository repository;

    public OrderUpdateEventHandler(IOrderSearchRepository repository)
    {
        this.repository = repository;
    }

    public override async Task HandleDynamic(string eventName, dynamic eventData)
    {
        await repository.UpdateAsync((Guid)eventData.OrderId, new Dictionary<string, string>
        {
            {nameof(eventData.ProjectText),(string)eventData.ProjectText  }
        });
    }
}
