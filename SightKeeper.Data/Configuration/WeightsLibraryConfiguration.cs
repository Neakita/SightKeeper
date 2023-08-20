using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SightKeeper.Domain.Model;

namespace SightKeeper.Data.Configuration;

public sealed class WeightsLibraryConfiguration : IEntityTypeConfiguration<WeightsLibrary>
{
    public void Configure(EntityTypeBuilder<WeightsLibrary> builder)
    {
        builder.ToTable("WeightsLibraries");
        builder.HasShadowKey();
    }
}