using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using CommunityToolkit.Diagnostics;
using SightKeeper.Data.Binary.DataSets;
using SightKeeper.Data.Binary.DataSets.Poser;
using SightKeeper.Data.Binary.DataSets.Poser3D;
using SightKeeper.Data.Binary.Services;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Model.DataSets.Poser;
using SightKeeper.Domain.Model.DataSets.Poser3D;

namespace SightKeeper.Data.Binary.Conversion.DataSets.Poser3D;

internal sealed class Poser3DDataSetsConverter
{
	public Poser3DDataSetsConverter(
		FileSystemScreenshotsDataAccess screenshotsDataAccess,
		FileSystemWeightsDataAccess weightsDataAccess,
		WeightsConverter weightsConverter)
	{
		_screenshotsDataAccess = screenshotsDataAccess;
		_weightsDataAccess = weightsDataAccess;
		_weightsConverter = weightsConverter;
		_screenshotsConverter = new ScreenshotsConverter(screenshotsDataAccess);
		_assetsConverter = new Poser3DAssetsConverter(screenshotsDataAccess);
	}

	internal SerializablePoser3DDataSet Convert(
		Poser3DDataSet dataSet,
		ConversionSession session)
	{
		SerializablePoser3DDataSet serializableDataSet = new(
			dataSet,
			GamesConverter.GetGameId(dataSet.Game, session),
			_screenshotsConverter.Convert(dataSet.Screenshots),
			Poser3DTagsConverter.Convert(dataSet.Tags, session),
			_assetsConverter.Convert(dataSet.Assets, session),
			_weightsConverter.Convert(dataSet.Weights, session));
		return serializableDataSet;
	}

	internal Poser3DDataSet ConvertBack(
		SerializablePoser3DDataSet raw,
		ReverseConversionSession session)
	{
		Guard.IsNotNull(session.Games);
		Poser3DDataSet dataSet = new()
		{
			Name = raw.Name,
			Description = raw.Description,
			Game = null,
			Resolution = raw.Resolution
		};
		if (raw.GameId != null)
			dataSet.Game = session.Games[raw.GameId.Value];
		dataSet.Screenshots.MaxQuantity = raw.MaxScreenshots;
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
	private readonly Poser3DAssetsConverter _assetsConverter;

	[UnsafeAccessor(UnsafeAccessorKind.Method, Name = "CreateScreenshot")]
	private static extern Screenshot<Poser3DAsset> CreateScreenshot(AssetScreenshotsLibrary<Poser3DAsset> library);

	[UnsafeAccessor(UnsafeAccessorKind.Field, Name = "<CreationDate>k__BackingField")]
	private static extern ref DateTime CreationDateBackingField(Screenshot screenshot);
		
	[UnsafeAccessor(UnsafeAccessorKind.Method)]
	private static extern Weights<TTag, TKeyPoint> CreateWeights<TTag, TKeyPoint>(
		WeightsLibrary<TTag, TKeyPoint> library,
		ModelSize size,
		WeightsMetrics metrics,
		ImmutableDictionary<Poser3DTag, ImmutableHashSet<KeyPointTag3D>> tags)
		where TTag : Tag
		where TKeyPoint : KeyPointTag<TTag>;

	private static void CreateTags(Poser3DDataSet dataSet, ImmutableArray<SerializablePoser3DTag> tags, ReverseConversionSession session)
	{
		foreach (var rawTag in tags)
			CreateTag(dataSet, session, rawTag);
	}

	private static void CreateTag(Poser3DDataSet dataSet, ReverseConversionSession session, SerializablePoser3DTag rawTag)
	{
		var tag = dataSet.Tags.CreateTag(rawTag.Name);
		tag.Color = rawTag.Color;
		CreateKeyPoints(session, rawTag.KeyPoints, tag);
		foreach (var property in rawTag.NumericProperties)
			tag.CreateNumericProperty(property.Name, property.MinimumValue, property.MaximumValue);
		foreach (var propertyName in rawTag.BooleanProperties)
			tag.CreateBooleanProperty(propertyName);
		session.Tags.Add(rawTag.Id, tag);
	}

	private static void CreateKeyPoints(ReverseConversionSession session, ImmutableArray<SerializableTag> keyPoints, Poser3DTag tag)
	{
		foreach (var rawKeyPoint in keyPoints)
			CreateKeyPoint(session, tag, rawKeyPoint);
	}

	private static void CreateKeyPoint(ReverseConversionSession session, Poser3DTag tag, SerializableTag rawKeyPoint)
	{
		var keyPoint = tag.CreateKeyPoint(rawKeyPoint.Name);
		keyPoint.Color = rawKeyPoint.Color;
		session.Tags.Add(rawKeyPoint.Id, keyPoint);
	}

	private void CreateScreenshots(Poser3DDataSet dataSet, ImmutableArray<SerializableScreenshot> screenshots, ReverseConversionSession session)
	{
		foreach (var rawScreenshot in screenshots)
		{
			var screenshot = CreateScreenshot(dataSet.Screenshots);
			session.Screenshots.Add(rawScreenshot.Id, screenshot);
			CreationDateBackingField(screenshot) = rawScreenshot.CreationDate;
			_screenshotsDataAccess.AssociateId(screenshot, rawScreenshot.Id);
		}
	}

	private static void CreateAssets(Poser3DDataSet dataSet, ImmutableArray<SerializablePoser3DAsset> assets, ReverseConversionSession session)
	{
		foreach (var rawAsset in assets)
		{
			var screenshot = (Screenshot<Poser3DAsset>)session.Screenshots[rawAsset.ScreenshotId];
			var asset = dataSet.Assets.MakeAsset(screenshot);
			foreach (var rawItem in rawAsset.Items)
				asset.CreateItem(
					(Poser3DTag)session.Tags[rawItem.TagId],
					rawItem.Bounding,
					rawItem.KeyPoints.Select(keyPoint => new KeyPoint3D(keyPoint.Position, keyPoint.IsVisible)).ToImmutableList(),
					rawItem.NumericProperties,
					rawItem.BooleanProperties);
		}
	}

	private void CreateWeights(Poser3DDataSet dataSet, ImmutableArray<SerializablePoserWeights> raw, ReverseConversionSession session)
	{
		foreach (var rawWeights in raw)
		{
			var weights = CreateWeights(dataSet.Weights, rawWeights.Size, rawWeights.Metrics, ConvertBack(rawWeights.Tags, session));
			_weightsDataAccess.AssociateId(weights, rawWeights.Id);
			session.Weights.Add(rawWeights.Id, weights);
		}
	}

	private ImmutableDictionary<Poser3DTag, ImmutableHashSet<KeyPointTag3D>> ConvertBack(
		ImmutableArray<(FlakeId.Id Id, ImmutableArray<FlakeId.Id> KeyPointIds)> tags,
		ReverseConversionSession session)
	{
		return tags.ToImmutableDictionary(
			t => (Poser3DTag)session.Tags[t.Id],
			t => t.KeyPointIds.Select(id => (KeyPointTag3D)session.Tags[id]).ToImmutableHashSet());
	}
}