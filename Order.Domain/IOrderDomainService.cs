using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using Order.Domain.Entities;
using Order.Domain.Result;
using System.Globalization;

namespace Order.Domain;

public  class IOrderDomainService(IOrderRepository orderRepository, ITableRepository tableRepository)
{
    private readonly IOrderRepository orderRepository = orderRepository;
    private readonly ITableRepository tableRepository = tableRepository;

    public async Task<(OperateResult, OrderTable?)> CreateTableAsync(string tableName)
    {
        var orderTable = await tableRepository.GetTableByNameAsync(tableName);
        if (orderTable != null)
            return (OperateResult.Failed(new OperateError { Code = "InvalidOrder", Description = "TableExit" }), null);
        else
        {
            if (!await tableRepository.CreateTableAsync(tableName))
                return (OperateResult.Failed(new OperateError { Code = "InvalidOrder", Description = "TableExit" }), null);
            return (OperateResult.Success, new OrderTable(tableName));
        }   
    }

    public async Task<(OperateResult, List<OrderTable>?)> GetAllTableAsync()
    {
        var table = await tableRepository.GetAllTablesAsync();
        if (table == null)
            return (OperateResult.Failed(new OperateError { Code = "InvalidTable", Description = "NoTable" }), null);
        else
            return (OperateResult.Success, table);
    }

    public async Task<OperateResult> DeletedTableAsync(OrderTable table)
    {
        if (await tableRepository.GetTableByIdAsync(table.Id) == null)
            return OperateResult.Failed(new OperateError { Code = "InvalidTable", Description = "TableNotExit" });
        table.SoftDelete();
        return OperateResult.Success;
    }

    public async Task<OperateResult> DeletedAllTableAsync()
    {
        var tables = await tableRepository.GetAllTablesAsync();
        if (tables == null)
            return OperateResult.Failed(new OperateError { Code = "InvalidTable", Description = "NoTable" });
        else
        {
            foreach (var table in tables)
            {
                table.SoftDelete();
            }
        }
        return OperateResult.Success;
    }

    public async Task<OperateResult> ChangeTableNameAsync(OrderTable table,string name)
    {
        if (await tableRepository.GetTableByIdAsync(table.Id) == null)
            return OperateResult.Failed(new OperateError { Code = "InvalidTable", Description = "TableNotExit" });
        table.ChangeTableName(name);
        return OperateResult.Success;
    }

    //Todo:
    public async Task<(OperateResult,bool)> SetTableCompleteAsync(OrderTable table)
    {
        if (await tableRepository.GetTableByIdAsync(table.Id) == null)
            return (OperateResult.Failed(new OperateError { Code = "InvalidTable", Description = "TableNotExit" }),false);
        var orders= await orderRepository.GetAllNoClosedOrderBytableIdAsync(table.Id);
        if (orders.Count != 0)
        {
            await SetAllOrdersCompletedByTableAsync(table.Id);
            table.SetComplete();
            return (OperateResult.Success, false);
        }
        table.SetComplete();
        return (OperateResult.Success,true);
    }

    public async Task<(OperateResult,OrderTable?)> GetOrderBelongTableAsync(Entities.Order order)
    {
        if (await orderRepository.GetOrderByIdAsync(order.Id) == null)
            return (OperateResult.Failed(new OperateError { Code = "InvalidOrder", Description = "OrderNotExit" }), null);
        var table = await tableRepository.GetOrderBelongTableAsync(order);
        if (table == null)
            return (OperateResult.Failed(new OperateError { Code = "InvalidTable", Description = "NoTable" }), null);
        return (OperateResult.Success, table);
    }

    public async Task<OperateResult> DelayAllOrderFromTableAsync(OrderTable table,int days)
    {
        if (await tableRepository.GetTableByIdAsync(table.Id) == null)
            return OperateResult.Failed(new OperateError { Code = "InvalidTable", Description = "TableNotExit" });
        var orders = await orderRepository.GetAllNoClosedOrderBytableIdAsync(table.Id);
        if (orders == null)
            return OperateResult.Failed(new OperateError { Code = "InvalidOrder", Description = "NoNotClosedOrder" });
        foreach (var order in orders)
        {
            order.DelayDeliveryDate(days);
        }
        return OperateResult.Success;
    }

    public async Task<OperateResult> DelayAllOrderFromTableAsync(OrderTable table,DateTime dateTime)
    {
        if (await tableRepository.GetTableByIdAsync(table.Id) == null)
            return OperateResult.Failed(new OperateError { Code = "InvalidTable", Description = "TableNotExit" });
        var orders = await orderRepository.GetAllNoClosedOrderBytableIdAsync(table.Id);
        if (orders == null)
            return OperateResult.Failed(new OperateError { Code = "InvalidOrder", Description = "NoNotClosedOrder" });
        if (dateTime < DateTime.Now)
            return OperateResult.Failed(new OperateError { Code = "InvalidDate", Description = "EarlierThanNow" });
        foreach (var order in orders)
        {
            order.ChangeDeliveryDate(dateTime);
        }
        return OperateResult.Success;
    }

    public async Task<(OperateResult, List<OrderTable>?)> GetTablesBySearch(string search)
    {
        var tables = await tableRepository.GetTablesBySearch(search);
        if (tables == null)
            return (OperateResult.Failed(new OperateError { Code = "InvalidOrder", Description = "NoOrder" }), null);
        return (OperateResult.Success, tables);
    }

    //订单操作
    public async Task<(OperateResult,Entities.Order?)> GetOrderByIdAsync(Guid guid)
    {
        var order=await orderRepository.GetOrderByIdAsync(guid);
        if (order == null)
            return (OperateResult.Failed(new OperateError { Code = "InvalidOrder", Description = "NoOrder" }), null);
        return (OperateResult.Success, order);
    }
    
    public async Task<(OperateResult,List<Entities.Order>?)> GetAllOrdersAsync()
    {
        var orders = await orderRepository.GetAllOrdersAsync();
        if (orders == null)
            return (OperateResult.Failed(new OperateError { Code = "InvalidOrder", Description = "NoOrder" }), null);
        return (OperateResult.Success, orders);
    }

    public async Task<(OperateResult,List<Entities.Order>?)> GetOrderByProductionOrderNumberAsync(string productionOrderNumber)
    {
        var orders = await orderRepository.GetByProductionOrderNumberAsync(productionOrderNumber);
        if (orders == null)
            return (OperateResult.Failed(new OperateError { Code = "InvalidOrder", Description = "NoOrder" }), null);
        return (OperateResult.Success, orders);
    }

    public async Task<(OperateResult, List<Entities.Order>?)> GetByProjectCategoryAsync(string projectCategory)
    {
        var orders = await orderRepository.GetByProjectCategoryAsync(projectCategory);
        if (orders == null)
            return (OperateResult.Failed(new OperateError { Code = "InvalidOrder", Description = "NoOrder" }), null);
        return (OperateResult.Success, orders);
    }

    public async Task<(OperateResult, List<Entities.Order>?)> GetByPurchaseGroupAsync(string purchaseGroup)
    {
        var orders = await orderRepository.GetByPurchaseGroupAsync(purchaseGroup);
        if (orders == null)
            return (OperateResult.Failed(new OperateError { Code = "InvalidOrder", Description = "NoOrder" }), null);
        return (OperateResult.Success, orders);
    }

    public async Task<(OperateResult,List<Entities.Order>?)> GetAllClosedOrderAsync()
    {
        var orders= await orderRepository.GetAllClosedOrderAsync();
        if (orders == null)
            return (OperateResult.Failed(new OperateError{ Code = "InvalidOrder", Description = "NoOrder" }), null);
        return (OperateResult.Success, orders);
    }

    public async Task<(OperateResult, List<Entities.Order>?)> GetAllNoClosedOrderAsync()
    {
        var orders = await orderRepository.GetAllNoClosedOrderAsync();
        if (orders == null)
            return (OperateResult.Failed(new OperateError { Code = "InvalidOrder", Description = "NoOrder" }), null);
        return (OperateResult.Success, orders);
    }

    public async Task<(OperateResult, List<Entities.Order>?)> GetAllDeleteOrdersAsync()
    {
        var orders = await orderRepository.GetAllDeleteOrdersAsync();
        if (orders == null)
            return (OperateResult.Failed(new OperateError { Code = "InvalidOrder", Description = "NoOrder" }), null);
        return (OperateResult.Success, orders);
    }

    public async Task<(OperateResult, List<Entities.Order>?)> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        var orders = await orderRepository.GetOrdersByDateRangeAsync(startDate, endDate);
        if (orders == null)
            return (OperateResult.Failed(new OperateError { Code = "InvalidOrder", Description = "NoOrder" }), null);
        return (OperateResult.Success, orders);
    }

    public async Task<(OperateResult,List<Entities.Order>?)> GetAllNoClosedOrderBytableIdAsync(Guid guid)
    {
        var table=await tableRepository.GetTableByIdAsync(guid);
        if (table == null)
            return (OperateResult.Failed(new OperateError { Code = "InvalidTable", Description = "NoTable" }),null);
        var orders = await orderRepository.GetAllNoClosedOrderBytableIdAsync(table.Id);
        if (orders == null)
            return (OperateResult.Failed(new OperateError { Code = "InvalidOrder", Description = "NoOrder" }), null);
        return (OperateResult.Success, orders);
    }

    public async Task<(OperateResult, List<Entities.Order>?)> GetAllClosedOrderBytableIdAsync(Guid guid)
    {
        var table = await tableRepository.GetTableByIdAsync(guid);
        if (table == null)
            return (OperateResult.Failed(new OperateError { Code = "InvalidTable", Description = "NoTable" }), null);
        var orders = await orderRepository.GetAllClosedOrderBytableIdAsync(guid);
        if (orders == null)
            return (OperateResult.Failed(new OperateError { Code = "InvalidOrder", Description = "NoOrder" }), null);
        return (OperateResult.Success, orders);
    }

    public async Task<(OperateResult, List<Entities.Order>?)> GetAllNoClosedOrderBytableNameAsync(string tableName)
    {
        var table = await tableRepository.GetTableByNameAsync(tableName);
        if (table == null)
            return (OperateResult.Failed(new OperateError { Code = "InvalidTable", Description = "NoTable" }), null);
        var orders = await orderRepository.GetAllNoClosedOrderBytableNameAsync(tableName);
        if (orders == null)
            return (OperateResult.Failed(new OperateError { Code = "InvalidOrder", Description = "NoOrder" }), null);
        return (OperateResult.Success, orders);
    }

    public async Task<(OperateResult, List<Entities.Order>?)> GetAllClosedOrderBytableNameAsync(string tableName)
    {
        var table = await tableRepository.GetTableByNameAsync(tableName);
        if (table == null)
            return (OperateResult.Failed(new OperateError { Code = "InvalidTable", Description = "NoTable" }), null);
        var orders = await orderRepository.GetAllClosedOrderBytableNameAsync(tableName);
        if (orders == null)
            return (OperateResult.Failed(new OperateError { Code = "InvalidOrder", Description = "NoOrder" }), null);
        return (OperateResult.Success, orders);
    }

    public async Task<(OperateResult,List<Entities.Order>?)> GetOrdersByTableIdAsync(Guid guid)
    {
        var table = await tableRepository.GetTableByIdAsync(guid);
        if (table == null)
            return (OperateResult.Failed(new OperateError { Code = "InvalidTable", Description = "NoTable" }), null);
        var orders = await orderRepository.GetOrdersByTableIdAsync(guid);
        if (orders == null)
            return (OperateResult.Failed(new OperateError { Code = "InvalidOrder", Description = "NoOrder" }), null);
        return (OperateResult.Success, orders);
    }

    public async Task<(OperateResult, List<Entities.Order>?)> GetOrdersByTableNameAsync(string tableName)
    {
        var table = await tableRepository.GetTableByNameAsync(tableName);
        if (table == null)
            return (OperateResult.Failed(new OperateError { Code = "InvalidTable", Description = "NoTable" }), null);
        var orders = await orderRepository.GetOrdersByTableNameAsync(tableName);
        if (orders == null)
            return (OperateResult.Failed(new OperateError { Code = "InvalidOrder", Description = "NoOrder" }), null);
        return (OperateResult.Success, orders);
    }

    public async Task<OperateResult> DeletedOrderByIdAsync(Guid id)
    {
        var order=await orderRepository.GetOrderByIdAsync(id);
        if (order == null)
            return OperateResult.Failed(new OperateError { Code = "InvalidOrder", Description = "NoOrder" });
        order.SoftDelete();
        var table = await tableRepository.GetOrderBelongTableAsync(order);
        table?.RemoveOrder(order);
        return OperateResult.Success;
    }

    public async Task<OperateResult> DeletedAllCompleteOrdersByTableAsync(Guid Id)
    {
        if (await tableRepository.GetTableByIdAsync(Id) == null)
            return OperateResult.Failed(new OperateError { Code = "InvalidOrder", Description = "NoTable" });
        var orders=await orderRepository.GetAllClosedOrderBytableIdAsync(Id);
        if (orders == null)
            return OperateResult.Failed(new OperateError { Code = "InvalidOrder", Description = "NoCompleteOrder" });
        foreach (var order in orders)
        {
            order.SoftDelete();
            var table = await tableRepository.GetOrderBelongTableAsync(order);
            table?.RemoveOrder(order);
        }
        return OperateResult.Success;
    }

    public async Task<OperateResult> SetAllOrdersCompletedByTableAsync(Guid Id)
    {
        if (await tableRepository.GetTableByIdAsync(Id) == null)
            return OperateResult.Failed(new OperateError { Code = "InvalidOrder", Description = "NoTable" });
        var orders = await orderRepository.GetAllNoClosedOrderBytableIdAsync(Id);
        if (orders == null)
            return OperateResult.Failed(new OperateError { Code = "InvalidOrder", Description = "NoNotCompleteOrder" });
        foreach (var order in orders)
        {
            order.SoftDelete();
            var table = await tableRepository.GetOrderBelongTableAsync(order);
            table?.RemoveOrder(order);
        }
        return OperateResult.Success;
    }

    public async Task<OperateResult> SetTableToOrdersAsync(OrderTable orderTable,Entities.Order order)
    {
        var table=await tableRepository.GetTableByIdAsync(orderTable.Id);
        if (table == null)
            return OperateResult.Failed(new OperateError { Code = "InvalidTable", Description = "TableNotExit" });
        else
        {
            order.SetOrderTableId(table);
            return OperateResult.Success;
        }
    }

    public async Task<OperateResult> SetTableToOrdersAsync(OrderTable orderTable,List< Entities.Order> orders)
    {
        var table = await tableRepository.GetTableByIdAsync(orderTable.Id);
        if (table == null)
            return OperateResult.Failed(new OperateError { Code = "InvalidTable", Description = "TableNotExit" });
        else
        {
            foreach (var order in orders)
            {
                order.SetOrderTableId(table);
            }
            return OperateResult.Success;
        }
    }

    public async Task<OperateResult> DelayOrderByIdAsync(Guid id,int days)
    {
        var order = await orderRepository.GetOrderByIdAsync(id);
        if (order == null)
            return OperateResult.Failed(new OperateError { Code = "InvalidOrder", Description = "NoOrder" });
        order.DelayDeliveryDate(days);
        return OperateResult.Success;
    }

    public async Task<OperateResult> DelayOrderByIdAsync(Guid id,DateTime dateTime)
    {
        var order = await orderRepository.GetOrderByIdAsync(id);
        if (order == null)
            return OperateResult.Failed(new OperateError { Code = "InvalidOrder", Description = "NoOrder" });
        if(dateTime<DateTime.Now)
            return OperateResult.Failed(new OperateError { Code = "InvalidDate", Description = "EarlierThanNow" });
        order.ChangeDeliveryDate(dateTime);
        return OperateResult.Success;
    }

    public async Task<(OperateResult,List<Entities.Order>?)> GetOrdersBySearchAsync(string search)
    {
        var orders=await orderRepository.GetOrdersBySearchAsync(search);
        if(orders==null)
            return (OperateResult.Failed(new OperateError { Code = "InvalidOrder", Description = "NoOrder" }),null);
        return (OperateResult.Success,orders);
    }
    
    public async Task<OperateResult> AddOrderQuantityAsync(OrderTable table, Entities.Order order)
    {
        var exitTable=await tableRepository.GetTableByNameAsync(table.TableName);
        if (exitTable != null)
        {
            var exitOrder=await orderRepository.GetOrderByProjectTextAsync(order.ProjectText);
            exitOrder?.AddQuantity(order.Quantity);
        }
        return OperateResult.Success;
    }

    public async Task<(OperateResult,List<Entities.Order>?)> InitOrdersAsync(IFormFile excelFile)
    {
        List<Entities.Order> orders = [];
        using var package = new ExcelPackage(excelFile.OpenReadStream());
        if (package.Workbook.Worksheets.Count <= 0)
            return (OperateResult.Failed(new OperateError { Code = "InvalidFile", Description = "NoWorksheets" }),null);
        else
        {
            ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
            if (worksheet == null)
                return (OperateResult.Failed(new OperateError { Code = "InvalidFile", Description = "WorksheetIsNull" }), null);
            else
            {
                for (int row = 3; row <= worksheet.Dimension.End.Row; row++)
                {
                    DateTime deliveryDat;
                    string str = worksheet.Cells[row, 11].Value?.ToString();
                    DateTime.TryParseExact(str, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out deliveryDat);
                    var order = Order.Domain.Entities.Order.
                        Create(worksheet.Cells[row, 1].Value?.ToString(),
                        worksheet.Cells[row, 2].Value?.ToString(),
                        worksheet.Cells[row, 3].Value?.ToString(),
                        Convert.ToInt32(worksheet.Cells[row, 4].Value),
                        worksheet.Cells[row, 5].Value?.ToString(),
                        worksheet.Cells[row, 6].Value?.ToString(),
                        worksheet.Cells[row, 7].Value?.ToString(),
                        worksheet.Cells[row, 8].Value?.ToString(),
                        worksheet.Cells[row, 9].Value?.ToString(),
                        Convert.ToInt32(worksheet.Cells[row, 10].Value),
                        deliveryDat,
                        Convert.ToInt32(worksheet.Cells[row, 12].Value)
                        );
                    orders.Add(order);
                }
                return (OperateResult.Success, orders);
            }
        }
    }
    //用于监听刷新事件
    public async Task<OperateResult> SetAllDeliveryOrderComplete()
    {
        var orders = await orderRepository.GetAllClosedOrderAsync();
        if (orders == null)
            return OperateResult.Success;
        foreach (var order in orders)
        {
            if (order.IsComplete == false)
                order.SetComplete();
        }
        return OperateResult.Success;
    }

    
}
