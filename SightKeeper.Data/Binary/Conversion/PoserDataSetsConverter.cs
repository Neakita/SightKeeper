using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using CommunityToolkit.Diagnostics;
using FlakeId;
using SightKeeper.Data.Binary.DataSets;
using SightKeeper.Data.Binary.DataSets.Poser;
using SightKeeper.Data.Binary.Services;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Model.DataSets.Poser;

namespace SightKeeper.Data.Binary.Conversion;

internal sealed class PoserDataSetsConverter
{
	public PoserDataSetsConverter(
		FileSystemScreenshotsDataAccess screenshotsDataAccess,
		FileSystemWeightsDataAccess weightsDataAccess)
	{
		_screenshotsDataAccess = screenshotsDataAccess;
		_weightsDataAccess = weightsDataAccess;
		_screenshotsConverter = new ScreenshotsConverter(screenshotsDataAccess);
		_assetsConverter = new PoserAssetsConverter(screenshotsDataAccess);
		_weightsConverter = new PoserWeightsConverter(weightsDataAccess);
	}

	internal SerializablePoserDataSet Convert(
		PoserDataSet dataSet,
		ConversionSession session)
	{
		SerializablePoserDataSet serializableDataSet = new(
			dataSet,
			GamesConverter.GetGameId(dataSet.Game, session),
			PoserTagsConverter.Convert(dataSet.Tags, session),
			_screenshotsConverter.Convert(dataSet.Screenshots),
			_assetsConverter.Convert(dataSet.Assets, session),
			_weightsConverter.Convert(dataSet.Weights, session));
		return serializableDataSet;
	}

	internal PoserDataSet ConvertBack(
		SerializablePoserDataSet raw,
		ReverseConversionSession session)
	{
		Guard.IsNotNull(session.Games);
		PoserDataSet dataSet = new(raw.Name, raw.Resolution);
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
	private readonly FileSystemWeightsDataAccess _weightsDataAccess;
	private readonly PoserAssetsConverter _assetsConverter;
	private readonly PoserWeightsConverter _weightsConverter;

	[UnsafeAccessor(UnsafeAccessorKind.Method, Name = "CreateScreenshot")]
	private static extern PoserScreenshot CreateScreenshot(PoserScreenshotsLibrary library);

	[UnsafeAccessor(UnsafeAccessorKind.Field, Name = "<CreationDate>k__BackingField")]
	private static extern ref DateTime CreationDateBackingField(Screenshot screenshot);
		
	[UnsafeAccessor(UnsafeAccessorKind.Method)]
	private static extern PoserWeights CreateWeights(
		PoserWeightsLibrary library,
		ModelSize size,
		WeightsMetrics metrics,
		ImmutableDictionary<PoserTag, ImmutableHashSet<KeyPointTag>> tags);

	private static void AddTags(PoserDataSet dataSet, ImmutableArray<SerializablePoserTag> tags, ReverseConversionSession session)
	{
		foreach (var rawTag in tags)
		{
			var tag = dataSet.Tags.CreateTag(rawTag.Name);
			tag.Color = rawTag.Color;
			session.Tags.Add(rawTag.Id, tag);
		}
	}

	private void AddScreenshots(PoserDataSet dataSet, ImmutableArray<SerializableScreenshot> screenshots, ReverseConversionSession session)
	{
		foreach (var rawScreenshot in screenshots)
		{
			var screenshot = CreateScreenshot(dataSet.Screenshots);
			session.Screenshots.Add(rawScreenshot.Id, screenshot);
			CreationDateBackingField(screenshot) = rawScreenshot.CreationDate;
			_screenshotsDataAccess.AssociateId(screenshot, rawScreenshot.Id);
		}
	}

	private static void AddAssets(PoserDataSet dataSet, ImmutableArray<SerializablePoserAsset> assets, ReverseConversionSession session)
	{
		foreach (var rawAsset in assets)
		{
			var screenshot = (PoserScreenshot)session.Screenshots[rawAsset.ScreenshotId];
			var asset = dataSet.Assets.MakeAsset(screenshot);
			foreach (var rawItem in rawAsset.Items)
				asset.CreateItem((PoserTag)session.Tags[rawItem.TagId], rawItem.Bounding, rawItem.KeyPoints);
		}
	}

	private void AddWeights(PoserDataSet dataSet, ImmutableArray<SerializablePoserWeights> raw, ReverseConversionSession session)
	{
		foreach (var rawWeights in raw)
		{
			var weights = CreateWeights(dataSet.Weights, rawWeights.Size, rawWeights.Metrics, ConvertBack(rawWeights.Tags, session));
			_weightsDataAccess.AssociateId(weights, rawWeights.Id);
		}
	}

	private ImmutableDictionary<PoserTag, ImmutableHashSet<KeyPointTag>> ConvertBack(
		ImmutableArray<(Id Id, ImmutableArray<Id> KeyPointIds)> tags,
		ReverseConversionSession session)
	{
		return tags.ToImmutableDictionary(
			t => (PoserTag)session.Tags[t.Id],
			t => t.KeyPointIds.Select(id => (KeyPointTag)session.Tags[id]).ToImmutableHashSet());
	}
}