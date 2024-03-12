using Infrastructure.EFCore;
using MediaEncoder.Domain.Entity;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaEncoder.Infrastructure;

public class MEDbContext : BaseDbContext
{
    public DbSet<EncodingItem> EncodingItems { get; private set; }

    public MEDbContext(DbContextOptions<MEDbContext> options):base(options)
    {
        
    }
    public MEDbContext(DbContextOptions<MEDbContext> options, IMediator mediator)
        : base(options, mediator)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
        modelBuilder.EnableSoftDeletionGlobalFilter();
    }
}
