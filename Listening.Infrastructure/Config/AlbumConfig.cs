﻿using Listening.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Listening.Infrastructure.Config;

class AlbumConfig : IEntityTypeConfiguration<Album>
{
    public void Configure(EntityTypeBuilder<Album> builder)
    {
        builder.ToTable("T_Albums");
        builder.HasKey(e => e.Id).IsClustered(false);//对于Guid主键，不要建聚集索引，否则插入性能很差
        builder.OwnsOneMultilingualString(e => e.Name);
        builder.HasIndex(e => new { e.CategoryId, e.IsDeleted });
    }
}
