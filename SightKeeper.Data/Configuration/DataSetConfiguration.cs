using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SightKeeper.Domain.Model.DataSets.Detector;

namespace SightKeeper.Data.Configuration;

public sealed class DataSetConfiguration : IEntityTypeConfiguration<DetectorDataSet>
{
    public void Configure(EntityTypeBuilder<DetectorDataSet> builder)
    {
        builder.HasFlakeIdKey();
        builder.Property(dataSet => dataSet.Resolution);
        builder.HasMany(dataSet => dataSet.ItemClasses).WithOne();
        builder.HasOne(dataSet => dataSet.Screenshots).WithOne().HasPrincipalKey<DetectorDataSet>();
        builder.HasOne(dataSet => dataSet.DetectorAssets).WithOne().HasPrincipalKey<DetectorDataSet>();
        builder.HasOne(dataSet => dataSet.Weights).WithOne().HasPrincipalKey<DetectorDataSet>();
        builder.Navigation(dataSet => dataSet.ItemClasses).AutoInclude();
        builder.Navigation(dataSet => dataSet.Game).AutoInclude();
        builder.Navigation(dataSet => dataSet.Screenshots).AutoInclude();
        builder.Navigation(dataSet => dataSet.DetectorAssets).AutoInclude();
        builder.Navigation(dataSet => dataSet.Weights).AutoInclude();
    }
}