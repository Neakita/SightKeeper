using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.DependencyModel;

namespace SightKeeper.Data.Configuration;

public sealed class ScreenshotsLibraryConfiguration : IEntityTypeConfiguration<Library>
{
    public void Configure(EntityTypeBuilder<Library> builder)
    {
        builder.HasFlakeIdKey();
        builder.ToTable("ScreenshotsLibraries");
    }
}