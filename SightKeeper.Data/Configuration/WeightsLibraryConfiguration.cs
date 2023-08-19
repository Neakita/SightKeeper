using Microsoft.EntityFrameworkCore;

namespace SightKeeper.Data.Configuration;

public sealed class WeightsLibraryConfiguration : IEntityTypeConfiguration<WeightsLibrary>
{
    public void Configure(EntityTypeBuilder<WeightsLibrary> builder)
    {
        builder.ToTable("ModelWeightsLibraries");
        builder.HasShadowKey();
        builder.HasMany(library => library.Weights).WithOne(weights => weights.Library).IsRequired();
    }
}