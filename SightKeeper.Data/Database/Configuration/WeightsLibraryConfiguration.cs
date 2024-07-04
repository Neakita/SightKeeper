using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SightKeeper.Domain.Model.DataSets;

namespace SightKeeper.Data.Database.Configuration;

public sealed class WeightsLibraryConfiguration : IEntityTypeConfiguration<WeightsLibrary>
{
	public void Configure(EntityTypeBuilder<WeightsLibrary> builder)
	{
		builder.HasFlakeIdKey();
		builder.ToTable("WeightsLibraries");
	}
}