using EventBus;
using SearchService.Domain;
namespace SearchService.WebAPI.EventHandlers;
using Domain.Entities;

[EventName("OrderTable.Created")]
public class OrderUpsertEventHandler : DynamicIntegrationEventHandler
{
    private readonly IOrderSearchRepository repository;

    public OrderUpsertEventHandler(IOrderSearchRepository repository)
    {
        this.repository = repository;
    }
    public override Task HandleDynamic(string eventName, dynamic eventData)
    {
        return  repository.UpsertAsync(new((Guid)eventData.TableId, (Guid)eventData.OrderId,(string) eventData.TableName,(string) eventData.ProjectText));
    }
}
