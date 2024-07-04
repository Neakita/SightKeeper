using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SightKeeper.Domain.Model.DataSets.Classifier;

namespace SightKeeper.Data.Database.Configuration.Classifier;

public sealed class ClassifierTagsLibraryConfiguration : IEntityTypeConfiguration<ClassifierTagsLibrary>
{
	public void Configure(EntityTypeBuilder<ClassifierTagsLibrary> builder)
	{
		builder.ToTable("ClassifierTagsLibraries");
		builder.HasMany<ClassifierTag>("_tags").WithOne(tag => tag.Library);
	}
}