using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using CommunityToolkit.Diagnostics;
using SightKeeper.Data.Binary.DataSets;
using SightKeeper.Data.Binary.DataSets.Classifier;
using SightKeeper.Data.Binary.Services;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Model.DataSets.Classifier;

namespace SightKeeper.Data.Binary.Conversion;

internal sealed class ClassifierDataSetsConverter
{
	public ClassifierDataSetsConverter(
		FileSystemScreenshotsDataAccess screenshotsDataAccess,
		FileSystemClassifierWeightsDataAccess weightsDataAccess)
	{
		_screenshotsDataAccess = screenshotsDataAccess;
		_weightsDataAccess = weightsDataAccess;
		_screenshotsConverter = new ScreenshotsConverter(screenshotsDataAccess);
		_assetsConverter = new ClassifierAssetsConverter(screenshotsDataAccess);
		_weightsConverter = new ClassifierWeightsConverter(weightsDataAccess);
	}

	public SerializableClassifierDataSet Convert(ClassifierDataSet dataSet, ConversionSession session)
	{
		SerializableClassifierDataSet serializableDataSet = new(
			dataSet,
			GamesConverter.GetGameId(dataSet.Game, session),
			TagsConverter.Convert(dataSet.Tags, session),
			_screenshotsConverter.Convert(dataSet.Screenshots),
			_assetsConverter.Convert(dataSet.Assets, session),
			_weightsConverter.Convert(dataSet.Weights, session));
		return serializableDataSet;
	}

	public ClassifierDataSet ConvertBack(
		SerializableClassifierDataSet raw,
		ReverseConversionSession session)
	{
		Guard.IsNotNull(session.Games);
		ClassifierDataSet dataSet = new(raw.Name, raw.Resolution);
		if (raw.GameId != null)
			dataSet.Game = session.Games[raw.GameId.Value];
		dataSet.Screenshots.MaxQuantity = raw.MaxScreenshots;
		dataSet.Description = raw.Description;
		AddTags(dataSet, raw.Tags, session);
		AddScreenshots(dataSet, raw.Screenshots, session);
		AddAssets(dataSet, raw.Assets, session);
		AddWeights(dataSet, raw.Weights, session);
		return dataSet;
	}

	private readonly FileSystemScreenshotsDataAccess _screenshotsDataAccess;
	private readonly FileSystemClassifierWeightsDataAccess _weightsDataAccess;
	private readonly ScreenshotsConverter _screenshotsConverter;
	private readonly ClassifierAssetsConverter _assetsConverter;
	private readonly ClassifierWeightsConverter _weightsConverter;

	[UnsafeAccessor(UnsafeAccessorKind.Method, Name = "CreateScreenshot")]
	private static extern ClassifierScreenshot CreateScreenshot(ClassifierScreenshotsLibrary library);

	[UnsafeAccessor(UnsafeAccessorKind.Field, Name = "<CreationDate>k__BackingField")]
	private static extern ref DateTime CreationDateBackingField(Screenshot screenshot);
		
	[UnsafeAccessor(UnsafeAccessorKind.Method)]
	private static extern ClassifierWeights CreateWeights(
		ClassifierWeightsLibrary library,
		ModelSize size,
		WeightsMetrics metrics,
		IEnumerable<ClassifierTag> tags);

	private static void AddTags(ClassifierDataSet dataSet, ImmutableArray<SerializableTag> tags, ReverseConversionSession session)
	{
		foreach (var rawTag in tags)
		{
			var tag = dataSet.Tags.CreateTag(rawTag.Name);
			tag.Color = rawTag.Color;
			session.Tags.Add(rawTag.Id, tag);
		}
	}

	private void AddScreenshots(ClassifierDataSet dataSet, ImmutableArray<SerializableScreenshot> screenshots, ReverseConversionSession session)
	{
		foreach (var rawScreenshot in screenshots)
		{
			var screenshot = CreateScreenshot(dataSet.Screenshots);
			session.Screenshots.Add(rawScreenshot.Id, screenshot);
			CreationDateBackingField(screenshot) = rawScreenshot.CreationDate;
			_screenshotsDataAccess.AssociateId(screenshot, rawScreenshot.Id);
		}
	}

	private static void AddAssets(ClassifierDataSet dataSet, ImmutableArray<SerializableClassifierAsset> assets, ReverseConversionSession session)
	{
		foreach (var rawAsset in assets)
		{
			var screenshot = (ClassifierScreenshot)session.Screenshots[rawAsset.ScreenshotId];
			dataSet.Assets.MakeAsset(screenshot, (ClassifierTag)session.Tags[rawAsset.TagId]);
		}
	}

	private void AddWeights(ClassifierDataSet dataSet, ImmutableArray<SerializableWeightsWithTags> raw, ReverseConversionSession session)
	{
		foreach (var rawWeights in raw)
		{
			var weights = CreateWeights(dataSet.Weights, rawWeights.Size, rawWeights.Metrics, rawWeights.Tags.Select(tagId => (ClassifierTag)session.Tags[tagId]));
			_weightsDataAccess.AssociateId(weights, rawWeights.Id);
		}
	}
}