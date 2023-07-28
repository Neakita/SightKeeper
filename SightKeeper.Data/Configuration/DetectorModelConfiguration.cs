using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SightKeeper.Domain.Model.Detector;

namespace SightKeeper.Data.Configuration;

public sealed class DetectorModelConfiguration : IEntityTypeConfiguration<DetectorModel>
{
    public void Configure(EntityTypeBuilder<DetectorModel> builder)
    {
        builder.ToTable("DetectorModels");
        builder.HasMany(model => model.Assets).WithOne("Model").IsRequired();
    }
}