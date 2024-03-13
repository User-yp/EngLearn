using MediaEncoder.Domain.Entity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace MediaEncoder.Infrastructure.Config;

class EncodingItemConfig : IEntityTypeConfiguration<EncodingItem>
{
    public void Configure(EntityTypeBuilder<EncodingItem> builder)
    {
        builder.ToTable("T_ME_EncodingItems");
        //todo:符合索引。
        builder.HasKey(e=>e.Id).IsClustered(false);
        builder.Property(e => e.Name).HasMaxLength(256);
        builder.Property(e => e.FileSHA256Hash).HasMaxLength(64).IsUnicode(false);
        builder.Property(e => e.OutputFormat).HasMaxLength(10).IsUnicode(false);
        builder.Property(e => e.Status).HasConversion<string>().HasMaxLength(10);
    }
}
