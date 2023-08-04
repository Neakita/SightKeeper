using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SightKeeper.Domain.Model;

namespace SightKeeper.Data.Configuration;

public sealed class ModelWeightsConfiguration : IEntityTypeConfiguration<ModelWeights>
{
    public void Configure(EntityTypeBuilder<ModelWeights> builder)
    {
        builder.HasShadowKey();
        builder.HasMany(weights => weights.Assets)
            .WithMany()
            .UsingEntity(join =>
            {
                join.ToTable("WeightsAssets");
                join.Property("AssetsId").HasColumnName("AssetId");
                join.Property("ModelWeightsId").HasColumnName("WeightsId");
            });
    }
}