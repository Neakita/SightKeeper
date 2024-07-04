using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SightKeeper.Domain.Model.DataSets;

namespace SightKeeper.Data.Database.Configuration;

public sealed class WeightsConfiguration : IEntityTypeConfiguration<Weights>
{
	public void Configure(EntityTypeBuilder<Weights> builder)
	{
		builder.HasFlakeIdKey();
		builder.ToTable("Weights");
	}
}