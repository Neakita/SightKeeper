using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SightKeeper.Domain.Model.DataSets;

namespace SightKeeper.Data.Configuration;

public sealed class ScreenshotsLibraryConfiguration : IEntityTypeConfiguration<ScreenshotsLibrary>
{
    public void Configure(EntityTypeBuilder<ScreenshotsLibrary> builder)
    {
        builder.HasFlakeIdKey();
        builder.ToTable("ScreenshotsLibraries");
        builder.HasMany<Screenshot>("_screenshots").WithOne(screenshot => screenshot.Library);
    }
}