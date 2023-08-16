using Microsoft.EntityFrameworkCore;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Model.Detector;
using SightKeeper.Tests.Common;

namespace SightKeeper.Data.Tests;

public sealed class DetectorModelTests : DbRelatedTests
{
	private static DetectorDataSet TestDetectorDataSet => new("TestDetectorModel");

	[Fact]
	public void ShouldAddDetectorModel()
	{
		// arrange
		using var dbContext = DbContextFactory.CreateDbContext();
		var testModel = TestDetectorDataSet;

		// act
		dbContext.Add(testModel);
		dbContext.SaveChanges();

		// assert
		dbContext.DetectorModels.Should().Contain(testModel);
	}

	[Fact]
	public void AddingModelWithAssetShouldAddImageScreenshotAndAsset()
	{
		using var dbContext = DbContextFactory.CreateDbContext();
		var model = TestDetectorDataSet;
		var screenshot = model.ScreenshotsLibrary.CreateScreenshot(Array.Empty<byte>(), new Resolution());
		var asset = model.MakeAsset(screenshot);
		var itemClass = model.CreateItemClass("Test item class");
		var item = asset.CreateItem(itemClass, new BoundingBox());
		
		dbContext.DetectorModels.Add(model);
		dbContext.SaveChanges();

		dbContext.DetectorModels.Should().Contain(model);
		dbContext.Set<Screenshot>().Should().Contain(screenshot);
		dbContext.Set<DetectorAsset>().Should().Contain(asset);
		dbContext.Set<DetectorItem>().Should().Contain(item);
		dbContext.Set<ItemClass>().Should().Contain(itemClass);
	}

	[Fact]
	public void ShouldCascadeDelete()
	{
		using var dbContext = DbContextFactory.CreateDbContext();
		DetectorDataSet dataSet = new("Test model");
		var screenshot = dataSet.ScreenshotsLibrary.CreateScreenshot(Array.Empty<byte>(), new Resolution());
		var asset = dataSet.MakeAsset(screenshot);
		var itemClass = dataSet.CreateItemClass("Test item class");
		asset.CreateItem(itemClass, new BoundingBox(0, 0, 1, 1));
		dbContext.DetectorModels.Add(dataSet);
		dbContext.SaveChanges();
		dbContext.DetectorModels.Remove(dataSet);
		dbContext.SaveChanges();
		dbContext.DetectorModels.Should().BeEmpty();
		dbContext.Set<ScreenshotsLibrary>().Should().BeEmpty();
		dbContext.Set<DetectorAsset>().Should().BeEmpty();
		dbContext.Set<Screenshot>().Should().BeEmpty();
		dbContext.Set<ItemClass>().Should().BeEmpty();
		dbContext.Set<DetectorItem>().Should().BeEmpty();
	}

	[Fact]
	public void ScreenshotShouldBeRepresentedInLibraryWhenItIsAsset()
	{
		using (var dbContext = DbContextFactory.CreateDbContext())
		{
			DetectorDataSet dataSet = new("Test model");
			var screenshot = dataSet.ScreenshotsLibrary.CreateScreenshot(Array.Empty<byte>(), new Resolution());
			dataSet.MakeAsset(screenshot);
			dbContext.Add(dataSet);
			dbContext.SaveChanges();
		}
		using (var dbContext = DbContextFactory.CreateDbContext())
		{
			var model = dbContext.DetectorModels
				.Include(m => m.Assets)
				.Include(m => m.ScreenshotsLibrary.Screenshots)
				.Single();
			model.Assets.Should().NotBeEmpty();
			model.ScreenshotsLibrary.Screenshots.Should().NotBeEmpty();
		}
	}

	[Fact]
	public void ShouldSetGameToNullWhenDeletingGame()
	{
		var model = TestDetectorDataSet;
		Game game = new("Test game", "game.exe");
		model.Game = game;
		using var dbContext = DbContextFactory.CreateDbContext();
		dbContext.Add(model);
		dbContext.SaveChanges();
		dbContext.Remove(game);
		dbContext.SaveChanges();
	}
}