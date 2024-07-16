using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using CommunityToolkit.Diagnostics;
using SightKeeper.Data.Binary.DataSets;
using SightKeeper.Data.Binary.DataSets.Detector;
using SightKeeper.Data.Binary.Services;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Model.DataSets.Detector;

namespace SightKeeper.Data.Binary.Conversion;

public sealed class DetectorDataSetsConverter
{
	public DetectorDataSetsConverter(
		FileSystemScreenshotsDataAccess screenshotsDataAccess,
		FileSystemDetectorWeightsDataAccess weightsDataAccess)
	{
		_screenshotsDataAccess = screenshotsDataAccess;
		_weightsDataAccess = weightsDataAccess;
		_screenshotsConverter = new ScreenshotsConverter(screenshotsDataAccess);
		_assetsConverter = new DetectorAssetsConverter(screenshotsDataAccess);
		_weightsConverter = new DetectorWeightsConverter(weightsDataAccess);
	}

	internal SerializableDetectorDataSet Convert(
		DetectorDataSet dataSet,
		ConversionSession session)
	{
		SerializableDetectorDataSet serializableDataSet = new(
			dataSet,
			GamesConverter.GetGameId(dataSet.Game, session),
			TagsConverter.Convert(dataSet.Tags, session),
			_screenshotsConverter.Convert(dataSet.Screenshots),
			_assetsConverter.Convert(dataSet.Assets, session),
			_weightsConverter.Convert(dataSet.Weights, session));
		return serializableDataSet;
	}

	internal DetectorDataSet ConvertBack(
		SerializableDetectorDataSet raw,
		ReverseConversionSession session)
	{
		Guard.IsNotNull(session.Games);
		DetectorDataSet dataSet = new(raw.Name, raw.Resolution);
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

	private readonly ScreenshotsConverter _screenshotsConverter;
	private readonly FileSystemScreenshotsDataAccess _screenshotsDataAccess;
	private readonly FileSystemDetectorWeightsDataAccess _weightsDataAccess;
	private readonly DetectorAssetsConverter _assetsConverter;
	private readonly DetectorWeightsConverter _weightsConverter;

	[UnsafeAccessor(UnsafeAccessorKind.Method, Name = "CreateScreenshot")]
	private static extern DetectorScreenshot CreateScreenshot(DetectorScreenshotsLibrary library);

	[UnsafeAccessor(UnsafeAccessorKind.Field, Name = "<CreationDate>k__BackingField")]
	private static extern ref DateTime CreationDateBackingField(Screenshot screenshot);
		
	[UnsafeAccessor(UnsafeAccessorKind.Method)]
	private static extern DetectorWeights CreateWeights(
		DetectorWeightsLibrary library,
		ModelSize size,
		WeightsMetrics metrics,
		IEnumerable<DetectorTag> tags);

	private static void AddTags(DetectorDataSet dataSet, ImmutableArray<SerializableTag> tags, ReverseConversionSession session)
	{
		foreach (var rawTag in tags)
		{
			var tag = dataSet.Tags.CreateTag(rawTag.Name);
			tag.Color = rawTag.Color;
			session.Tags.Add(rawTag.Id, tag);
		}
	}

	private void AddScreenshots(DetectorDataSet dataSet, ImmutableArray<SerializableScreenshot> screenshots, ReverseConversionSession session)
	{
		foreach (var rawScreenshot in screenshots)
		{
			var screenshot = CreateScreenshot(dataSet.Screenshots);
			session.Screenshots.Add(rawScreenshot.Id, screenshot);
			CreationDateBackingField(screenshot) = rawScreenshot.CreationDate;
			_screenshotsDataAccess.AssociateId(screenshot, rawScreenshot.Id);
		}
	}

	private static void AddAssets(DetectorDataSet dataSet, ImmutableArray<SerializableDetectorAsset> assets, ReverseConversionSession session)
	{
		foreach (var rawAsset in assets)
		{
			var screenshot = (DetectorScreenshot)session.Screenshots[rawAsset.ScreenshotId];
			var asset = dataSet.Assets.MakeAsset(screenshot);
			foreach (var rawItem in rawAsset.Items)
				asset.CreateItem((DetectorTag)session.Tags[rawItem.TagId], rawItem.Bounding);
		}
	}

	private void AddWeights(DetectorDataSet dataSet, ImmutableArray<SerializableDetectorWeights> raw, ReverseConversionSession session)
	{
		foreach (var rawWeights in raw)
		{
			var weights = CreateWeights(dataSet.Weights, rawWeights.Size, rawWeights.Metrics, rawWeights.Tags.Select(tagId => (DetectorTag)session.Tags[tagId]));
			_weightsDataAccess.AssociateId(weights, rawWeights.Id);
		}
	}
}