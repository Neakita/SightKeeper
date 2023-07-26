using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SightKeeper.Domain.Model;

namespace SightKeeper.Data.Configuration;

public sealed class ModelConfiguration : IEntityTypeConfiguration<Model>
{
    public void Configure(EntityTypeBuilder<Model> builder)
    {
        builder.HasShadowKey();
        builder.OwnsOne(model => model.Resolution);
        builder.HasMany(model => model.ItemClasses).WithOne().IsRequired();
        builder.HasOne(model => model.ScreenshotsLibrary).WithOne(library => library.Model).HasPrincipalKey<Model>().IsRequired();
        builder.HasIndex(model => model.Name).IsUnique();
    }
}