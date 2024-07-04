using System.Diagnostics;
using FluentAssertions;
using SightKeeper.Data.Database;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Model.DataSets.Detector;

namespace SightKeeper.Data.Tests.Detector;

public sealed class DetectorDataSetTests
{
	[Fact]
	public void ShouldNotCreateLeftovers()
	{
		var dbContext = new DefaultAppDbContextFactory().CreateDbContext();
		Process.GetProcessesByName("DB Browser for SQLite").SingleOrDefault()?.Kill();
		dbContext.Database.EnsureDeleted();
		dbContext.Database.EnsureCreated();
		DetectorDataSet dataSet = new("Test", 320);
		dbContext.Add(dataSet);
		var tag = dataSet.Tags.CreateTag("Test");
		DbScreenshotsDataAccess screenshotsDataAccess = new(dbContext);
		var screenshot = screenshotsDataAccess.CreateScreenshot(dataSet.Screenshots, []);
		var asset = dataSet.Assets.MakeAsset(screenshot);
		asset.CreateItem(tag, new Bounding(0.1, 0.2, 0.3, 0.4));
		dbContext.SaveChanges();
		dbContext.Remove(dataSet);
		dbContext.SaveChanges();
		dbContext.Set<Tag>().Should().BeEmpty();
		dbContext.Set<ScreenshotsLibrary>().Should().BeEmpty();
		dbContext.Set<Screenshot>().Should().BeEmpty();
		dbContext.Set<Image>().Should().BeEmpty();
		dbContext.Set<Asset>().Should().BeEmpty();
		dbContext.Set<DetectorItem>().Should().BeEmpty();
	}
}