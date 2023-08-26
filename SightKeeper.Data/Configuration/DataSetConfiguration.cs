using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SightKeeper.Domain.Model;

namespace SightKeeper.Data.Configuration;

public sealed class DataSetConfiguration : IEntityTypeConfiguration<DataSet>
{
    public void Configure(EntityTypeBuilder<DataSet> builder)
    {
        builder.HasShadowKey();
        builder.OwnsOne(dataSet => dataSet.Resolution);
        builder.HasIndex(dataSet => dataSet.Name).IsUnique();
        builder.HasOne(dataSet => dataSet.ScreenshotsLibrary).WithOne(library => library.DataSet).HasPrincipalKey<DataSet>();
        builder.HasOne(dataSet => dataSet.WeightsLibrary).WithOne(library => library.DataSet).HasPrincipalKey<DataSet>();
        builder.Navigation(dataSet => dataSet.ScreenshotsLibrary).AutoInclude();
        builder.Navigation(dataSet => dataSet.WeightsLibrary).AutoInclude();
    }
}