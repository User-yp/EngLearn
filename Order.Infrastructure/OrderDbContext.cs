using Infrastructure.EFCore;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Order.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Infrastructure;

public class OrderDbContext : BaseDbContext
{
    public DbSet<Domain.Entities.Order> Orders {  get;private set; }
    public DbSet<OrderTable> OrderTables { get;private set; }
    public OrderDbContext(DbContextOptions options) : base(options)
    {
    }

    public OrderDbContext(DbContextOptions options, IMediator? mediator) : base(options, mediator)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
        modelBuilder.EnableSoftDeletionGlobalFilter();
    }
}
