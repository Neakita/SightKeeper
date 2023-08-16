using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SightKeeper.Domain.Model.Detector;

namespace SightKeeper.Data.Configuration;

public sealed class DetectorItemConfiguration : IEntityTypeConfiguration<DetectorItem>
{
    public void Configure(EntityTypeBuilder<DetectorItem> builder)
    {
        builder.ToTable("DetectorItems");
        builder.HasShadowKey();
        builder.OwnsOne(item => item.Bounding);
    }
}