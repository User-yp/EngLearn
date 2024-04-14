using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Infrastructure.Config;

public class OrderConfig : IEntityTypeConfiguration<Domain.Entities.Order>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Order> builder)
    {
        builder.ToTable($"T_{nameof(Order)}");
        builder.HasQueryFilter(o => o.IsDeleted == false);
        builder.HasOne(o=>o.OrderTable).WithMany(o=>o.Orders).HasForeignKey(o=>o.OrderTableId);
    }

}
