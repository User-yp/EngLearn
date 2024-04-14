using Domain;
using Order.Domain.Event;

namespace Order.Domain.Entities;

public record OrderTable: AggregateRootEntity, IAggregateRoot
{
    public string TableName { get;private set; }//订单表名
    public bool IsComplete { get; private set; } = false;//是否完成

    public List<Order> Orders { get;private set; }

    public OrderTable(string tableName)
    {
        TableName = tableName;
        Orders = [];
    }
    public bool ChangeTableName(string name)
    {
        TableName=name;
        NotifyModified();
        return true;
    }
    public override void SoftDelete()
    {
        base.SoftDelete();
        //AddDomainEvent(new OrderDeletedEvent(Id));
    }
    public void SetComplete()
    {
        IsComplete=true;
        NotifyModified();
    }
    public void AddOrder(Order order)
    {
        Orders.Add(order);
        NotifyModified();
    }
}
