using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using CommunityToolkit.Diagnostics;
using FlakeId;
using SightKeeper.Data.Binary.DataSets;
using SightKeeper.Data.Binary.DataSets.Poser2D;
using SightKeeper.Data.Binary.Services;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Model.DataSets.Poser2D;

namespace SightKeeper.Data.Binary.Conversion.DataSets.Poser2D;

internal sealed class Poser2DDataSetsConverter
{
	public Poser2DDataSetsConverter(
		FileSystemScreenshotsDataAccess screenshotsDataAccess,
		FileSystemWeightsDataAccess weightsDataAccess)
	{
		_screenshotsDataAccess = screenshotsDataAccess;
		_weightsDataAccess = weightsDataAccess;
		_screenshotsConverter = new ScreenshotsConverter(screenshotsDataAccess);
		_assetsConverter = new Poser2DAssetsConverter(screenshotsDataAccess);
		_weightsConverter = new Poser2DWeightsConverter(weightsDataAccess);
	}

	internal SerializablePoser2DDataSet Convert(
		Poser2DDataSet dataSet,
		ConversionSession session)
	{
		SerializablePoser2DDataSet serializableDataSet = new(
			dataSet,
			GamesConverter.GetGameId(dataSet.Game, session),
			Poser2DTagsConverter.Convert(dataSet.Tags, session),
			_screenshotsConverter.Convert(dataSet.Screenshots),
			_assetsConverter.Convert(dataSet.Assets, session),
			_weightsConverter.Convert(dataSet.Weights, session));
		return serializableDataSet;
	}

	internal Poser2DDataSet ConvertBack(
		SerializablePoser2DDataSet raw,
		ReverseConversionSession session)
	{
		Guard.IsNotNull(session.Games);
		Poser2DDataSet dataSet = new(raw.Name, raw.Resolution);
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
	private readonly Poser2DAssetsConverter _assetsConverter;
	private readonly Poser2DWeightsConverter _weightsConverter;

	[UnsafeAccessor(UnsafeAccessorKind.Method, Name = "CreateScreenshot")]
	private static extern Poser2DScreenshot CreateScreenshot(Poser2DScreenshotsLibrary library);

	[UnsafeAccessor(UnsafeAccessorKind.Field, Name = "<CreationDate>k__BackingField")]
	private static extern ref DateTime CreationDateBackingField(Screenshot screenshot);
		
	[UnsafeAccessor(UnsafeAccessorKind.Method)]
	private static extern Poser2DWeights CreateWeights(
		Poser2DWeightsLibrary library,
		ModelSize size,
		WeightsMetrics metrics,
		ImmutableDictionary<Poser2DTag, ImmutableHashSet<KeyPointTag2D>> tags);

	private static void AddTags(Poser2DDataSet dataSet, ImmutableArray<SerializablePoser2DTag> tags, ReverseConversionSession session)
	{
		foreach (var rawTag in tags)
		{
			var tag = dataSet.Tags.CreateTag(rawTag.Name);
			tag.Color = rawTag.Color;
			session.Tags.Add(rawTag.Id, tag);
		}
	}

	private void AddScreenshots(Poser2DDataSet dataSet, ImmutableArray<SerializableScreenshot> screenshots, ReverseConversionSession session)
	{
		foreach (var rawScreenshot in screenshots)
		{
			var screenshot = CreateScreenshot(dataSet.Screenshots);
			session.Screenshots.Add(rawScreenshot.Id, screenshot);
			CreationDateBackingField(screenshot) = rawScreenshot.CreationDate;
			_screenshotsDataAccess.AssociateId(screenshot, rawScreenshot.Id);
		}
	}

	private static void AddAssets(Poser2DDataSet dataSet, ImmutableArray<SerializablePoser2DAsset> assets, ReverseConversionSession session)
	{
		foreach (var rawAsset in assets)
		{
			var screenshot = (Poser2DScreenshot)session.Screenshots[rawAsset.ScreenshotId];
			var asset = dataSet.Assets.MakeAsset(screenshot);
			foreach (var rawItem in rawAsset.Items)
				asset.CreateItem((Poser2DTag)session.Tags[rawItem.TagId], rawItem.Bounding, rawItem.KeyPoints, rawItem.Properties);
		}
	}

	private void AddWeights(Poser2DDataSet dataSet, ImmutableArray<SerializablePoser2DWeights> raw, ReverseConversionSession session)
	{
		foreach (var rawWeights in raw)
		{
			var weights = CreateWeights(dataSet.Weights, rawWeights.Size, rawWeights.Metrics, ConvertBack(rawWeights.Tags, session));
			_weightsDataAccess.AssociateId(weights, rawWeights.Id);
			session.Weights.Add(rawWeights.Id, weights);
		}
	}

	private ImmutableDictionary<Poser2DTag, ImmutableHashSet<KeyPointTag2D>> ConvertBack(
		ImmutableArray<(Id Id, ImmutableArray<Id> KeyPointIds)> tags,
		ReverseConversionSession session)
	{
		return tags.ToImmutableDictionary(
			t => (Poser2DTag)session.Tags[t.Id],
			t => t.KeyPointIds.Select(id => (KeyPointTag2D)session.Tags[id]).ToImmutableHashSet());
	}
}