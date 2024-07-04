using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SightKeeper.Domain.Model.DataSets.Classifier;

namespace SightKeeper.Data.Database.Configuration.Classifier;

public sealed class ClassifierWeightsConfiguration : IEntityTypeConfiguration<ClassifierWeights>
{
	public void Configure(EntityTypeBuilder<ClassifierWeights> builder)
	{
		builder.ToTable("ClassifierWeights");
	}
}