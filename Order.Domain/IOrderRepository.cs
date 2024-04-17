namespace Order.Domain.Entities;

public interface IOrderRepository
{
    Task<List<Order>?> GetAllNoClosedOrderBytableIdAsync(Guid id);//根据表Id获取表中未完成订单
    Task<List<Order>?> GetAllClosedOrderBytableIdAsync(Guid id);//根据表Id获取表中完成订单
    Task<List<Order>?> GetAllNoClosedOrderBytableNameAsync(string tableName);//根据表name获取表中未完成订单
    Task<List<Order>?> GetAllClosedOrderBytableNameAsync(string tableName);//根据表name获取表中完成订单
    Task<List<Order>?> GetOrdersByTableIdAsync(Guid tableId);//根据订单表Id获取所有订单
    Task<List<Order>?> GetOrdersByTableNameAsync(string tableName);//根据订单表Name获取所有订单
    

    Task<List<Order>?> GetAllOrdersAsync(); // 获取所有订单
    Task<Order?> GetOrderByIdAsync(Guid guid);// 根据订单Id获取订单
    Task<Order?> GetOrderByProjectTextAsync(string ProjectText);// 根据订单Id获取订单
    Task<List<Order>?> GetOrdersBySearchAsync(string search);//根据订单表Name获取所有订单
    Task<List<Order>?> GetByProductionOrderNumberAsync(string productionOrderNumber); //根据订单生产订单号获取订单
    Task<List<Order>?> GetByProjectCategoryAsync(string projectCategory);//根据订单项目类别获取订单
    Task<List<Order>?> GetByPurchaseGroupAsync(string purchaseGroup);//根据订单采购组获取订单
    Task<List<Order>?> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate); // 根据日期范围获取订单列表
    Task<List<Order>?> GetAllClosedOrderAsync();// 获取所有截止订单
    Task<List<Order>?> GetAllNoClosedOrderAsync();// 获取所有未截止订单

    Task<List<Order>?> GetAllDeleteOrdersAsync(); // 获取所有已删除订单

    
}
