using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SightKeeper.Domain.Model;

namespace SightKeeper.Data.Configuration;

public sealed class ScreenshotsLibraryConfiguration : IEntityTypeConfiguration<ScreenshotsLibrary>
{
    public void Configure(EntityTypeBuilder<ScreenshotsLibrary> builder)
    {
        builder.ToTable("ScreenshotsLibraries").HasShadowKey();
        builder.HasMany(screenshotsLibrary => screenshotsLibrary.Screenshots).WithOne(screenshot => screenshot.Library).IsRequired();
    }
}