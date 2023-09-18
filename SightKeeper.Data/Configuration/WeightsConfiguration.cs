using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Data.Configuration;

public sealed class WeightsConfiguration : IEntityTypeConfiguration<Weights>
{
    public void Configure(EntityTypeBuilder<Weights> builder)
    {
        builder.HasShadowKey();
        builder
            .HasMany(weights => weights.Assets)
            .WithMany()
            .UsingEntity(
            left => left.HasOne(typeof(Asset)).WithMany().HasForeignKey("AssetId"),
            right => right.HasOne(typeof(Weights)).WithMany().HasForeignKey("WeightsId"),
            join => join.ToTable("WeightsAssets"));
        builder.Navigation(weights => weights.Library).AutoInclude();
    }
}