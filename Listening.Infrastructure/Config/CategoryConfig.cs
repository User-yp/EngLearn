using Listening.Domain.Entity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Listening.Infrastructure.Config;

class CategoryConfig : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("T_Categories");
        builder.HasKey(e => e.Id).IsClustered(false);
        builder.OwnsOneMultilingualString(e => e.Name);
        builder.Property(e => e.CoverUrl).IsRequired(false).HasMaxLength(500).IsUnicode();
    }
}
