using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SightKeeper.Domain.Model.DataSets;

namespace SightKeeper.Data.Database.Configuration;

public sealed class TagsLibraryConfiguration : IEntityTypeConfiguration<TagsLibrary>
{
	public void Configure(EntityTypeBuilder<TagsLibrary> builder)
	{
		builder.HasFlakeIdKey();
		builder.ToTable("TagsLibraries");
	}
}