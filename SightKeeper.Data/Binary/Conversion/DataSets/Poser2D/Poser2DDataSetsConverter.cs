using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using CommunityToolkit.Diagnostics;
using FlakeId;
using SightKeeper.Data.Binary.DataSets.Poser;
using SightKeeper.Data.Binary.Services;
using SightKeeper.Domain.Model.DataSets.Poser;
using SightKeeper.Domain.Model.DataSets.Poser2D;
using SightKeeper.Domain.Model.DataSets.Screenshots;
using SightKeeper.Domain.Model.DataSets.Weights;
using Poser2DAsset = SightKeeper.Data.Binary.DataSets.Poser2D.Poser2DAsset;
using Poser2DDataSet = SightKeeper.Data.Binary.DataSets.Poser2D.Poser2DDataSet;
using Poser2DTag = SightKeeper.Data.Binary.DataSets.Poser2D.Poser2DTag;
using Screenshot = SightKeeper.Data.Binary.DataSets.Screenshot;
using Tag = SightKeeper.Data.Binary.DataSets.Tag;

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

	internal Poser2DDataSet Convert(
		Domain.Model.DataSets.Poser2D.Poser2DDataSet dataSet,
		ConversionSession session)
	{
		Poser2DDataSet serializableDataSet = new(
			dataSet,
			GamesConverter.GetGameId(dataSet.Game, session),
			Poser2DTagsConverter.Convert(dataSet.Tags, session),
			_screenshotsConverter.Convert(dataSet.Screenshots),
			_assetsConverter.Convert(dataSet.Assets, session),
			_weightsConverter.Convert(dataSet.Weights, session));
		return serializableDataSet;
	}

	internal Domain.Model.DataSets.Poser2D.Poser2DDataSet ConvertBack(
		Poser2DDataSet raw,
		ReverseConversionSession session)
	{
		Guard.IsNotNull(session.Games);
		Domain.Model.DataSets.Poser2D.Poser2DDataSet dataSet = new()
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
	private readonly Poser2DAssetsConverter _assetsConverter;

	[UnsafeAccessor(UnsafeAccessorKind.Method)]
	private static extern Weights<TTag, TKeyPointTag> CreateWeights<TTag, TKeyPointTag>(
		WeightsLibrary<TTag, TKeyPointTag> library,
		ModelSize size,
		WeightsMetrics metrics,
		ImmutableDictionary<Domain.Model.DataSets.Poser2D.Poser2DTag, ImmutableHashSet<KeyPointTag2D>> tags)
		where TTag : PoserTag
		where TKeyPointTag : KeyPointTag<TTag>;

	private static void CreateTags(Domain.Model.DataSets.Poser2D.Poser2DDataSet dataSet, ImmutableArray<Poser2DTag> tags, ReverseConversionSession session)
	{
		foreach (var rawTag in tags)
			CreateTag(dataSet, session, rawTag);
	}

	private static void CreateTag(Domain.Model.DataSets.Poser2D.Poser2DDataSet dataSet, ReverseConversionSession session, Poser2DTag rawTag)
	{
		var tag = dataSet.Tags.CreateTag(rawTag.Name);
		tag.Color = rawTag.Color;
		CreateKeyPoints(session, rawTag.KeyPoints, tag);
		foreach (var property in rawTag.Properties)
			tag.CreateProperty(property.Name, property.MinimumValue, property.MaximumValue);
		session.Tags.Add(rawTag.Id, tag);
	}

	private static void CreateKeyPoints(ReverseConversionSession session, ImmutableArray<Tag> keyPoints, Domain.Model.DataSets.Poser2D.Poser2DTag tag)
	{
		foreach (var rawKeyPoint in keyPoints)
			CreateKeyPoint(session, tag, rawKeyPoint);
	}

	private static void CreateKeyPoint(ReverseConversionSession session, Domain.Model.DataSets.Poser2D.Poser2DTag tag, Tag rawKeyPoint)
	{
		var keyPoint = tag.CreateKeyPoint(rawKeyPoint.Name);
		keyPoint.Color = rawKeyPoint.Color;
		session.Tags.Add(rawKeyPoint.Id, keyPoint);
	}

	private void CreateScreenshots(Domain.Model.DataSets.Poser2D.Poser2DDataSet dataSet, ImmutableArray<Screenshot> screenshots, ReverseConversionSession session)
	{
		foreach (var rawScreenshot in screenshots)
		{
			var screenshot = dataSet.Screenshots.CreateScreenshot(rawScreenshot.CreationDate, out var removedScreenshots);
			Guard.IsTrue(removedScreenshots.IsEmpty);
			session.Screenshots.Add(rawScreenshot.Id, screenshot);
			_screenshotsDataAccess.AssociateId(screenshot, rawScreenshot.Id);
		}
	}

	private static void CreateAssets(Domain.Model.DataSets.Poser2D.Poser2DDataSet dataSet, ImmutableArray<Poser2DAsset> assets, ReverseConversionSession session)
	{
		foreach (var rawAsset in assets)
		{
			var screenshot = (Screenshot<Domain.Model.DataSets.Poser2D.Poser2DAsset>)session.Screenshots[rawAsset.ScreenshotId];
			var asset = dataSet.Assets.MakeAsset(screenshot);
			foreach (var rawItem in rawAsset.Items)
				asset.CreateItem((Domain.Model.DataSets.Poser2D.Poser2DTag)session.Tags[rawItem.TagId], rawItem.Bounding, rawItem.KeyPoints, rawItem.Properties);
		}
	}

	private void CreateWeights(Domain.Model.DataSets.Poser2D.Poser2DDataSet dataSet, ImmutableArray<PoserWeights> raw, ReverseConversionSession session)
	{
		foreach (var rawWeights in raw)
		{
			var weights = CreateWeights(dataSet.Weights, rawWeights.Size, rawWeights.Metrics, ConvertBack(rawWeights.Tags, session));
			_weightsDataAccess.AssociateId(weights, rawWeights.Id);
			session.Weights.Add(rawWeights.Id, weights);
		}
	}

	private ImmutableDictionary<Domain.Model.DataSets.Poser2D.Poser2DTag, ImmutableHashSet<KeyPointTag2D>> ConvertBack(
		ImmutableArray<(Id Id, ImmutableArray<Id> KeyPointIds)> tags,
		ReverseConversionSession session)
	{
		return tags.ToImmutableDictionary(
			t => (Domain.Model.DataSets.Poser2D.Poser2DTag)session.Tags[t.Id],
			t => t.KeyPointIds.Select(id => (KeyPointTag2D)session.Tags[id]).ToImmutableHashSet());
	}
}