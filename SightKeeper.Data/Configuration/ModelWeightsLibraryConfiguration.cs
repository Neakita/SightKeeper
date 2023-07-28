using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SightKeeper.Domain.Model;

namespace SightKeeper.Data.Configuration;

public sealed class ModelWeightsLibraryConfiguration : IEntityTypeConfiguration<ModelWeightsLibrary>
{
    public void Configure(EntityTypeBuilder<ModelWeightsLibrary> builder)
    {
        builder.HasMany(library => library.Weights).WithOne("Library").IsRequired();
    }
}