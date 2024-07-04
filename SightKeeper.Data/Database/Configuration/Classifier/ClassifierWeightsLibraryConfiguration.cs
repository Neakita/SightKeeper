using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SightKeeper.Domain.Model.DataSets.Classifier;

namespace SightKeeper.Data.Database.Configuration.Classifier;

public sealed class ClassifierWeightsLibraryConfiguration : IEntityTypeConfiguration<ClassifierWeightsLibrary>
{
	public void Configure(EntityTypeBuilder<ClassifierWeightsLibrary> builder)
	{
		builder.HasMany<ClassifierWeights>("_weights").WithOne(weights => weights.Library);
		builder.ToTable("ClassifierWeightsLibraries");
	}
}