using Order.Domain.Entities;

namespace Order.Domain;

public interface ITableRepository
{
    Task<List<OrderTable>?> GetAllTablesAsync(); // 获取所有订单表
    Task<OrderTable?> GetTableByNameAsync(string tableName); // 根据表名获取订单表
    Task<OrderTable?> GetTableByIdAsync(Guid id); // 根据表Id获取订单表
    Task<List<OrderTable>?> GetAllCompleteTablesAsync(); // 获取所有完成订单表
    Task<List<OrderTable>?> GetAllNoCompleteTablesAsync(); // 获取所有未完成订单表

    Task<List<OrderTable>?> GetAllDeleteTablesAsync(); // 获取所有已删除订单表

    Task<OrderTable?> GetOrderBelongTableAsync(Entities.Order order);//查看订单所属订单表
    Task<List<OrderTable>?> GetTablesBySearch(string search);//根据订单表Name获取所有订单
}
