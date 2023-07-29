using Microsoft.EntityFrameworkCore;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Model.Detector;
using SightKeeper.Tests.Common;

namespace SightKeeper.Data.Tests;

public sealed class DetectorModelTests : DbRelatedTests
{
	private static DetectorModel TestDetectorModel => new("TestDetectorModel");

	[Fact]
	public void ShouldAddDetectorModel()
	{
		// arrange
		using var dbContext = DbContextFactory.CreateDbContext();
		var testModel = TestDetectorModel;

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
		var model = TestDetectorModel;
		var screenshot = model.ScreenshotsLibrary.CreateScreenshot(Array.Empty<byte>());
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
		DetectorModel model = new("Test model");
		var screenshot = model.ScreenshotsLibrary.CreateScreenshot(Array.Empty<byte>());
		var asset = model.MakeAsset(screenshot);
		var itemClass = model.CreateItemClass("Test item class");
		asset.CreateItem(itemClass, new BoundingBox(0, 0, 1, 1));
		dbContext.DetectorModels.Add(model);
		dbContext.SaveChanges();
		dbContext.DetectorModels.Remove(model);
		dbContext.SaveChanges();
		dbContext.DetectorModels.Should().BeEmpty();
		dbContext.Set<ScreenshotsLibrary>().Should().BeEmpty();
		dbContext.Set<DetectorAsset>().Should().BeEmpty();
		dbContext.Set<Screenshot>().Should().BeEmpty(); // test fails because screenshot is principal entity, and so, it is not cascade deleting when asset got deleted
		dbContext.Set<ItemClass>().Should().BeEmpty();
		dbContext.Set<DetectorItem>().Should().BeEmpty();
	}

	[Fact]
	public void ShouldNotDeleteConfigOnModelDelete()
	{
		using var dbContext = DbContextFactory.CreateDbContext();
		DetectorModel model = new("Test model");
		ModelConfig config = new("Test config", "Test content", ModelType.Detector);
		model.Config = config;
		dbContext.Add(model);
		dbContext.SaveChanges();
		dbContext.ModelConfigs.Should().Contain(config);
		dbContext.Remove(model);
		dbContext.SaveChanges();
		dbContext.ModelConfigs.Should().Contain(config);
	}

	[Fact]
	public void ShouldSetConfigToNullOnConfigDelete()
	{
		using var dbContext = DbContextFactory.CreateDbContext();
		DetectorModel model = new("Test model");
		ModelConfig config = new("Test config", "Test content", ModelType.Detector);
		model.Config = config;
		dbContext.Add(model);
		dbContext.SaveChanges();
		dbContext.ModelConfigs.Should().Contain(config);
		dbContext.Remove(config);
		dbContext.SaveChanges();
		model.Should().NotBeNull();
		model.Config.Should().BeNull();
	}

	[Fact]
	public void ScreenshotShouldBeRepresentedInLibraryWhenItIsAsset()
	{
		using (var dbContext = DbContextFactory.CreateDbContext())
		{
			DetectorModel model = new("Test model");
			var screenshot = model.ScreenshotsLibrary.CreateScreenshot(Array.Empty<byte>());
			model.MakeAsset(screenshot);
			dbContext.Add(model);
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
		var model = TestDetectorModel;
		Game game = new("Test game", "game.exe");
		model.Game = game;
		using var dbContext = DbContextFactory.CreateDbContext();
		dbContext.Add(model);
		dbContext.SaveChanges();
		dbContext.Remove(game);
		dbContext.SaveChanges();
	}
}