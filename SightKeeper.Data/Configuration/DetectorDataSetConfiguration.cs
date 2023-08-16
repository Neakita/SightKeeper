using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SightKeeper.Domain.Model.Detector;

namespace SightKeeper.Data.Configuration;

public sealed class DetectorDataSetConfiguration : IEntityTypeConfiguration<DetectorDataSet>
{
    public void Configure(EntityTypeBuilder<DetectorDataSet> builder)
    {
        builder.ToTable("DetectorModels");
        builder.HasMany(model => model.Assets).WithOne("Model").IsRequired();
    }
}