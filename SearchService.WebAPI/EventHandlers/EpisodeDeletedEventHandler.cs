using EventBus;
using SearchService.Domain;

namespace SearchService.WebAPI.EventHandlers;

[EventName("ListeningEpisode.Deleted")]
[EventName("ListeningEpisode.Hidden")]//被隐藏也看作删除
public class EpisodeDeletedEventHandler : DynamicIntegrationEventHandler
{
    private readonly IEpisodeSearchRepository repository;

    public EpisodeDeletedEventHandler(IEpisodeSearchRepository repository)
    {
        this.repository = repository;
    }

    public override Task HandleDynamic(string eventName, dynamic eventData)
    {
        Guid id = Guid.Parse(eventData.Id);
        return repository.DeleteAsync(id);
    }
}
