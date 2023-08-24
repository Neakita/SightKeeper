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
		var dataSet = DomainTestsHelper.NewDetectorDataSet;
		var itemClass = dataSet.CreateItemClass("Test item class");
		var screenshot = dataSet.ScreenshotsLibrary.CreateScreenshot(Array.Empty<byte>(), new Resolution());
		var asset = dataSet.MakeAsset(screenshot);
		var item = asset.CreateItem(itemClass, new Bounding());
		dbContext.DataSets.Add(dataSet);
		dbContext.SaveChanges();

		dbContext.Set<ItemClass>().Should().Contain(itemClass);
		dbContext.Set<DetectorItem>().Should().Contain(item);

		dbContext.Set<DetectorItem>().Remove(item);
		dbContext.SaveChanges();

		dbContext.Set<ItemClass>().Should().Contain(itemClass);
		dbContext.Set<DetectorItem>().Should().BeEmpty();
	}
}
