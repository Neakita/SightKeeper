using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SightKeeper.Domain.Model.DataSets;

namespace SightKeeper.Data.Configuration;

internal sealed class WeightsDataConfiguration : IEntityTypeConfiguration<WeightsData>
{
	public void Configure(EntityTypeBuilder<WeightsData> builder)
	{
		builder.HasFlakeIdKey();
	}
}