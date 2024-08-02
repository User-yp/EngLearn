using ASPNETCore;
using ASPNETCore.RedisService;
using EventBus;
using Microsoft.AspNetCore.Mvc;
using Order.Domain;
using Order.Domain.Entities;
using Order.Infrastructure;
using OrderService.WebAPI.EventHandlers;
using OrderService.WebAPI.Response;

namespace OrderService.WebAPI.Controllers;

[Route("[controller]/[action]")]
[UnitOfWork(typeof(OrderDbContext))]
[ApiController]
public class OrderController:ControllerBase
{
    private readonly IOrderDomainService domainService;
    private readonly IOrderRepository oederRepository;
    private readonly ITableRepository tableRepository;
    private readonly IEventBus eventBus;
    private readonly IRedisHelper redis;
    private readonly OrderDbContext dbContext;

    public OrderController(IOrderDomainService domainService,IOrderRepository oederRepository, 
        ITableRepository tableRepository, IEventBus eventBus,IRedisHelper redis,OrderDbContext dbContext)
    {
        this.domainService = domainService;
        this.oederRepository = oederRepository;
        this.tableRepository = tableRepository;
        this.eventBus = eventBus;
        this.redis = redis;
        this.dbContext = dbContext;
    }

    [HttpGet]
    public async Task<ActionResult<GetOrderReponse>> GetOrderById(Guid Id)
    {
        (var operate, var order) = await domainService.GetOrderByIdAsync(Id);
        if (!operate.Succeeded)
            return BadRequest(operate.Errors);
        var table = await tableRepository.GetOrderBelongTableAsync(order);
        GetOrderReponse res =await InitGetOrderReponse(table, order);
        return Ok(res);
    }

    [HttpGet]
    public async Task<ActionResult<List<GetOrderReponse>>> GetAllOrders(int pageSize, int pageNumber)
    {
        (var ope,var orders)=await domainService.GetAllOrdersAsync();
        if(!ope.Succeeded)
            return BadRequest(ope.Errors);
        List<GetOrderReponse> res = [];
        foreach (var order in orders)
        {
            var table = await tableRepository.GetOrderBelongTableAsync(order);
            res.Add(await InitGetOrderReponse(table, order));
        }
        return Ok(ReponsePagination(res,pageSize,pageNumber));
    }

    [HttpGet]
    public async Task<ActionResult<List<GetOrderReponse>>> GetOrderByProductionOrderNumber(string productionOrderNumber)
    {
        (var ope, var orders) = await domainService.GetOrderByProductionOrderNumberAsync(productionOrderNumber);
        if (!ope.Succeeded)
            return BadRequest(ope.Errors);
        List<GetOrderReponse> res = [];
        foreach (var order in orders)
        {
            var table = await tableRepository.GetOrderBelongTableAsync(order);
            res.Add(await InitGetOrderReponse(table, order));
        }
        return Ok(res);
    }

    [HttpGet]
    public async Task<ActionResult<List<GetOrderReponse>>> GetByProjectCategory(string projectCategory)
    {
        (var ope, var orders) = await domainService.GetByProjectCategoryAsync(projectCategory);
        if (!ope.Succeeded)
            return BadRequest(ope.Errors);
        List<GetOrderReponse> res = [];
        foreach (var order in orders)
        {
            var table = await tableRepository.GetOrderBelongTableAsync(order);
            res.Add(await InitGetOrderReponse(table, order));
        }
        return Ok(res);
    }

    [HttpGet]
    public async Task<ActionResult<List<GetOrderReponse>>> GetByPurchaseGroup(string purchaseGroup)
    {
        (var ope, var orders) = await domainService.GetByPurchaseGroupAsync(purchaseGroup);
        if (!ope.Succeeded)
            return BadRequest(ope.Errors);
        List<GetOrderReponse> res = [];
        foreach (var order in orders)
        {
            var table = await tableRepository.GetOrderBelongTableAsync(order);
            res.Add(await InitGetOrderReponse(table, order));
        }
        return Ok(res);
    }

    [HttpGet]
    public async Task<ActionResult<List<GetOrderReponse>>> GetAllClosedOrder()
    {
        (var ope, var orders) = await domainService.GetAllClosedOrderAsync();
        if (!ope.Succeeded)
            return BadRequest(ope.Errors);
        List<GetOrderReponse> res = [];
        foreach (var order in orders)
        {
            var table = await tableRepository.GetOrderBelongTableAsync(order);
            res.Add(await InitGetOrderReponse(table, order));
        }
        return Ok(res);
    }

    [HttpGet]
    public async Task<ActionResult<List<GetOrderReponse>>> GetAllNoClosedOrder()
    {
        (var ope, var orders) = await domainService.GetAllNoClosedOrderAsync();
        if (!ope.Succeeded)
            return BadRequest(ope.Errors);
        List<GetOrderReponse> res = [];
        foreach (var order in orders)
        {
            var table = await tableRepository.GetOrderBelongTableAsync(order);
            res.Add(await InitGetOrderReponse(table, order));
        }
        return Ok(res);
    }

    [HttpGet]
    public async Task<ActionResult<List<GetOrderReponse>>> GetAllDeleteOrders()
    {
        (var ope, var orders) = await domainService.GetAllDeleteOrdersAsync();
        if (!ope.Succeeded)
            return BadRequest(ope.Errors);
        List<GetOrderReponse> res = [];
        foreach (var order in orders)
        {
            var table = await tableRepository.GetOrderBelongTableAsync(order);
            res.Add(await InitGetOrderReponse(table, order));
        }
        return Ok(res);
    }

    [HttpGet]
    public async Task<ActionResult<List<GetOrderReponse>>> GetOrdersByDateRange(DateTime startDate, DateTime endDate)
    {
        (var ope, var orders) = await domainService.GetOrdersByDateRangeAsync(startDate, endDate);
        if (!ope.Succeeded)
            return BadRequest(ope.Errors);
        List<GetOrderReponse> res = [];
        foreach (var order in orders)
        {
            var table = await tableRepository.GetOrderBelongTableAsync(order);
            res.Add(await InitGetOrderReponse(table, order));
        }
        return Ok(res);
    }

    [HttpGet]
    public async Task<ActionResult<List<GetOrderReponse>>> GetAllNoClosedOrderBytableId(Guid guid)
    {
        (var ope, var orders) = await domainService.GetAllNoClosedOrderBytableIdAsync(guid);
        if (!ope.Succeeded)
            return BadRequest(ope.Errors);
        List<GetOrderReponse> res = [];
        foreach (var order in orders)
        {
            var table = await tableRepository.GetOrderBelongTableAsync(order);
            res.Add(await InitGetOrderReponse(table, order));
        }
        return Ok(res);
    }

    [HttpGet]
    public async Task<ActionResult<List<GetOrderReponse>>> GetAllClosedOrderBytableId(Guid guid)
    {
        (var ope, var orders) = await domainService.GetAllClosedOrderBytableIdAsync(guid);
        if (!ope.Succeeded)
            return BadRequest(ope.Errors);
        List<GetOrderReponse> res = [];
        foreach (var order in orders)
        {
            var table = await tableRepository.GetOrderBelongTableAsync(order);
            res.Add(await InitGetOrderReponse(table, order));
        }
        return Ok(res);
    }

    [HttpGet]
    public async Task<ActionResult<List<GetOrderReponse>>> GetAllNoClosedOrderBytableName(string tableName)
    {
        (var ope, var orders) = await domainService.GetAllNoClosedOrderBytableNameAsync(tableName);
        if (!ope.Succeeded)
            return BadRequest(ope.Errors);
        List<GetOrderReponse> res = [];
        foreach (var order in orders)
        {
            var table = await tableRepository.GetOrderBelongTableAsync(order);
            res.Add(await InitGetOrderReponse(table, order));
        }
        return Ok(res);
    }

    [HttpGet]
    public async Task<ActionResult<List<GetOrderReponse>>> GetAllClosedOrderBytableName(string tableName)
    {
        (var ope, var orders) = await domainService.GetAllClosedOrderBytableNameAsync(tableName);
        if (!ope.Succeeded)
            return BadRequest(ope.Errors);
        List<GetOrderReponse> res = [];
        foreach (var order in orders)
        {
            var table = await tableRepository.GetOrderBelongTableAsync(order);
            res.Add(await InitGetOrderReponse(table, order));
        }
        return Ok(res);
    }

    [HttpGet]
    public async Task<ActionResult<List<GetOrderReponse>>> GetOrdersByTableId(Guid guid)
    {
        (var ope, var orders) = await domainService.GetOrdersByTableIdAsync(guid);
        if (!ope.Succeeded)
            return BadRequest(ope.Errors);
        List<GetOrderReponse> res = [];
        foreach (var order in orders)
        {
            var table = await tableRepository.GetOrderBelongTableAsync(order);
            res.Add(await InitGetOrderReponse(table, order));
        }
        return Ok(res);
    }

    [HttpGet]
    public async Task<ActionResult<List<GetOrderReponse>>> GetOrdersByTableName(string tableName)
    {
        (var ope, var orders) = await domainService.GetOrdersByTableNameAsync(tableName);
        if (!ope.Succeeded)
            return BadRequest(ope.Errors);
        List<GetOrderReponse> res = [];
        foreach (var order in orders)
        {
            var table = await tableRepository.GetOrderBelongTableAsync(order);
            res.Add(await InitGetOrderReponse(table, order));
        }
        return Ok(res);
    }

    [HttpDelete]
    public async Task<ActionResult> DeletedOrderById(Guid id)
    {
        var res = await domainService.DeletedOrderByIdAsync(id);
        if (!res.Succeeded)
            return BadRequest(res.Errors);
        eventBus.Publish("OrderService.Order.Deleted", new OrderDeletedEvent(id));
        return Ok(res);
    }

    [HttpDelete]
    public async Task<ActionResult> DeletedAllCompleteOrdersByTable(Guid Id)
    {
        var res = await domainService.DeletedAllCompleteOrdersByTableAsync(Id);
        if (!res.Succeeded)
            return BadRequest(res.Errors);
        eventBus.Publish("OrderService.Table.DeletedAllClosedOrder", new TableDeletedEvent(Id));
        return Ok(res);
    }

    [HttpPut]
    public async Task<ActionResult> SetAllOrdersCompletedByTable(Guid Id)
    {
        var res=await domainService.SetAllOrdersCompletedByTableAsync(Id);
        if (!res.Succeeded)
            return BadRequest(res.Errors);
        return Ok(res);
    }

    [HttpPut]
    public async Task<ActionResult> SetTableToOrdersAsync(Guid tableId, Guid orderId)
    {
        var orderTable=await tableRepository.GetTableByIdAsync(tableId);
        var order=await oederRepository.GetOrderByIdAsync(orderId);
        (var ope, var res) = await domainService.SetTableToOrdersAsync(orderTable, order);
        if (!ope.Succeeded)
            return BadRequest(ope.Errors);
        return Ok(res);
    }

    [HttpPut]
    public async Task<ActionResult> DelayOrderByDaysAsync(Guid id, int days)
    {
        var res = await domainService.DelayOrderByIdAsync(id,days);
        if (!res.Succeeded)
            return BadRequest(res.Errors);
        return Ok(res);
    }

    [HttpPut]
    public async Task<ActionResult> DelayOrderByTimeAsync(Guid id, DateTime dateTime)
    {
        var res = await domainService.DelayOrderByIdAsync(id, dateTime);
        if (!res.Succeeded)
            return BadRequest(res.Errors);
        return Ok(res);
    }

    [HttpGet]
    public async Task<ActionResult<List<GetOrderReponse>>> GetOrdersBySearchAsync(string search)
    {
        (var ope, var orders) = await domainService.GetOrdersBySearchAsync(search);
        if (!ope.Succeeded)
            return BadRequest(ope.Errors);
        List<GetOrderReponse> res = [];
        foreach (var order in orders)
        {
            var table = await tableRepository.GetOrderBelongTableAsync(order);
            res.Add(await InitGetOrderReponse(table, order));
        }
        return Ok(res);
    }

    [HttpPut]
    public async Task<ActionResult> AddOrderQuantityAsync(Guid tableId, Guid orderId)
    {
        var orderTable = await tableRepository.GetTableByIdAsync(tableId);
        var order = await oederRepository.GetOrderByIdAsync(orderId);
        (var ope, var res) = await domainService.AddOrderQuantityAsync(orderTable, order);
        if (!ope.Succeeded)
            return BadRequest(ope.Errors);
        return Ok(ope);
    }
    [HttpGet]
    public async Task<ActionResult> PreloadedOrderAsync()
    {
        await domainService.PreloadedAsync();
        return Ok();
    }
    [HttpGet]
    public async Task<ActionResult> GetPreloadedOrderAsync()
    {
        var res= await redis.HashGetAsync(nameof(Order.Domain.Entities.Order));
        List<Order.Domain.Entities.Order> orders = [];
        foreach (var item in res)
        {
            orders.Add(item.Value.ParseJson<Order.Domain.Entities.Order>());
        }
        return Ok(orders);
    }
    private async Task<GetOrderReponse> InitGetOrderReponse(OrderTable table, Order.Domain.Entities.Order order)
    {
        if (table == null)
            return new GetOrderReponse(null, order.ProductionOrderNumber, order.ItemNumber, order.ProjectText,
            order.Quantity, order.Unit, order.ProjectCategory, order.Process, order.PurchaseGroup, order.ItemGroup, order.DeliveryTime,
            order.DeliveryDate, order.ProcessTime, order.IsComplete, order.CreationTime, order.DeletionTime, order.LastModificationTime);
        return new GetOrderReponse(table.TableName, order.ProductionOrderNumber, order.ItemNumber, order.ProjectText,
            order.Quantity, order.Unit, order.ProjectCategory, order.Process, order.PurchaseGroup, order.ItemGroup, order.DeliveryTime,
            order.DeliveryDate, order.ProcessTime, order.IsComplete, order.CreationTime, order.DeletionTime, order.LastModificationTime);
    }
    private  List<GetOrderReponse> ReponsePagination(List<GetOrderReponse> orderReponses,int pageSize,int pageNumber)
    {
        return orderReponses.Skip((pageNumber-1)*pageSize).Take(pageSize).ToList();
    }
}
