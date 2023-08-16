using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SightKeeper.Domain.Model;

namespace SightKeeper.Data.Configuration;

public sealed class ModelConfigConfiguration : IEntityTypeConfiguration<ModelConfig>
{
    public void Configure(EntityTypeBuilder<ModelConfig> builder)
    {
        builder.HasShadowKey();
    }
}