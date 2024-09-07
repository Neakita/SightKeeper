using FluentAssertions;
using FluentAssertions.Equivalency;
using MemoryPack;
using SightKeeper.Application;
using SightKeeper.Data.Binary;
using SightKeeper.Data.Binary.Services;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets.Detector;
using SightKeeper.Domain.Model.DataSets.Screenshots;

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
		var testingDataSet = CreateDetectorDataSet(screenshotsDataAccess, game);
		dataAccess.Data.DataSets.Add(testingDataSet);
		dataAccess.Save();
		var data = dataAccess.Data;
		dataAccess.Load();
		dataAccess.Data.Should().BeEquivalentTo(data, ConfigureEquivalencyAssertion);
		screenshotsDataAccess.DeleteScreenshot(testingDataSet.ScreenshotsLibrary.As<IEnumerable<Screenshot>>().First());
		screenshotsDataAccess.DeleteScreenshot(testingDataSet.ScreenshotsLibrary.As<IEnumerable<Screenshot>>().First());
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
		dataSet.ScreenshotsLibrary.MaxQuantity = 2;
		screenshotsDataAccess.CreateScreenshot(dataSet.ScreenshotsLibrary, SampleImageData, DateTime.Now, SampleImageResolution);
		screenshotsDataAccess.CreateScreenshot(dataSet.ScreenshotsLibrary, SampleImageData, DateTime.Now, SampleImageResolution);
		return dataSet;
	}

	private static EquivalencyAssertionOptions<AppData> ConfigureEquivalencyAssertion(EquivalencyAssertionOptions<AppData> options)
	{
		return options.IgnoringCyclicReferences();
	}
}