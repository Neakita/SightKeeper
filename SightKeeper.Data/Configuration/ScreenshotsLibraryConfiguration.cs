using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Data.Configuration;

public sealed class ScreenshotsLibraryConfiguration : IEntityTypeConfiguration<ScreenshotsLibrary>
{
    public void Configure(EntityTypeBuilder<ScreenshotsLibrary> builder)
    {
        builder.ToTable("ScreenshotsLibraries").HasShadowKey();
    }
}