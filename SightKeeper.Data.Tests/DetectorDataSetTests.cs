using Microsoft.EntityFrameworkCore;
using SightKeeper.Domain.Model;
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
		var screenshot = dataSet.Screenshots.CreateScreenshot(Array.Empty<byte>());
		var asset = dataSet.MakeAsset(screenshot);
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
		var screenshot = dataSet.Screenshots.CreateScreenshot(Array.Empty<byte>());
		var asset = dataSet.MakeAsset(screenshot);
		var itemClass = dataSet.CreateItemClass("Test item class", 0);
		asset.CreateItem(itemClass, new Bounding(0, 0, 1, 1));
		dbContext.DataSets.Add(dataSet);
		dbContext.SaveChanges();
		dbContext.DataSets.Remove(dataSet);
		dbContext.SaveChanges();
		dbContext.DataSets.Should().BeEmpty();
		dbContext.Set<Library>().Should().BeEmpty();
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
			var screenshot = dataSet.Screenshots.CreateScreenshot(Array.Empty<byte>());
			dataSet.MakeAsset(screenshot);
			dbContext.Add(dataSet);
			dbContext.SaveChanges();
		}
		using (var dbContext = DbContextFactory.CreateDbContext())
		{
			var dataSet = dbContext.Set<DataSet>()
				.Include(m => m.Assets)
				.Include(m => m.Screenshots.Screenshots)
				.Single();
			dataSet.Assets.Should().NotBeEmpty();
			dataSet.Screenshots.Screenshots.Should().NotBeEmpty();
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