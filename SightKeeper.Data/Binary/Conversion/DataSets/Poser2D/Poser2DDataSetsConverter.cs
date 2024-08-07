using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using CommunityToolkit.Diagnostics;
using FlakeId;
using SightKeeper.Data.Binary.DataSets;
using SightKeeper.Data.Binary.DataSets.Poser2D;
using SightKeeper.Data.Binary.Services;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Model.DataSets.Poser;
using SightKeeper.Domain.Model.DataSets.Poser2D;
using SerializablePoserWeights = SightKeeper.Data.Binary.DataSets.Poser.SerializablePoserWeights;

namespace SightKeeper.Data.Binary.Conversion.DataSets.Poser2D;

internal sealed class Poser2DDataSetsConverter
{
	public Poser2DDataSetsConverter(
		FileSystemScreenshotsDataAccess screenshotsDataAccess,
		FileSystemWeightsDataAccess weightsDataAccess,
		WeightsConverter weightsConverter)
	{
		_screenshotsDataAccess = screenshotsDataAccess;
		_weightsDataAccess = weightsDataAccess;
		_weightsConverter = weightsConverter;
		_screenshotsConverter = new ScreenshotsConverter(screenshotsDataAccess);
		_assetsConverter = new Poser2DAssetsConverter(screenshotsDataAccess);
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
		CreateTags(dataSet, raw.Tags, session);
		CreateScreenshots(dataSet, raw.Screenshots, session);
		CreateAssets(dataSet, raw.Assets, session);
		CreateWeights(dataSet, raw.Weights, session);
		return dataSet;
	}

	private readonly ScreenshotsConverter _screenshotsConverter;
	private readonly FileSystemScreenshotsDataAccess _screenshotsDataAccess;
	private readonly FileSystemWeightsDataAccess _weightsDataAccess;
	private readonly WeightsConverter _weightsConverter;
	private readonly Poser2DAssetsConverter _assetsConverter;

	[UnsafeAccessor(UnsafeAccessorKind.Method, Name = "CreateScreenshot")]
	private static extern Screenshot<Poser2DAsset> CreateScreenshot(AssetScreenshotsLibrary<Poser2DAsset> library);

	[UnsafeAccessor(UnsafeAccessorKind.Field, Name = "<CreationDate>k__BackingField")]
	private static extern ref DateTime CreationDateBackingField(Screenshot screenshot);
		
	[UnsafeAccessor(UnsafeAccessorKind.Method)]
	private static extern Weights<TTag, TKeyPointTag> CreateWeights<TTag, TKeyPointTag>(
		WeightsLibrary<TTag, TKeyPointTag> library,
		ModelSize size,
		WeightsMetrics metrics,
		ImmutableDictionary<Poser2DTag, ImmutableHashSet<KeyPointTag2D>> tags)
		where TTag : Tag
		where TKeyPointTag : KeyPointTag<TTag>;

	private static void CreateTags(Poser2DDataSet dataSet, ImmutableArray<SerializablePoser2DTag> tags, ReverseConversionSession session)
	{
		foreach (var rawTag in tags)
			CreateTag(dataSet, session, rawTag);
	}

	private static void CreateTag(Poser2DDataSet dataSet, ReverseConversionSession session, SerializablePoser2DTag rawTag)
	{
		var tag = dataSet.Tags.CreateTag(rawTag.Name);
		tag.Color = rawTag.Color;
		CreateKeyPoints(session, rawTag.KeyPoints, tag);
		foreach (var property in rawTag.Properties)
			tag.CreateProperty(property.Name, property.MinimumValue, property.MaximumValue);
		session.Tags.Add(rawTag.Id, tag);
	}

	private static void CreateKeyPoints(ReverseConversionSession session, ImmutableArray<SerializableTag> keyPoints, Poser2DTag tag)
	{
		foreach (var rawKeyPoint in keyPoints)
			CreateKeyPoint(session, tag, rawKeyPoint);
	}

	private static void CreateKeyPoint(ReverseConversionSession session, Poser2DTag tag, SerializableTag rawKeyPoint)
	{
		var keyPoint = tag.CreateKeyPoint(rawKeyPoint.Name);
		keyPoint.Color = rawKeyPoint.Color;
		session.Tags.Add(rawKeyPoint.Id, keyPoint);
	}

	private void CreateScreenshots(Poser2DDataSet dataSet, ImmutableArray<SerializableScreenshot> screenshots, ReverseConversionSession session)
	{
		foreach (var rawScreenshot in screenshots)
		{
			var screenshot = CreateScreenshot(dataSet.Screenshots);
			session.Screenshots.Add(rawScreenshot.Id, screenshot);
			CreationDateBackingField(screenshot) = rawScreenshot.CreationDate;
			_screenshotsDataAccess.AssociateId(screenshot, rawScreenshot.Id);
		}
	}

	private static void CreateAssets(Poser2DDataSet dataSet, ImmutableArray<SerializablePoser2DAsset> assets, ReverseConversionSession session)
	{
		foreach (var rawAsset in assets)
		{
			var screenshot = (Screenshot<Poser2DAsset>)session.Screenshots[rawAsset.ScreenshotId];
			var asset = dataSet.Assets.MakeAsset(screenshot);
			foreach (var rawItem in rawAsset.Items)
				asset.CreateItem((Poser2DTag)session.Tags[rawItem.TagId], rawItem.Bounding, rawItem.KeyPoints, rawItem.Properties);
		}
	}

	private void CreateWeights(Poser2DDataSet dataSet, ImmutableArray<SerializablePoserWeights> raw, ReverseConversionSession session)
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