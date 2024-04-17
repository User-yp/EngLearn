using ASPNETCore;
using ASPNETCore.RedisService;
using EventBus;
using Microsoft.AspNetCore.Mvc;
using Order.Domain.Entities;
using Order.Domain;
using Order.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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
    public async Task<ActionResult> InitTable(IFormFile excelFile)
    {
        await tableRepository.CreateTableAsync(excelFile.FileName);
        return Ok();
    }
    [HttpPost]
    public async Task<ActionResult> UploadExcel(IFormFile excelFile)
    {
        (var initop, var initres) = await domainService.InitOrdersAsync(excelFile);
        if (!initop.Succeeded)
            return BadRequest(initop.Errors);
        else
        {
            var table= await tableRepository.GetTableByNameAsync(excelFile.FileName);
            var orderres = await domainService.SetTableToOrdersAsync(table, initres);
            if (!orderres.Succeeded)
                return BadRequest("OrderToTableError");
            foreach (var order in initres)
            {
                eventBus.Publish("OrderTable.Created", new { TableId = table.Id, OrderId = order.Id, table.TableName, order.ProjectText });
            }
            return Ok();
        }
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
