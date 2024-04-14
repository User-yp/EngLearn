using ASPNETCore;
using ASPNETCore.RedisService;
using EventBus;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using Order.Domain;
using Order.Domain.Entities;
using Order.Infrastructure;
using System.Globalization;

namespace OrderService.WebAPI.Controllers;

[Route("[controller]/[action]")]
[UnitOfWork(typeof(OrderDbContext))]
[ApiController]
public class OrderTableController:ControllerBase
{
    private readonly IOrderDomainService domainService;
    private readonly IOrderRepository oederRepository;
    private readonly ITableRepository tableRepository;
    private readonly IEventBus eventBus;
    private readonly IRedisHelper redis;
    private readonly OrderDbContext dbContext;

    public OrderTableController(IOrderDomainService domainService,IOrderRepository oederRepository, 
        ITableRepository tableRepository, IEventBus eventBus,IRedisHelper redis,OrderDbContext dbContext)
    {
        this.domainService = domainService;
        this.oederRepository = oederRepository;
        this.tableRepository = tableRepository;
        this.eventBus = eventBus;
        this.redis = redis;
        this.dbContext = dbContext;
    }
    [HttpPost]
    public async Task<ActionResult<OrderTable>> UploadExcel(IFormFile excelFile)
    {
        OrderTable table = new(excelFile.FileName);
        List<Order.Domain.Entities.Order> orders = [];
        using (var package = new ExcelPackage(excelFile.OpenReadStream()))
            if (package.Workbook.Worksheets.Count > 0)
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                if (worksheet != null)
                {
                    for (int row = 3; row <= worksheet.Dimension.End.Row; row++)
                    {
                        DateTime deliveryDat;
                        string str = worksheet.Cells[row, 11].Value?.ToString();
                        DateTime.TryParseExact(str, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out  deliveryDat);
                        var  order = Order.Domain.Entities.Order.
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
                        order.SetOrderTableId(table);
                        table.AddOrder(order);
                        orders.Add( order );
                        dbContext.Orders.Add( order );
                    }
                }
                else
                {
                    // 处理工作表为null的情况
                }
            }
            else
            {
                // 处理工作簿中没有工作表的情况
            }
        await dbContext.OrderTables.AddAsync(table);
        await dbContext.SaveChangesAsync();
        return Ok();
    }

    [HttpDelete]
    public async Task<ActionResult<Order.Domain.Entities.Order>> Deleted(Guid guid)
    {
        var order = await oederRepository.GetOrderByIdAsync(guid);
        order.SoftDelete();
        return Ok(order);
    }
    [HttpPost]
    public async Task<ActionResult<bool>> TableTest(Guid guid)
    {
        /*var table=await oederRepository.GetTableByIdAsync(guid);
        await domainService.DeletedTable(table);
        return Ok(table);*/

        /*var table = await tableRepository.GetTableByIdAsync(guid);
        await domainService.DeletedTable(table);
        return Ok(table.IsDeleted);*/

        var table = await tableRepository.GetTableByIdAsync(guid);
        await domainService.SetTableCompleteAsync(table);
        return Ok(table.IsComplete);
    }
}
