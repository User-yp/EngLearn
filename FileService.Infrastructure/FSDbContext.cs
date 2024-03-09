using FileService.Domain.Entity;
using Infrastructure.EFCore;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileService.Infrastructure;

public class FSDbContext : BaseDbContext
{
    public DbSet<UploadedItem> UploadItems { get; private set; }

    public FSDbContext(DbContextOptions<FSDbContext> options, IMediator mediator)
        : base(options, mediator)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
    }
}
