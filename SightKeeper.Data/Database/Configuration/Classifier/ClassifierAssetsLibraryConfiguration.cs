using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SightKeeper.Domain.Model.DataSets.Classifier;

namespace SightKeeper.Data.Database.Configuration.Classifier;

public sealed class ClassifierAssetsLibraryConfiguration : IEntityTypeConfiguration<ClassifierAssetsLibrary>
{
	public void Configure(EntityTypeBuilder<ClassifierAssetsLibrary> builder)
	{
		builder.ToTable("ClassifierAssetsLibraries");
		builder.HasMany<ClassifierAsset>("_assets")
			.WithOne(asset => asset.Library);
		builder.ToTable("ClassifierAssetsLibraries");
	}
}