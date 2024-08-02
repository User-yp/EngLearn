using ASPNETCore;
using ASPNETCore.RedisService;
using EventBus;
using Microsoft.AspNetCore.Mvc;
using Order.Domain.Entities;
using Order.Domain;
using Order.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OrderService.WebAPI.EventHandlers;
using Newtonsoft.Json;
using System.IO;
using Microsoft.AspNetCore.Http;
using OrderService.WebAPI.Request;

namespace OrderService.WebAPI.Controllers;

[Route("[controller]/[action]")]
[UnitOfWork(typeof(OrderDbContext))]
[ApiController]
public class InitTableController : ControllerBase
{
    private readonly IOrderDomainService domainService;
    private readonly IOrderRepository oederRepository;
    private readonly ITableRepository tableRepository;
    private readonly IEventBus eventBus;
    private readonly IRedisHelper redis;
    private readonly OrderDbContext dbContext;

    public InitTableController(IOrderDomainService domainService, IOrderRepository oederRepository,
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
    [RequestSizeLimit(60_000_000)]
    public async Task<ActionResult> InitTable([FromForm] UploadRequest req)
    {
        /*using (var memoryStream = new MemoryStream())
        {
            await req.File.CopyToAsync(memoryStream);
            var streamBytes = memoryStream.ToArray();
            eventBus.Publish("FileService.UploadFile", new { File = streamBytes, req.File.FileName });
        }*/
        await tableRepository.CreateTableAsync(req.File.FileName);
        return Ok();
    }
    [HttpPost]
    public async Task<ActionResult> UploadExcel(Guid Id, IFormFile excelFile)
    {
        (var initop, var initres) = await domainService.InitOrdersAsync(excelFile);
        if (!initop.Succeeded)
            return BadRequest(initop.Errors);

        var table = await tableRepository.GetTableByIdAsync(Id);
        (var ope,var orderres)   = await domainService.SetTableToOrdersAsync(table, initres);
        if (!ope.Succeeded)
            return BadRequest(ope.Errors);
        await dbContext.Orders.AddRangeAsync(orderres);
        foreach (var order in orderres)
        {
            eventBus.Publish("OrderTable.Created", new { TableId = table.Id, OrderId = order.Id, table.TableName, order.ProjectText });
        }

        using (var memoryStream = new MemoryStream())
        {
            await excelFile.CopyToAsync(memoryStream);
            var streamBytes = memoryStream.ToArray();
            eventBus.Publish("FileService.UploadFile", new { File = streamBytes, excelFile.FileName });
        }

        return Ok(ope);
    }

    [HttpPost]
    public async Task<ActionResult> TableMerge(Guid oldTableId, Guid newTableId)
    {
        var oldTable=await tableRepository.GetTableByIdAsync(oldTableId);
        var newTable = await tableRepository.GetTableByIdAsync(newTableId);
        (var ope,var res)= await domainService.TableMerge(oldTable, newTable);
        if (!ope.Succeeded)
            return BadRequest(ope.Errors);
        foreach (var order in res)
        {
            eventBus.Publish("OrderTable.Created", new { TableId = oldTableId, OrderId = order.Id, oldTable.TableName, order.ProjectText });
        }
        var opea=await domainService.DeletedTableAndOrderAsync(newTable);
        if (!opea.Succeeded)
            return BadRequest(opea.Errors);
        eventBus.Publish("OrderService.Table.DeletedAll", new TableDeletedEvent(newTable.Id));
        return Ok();
    }

    [HttpPut]
    public async Task<ActionResult> SetAllDeliveryOrderComplete()
    {
        var res = await domainService.SetAllDeliveryOrderComplete();
        if (!res.Succeeded)
            return BadRequest(res);
        return Ok(res);
    }

}
