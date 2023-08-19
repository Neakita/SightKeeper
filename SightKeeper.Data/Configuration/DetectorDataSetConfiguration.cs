using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.Detector;

namespace SightKeeper.Data.Configuration;

public sealed class DetectorDataSetConfiguration : IEntityTypeConfiguration<DataSet<DetectorAsset>>
{
    public void Configure(EntityTypeBuilder<DataSet<DetectorAsset>> builder)
    {
        builder.HasShadowKey();
        builder.OwnsOne(model => model.Resolution);
        builder.HasIndex(model => model.Name).IsUnique();
        builder.Navigation(model => model.ScreenshotsLibrary).AutoInclude();
        builder.Navigation(model => model.WeightsLibrary).AutoInclude();
    }
}