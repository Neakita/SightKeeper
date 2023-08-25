using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Model.Detector;

namespace SightKeeper.Data.Configuration;

public sealed class AssetConfiguration : IEntityTypeConfiguration<Asset>
{
    public void Configure(EntityTypeBuilder<Asset> builder)
    {
        builder.ToTable("Assets");
        builder.HasShadowKey();
        builder.HasOne(asset => asset.Screenshot).WithOne(screenshot => screenshot.Asset).HasPrincipalKey<Screenshot>();
        builder.HasDiscriminator<ModelType>("Type")
            .HasValue<DetectorAsset>(ModelType.Detector);
        builder.HasOne(asset => asset.DataSet).WithMany().IsRequired().HasForeignKey("DataSetId");
    }
}