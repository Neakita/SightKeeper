using CommunityToolkit.Diagnostics;
using CommunityToolkit.HighPerformance;
using FluentAssertions;
using FluentAssertions.Equivalency;
using MemoryPack;
using SightKeeper.Data.Services;
using SightKeeper.Domain;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Classifier;
using SightKeeper.Domain.DataSets.Detector;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Poser2D;
using SightKeeper.Domain.DataSets.Poser3D;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.DataSets.Weights;
using SightKeeper.Domain.Screenshots;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace SightKeeper.Data.Tests;

public sealed class BinarySerializationTests
{
	private static readonly Image<Rgba32> SampleImage = Image.Load<Rgba32>("sample.png");

	private static ReadOnlySpan2D<Rgba32> SampleImageData
	{
		get
		{
			Guard.IsTrue(SampleImage.DangerousTryGetSinglePixelMemory(out var memory));
			return memory.Span.AsSpan2D(SampleImage.Height, SampleImage.Width);
		}
	}

	[Fact]
	public void ShouldSaveAndLoadAppData()
	{
		AppDataAccess appDataAccess = new();
		Lock appDataLock = new();
		AppDataScreenshotsLibrariesDataAccess screenshotsLibrariesDataAccess = new(appDataAccess, appDataLock);
		FileSystemScreenshotsDataAccess screenshotsDataAccess = new(appDataAccess, appDataLock);
		MemoryPackFormatterProvider.Register(new AppDataFormatter(screenshotsDataAccess, appDataLock));
		ScreenshotsLibrary screenshotsLibrary = new()
		{
			Name = "PD2"
		};
		screenshotsLibrariesDataAccess.Add(screenshotsLibrary);
		var screenshot = screenshotsDataAccess.CreateScreenshot(screenshotsLibrary, SampleImageData, DateTimeOffset.Now);
		AppDataDataSetsDataAccess appDataDataSetsDataAccess = new(appDataAccess, appDataLock);
		foreach (var dataSet in CreateDataSets(screenshot))
			appDataDataSetsDataAccess.Add(dataSet);
		appDataAccess.Save();
		var data = appDataAccess.Data;
		appDataAccess.Load();
		appDataAccess.Data.Should().BeEquivalentTo(data, ConfigureEquivalencyAssertion);
		Directory.Delete(screenshotsDataAccess.DirectoryPath, true);
	}

	private static EquivalencyAssertionOptions<AppData> ConfigureEquivalencyAssertion(
		EquivalencyAssertionOptions<AppData> options)
	{
		return options
			.RespectingRuntimeTypes()
			.AllowingInfiniteRecursion()
			.Excluding(info => info.Type.Name.Contains("Dictionary")); // https://github.com/fluentassertions/fluentassertions/issues/1136
	}

	private static IEnumerable<DataSet> CreateDataSets(Screenshot screenshot)
	{
		yield return CreateClassifierDataSet(screenshot);
		yield return CreateDetectorDataSet(screenshot);
		yield return CreatePoser2DDataSet(screenshot);
		yield return CreatePoser3DDataSet(screenshot);
	}

	private static ClassifierDataSet CreateClassifierDataSet(Screenshot screenshot)
	{
		ClassifierDataSet dataSet = new()
		{
			Name = "PD2Classifier",
			Description = "Test dataset"
		};
		dataSet.TagsLibrary.CreateTag("Don't Shoot");
		var shootTag = dataSet.TagsLibrary.CreateTag("shoot");
		var asset = dataSet.AssetsLibrary.MakeAsset(screenshot);
		asset.Tag = shootTag;
		dataSet.WeightsLibrary.CreateWeights(
			DateTime.Now,
			ModelSize.Nano,
			new WeightsMetrics(100, new LossMetrics(0.1f, 0.2f, 0.3f)),
			new Vector2<ushort>(320, 320),
			null,
			dataSet.TagsLibrary.Tags);
		return dataSet;
	}

	private static DetectorDataSet CreateDetectorDataSet(Screenshot screenshot)
	{
		DetectorDataSet dataSet = new()
		{
			Name = "PD2Detector",
			Description = "Test dataset"
		};
		var copTag = dataSet.TagsLibrary.CreateTag("Cop");
		copTag.Color = 123;
		var bulldozerTag = dataSet.TagsLibrary.CreateTag("Bulldozer");
		bulldozerTag.Color = 456;
		var asset = dataSet.AssetsLibrary.MakeAsset(screenshot);
		asset.CreateItem(copTag, new Bounding(0.1, 0.15, 0.5, 0.8));
		asset.CreateItem(bulldozerTag, new Bounding(0.2, 0.2, 0.6, 0.9));
		dataSet.WeightsLibrary.CreateWeights(
			DateTime.Now,
			ModelSize.Nano,
			new WeightsMetrics(100, new LossMetrics(0.1f, 0.2f, 0.3f)),
			new Vector2<ushort>(320, 320),
			null,
			dataSet.TagsLibrary.Tags);
		return dataSet;
	}

	private static Poser2DDataSet CreatePoser2DDataSet(Screenshot screenshot)
	{
		Poser2DDataSet dataSet = new()
		{
			Name = "PD2Poser",
			Description = "Test dataset"
		};
		var copTag = dataSet.TagsLibrary.CreateTag("Cop");
		copTag.Color = 123;
		var copHeadKeyPoint = copTag.CreateKeyPointTag("Head");
		var bulldozerTag = dataSet.TagsLibrary.CreateTag("Bulldozer");
		bulldozerTag.Color = 456;
		var bulldozerFaceKeyPoint = bulldozerTag.CreateKeyPointTag("Face");
		var asset = dataSet.AssetsLibrary.MakeAsset(screenshot);
		var copItem = asset.CreateItem(copTag, new Bounding(0.1, 0.15, 0.5, 0.8));
		copItem.CreateKeyPoint(copHeadKeyPoint, new Vector2<double>(0.3, 0.2));
		var bulldozerItem = asset.CreateItem(bulldozerTag, new Bounding(0.2, 0.2, 0.6, 0.9));
		bulldozerItem.CreateKeyPoint(bulldozerFaceKeyPoint, new Vector2<double>(0.4, 0.3));
		Dictionary<PoserTag, IReadOnlyCollection<Tag>> weightsTags = new()
		{
			{ copTag, copTag.KeyPointTags },
			{ bulldozerTag, bulldozerTag.KeyPointTags }
		};
		dataSet.WeightsLibrary.CreateWeights(
			DateTime.Now,
			ModelSize.Nano,
			new WeightsMetrics(100, new LossMetrics(0.1f, 0.2f, 0.3f)),
			new Vector2<ushort>(320, 320),
			null,
			weightsTags);
		return dataSet;
	}

	private static Poser3DDataSet CreatePoser3DDataSet(Screenshot screenshot)
	{
		Poser3DDataSet dataSet = new()
		{
			Name = "PD2Poser3D",
			Description = "Test dataset"
		};
		var copTag = dataSet.TagsLibrary.CreateTag("Cop");
		copTag.Color = 123;
		var copHeadKeyPoint = copTag.CreateKeyPointTag("Head");
		var bulldozerTag = dataSet.TagsLibrary.CreateTag("Bulldozer");
		bulldozerTag.Color = 456;
		var bulldozerFaceKeyPoint = bulldozerTag.CreateKeyPointTag("Face");
		var asset = dataSet.AssetsLibrary.MakeAsset(screenshot);
		var copItem = asset.CreateItem(copTag, new Bounding(0.1, 0.15, 0.5, 0.8));
		copItem.CreateKeyPoint(copHeadKeyPoint, new Vector2<double>(0.3, 0.2));
		var bulldozerItem = asset.CreateItem(bulldozerTag, new Bounding(0.2, 0.2, 0.6, 0.9));
		bulldozerItem.CreateKeyPoint(bulldozerFaceKeyPoint, new Vector2<double>(0.4, 0.3), false);
		Dictionary<PoserTag, IReadOnlyCollection<Tag>> weightsTags = new()
		{
			{ copTag, copTag.KeyPointTags },
			{ bulldozerTag, bulldozerTag.KeyPointTags }
		};
		dataSet.WeightsLibrary.CreateWeights(
			DateTime.Now,
			ModelSize.Nano,
			new WeightsMetrics(100, new LossMetrics(0.1f, 0.2f, 0.3f)),
			new Vector2<ushort>(320, 320),
			null,
			weightsTags);
		return dataSet;
	}
}