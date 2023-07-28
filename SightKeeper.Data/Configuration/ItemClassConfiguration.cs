using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Model.Detector;

namespace SightKeeper.Data.Configuration;

public sealed class ItemClassConfiguration : IEntityTypeConfiguration<ItemClass>
{
    public void Configure(EntityTypeBuilder<ItemClass> builder)
    {
        builder.ToTable("ItemClasses").HasShadowKey();
        builder.HasMany<DetectorItem>("DetectorItems").WithOne(item => item.ItemClass);
    }
}