using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SightKeeper.Domain.Model.DataSets.Detector;

namespace SightKeeper.Data.Configuration;

internal sealed class AssetsLibraryConfiguration : IEntityTypeConfiguration<DetectorAssetsLibrary>
{
	public void Configure(EntityTypeBuilder<DetectorAssetsLibrary> builder)
	{
		builder.HasFlakeIdKey();
		builder.ToTable("AssetsLibraries");
		builder.HasMany<DetectorAsset>("_assets").WithOne();
	}
}