using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SightKeeper.Domain.Model;

namespace SightKeeper.Data.Configuration;

public sealed class ScreenshotImageConfiguration : IEntityTypeConfiguration<ScreenshotImage>
{
    public void Configure(EntityTypeBuilder<ScreenshotImage> builder)
    {
        builder.ToTable("ScreenshotImages");
    }
}