using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SightKeeper.Data.Configuration;

internal sealed class WeightsDataConfiguration : IEntityTypeConfiguration<DbWeightsData>
{
	public void Configure(EntityTypeBuilder<DbWeightsData> builder)
	{
		builder.HasFlakeIdKey();
		builder.ToTable("WeightsData");
		builder.ComplexProperty(weightsData => weightsData.Data, subBuilder => subBuilder.Property(weights => weights.Content).HasColumnName("Content"));
		builder.HasDiscriminator<byte>("Type").HasValue<ONNXWeightsData>(0).HasValue<PTWeightsData>(1);
		builder.HasIndex("Type", "WeightsId").IsUnique();
	}
}