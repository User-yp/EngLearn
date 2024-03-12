using EventBus;
using MediaEncoder.Domain.Event;
using MediatR;

namespace MediaEncoder.WebAPI.EventHandler;

class EncodingItemFailedEventHandler : INotificationHandler<EncodingItemFailedEvent>
{
    private readonly IEventBus eventBus;

    public EncodingItemFailedEventHandler(IEventBus eventBus)
    {
        this.eventBus = eventBus;
    }
    public Task Handle(EncodingItemFailedEvent notification, CancellationToken cancellationToken)
    {
        eventBus.Publish("MediaEncoding.Failed", notification);
        return Task.CompletedTask;
    }
}
