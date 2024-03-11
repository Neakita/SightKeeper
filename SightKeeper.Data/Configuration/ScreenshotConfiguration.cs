using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SightKeeper.Domain.Model.DataSets;

namespace SightKeeper.Data.Configuration;

public sealed class ScreenshotConfiguration : IEntityTypeConfiguration<Screenshot>
{
    public void Configure(EntityTypeBuilder<Screenshot> builder)
    {
        builder.HasFlakeIdKey();
        builder.ToTable("Screenshots");
        builder.HasOne(screenshot => screenshot.Asset).WithOne(asset => asset.Screenshot).HasPrincipalKey<Screenshot>();
        builder.HasOne<Image>().WithOne().HasPrincipalKey<Screenshot>().IsRequired();
        builder.Navigation(screenshot => screenshot.Asset).AutoInclude();
    }
}