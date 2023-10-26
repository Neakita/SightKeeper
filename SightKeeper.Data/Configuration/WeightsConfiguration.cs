using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Data.Configuration;

public sealed class WeightsConfiguration : IEntityTypeConfiguration<Weights>
{
    public void Configure(EntityTypeBuilder<Weights> builder)
    {
        builder.HasKey(weights => weights.Id);
        builder.HasFlakeId(weights => weights.Id);
        builder
            .HasMany(weights => weights.Assets)
            .WithMany()
            .UsingEntity<WeightsAsset>(join => join.ToTable("WeightsAssets"));
        builder.Navigation(weights => weights.Library).AutoInclude();
        builder.HasOne(weights => weights.PTData).WithOne().HasPrincipalKey<Weights>().IsRequired();
        builder.HasOne(weights => weights.ONNXData).WithOne().HasPrincipalKey<Weights>().IsRequired();
    }
}