using ASPNETCore.RedisService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Order.Domain;
using Order.Domain.Entities;

namespace Order.Infrastructure;

public class OrderRepository : IOrderRepository
{
    private readonly ITableRepository repository;
    private readonly OrderDbContext dbContext;
    private readonly ILogger<IOrderRepository> logger;
    private readonly IRedisHelper redisHelper;

    public OrderRepository(ITableRepository repository, OrderDbContext dbContext, ILogger<IOrderRepository> logger, IRedisHelper redisHelper)
    {
        this.repository = repository;
        this.dbContext = dbContext;
        this.logger = logger;
        this.redisHelper = redisHelper;
    }

    public async Task<List<Domain.Entities.Order>?> GetAllNoClosedOrderBytableIdAsync(Guid id)
    {
        return await dbContext.Orders.Where(o => o.IsComplete==false&& o.OrderTableId == id).ToListAsync();
    }

    public async Task<Domain.Entities.Order?> GetOrderByIdAsync(Guid guid)
    {
        return await dbContext.Orders.FirstOrDefaultAsync(o => o.Id == guid);
    }

    public async Task<Domain.Entities.Order?> GetOrderByProjectTextAsync(string ProjectText)
    {
        return await dbContext.Orders.FirstOrDefaultAsync(o => o.ProjectText == ProjectText);
    }

    public async Task<List<Domain.Entities.Order>?> GetOrdersBySearchAsync(string search)
    {
        return await dbContext.Orders.Where(o=>o.ProjectText.ToLower().Contains(search.ToLower()) 
        || o.ProjectText.StartsWith(search)
        || o.ProjectText.EndsWith(search)
        || EF.Functions.Like(o.ProjectText, $"%{search}%")).ToListAsync();
    }

    public async Task<List<Domain.Entities.Order>?> GetByProductionOrderNumberAsync(string productionOrderNumber)
    {
        return await dbContext.Orders.Where(o=>o.ProductionOrderNumber == productionOrderNumber).ToListAsync();
    }

    public async Task<List<Domain.Entities.Order>?> GetByProjectCategoryAsync(string projectCategory)
    {
        return await dbContext.Orders.Where(o => o.ProjectCategory == projectCategory).ToListAsync();
    }

    public async Task<List<Domain.Entities.Order>?> GetByPurchaseGroupAsync(string purchaseGroup)
    {
        return await dbContext.Orders.Where(o => o.PurchaseGroup == purchaseGroup).ToListAsync();
    }

    public async Task<List<Domain.Entities.Order>?> GetOrdersByTableIdAsync(Guid tableId)
    {
        return await dbContext.Orders.Where(o => o.OrderTableId == tableId).ToListAsync();
    }

    public async Task<List<Domain.Entities.Order>?> GetOrdersByTableNameAsync(string tableName)
    {
        var table =await repository.GetTableByNameAsync(tableName);
        return await GetOrdersByTableIdAsync(table.Id);
    }

    public async Task<List<Domain.Entities.Order>?> GetAllClosedOrderAsync()
    {
        return await dbContext.Orders.Where(o=>o.DeliveryDate<DateTime.Now).ToListAsync();
    }
    
    public async Task<List<Domain.Entities.Order>?> GetAllClosedOrderBytableIdAsync(Guid id)
    {
        return await dbContext.Orders.Where(o => o.IsComplete==true&&o.OrderTableId==id).ToListAsync();
    }

    public async Task<List<Domain.Entities.Order>?> GetAllNoClosedOrderBytableNameAsync(string name)
    {
        var table = await repository.GetTableByNameAsync(name);
        return await dbContext.Orders.Where(o => o.IsComplete==false && o.OrderTableId == table.Id).ToListAsync();
    }

    public async Task<List<Domain.Entities.Order>?> GetAllClosedOrderBytableNameAsync(string name)
    {
        var table = await repository.GetTableByNameAsync(name);
        return await dbContext.Orders.Where(o => o.IsComplete==true && o.OrderTableId == table.Id).ToListAsync();
    }

    public async Task<List<Domain.Entities.Order>?> GetAllNoClosedOrderAsync()
    {
        return await dbContext.Orders.Where(o => o.DeliveryDate > DateTime.Now).ToListAsync();
    }

    public async Task<List<Domain.Entities.Order>?> GetAllOrdersAsync()
    {
        return await dbContext.Orders.ToListAsync();
    }

    public async Task<List<Domain.Entities.Order>?> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await dbContext.Orders.Where(o => o.DeliveryDate >= startDate && o.DeliveryDate <= endDate).ToListAsync();
    }

    public async Task<List<Domain.Entities.Order>?> GetAllDeleteOrdersAsync()
    {
        return await dbContext.Orders.IgnoreQueryFilters().Where(o => o.IsDeleted == true).ToListAsync();
    }
    
}
