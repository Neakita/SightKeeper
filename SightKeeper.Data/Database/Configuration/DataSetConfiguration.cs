using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SightKeeper.Domain.Model.DataSets;

namespace SightKeeper.Data.Database.Configuration;

public sealed class DataSetConfiguration : IEntityTypeConfiguration<DataSet>
{
	public void Configure(EntityTypeBuilder<DataSet> builder)
	{
		builder.HasFlakeIdKey();
		builder.Ignore(dataSet => dataSet.Tags);
		builder.Ignore(dataSet => dataSet.Screenshots);
		builder.Ignore(dataSet => dataSet.Assets);
	}
}