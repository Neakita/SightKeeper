using SightKeeper.Data.Services;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Tests.Common;

namespace SightKeeper.Data.Tests;

public sealed class ItemClassesTests : DbRelatedTests
{
	[Fact]
	public void ShouldNotDeleteItemClassesOnItemDelete()
	{
		using var dbContext = DbContextFactory.CreateDbContext();
		var dataSet = DomainTestsHelper.NewDataSet;
		var itemClass = dataSet.CreateItemClass("Test item class", 0);
		DbScreenshotsDataAccess screenshotsDataAccess = new(dbContext);
		var screenshot = screenshotsDataAccess.CreateScreenshot(dataSet.Screenshots, Array.Empty<byte>());
		var asset = dataSet.Assets.MakeAsset(screenshot);
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
