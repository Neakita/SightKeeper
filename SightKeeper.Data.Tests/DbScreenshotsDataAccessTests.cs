using SightKeeper.Data.Services;
using SightKeeper.Tests.Common;

namespace SightKeeper.Data.Tests;

public sealed class DbScreenshotsDataAccessTests : DbRelatedTests
{
	[Fact]
	public void ShouldCreateAndLoadScreenshot()
	{
		var dbContext = DbContextFactory.CreateDbContext();
		DbScreenshotsDataAccess screenshotsDataAccess = new(dbContext);
		var dataSet = DomainTestsHelper.NewDataSet;
		var screenshot = screenshotsDataAccess.CreateScreenshot(dataSet.Screenshots, [0, 1]);
		dbContext.SaveChanges();
		var screenshotImageData = screenshotsDataAccess.LoadImage(screenshot);
		screenshotImageData.Content.Should().BeEquivalentTo([0, 1]);
	}

	[Fact]
	public void ShouldCreateAndLoadScreenshotWithoutSaving()
	{
		var dbContext = DbContextFactory.CreateDbContext();
		DbScreenshotsDataAccess screenshotsDataAccess = new(dbContext);
		var dataSet = DomainTestsHelper.NewDataSet;
		var screenshot = screenshotsDataAccess.CreateScreenshot(dataSet.Screenshots, [0, 1]);
		var screenshotImageData = screenshotsDataAccess.LoadImage(screenshot);
		screenshotImageData.Content.Should().BeEquivalentTo([0, 1]);
	}

	[Fact]
	public void ShouldCreateAndLoadSeveralScreenshots()
	{
		var dbContext = DbContextFactory.CreateDbContext();
		DbScreenshotsDataAccess screenshotsDataAccess = new(dbContext);
		var dataSet = DomainTestsHelper.NewDataSet;
		screenshotsDataAccess.CreateScreenshot(dataSet.Screenshots, [0, 1]);
		screenshotsDataAccess.CreateScreenshot(dataSet.Screenshots, [2, 3]);
		dbContext.SaveChanges();
		var data = screenshotsDataAccess.LoadImages(dataSet).ToList();
		data[0].image.Content.Should().BeEquivalentTo([0, 1]);
		data[1].image.Content.Should().BeEquivalentTo([2, 3]);
		data.Count.Should().Be(2);
	}

	[Fact]
	public void ShouldCreateAndLoadSeveralPartialSavedScreenshots()
	{
		var dbContext = DbContextFactory.CreateDbContext();
		DbScreenshotsDataAccess screenshotsDataAccess = new(dbContext);
		var dataSet = DomainTestsHelper.NewDataSet;
		screenshotsDataAccess.CreateScreenshot(dataSet.Screenshots, [0, 1]);
		dbContext.SaveChanges();
		screenshotsDataAccess.CreateScreenshot(dataSet.Screenshots, [2, 3]);
		var data = screenshotsDataAccess.LoadImages(dataSet).ToList();
		data[0].image.Content.Should().BeEquivalentTo([0, 1]);
		data[1].image.Content.Should().BeEquivalentTo([2, 3]);
		data.Count.Should().Be(2);
	}

	[Fact]
	public void ShouldCreateAndLoadSeveralButNotAllScreenshots()
	{
		var dbContext = DbContextFactory.CreateDbContext();
		DbScreenshotsDataAccess screenshotsDataAccess = new(dbContext);
		var dataSet = DomainTestsHelper.NewDataSet;
		screenshotsDataAccess.CreateScreenshot(dataSet.Screenshots, [0, 1]);
		var firstScreenshotToLoad = screenshotsDataAccess.CreateScreenshot(dataSet.Screenshots, [2, 3]);
		var secondScreenshotToLoad = screenshotsDataAccess.CreateScreenshot(dataSet.Screenshots, [4, 5]);
		dbContext.SaveChanges();
		var data = screenshotsDataAccess.LoadImages([firstScreenshotToLoad, secondScreenshotToLoad]).ToList();
		data[0].image.Content.Should().BeEquivalentTo([2, 3]);
		data[1].image.Content.Should().BeEquivalentTo([4, 5]);
		data.Count.Should().Be(2);
	}

	[Fact]
	public void ShouldCreateAndLoadSeveralScreenshotsInProvidedOrder()
	{
		var dbContext = DbContextFactory.CreateDbContext();
		DbScreenshotsDataAccess screenshotsDataAccess = new(dbContext);
		var dataSet = DomainTestsHelper.NewDataSet;
		var firstScreenshot = screenshotsDataAccess.CreateScreenshot(dataSet.Screenshots, [0, 1]);
		var secondScreenshot = screenshotsDataAccess.CreateScreenshot(dataSet.Screenshots, [2, 3]);
		var thirdScreenshot = screenshotsDataAccess.CreateScreenshot(dataSet.Screenshots, [4, 5]);
		dbContext.SaveChanges();
		var data = screenshotsDataAccess.LoadImages([thirdScreenshot, firstScreenshot, secondScreenshot], true).ToList();
		data[0].image.Content.Should().BeEquivalentTo([4, 5]);
		data[1].image.Content.Should().BeEquivalentTo([0, 1]);
		data[2].image.Content.Should().BeEquivalentTo([2, 3]);
		data.Count.Should().Be(3);
	}

	[Fact]
	public void ShouldCreateAndLoadSeveralPartialSavedScreenshotsInProvidedOrder()
	{
		var dbContext = DbContextFactory.CreateDbContext();
		DbScreenshotsDataAccess screenshotsDataAccess = new(dbContext);
		var dataSet = DomainTestsHelper.NewDataSet;
		var firstScreenshot = screenshotsDataAccess.CreateScreenshot(dataSet.Screenshots, [0, 1]);
		dbContext.SaveChanges();
		var secondScreenshot = screenshotsDataAccess.CreateScreenshot(dataSet.Screenshots, [2, 3]);
		var thirdScreenshot = screenshotsDataAccess.CreateScreenshot(dataSet.Screenshots, [4, 5]);
		var data = screenshotsDataAccess.LoadImages([thirdScreenshot, firstScreenshot, secondScreenshot], true).ToList();
		data[0].image.Content.Should().BeEquivalentTo([4, 5]);
		data[1].image.Content.Should().BeEquivalentTo([0, 1]);
		data[2].image.Content.Should().BeEquivalentTo([2, 3]);
		data.Count.Should().Be(3);
	}
}