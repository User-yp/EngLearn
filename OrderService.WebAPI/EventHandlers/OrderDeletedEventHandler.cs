using ASPNETCore;
using MediatR;
using Order.Domain.Entities;
using Order.Infrastructure;

namespace Order.Domain.Event;

public class OrderDeletedEventHandler : INotificationHandler<OrderDeletedEvent>
{
    private readonly IOrderRepository repository;
    private readonly ILogger<OrderDeletedEventHandler> logger;

    public OrderDeletedEventHandler(IOrderRepository repository, ILogger<OrderDeletedEventHandler> logger)
    {
        this.repository = repository;
        this.logger = logger;
    }
    public async Task Handle(OrderDeletedEvent notification, CancellationToken cancellationToken)
    {
        var order = await repository.GetOrderByIdAsync(notification.Guid);
        logger.LogInformation($"{nameof(OrderDeletedEventHandler)}-{order.Id}-{order.DeletionTime}");

    }
}
