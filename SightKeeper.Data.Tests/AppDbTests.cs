using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using SightKeeper.Data.Database;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Model.DataSets.Classifier;
using SightKeeper.Domain.Model.DataSets.Detector;
using Xunit.Abstractions;

namespace SightKeeper.Data.Tests;

public sealed class AppDbTests
{
	private readonly ITestOutputHelper _testOutputHelper;

	public AppDbTests(ITestOutputHelper testOutputHelper)
	{
		_testOutputHelper = testOutputHelper;
	}

	[Fact]
	public void ShouldCreateDatabase()
	{
		var options = new DbContextOptionsBuilder();
		options.EnableSensitiveDataLogging();
		options.LogTo(_testOutputHelper.WriteLine);
		var dbContext = new AppDbContext(options.Options);

		DetectorDataSet detectorDataSet = new("DetectorTests", 320);
		var detectorTag = detectorDataSet.Tags.CreateTag("Test");
		DbScreenshotsDataAccess detectorScreenshotsDataAccess = new(dbContext);
		var detectorScreenshot = detectorScreenshotsDataAccess.CreateScreenshot(detectorDataSet.Screenshots, []);
		var detectorAsset = detectorDataSet.Assets.MakeAsset(detectorScreenshot);
		detectorAsset.CreateItem(detectorTag, new Bounding(0.1, 0.2, 0.3, 0.4));

		ClassifierDataSet classifierDataSet = new("ClassifierTest", 320);
		var classifierTag = classifierDataSet.Tags.CreateTag("Test");
		DbScreenshotsDataAccess classifierScreenshotsDataAccess = new(dbContext);
		var classifierScreenshot = classifierScreenshotsDataAccess.CreateScreenshot(classifierDataSet.Screenshots, []);
		classifierDataSet.Assets.MakeAsset(classifierScreenshot, classifierTag);

		dbContext.Add(detectorDataSet);
		dbContext.Add(classifierDataSet);
		Process.GetProcessesByName("DB Browser for SQLite").SingleOrDefault()?.Kill();
		dbContext.Database.EnsureDeleted();
		dbContext.Database.EnsureCreated();
		dbContext.SaveChanges();
	}
}