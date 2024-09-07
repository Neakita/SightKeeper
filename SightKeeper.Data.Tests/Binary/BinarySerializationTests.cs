using FluentAssertions;
using MemoryPack;
using SightKeeper.Application;
using SightKeeper.Data.Binary;
using SightKeeper.Data.Binary.Services;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets.Assets;
using SightKeeper.Domain.Model.DataSets.Classifier;
using SightKeeper.Domain.Model.DataSets.Detector;
using SightKeeper.Domain.Model.DataSets.Poser2D;

namespace SightKeeper.Data.Tests.Binary;

public sealed class BinarySerializationTests
{
	private static readonly byte[] SampleImageData = File.ReadAllBytes("sample.png");
	private static readonly Vector2<ushort> SampleImageResolution = new(320, 320);

	[Fact]
	public void ShouldSaveAndLoadAppData()
	{
		AppDataAccess dataAccess = new();
		FileSystemScreenshotsDataAccess screenshotsDataAccess = new();
		MemoryPackFormatterProvider.Register(new AppDataFormatter(screenshotsDataAccess));
		Game game = new("PayDay 2", "payday2");
		dataAccess.Data.Games.Add(game);
		dataAccess.Data.DataSets.Add(CreateClassifierDataSet(screenshotsDataAccess, game));
		dataAccess.Data.DataSets.Add(CreateDetectorDataSet(screenshotsDataAccess, game));
		dataAccess.Data.DataSets.Add(CreatePoser2DDataSet(screenshotsDataAccess, game));
		dataAccess.Save();
		var data = dataAccess.Data;
		dataAccess.Load();
		dataAccess.Data.Should().BeEquivalentTo(data, options => options.IgnoringCyclicReferences());
		Directory.Delete(screenshotsDataAccess.DirectoryPath, true);
	}

	private static ClassifierDataSet CreateClassifierDataSet(ScreenshotsDataAccess screenshotsDataAccess, Game game)
	{
		ClassifierDataSet dataSet = new()
		{
			Name = "PD2",
			Description = "Test dataset",
			Game = game
		};
		dataSet.TagsLibrary.CreateTag("Don't Shoot");
		var shootTag = dataSet.TagsLibrary.CreateTag("shoot");
		dataSet.ScreenshotsLibrary.MaxQuantity = 1;
		var screenshot = screenshotsDataAccess.CreateScreenshot(dataSet.ScreenshotsLibrary, SampleImageData, DateTime.Now, SampleImageResolution);
		var asset = dataSet.AssetsLibrary.MakeAsset(screenshot);
		asset.Tag = shootTag;
		screenshotsDataAccess.CreateScreenshot(dataSet.ScreenshotsLibrary, SampleImageData, DateTime.Now, SampleImageResolution);
		return dataSet;
	}

	private static DetectorDataSet CreateDetectorDataSet(ScreenshotsDataAccess screenshotsDataAccess, Game game)
	{
		DetectorDataSet dataSet = new()
		{
			Name = "PD2",
			Description = "Test dataset",
			Game = game
		};
		var copTag = dataSet.TagsLibrary.CreateTag("Cop");
		copTag.Color = 123;
		var bulldozerTag = dataSet.TagsLibrary.CreateTag("Bulldozer");
		bulldozerTag.Color = 456;
		dataSet.ScreenshotsLibrary.MaxQuantity = 1;
		var screenshot = screenshotsDataAccess.CreateScreenshot(dataSet.ScreenshotsLibrary, SampleImageData, DateTime.Now, SampleImageResolution);
		var asset = dataSet.AssetsLibrary.MakeAsset(screenshot);
		asset.CreateItem(copTag, new Bounding(0.1, 0.15, 0.5, 0.8));
		asset.CreateItem(bulldozerTag, new Bounding(0.2, 0.2, 0.6, 0.9));
		screenshotsDataAccess.CreateScreenshot(dataSet.ScreenshotsLibrary, SampleImageData, DateTime.Now, SampleImageResolution);
		return dataSet;
	}

	private static Poser2DDataSet CreatePoser2DDataSet(ScreenshotsDataAccess screenshotsDataAccess, Game game)
	{
		Poser2DDataSet dataSet = new()
		{
			Name = "PD2",
			Description = "Test dataset",
			Game = game
		};
		var copTag = dataSet.TagsLibrary.CreateTag("Cop");
		copTag.Color = 123;
		copTag.CreateKeyPoint("Head");
		copTag.CreateProperty("Distance", 0, 200);
		var bulldozerTag = dataSet.TagsLibrary.CreateTag("Bulldozer");
		bulldozerTag.Color = 456;
		bulldozerTag.CreateKeyPoint("Face");
		bulldozerTag.CreateProperty("Distance", 0, 200);
		dataSet.ScreenshotsLibrary.MaxQuantity = 1;
		var screenshot = screenshotsDataAccess.CreateScreenshot(dataSet.ScreenshotsLibrary, SampleImageData, DateTime.Now, SampleImageResolution);
		var asset = dataSet.AssetsLibrary.MakeAsset(screenshot);
		asset.CreateItem(copTag, new Bounding(0.1, 0.15, 0.5, 0.8), [new Vector2<double>(0.3, 0.2)], [20]);
		asset.CreateItem(bulldozerTag, new Bounding(0.2, 0.2, 0.6, 0.9), [new Vector2<double>(0.4, 0.3)], [25]);
		screenshotsDataAccess.CreateScreenshot(dataSet.ScreenshotsLibrary, SampleImageData, DateTime.Now, SampleImageResolution);
		return dataSet;
	}
}