using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Model.Detector;
using SightKeeper.Tests.Common;

namespace SightKeeper.Data.Tests;

public sealed class ItemClassesTests : DbRelatedTests
{
	[Fact]
	public void ShouldNotDeleteItemClassesOnItemDelete()
	{
		using var dbContext = DbContextFactory.CreateDbContext();
		DetectorModel model = new("Test model");
		var itemClass = model.CreateItemClass("Test item class");
		Screenshot screenshot = new(new Image(Array.Empty<byte>()));
		model.ScreenshotsLibrary.AddScreenshot(screenshot);
		var asset = model.MakeAssetFromScreenshot(screenshot);
		var item = asset.CreateItem(itemClass, new BoundingBox());
		dbContext.DetectorModels.Add(model);
		dbContext.SaveChanges();

		dbContext.Set<ItemClass>().Should().Contain(itemClass);
		dbContext.Set<DetectorItem>().Should().Contain(item);

		dbContext.Set<DetectorItem>().Remove(item);
		dbContext.SaveChanges();

		dbContext.Set<ItemClass>().Should().Contain(itemClass);
		dbContext.Set<DetectorItem>().Should().BeEmpty();
	}
}
