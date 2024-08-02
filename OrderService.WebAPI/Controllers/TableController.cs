using ASPNETCore;
using ASPNETCore.RedisService;
using EventBus;
using Microsoft.AspNetCore.Mvc;
using Order.Domain.Entities;
using Order.Domain;
using Order.Infrastructure;
using OrderService.WebAPI.Response;
using OrderService.WebAPI.Request;
using OrderService.WebAPI.EventHandlers;

namespace OrderService.WebAPI.Controllers;

[Route("[controller]/[action]")]
[UnitOfWork(typeof(OrderDbContext))]
[ApiController]
public class TableController : ControllerBase
{
    private readonly IOrderDomainService domainService;
    private readonly IOrderRepository oederRepository;
    private readonly ITableRepository tableRepository;
    private readonly IEventBus eventBus;
    private readonly IRedisHelper redis;
    private readonly OrderDbContext dbContext;

    public TableController(IOrderDomainService domainService, IOrderRepository oederRepository,
        ITableRepository tableRepository, IEventBus eventBus, IRedisHelper redis, OrderDbContext dbContext)
    {
        this.domainService = domainService;
        this.oederRepository = oederRepository;
        this.tableRepository = tableRepository;
        this.eventBus = eventBus;
        this.redis = redis;
        this.dbContext = dbContext;
    }

    [HttpPost]
    public async Task<ActionResult<GetTableResponse>> GetTableById(GetTableByIdRequest req)
    {
        var table=await tableRepository.GetTableByIdAsync(req.Id);
        if (table == null)
            return BadRequest("NoTable");
        return Ok(new GetTableResponse(table.TableName, table.IsComplete, table.CreationTime, table.LastModificationTime));
    }

    [HttpGet]
    public async Task<ActionResult<List<GetTableResponse>>> GetAllTables()
    {
        (var operate,var res) =await domainService.GetAllTableAsync();
        if (!operate.Succeeded)
            return BadRequest(operate.Errors);
        else
        {
            List<GetTableResponse>  responses = [];
            foreach (var item in res)
            {
                GetTableResponse getTableResponse = new(item.TableName, item.IsComplete, item.CreationTime, item.LastModificationTime);
                responses.Add(getTableResponse);
            }
            return Ok(responses);
        }
    }

    [HttpPost]
    public async Task<ActionResult> GetOrderBelongTable(Guid Id)
    {
        (var operate,var order)=await domainService.GetOrderByIdAsync(Id);
        if(!operate.Succeeded)
            return BadRequest(operate.Errors);
        else
        {
            (var res,var table)=await domainService.GetOrderBelongTableAsync(order);
            if(!res.Succeeded)
                return BadRequest(res.Errors);
            GetTableResponse getTableResponse = new(table.TableName, table.IsComplete, table.CreationTime, table.LastModificationTime);
            return Ok(getTableResponse);
        }
    }

    [HttpDelete]
    public async Task<ActionResult> DeleteAllTables()
    {
        var res=await domainService.DeletedAllTableAsync();
        if (!res.Succeeded)
            return BadRequest(res.Errors);
        return Ok(res);
    }

    [HttpDelete]
    public async Task<ActionResult> DeleteTableById(Guid Id)
    {
        var table = await tableRepository.GetTableByIdAsync(Id);
        var res=await domainService.DeletedTableAsync(table);
        if (!res.Succeeded)
            return BadRequest(res.Errors);
        eventBus.Publish("OrderService.Table.DeletedAll", new TableDeletedEvent(Id));
        return Ok(res);
    }

    [HttpPut]
    public async Task<ActionResult> ChangeTableNameById(Guid Id, string tableName)
    {
        var table=await tableRepository.GetTableByIdAsync(Id);
        var res = await domainService.ChangeTableNameAsync(table, tableName);
        if(!res.Succeeded)
            return BadRequest(res.Errors);
        eventBus.Publish("OrderService.Table.ChangeName", new TableChangeNameEvent(Id, tableName));
        return Ok(res);
    }

    [HttpPut]
    public async Task<ActionResult> SetTableCompleteById(Guid Id)
    {
        var table = await tableRepository.GetTableByIdAsync(Id);
        (var operate, var res) = await domainService.SetTableCompleteAsync(table);
        if (!operate.Succeeded)
            return BadRequest(operate.Errors);
        if(!res)
            return Ok("ExitNotCompleteOrder");
        return Ok(res);

    }

    [HttpPut]
    public async Task<ActionResult> DelayAllOrderFromTableByDay(Guid Id,int Days)
    {
        var table = await tableRepository.GetTableByIdAsync(Id);
        var res= await domainService.DelayAllOrderFromTableAsync(table, Days);
        if(!res.Succeeded)
            return BadRequest(res.Errors);
        return Ok(res);
    }

    [HttpPut]
    public async Task<ActionResult> DelayAllOrderFromTableByTime(Guid Id, DateTime dateTime)
    {
        var table = await tableRepository.GetTableByIdAsync(Id);
        var res = await domainService.DelayAllOrderFromTableAsync(table, dateTime);
        if (!res.Succeeded)
            return BadRequest(res.Errors);
        return Ok(res);
    }

    [HttpPost]
    public async Task<ActionResult<List<GetTableResponse>>> GetTablesBySearch(string search)
    {
        (var opereat, var tables) = await domainService.GetTablesBySearch(search);
        if (!opereat.Succeeded)
            return BadRequest(opereat.Errors);
        List<GetTableResponse> responses = [];
        foreach (var table in tables)
        {
            GetTableResponse res = new(table.TableName, table.IsComplete, table.CreationTime, table.LastModificationTime);
            responses.Add(res);
        }
        return Ok(responses);
    }


}
