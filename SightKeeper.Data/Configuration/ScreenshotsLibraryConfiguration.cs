using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SightKeeper.Domain.Model;

namespace SightKeeper.Data.Configuration;

public sealed class ScreenshotsLibraryConfiguration : IEntityTypeConfiguration<ScreenshotsLibrary>
{
    public void Configure(EntityTypeBuilder<ScreenshotsLibrary> builder)
    {
        builder.HasKey(library => library.Id);
        builder.HasFlakeId(library => library.Id);
        builder.ToTable("ScreenshotsLibraries");
    }
}