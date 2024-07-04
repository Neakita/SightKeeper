using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SightKeeper.Domain.Model.DataSets.Classifier;

namespace SightKeeper.Data.Database.Configuration.Classifier;

public sealed class ClassifierAssetConfiguration : IEntityTypeConfiguration<ClassifierAsset>
{
	public void Configure(EntityTypeBuilder<ClassifierAsset> builder)
	{
		builder.ToTable("ClassifierAssets");
	}
}