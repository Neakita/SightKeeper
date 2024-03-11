using Microsoft.EntityFrameworkCore;
using SightKeeper.Data.Services;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Tests.Common;

namespace SightKeeper.Data.Tests;

public sealed class DetectorDataSetTests : DbRelatedTests
{
	[Fact]
	public void ShouldAddDetectorDataSet()
	{
		// arrange
		using var dbContext = DbContextFactory.CreateDbContext();
		var dataSet = DomainTestsHelper.NewDataSet;

		// act
		dbContext.Add(dataSet);
		dbContext.SaveChanges();

		// assert
		dbContext.DataSets.Should().Contain(dataSet);
	}

	[Fact]
	public void AddingDataSetWithAssetShouldAddImageScreenshotAndAsset()
	{
		using var dbContext = DbContextFactory.CreateDbContext();
		var dataSet = DomainTestsHelper.NewDataSet;
		DbScreenshotsDataAccess screenshotsDataAccess = new(dbContext);
		var screenshot = screenshotsDataAccess.CreateScreenshot(dataSet.Screenshots, Array.Empty<byte>());
		var asset = dataSet.Assets.MakeAsset(screenshot);
		var itemClass = dataSet.CreateItemClass("Test item class", 0);
		var item = asset.CreateItem(itemClass, new Bounding());
		
		dbContext.DataSets.Add(dataSet);
		dbContext.SaveChanges();

		dbContext.DataSets.Should().Contain(dataSet);
		dbContext.Set<Screenshot>().Should().Contain(screenshot);
		dbContext.Set<Asset>().Should().Contain(asset);
		dbContext.Set<DetectorItem>().Should().Contain(item);
		dbContext.Set<ItemClass>().Should().Contain(itemClass);
	}

	[Fact]
	public void ShouldCascadeDelete()
	{
		using var dbContext = DbContextFactory.CreateDbContext();
		var dataSet = DomainTestsHelper.NewDataSet;
		DbScreenshotsDataAccess screenshotsDataAccess = new(dbContext);
		var screenshot = screenshotsDataAccess.CreateScreenshot(dataSet.Screenshots, Array.Empty<byte>());
		var asset = dataSet.Assets.MakeAsset(screenshot);
		var itemClass = dataSet.CreateItemClass("Test item class", 0);
		asset.CreateItem(itemClass, new Bounding(0, 0, 1, 1));
		dbContext.DataSets.Add(dataSet);
		dbContext.SaveChanges();
		dbContext.DataSets.Remove(dataSet);
		dbContext.SaveChanges();
		dbContext.DataSets.Should().BeEmpty();
		dbContext.Set<ScreenshotsLibrary>().Should().BeEmpty();
		dbContext.Set<Asset>().Should().BeEmpty();
		dbContext.Set<Screenshot>().Should().BeEmpty();
		dbContext.Set<ItemClass>().Should().BeEmpty();
		dbContext.Set<DetectorItem>().Should().BeEmpty();
	}

	[Fact]
	public void ScreenshotShouldBeRepresentedInLibraryWhenItIsAsset()
	{
		using (var dbContext = DbContextFactory.CreateDbContext())
		{
			var dataSet = DomainTestsHelper.NewDataSet;
			DbScreenshotsDataAccess screenshotsDataAccess = new(dbContext);
			var screenshot = screenshotsDataAccess.CreateScreenshot(dataSet.Screenshots, Array.Empty<byte>());
			dataSet.Assets.MakeAsset(screenshot);
			dbContext.Add(dataSet);
			dbContext.SaveChanges();
		}
		using (var dbContext = DbContextFactory.CreateDbContext())
		{
			var dataSet = dbContext.Set<DataSet>().Single();
			dataSet.Assets.Should().NotBeEmpty();
			dataSet.Screenshots.Should().NotBeEmpty();
		}
	}

	[Fact]
	public void ShouldSetGameToNullWhenDeletingGame()
	{
		var dataSet = DomainTestsHelper.NewDataSet;
		Game game = new("Test game", "game.exe");
		dataSet.Game = game;
		using var dbContext = DbContextFactory.CreateDbContext();
		dbContext.Add(dataSet);
		dbContext.SaveChanges();
		dbContext.Remove(game);
		dbContext.SaveChanges();
	}
}