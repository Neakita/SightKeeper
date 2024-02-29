using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSet;

namespace SightKeeper.Data.Configuration;

public sealed class DataSetConfiguration : IEntityTypeConfiguration<DataSet>
{
    public void Configure(EntityTypeBuilder<DataSet> builder)
    {
        builder.HasKey(dataSet => dataSet.Id);
        builder.HasFlakeId(dataSet => dataSet.Id);
        builder.HasIndex(dataSet => dataSet.Name).IsUnique();
        builder.HasOne(dataSet => dataSet.ScreenshotsLibrary).WithOne(library => library.DataSet).HasPrincipalKey<DataSet>();
        builder.HasOne(dataSet => dataSet.WeightsLibrary).WithOne(library => library.DataSet).HasPrincipalKey<DataSet>();
        builder.Navigation(dataSet => dataSet.ScreenshotsLibrary).AutoInclude();
        builder.Navigation(dataSet => dataSet.WeightsLibrary).AutoInclude();
        builder.Navigation(dataSet => dataSet.Game).AutoInclude();
        builder.Navigation(dataSet => dataSet.ScreenshotsLibrary).AutoInclude();
    }
}