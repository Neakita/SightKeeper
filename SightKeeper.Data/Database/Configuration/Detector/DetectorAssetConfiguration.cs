using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SightKeeper.Domain.Model.DataSets.Detector;

namespace SightKeeper.Data.Database.Configuration.Detector;

public sealed class DetectorAssetConfiguration : IEntityTypeConfiguration<DetectorAsset>
{
	public void Configure(EntityTypeBuilder<DetectorAsset> builder)
	{
		builder.HasMany(asset => asset.Items).WithOne(item => item.Asset);
		builder.ToTable("DetectorAssets");
	}
}