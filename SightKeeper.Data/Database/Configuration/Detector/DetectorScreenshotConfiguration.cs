using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SightKeeper.Domain.Model.DataSets.Detector;

namespace SightKeeper.Data.Database.Configuration.Detector;

public sealed class DetectorScreenshotConfiguration : IEntityTypeConfiguration<DetectorScreenshot>
{
	public void Configure(EntityTypeBuilder<DetectorScreenshot> builder)
	{
		builder.ToTable("DetectorScreenshots");
	}
}