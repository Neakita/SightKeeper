using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SightKeeper.Domain.Model.DataSets.Classifier;

namespace SightKeeper.Data.Database.Configuration.Classifier;

public sealed class ClassifierScreenshotConfiguration : IEntityTypeConfiguration<ClassifierScreenshot>
{
	public void Configure(EntityTypeBuilder<ClassifierScreenshot> builder)
	{
		builder.ToTable("ClassifierScreenshots");
	}
}