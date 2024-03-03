using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SightKeeper.Domain.Model;

namespace SightKeeper.Data.Configuration;

public sealed class ItemClassConfiguration : IEntityTypeConfiguration<ItemClass>
{
    public void Configure(EntityTypeBuilder<ItemClass> builder)
    {
        builder.HasKey(itemClass => itemClass.Id);
        builder.HasFlakeId(itemClass => itemClass.Id);
        builder.ToTable("ItemClasses");
    }
}