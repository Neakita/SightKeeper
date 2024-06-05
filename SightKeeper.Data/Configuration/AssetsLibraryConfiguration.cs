using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SightKeeper.Domain.Model.DataSets;

namespace SightKeeper.Data.Configuration;

internal sealed class AssetsLibraryConfiguration : IEntityTypeConfiguration<AssetsLibrary>
{
	public void Configure(EntityTypeBuilder<AssetsLibrary> builder)
	{
		builder.HasFlakeIdKey();
		builder.ToTable("AssetsLibraries");
		builder.HasMany<Asset>("_assets").WithOne();
	}
}