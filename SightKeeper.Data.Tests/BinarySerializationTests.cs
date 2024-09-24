using System.Collections.Immutable;
using FluentAssertions;
using FluentAssertions.Equivalency;
using MemoryPack;
using SightKeeper.Application;
using SightKeeper.Data.Binary;
using SightKeeper.Data.Binary.Services;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Model.DataSets.Assets;
using SightKeeper.Domain.Model.DataSets.Classifier;
using SightKeeper.Domain.Model.DataSets.Detector;
using SightKeeper.Domain.Model.DataSets.Poser2D;
using SightKeeper.Domain.Model.DataSets.Poser3D;
using SightKeeper.Domain.Model.DataSets.Weights;
using SightKeeper.Domain.Model.Profiles;
using SightKeeper.Domain.Model.Profiles.Behaviors;
using SixLabors.ImageSharp;

namespace SightKeeper.Data.Tests;

public sealed class BinarySerializationTests
{
	private static readonly Image SampleImageData = Image.Load("sample.png");
	private static readonly Vector2<ushort> SampleImageResolution = new(320, 320);

	[Fact]
	public void ShouldSaveAndLoadAppData()
	{
		AppDataAccess dataAccess = new();
		FileSystemScreenshotsDataAccess screenshotsDataAccess = new();
		MemoryPackFormatterProvider.Register(new AppDataFormatter(screenshotsDataAccess));
		Game game = new("PayDay 2", "payday2");
		dataAccess.Data.Games.Add(game);
		foreach (var dataSet in CreateDataSets(screenshotsDataAccess, game))
			dataAccess.Data.DataSets.Add(dataSet);
		var profile = CreateProfile(
			dataAccess.Data.DataSets.OfType<ClassifierDataSet>().Single().WeightsLibrary.Weights.Single(),
			dataAccess.Data.DataSets.OfType<DetectorDataSet>().Single().WeightsLibrary.Weights.Single(),
			dataAccess.Data.DataSets.OfType<Poser2DDataSet>().Single().WeightsLibrary.Weights.Single(),
			dataAccess.Data.DataSets.OfType<Poser3DDataSet>().Single().WeightsLibrary.Weights.Single());
		dataAccess.Data.Profiles.Add(profile);
		dataAccess.Save();
		var data = dataAccess.Data;
		dataAccess.Load();
		dataAccess.Data.Should().BeEquivalentTo(data, ConfigureEquivalencyAssertion);
		Directory.Delete(screenshotsDataAccess.DirectoryPath, true);
	}

	private static EquivalencyAssertionOptions<AppData> ConfigureEquivalencyAssertion(EquivalencyAssertionOptions<AppData> options)
	{
		return options
			.RespectingRuntimeTypes()
			.IgnoringCyclicReferences()
			.AllowingInfiniteRecursion();
	}

	private static IEnumerable<DataSet> CreateDataSets(ScreenshotsDataAccess screenshotsDataAccess, Game game)
	{
		yield return CreateClassifierDataSet(screenshotsDataAccess, game);
		yield return CreateDetectorDataSet(screenshotsDataAccess, game);
		yield return CreatePoser2DDataSet(screenshotsDataAccess, game);
		yield return CreatePoser3DDataSet(screenshotsDataAccess, game);
	}

	private static ClassifierDataSet CreateClassifierDataSet(ScreenshotsDataAccess screenshotsDataAccess, Game game)
	{
		ClassifierDataSet dataSet = new()
		{
			Name = "PD2Classifier",
			Description = "Test dataset",
			Game = game
		};
		dataSet.TagsLibrary.CreateTag("Don't Shoot");
		var shootTag = dataSet.TagsLibrary.CreateTag("shoot");
		dataSet.ScreenshotsLibrary.MaxQuantity = 1;
		var screenshot = screenshotsDataAccess.CreateScreenshot(dataSet.ScreenshotsLibrary, SampleImageData, DateTimeOffset.Now, SampleImageResolution);
		var asset = dataSet.AssetsLibrary.MakeAsset(screenshot);
		asset.Tag = shootTag;
		screenshotsDataAccess.CreateScreenshot(dataSet.ScreenshotsLibrary, SampleImageData, DateTimeOffset.Now, SampleImageResolution);
		dataSet.WeightsLibrary.CreateWeights(
			DateTime.Now,
			ModelSize.Nano,
			new WeightsMetrics(100, new LossMetrics(0.1f, 0.2f, 0.3f)),
			new Vector2<ushort>(320, 320),
			dataSet.TagsLibrary.Tags,
			null);
		return dataSet;
	}

	private static DetectorDataSet CreateDetectorDataSet(ScreenshotsDataAccess screenshotsDataAccess, Game game)
	{
		DetectorDataSet dataSet = new()
		{
			Name = "PD2Detector",
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
		dataSet.WeightsLibrary.CreateWeights(
			DateTime.Now,
			ModelSize.Nano,
			new WeightsMetrics(100, new LossMetrics(0.1f, 0.2f, 0.3f)),
			new Vector2<ushort>(320, 320),
			dataSet.TagsLibrary.Tags,
			null);
		return dataSet;
	}

	private static Poser2DDataSet CreatePoser2DDataSet(ScreenshotsDataAccess screenshotsDataAccess, Game game)
	{
		Poser2DDataSet dataSet = new()
		{
			Name = "PD2Poser",
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
		dataSet.WeightsLibrary.CreateWeights(
			DateTime.Now,
			ModelSize.Nano,
			new WeightsMetrics(100, new LossMetrics(0.1f, 0.2f, 0.3f)),
			new Vector2<ushort>(320, 320),
			dataSet.TagsLibrary.Tags,
			dataSet.TagsLibrary.Tags.SelectMany(tag => tag.KeyPoints),
			null);
		return dataSet;
	}

	private static Poser3DDataSet CreatePoser3DDataSet(ScreenshotsDataAccess screenshotsDataAccess, Game game)
	{
		Poser3DDataSet dataSet = new()
		{
			Name = "PD2Poser3D",
			Description = "Test dataset",
			Game = game
		};
		var copTag = dataSet.TagsLibrary.CreateTag("Cop");
		copTag.Color = 123;
		copTag.CreateKeyPoint("Head");
		copTag.CreateNumericProperty("Distance", 0, 200);
		copTag.CreateBooleanProperty("ShouldShoot");
		var bulldozerTag = dataSet.TagsLibrary.CreateTag("Bulldozer");
		bulldozerTag.Color = 456;
		bulldozerTag.CreateKeyPoint("Face");
		bulldozerTag.CreateNumericProperty("Distance", 0, 200);
		bulldozerTag.CreateBooleanProperty("ShouldShoot");
		dataSet.ScreenshotsLibrary.MaxQuantity = 1;
		var screenshot = screenshotsDataAccess.CreateScreenshot(dataSet.ScreenshotsLibrary, SampleImageData, DateTime.Now, SampleImageResolution);
		var asset = dataSet.AssetsLibrary.MakeAsset(screenshot);
		asset.CreateItem(copTag, new Bounding(0.1, 0.15, 0.5, 0.8), [new KeyPoint3D(new Vector2<double>(0.3, 0.2), true)], [20], [true]);
		asset.CreateItem(bulldozerTag, new Bounding(0.2, 0.2, 0.6, 0.9), [new KeyPoint3D(new Vector2<double>(0.4, 0.3), false)], [25], [false]);
		screenshotsDataAccess.CreateScreenshot(dataSet.ScreenshotsLibrary, SampleImageData, DateTime.Now, SampleImageResolution);
		dataSet.WeightsLibrary.CreateWeights(
			DateTime.Now,
			ModelSize.Nano,
			new WeightsMetrics(100, new LossMetrics(0.1f, 0.2f, 0.3f)),
			new Vector2<ushort>(320, 320),
			dataSet.TagsLibrary.Tags,
			dataSet.TagsLibrary.Tags.SelectMany(tag => tag.KeyPoints),
			null);
		return dataSet;
	}

	private static Profile CreateProfile(
		PlainWeights<ClassifierTag> classifierWeights,
		PlainWeights<DetectorTag> detectorWeights,
		PoserWeights<Poser2DTag, KeyPointTag2D> poser2DWeights,
		PoserWeights<Poser3DTag, KeyPointTag3D> poser3DWeights)
	{
		Profile profile = new("Test profile");
		profile.CreateModule(classifierWeights);
		// TODO trigger actions
		var detectorModule = profile.CreateModule(detectorWeights);
		detectorModule.SetBehavior<AimBehavior>().Tags = detectorWeights
			.Tags
			.Select(tag => new AimBehavior.TagOptions(tag, 0, -0.1f))
			.ToImmutableArray();
		var poser2DModule = profile.CreateModule(poser2DWeights);
		var poser2DAimAssistBehavior = poser2DModule.SetBehavior<AimAssistBehavior>();
		poser2DAimAssistBehavior.Tags = poser2DWeights
			.Tags
			.Select(tag => new AimAssistBehavior.TagOptions(tag, 0, new Vector2<float>(0.1f, 0.05f), -0.1f))
			.ToImmutableArray();
		var poser3DModule = profile.CreateModule(poser3DWeights);
		var poser3DAimAssistBehavior = poser3DModule.SetBehavior<AimAssistBehavior>();
		poser3DAimAssistBehavior.Tags = poser3DWeights
			.Tags
			.Select(tag => new AimAssistBehavior.TagOptions(tag, 0, new Vector2<float>(0.1f, 0.05f), -0.1f))
			.ToImmutableArray();
		return profile;
	}
}