using MediatR;

namespace Order.Domain.Event
{
    public  record OrderDeletedEvent(Guid Guid) : INotification;
}