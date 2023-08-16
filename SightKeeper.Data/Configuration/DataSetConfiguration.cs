using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SightKeeper.Domain.Model;

namespace SightKeeper.Data.Configuration;

public sealed class DataSetConfiguration : IEntityTypeConfiguration<DataSet>
{
    public void Configure(EntityTypeBuilder<DataSet> builder)
    {
        builder.HasShadowKey();
        builder.OwnsOne(model => model.Resolution);
        builder.HasMany(model => model.ItemClasses).WithOne(itemClass => itemClass.DataSet).IsRequired();
        builder.HasOne(model => model.ScreenshotsLibrary).WithOne(library => library.DataSet).HasPrincipalKey<DataSet>().IsRequired();
        builder.HasIndex(model => model.Name).IsUnique();
    }
}