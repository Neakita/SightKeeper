using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Model.DataSets.Detector;

namespace SightKeeper.Data.Configuration;

public sealed class AssetConfiguration : IEntityTypeConfiguration<DetectorAsset>
{
    public void Configure(EntityTypeBuilder<DetectorAsset> builder)
    {
        builder.HasFlakeIdKey();
        builder.ToTable("Assets");
        builder.HasOne(asset => asset.Screenshot).WithOne().HasPrincipalKey<Screenshot>();
        builder.HasMany(asset => asset.Items).WithOne();
        builder.Navigation(asset => asset.Screenshot).AutoInclude();
    }
}