using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SightKeeper.Domain.Model;

namespace SightKeeper.Data.Configuration;

public sealed class ScreenshotConfiguration : IEntityTypeConfiguration<Screenshot>
{
    public void Configure(EntityTypeBuilder<Screenshot> builder)
    {
        builder.ToTable("Screenshots");
        builder.HasShadowKey();
        builder.HasOne(screenshot => screenshot.Asset).WithOne(asset => asset.Screenshot).HasPrincipalKey<Screenshot>();
        builder.HasOne(screenshot => screenshot.Image).WithOne().HasPrincipalKey<Screenshot>().IsRequired();
    }
}