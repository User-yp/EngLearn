using EventBus;
using SearchService.Domain;

namespace SearchService.WebAPI.EventHandlers;

[EventName("SearchService.Table.ChangeName")]
public class TableChangeNameEventHandler : DynamicIntegrationEventHandler
{
    private readonly IOrderSearchRepository repository;

    public TableChangeNameEventHandler(IOrderSearchRepository repository)
    {
        this.repository = repository;
    }
    public override async Task HandleDynamic(string eventName, dynamic eventData)
    {
        await repository.UpdateAsync((Guid)eventData.OrderId, new Dictionary<string, string>
        {
            {nameof(eventData.TableName),(string)eventData.TableName  }
        });
    }
}
