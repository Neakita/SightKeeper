using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Model.Detector;
using SightKeeper.Tests.Common;

namespace SightKeeper.Data.Tests;

public sealed class ItemClassesTests : DbRelatedTests
{
	[Fact]
	public void ShouldNotDeleteItemClassesOnItemDelete()
	{
		using AppDbContext dbContext = DbContextFactory.CreateDbContext();
		ItemClass itemClass = new("Test item class");
		DetectorItem item = new(itemClass, new BoundingBox(0, 0, 1, 1));
		DetectorModel model = new("Test model");
		DetectorScreenshot screenshot = new(model, new Image(Array.Empty<byte>()));
		screenshot.Items.Add(item);
		model.DetectorScreenshots.Add(screenshot);
		dbContext.DetectorModels.Add(model);
		dbContext.SaveChanges();

		dbContext.ItemClasses.Should().Contain(itemClass);
		dbContext.DetectorItems.Should().Contain(item);

		dbContext.DetectorItems.Remove(item);
		dbContext.SaveChanges();

		dbContext.ItemClasses.Should().Contain(itemClass);
		dbContext.DetectorItems.Should().BeEmpty();
	}
}
