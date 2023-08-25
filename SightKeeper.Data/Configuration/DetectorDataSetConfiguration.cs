using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.Detector;

namespace SightKeeper.Data.Configuration;

public sealed class DetectorDataSetConfiguration : IEntityTypeConfiguration<DataSet<DetectorAsset>>
{
    public void Configure(EntityTypeBuilder<DataSet<DetectorAsset>> builder)
    {
        builder.Navigation(dataSet => dataSet.WeightsLibrary).AutoInclude();
        builder.HasMany(dataSet => dataSet.Assets).WithOne().IsRequired().HasForeignKey("DataSetId");
    }
}