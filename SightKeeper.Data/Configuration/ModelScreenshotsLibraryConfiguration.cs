using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SightKeeper.Domain.Model;

namespace SightKeeper.Data.Configuration;

public sealed class ModelScreenshotsLibraryConfiguration : IEntityTypeConfiguration<ModelScreenshotsLibrary>
{
    public void Configure(EntityTypeBuilder<ModelScreenshotsLibrary> builder)
    {
        builder.ToTable("ModelScreenshotsLibraries");
    }
}