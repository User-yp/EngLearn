using EventBus;
using SearchService.Domain;

namespace SearchService.WebAPI.EventHandlers;

[EventName("OrderTable.Deleted")]
public class OrderDeletedEventHandler : DynamicIntegrationEventHandler
{
    private readonly IOrderSearchRepository repository;

    public OrderDeletedEventHandler(IOrderSearchRepository repository)
    {
        this.repository = repository;
    }
    public override Task HandleDynamic(string eventName, dynamic eventData)
    {
        //Guid id = Guid.Parse(eventData.Id);
        return repository.DeleteAsync((Guid)eventData.OrderId);
    }
}
