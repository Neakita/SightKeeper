using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SightKeeper.Domain.Model.DataSets.Detector;

namespace SightKeeper.Data.Database.Configuration.Detector;

public sealed class DetectorAssetsLibraryConfiguration : IEntityTypeConfiguration<DetectorAssetsLibrary>
{
	public void Configure(EntityTypeBuilder<DetectorAssetsLibrary> builder)
	{
		builder.ToTable("DetectorAssetsLibraries");
		builder.HasMany<DetectorAsset>()
			.WithOne(asset => asset.Library);
	}
}