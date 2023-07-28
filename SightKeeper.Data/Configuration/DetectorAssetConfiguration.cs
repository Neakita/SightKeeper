using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SightKeeper.Domain.Model.Detector;

namespace SightKeeper.Data.Configuration;

public sealed class DetectorAssetConfiguration : IEntityTypeConfiguration<DetectorAsset>
{
    public void Configure(EntityTypeBuilder<DetectorAsset> builder)
    {
        builder.ToTable("DetectorAssets");
    }
}