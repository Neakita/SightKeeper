using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SightKeeper.Domain.Model.DataSets.Classifier;

namespace SightKeeper.Data.Database.Configuration.Classifier;

public sealed class ClassifierDataSetConfiguration : IEntityTypeConfiguration<ClassifierDataSet>
{
	public void Configure(EntityTypeBuilder<ClassifierDataSet> builder)
	{
		builder.HasOne(dataSet => dataSet.Tags)
			.WithOne(library => library.DataSet)
			.HasPrincipalKey<ClassifierDataSet>();

		builder.HasOne(dataSet => dataSet.Screenshots)
			.WithOne(library => library.DataSet)
			.HasPrincipalKey<ClassifierDataSet>();

		builder.HasOne(dataSet => dataSet.Assets)
			.WithOne(library => library.DataSet)
			.HasPrincipalKey<ClassifierDataSet>();

		builder.HasOne(dataSet => dataSet.Weights)
			.WithOne(library => library.DataSet)
			.HasPrincipalKey<ClassifierDataSet>();
	}
}