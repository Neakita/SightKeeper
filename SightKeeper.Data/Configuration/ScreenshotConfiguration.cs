using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSet.Screenshots;

namespace SightKeeper.Data.Configuration;

public sealed class ScreenshotConfiguration : IEntityTypeConfiguration<Screenshot>
{
    public void Configure(EntityTypeBuilder<Screenshot> builder)
    {
        builder.HasKey(screenshot => screenshot.Id);
        builder.HasFlakeId(screenshot => screenshot.Id);
        builder.ToTable("Screenshots");
        builder.HasOne(screenshot => screenshot.Asset).WithOne(asset => asset.Screenshot).HasPrincipalKey<Screenshot>();
        builder.HasOne(screenshot => screenshot.Image).WithOne().HasPrincipalKey<Screenshot>().IsRequired();
        builder.Navigation(screenshot => screenshot.Asset).AutoInclude();
    }
}