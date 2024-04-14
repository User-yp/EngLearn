using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Order.Domain.Entities;

namespace Order.Infrastructure.Config;

public class OrderTableConfig : IEntityTypeConfiguration<OrderTable>
{

    public void Configure(EntityTypeBuilder<OrderTable> builder)
    {
        builder.ToTable($"T_{nameof(OrderTable)}");
        builder.HasQueryFilter(o => o.IsDeleted == false);
    }
}
