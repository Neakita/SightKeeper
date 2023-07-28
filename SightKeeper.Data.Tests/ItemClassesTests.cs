using Microsoft.EntityFrameworkCore;
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
		var screenshot = model.ScreenshotsLibrary.CreateScreenshot(Array.Empty<byte>());
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

	[Fact]
	public void ShouldLoadDetectorItemsOfItemClass()
	{
		using (var arrangeDbContext = DbContextFactory.CreateDbContext())
		{
			DetectorModel model = new("Test model");
			var itemClass = model.CreateItemClass("Item class");
			var screenshot = model.ScreenshotsLibrary.CreateScreenshot(Array.Empty<byte>());
			var asset = model.MakeAssetFromScreenshot(screenshot);
			asset.CreateItem(itemClass, new BoundingBox());
			arrangeDbContext.Add(model);
			arrangeDbContext.SaveChanges();
		}
		using (var assertDbContext = DbContextFactory.CreateDbContext())
		{
			var model = assertDbContext.DetectorModels.Include(model => model.ItemClasses).ThenInclude(itemClass => itemClass.DetectorItems).Single();
			model.ItemClasses.Single().DetectorItems.Should().NotBeEmpty();
		}
	}
}
