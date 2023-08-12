using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SightKeeper.Domain.Model;

namespace SightKeeper.Data.Configuration;

public sealed class InternalTrainedModelWeightsConfiguration : IEntityTypeConfiguration<InternalTrainedModelWeights>
{
    public void Configure(EntityTypeBuilder<InternalTrainedModelWeights> builder)
    {
        builder.HasMany(weights => weights.Assets)
            .WithMany();
    }
}