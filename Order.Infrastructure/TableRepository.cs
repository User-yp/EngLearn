using Microsoft.EntityFrameworkCore;
using Order.Domain;
using Order.Domain.Entities;
using Order.Domain.Result;

namespace Order.Infrastructure;

public class TableRepository : ITableRepository
{
    private readonly OrderDbContext dbContext;

    public TableRepository(OrderDbContext dbContext)
    {
        this.dbContext = dbContext;
    }
    public async Task<bool> CreateTableAsync(string tableName)
    {
        await dbContext.OrderTables.AddAsync(new OrderTable(tableName));
        await dbContext.SaveChangesAsync();
        return true;
    }
    public async Task<List<OrderTable>?> GetAllTablesAsync()
    {
        return await dbContext.OrderTables.ToListAsync();
    }
    public async Task<OrderTable?> GetTableByNameAsync(string tableName)
    {
        return await dbContext.OrderTables.SingleOrDefaultAsync(o => o.TableName == tableName);
    }
    public async Task<OrderTable?> GetTableByIdAsync(Guid id)
    {
        return await dbContext.OrderTables.SingleOrDefaultAsync(o => o.Id == id);
    }
    public async Task<List<OrderTable>?> GetAllCompleteTablesAsync()
    {
        return await dbContext.OrderTables.Where(t => t.IsComplete == true).ToListAsync();
    }

    public async Task<List<OrderTable>?> GetAllNoCompleteTablesAsync()
    {
        return await dbContext.OrderTables.Where(t => t.IsComplete == false).ToListAsync();
    }
    public async Task<List<OrderTable>?> GetAllDeleteTablesAsync()
    {
        return await dbContext.OrderTables.IgnoreQueryFilters().Where(t => t.IsDeleted == true).ToListAsync();
    }
    public async Task<OrderTable?> GetOrderBelongTableAsync(Domain.Entities.Order order)
    {
        return await dbContext.OrderTables.SingleOrDefaultAsync(t => t.Id == order.OrderTableId);
    }

    public async Task<List<OrderTable>?> GetTablesBySearch(string search)
    {
        return await dbContext.OrderTables.Where(o => o.TableName.ToLower().Contains(search.ToLower())
        || o.TableName.StartsWith(search)
        || o.TableName.EndsWith(search)
        || EF.Functions.Like(o.TableName, $"%{search}%")).ToListAsync();
    }
}
