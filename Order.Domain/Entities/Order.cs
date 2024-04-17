using Domain;

namespace Order.Domain.Entities;

public record Order : AggregateRootEntity, IAggregateRoot
{
    public bool IsComplete { get;private set; } = false;//是否完成
    public Guid OrderTableId { get; private set; }
    public OrderTable OrderTable { get; private set; }
    #region 表单实体信息
    public string ProductionOrderNumber { get;private set; }//生产订单号
    public string? ItemNumber { get;private set; }//物料号
    public string ProjectText { get;private set; }//BOM项目文本
    public int? Quantity { get;private set; }//数量
    public string Unit { get;private set; }//单位
    public string ProjectCategory { get;private set; }//项目类别
    public string Process { get;private set; }//工序
    public string PurchaseGroup { get;private set; }//采购组
    public string ItemGroup { get;private set; }//物料组
    public int? DeliveryTime { get;private set; }//几天后要求交货
    public DateTime DeliveryDate { get; private set; }//交货日期
    public int ProcessTime { get; private set; }//收货处理时间
    #endregion
    public Order()
    {
        
    }
    public static Order Create(string ProductionOrderNumber, string? ItemNumber, string ProjectText,
        int? Quantity, string Unit, string ProjectCategory, string Process, string PurchaseGroup,
        string ItemGroup, int? DeliveryTime,DateTime DeliveryDate, int ProcessTime)
    {
        Order order = new()
        {
            ProductionOrderNumber = ProductionOrderNumber,
            ItemNumber = ItemNumber,
            ProjectText = ProjectText,
            Quantity = Quantity,
            Unit = Unit,
            ProjectCategory = ProjectCategory,
            Process = Process,
            PurchaseGroup = PurchaseGroup,
            ItemGroup = ItemGroup,
            DeliveryTime = DeliveryTime,
            DeliveryDate = DeliveryDate,
            ProcessTime = ProcessTime
        };
        return order;
    }
    public void SetOrderTableId(OrderTable orderTable)
    {
        OrderTableId = orderTable.Id;
    }
    public override void SoftDelete()
    {
        base.SoftDelete();
        //AddDomainEvent(new OrderDeletedEvent(Id));
    }
    public void ChangeDeliveryTime(int deliveryTime)
    {
        DeliveryTime = deliveryTime;
        NotifyModified();
    }
    public void ChangeDeliveryDate(DateTime dateDate)
    {
        DeliveryDate = dateDate;
        NotifyModified();
    }
    public void SetComplete()
    {
        IsComplete = true;
        NotifyModified();
    }
    public void DelayDeliveryDate(int days)
    {
        DeliveryDate.AddDays(days);
        NotifyModified();
    }
    public void AddQuantity(int? quantity)
    {
        Quantity += quantity;
        NotifyModified();
    }
}