using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SightKeeper.Domain.Model.DataSets;

namespace SightKeeper.Data.Database.Configuration;

public sealed class AssetsLibraryConfiguration : IEntityTypeConfiguration<AssetsLibrary>
{
	public void Configure(EntityTypeBuilder<AssetsLibrary> builder)
	{
		builder.HasFlakeIdKey();
		builder.ToTable("AssetsLibraries");
	}
}