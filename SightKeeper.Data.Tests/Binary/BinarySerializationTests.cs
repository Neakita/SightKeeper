using FluentAssertions;
using MemoryPack;
using SightKeeper.Data.Binary;
using SightKeeper.Data.Binary.Formatters;
using SightKeeper.Data.Binary.Services;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets.Detector;
using SightKeeper.Domain.Model.DataSets.Screenshots;
using SightKeeper.Domain.Services;

namespace SightKeeper.Data.Tests.Binary;

public sealed class BinarySerializationTests
{
	private static readonly byte[] SampleImageData = File.ReadAllBytes("sample.png");

	[Fact]
	public void ShouldSaveAndLoadAppData()
	{
		AppDataAccess dataAccess = new();
		FileSystemScreenshotsDataAccess screenshotsDataAccess = new();
		FileSystemWeightsDataAccess weightsDataAccess = new();
		MemoryPackFormatterProvider.Register(new AppDataFormatter(screenshotsDataAccess, weightsDataAccess));
		Game game = new("PayDay 2", "payday2");
		dataAccess.Data.Games.Add(game);
		var dataSet = CreateDetectorDataSet(screenshotsDataAccess, game);
		dataAccess.Data.DataSets.Add(dataSet);
		dataAccess.Save();
		var data = dataAccess.Data;
		dataAccess.Load();
		dataAccess.Data.Should().NotBeSameAs(data);
		dataAccess.Data.DataSets.Should().NotBeEmpty();
		screenshotsDataAccess.DeleteScreenshot(dataSet.Screenshots.As<IEnumerable<Screenshot>>().First());
		screenshotsDataAccess.DeleteScreenshot(dataSet.Screenshots.As<IEnumerable<Screenshot>>().First());
	}

	private DetectorDataSet CreateDetectorDataSet(ScreenshotsDataAccess screenshotsDataAccess, Game game)
	{
		DetectorDataSet dataSet = new()
		{
			Name = "PD2",
			Description = "Test dataset",
			Game = game
		};
		var copTag = dataSet.Tags.CreateTag("Cop");
		copTag.Color = 123;
		var bulldozerTag = dataSet.Tags.CreateTag("Bulldozer");
		bulldozerTag.Color = 456;
		dataSet.Screenshots.MaxQuantity = 2;
		screenshotsDataAccess.CreateScreenshot(dataSet.Screenshots, SampleImageData);
		screenshotsDataAccess.CreateScreenshot(dataSet.Screenshots, SampleImageData);
		return dataSet;
	}
}