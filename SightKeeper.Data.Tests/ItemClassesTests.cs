using Microsoft.EntityFrameworkCore;
using SightKeeper.Domain.Model;
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
		DataSet<DetectorAsset> dataSet = new("Test model");
		var itemClass = dataSet.CreateItemClass("Test item class");
		var screenshot = dataSet.ScreenshotsLibrary.CreateScreenshot(Array.Empty<byte>(), new Resolution());
		var asset = dataSet.MakeAsset(screenshot);
		var item = asset.CreateItem(itemClass, new Bounding());
		dbContext.DetectorDataSets.Add(dataSet);
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
			DataSet<DetectorAsset> dataSet = new("Test model");
			var itemClass = dataSet.CreateItemClass("Item class");
			var screenshot = dataSet.ScreenshotsLibrary.CreateScreenshot(Array.Empty<byte>(), new Resolution());
			var asset = dataSet.MakeAsset(screenshot);
			asset.CreateItem(itemClass, new Bounding());
			arrangeDbContext.Add(dataSet);
			arrangeDbContext.SaveChanges();
		}
		using (var assertDbContext = DbContextFactory.CreateDbContext())
		{
			var model = assertDbContext.DetectorDataSets.Include(model => model.ItemClasses).ThenInclude(itemClass => itemClass.DetectorItems).Single();
			model.ItemClasses.Single().DetectorItems.Should().NotBeEmpty();
		}
	}
}
