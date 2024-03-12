using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SightKeeper.Data.Configuration;

internal sealed class WeightsDataConfiguration : IEntityTypeConfiguration<DbWeightsData>
{
	public void Configure(EntityTypeBuilder<DbWeightsData> builder)
	{
		builder.HasFlakeIdKey();
		builder.ToTable("WeightsData");
		// probably builder.ComplexProperty can be used, but https://github.com/dotnet/efcore/issues/9849 or smthng (Works with SqLite, but won't with InMemory)
		builder.OwnsOne(weightsData => weightsData.Data, subBuilder => subBuilder.Property(weights => weights.Content).HasColumnName("Content"));
		builder.Property(weightsData => weightsData.Format);
		builder.HasIndex("WeightsId", "Format").IsUnique();
	}
}