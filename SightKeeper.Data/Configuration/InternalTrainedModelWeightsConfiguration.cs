using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SightKeeper.Domain.Model;

namespace SightKeeper.Data.Configuration;

public sealed class InternalTrainedModelWeightsConfiguration : IEntityTypeConfiguration<InternalTrainedWeights>
{
    public void Configure(EntityTypeBuilder<InternalTrainedWeights> builder)
    {
        builder.HasMany(weights => weights.Assets)
            .WithMany();
    }
}