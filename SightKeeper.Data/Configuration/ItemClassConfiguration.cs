using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SightKeeper.Domain.Model.DataSets;

namespace SightKeeper.Data.Configuration;

public sealed class ItemClassConfiguration : IEntityTypeConfiguration<ItemClass>
{
    public void Configure(EntityTypeBuilder<ItemClass> builder)
    {
        builder.HasFlakeIdKey();
        builder.ToTable("ItemClasses");
    }
}