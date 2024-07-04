using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SightKeeper.Domain.Model.DataSets.Detector;

namespace SightKeeper.Data.Database.Configuration.Detector;

public sealed class DetectorDataSetConfiguration : IEntityTypeConfiguration<DetectorDataSet>
{
	public void Configure(EntityTypeBuilder<DetectorDataSet> builder)
	{
		builder.HasOne(dataSet => dataSet.Tags)
			.WithOne(library => library.DataSet)
			.HasPrincipalKey<DetectorDataSet>();

		builder.HasOne(dataSet => dataSet.Screenshots)
			.WithOne(library => library.DataSet)
			.HasPrincipalKey<DetectorDataSet>();

		builder.HasOne(dataSet => dataSet.Assets)
			.WithOne(library => library.DataSet)
			.HasPrincipalKey<DetectorDataSet>();

		builder.HasOne(dataSet => dataSet.Weights)
			.WithOne(library => library.DataSet)
			.HasPrincipalKey<DetectorDataSet>();
	}
}