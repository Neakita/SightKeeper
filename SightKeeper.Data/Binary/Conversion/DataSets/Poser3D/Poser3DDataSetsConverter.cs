using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using CommunityToolkit.Diagnostics;
using FlakeId;
using SightKeeper.Data.Binary.DataSets.Poser;
using SightKeeper.Data.Binary.Services;
using SightKeeper.Domain.Model.DataSets.Poser;
using SightKeeper.Domain.Model.DataSets.Poser3D;
using SightKeeper.Domain.Model.DataSets.Screenshots;
using SightKeeper.Domain.Model.DataSets.Weights;
using Poser3DAsset = SightKeeper.Data.Binary.DataSets.Poser3D.Poser3DAsset;
using Poser3DDataSet = SightKeeper.Data.Binary.DataSets.Poser3D.Poser3DDataSet;
using Poser3DTag = SightKeeper.Data.Binary.DataSets.Poser3D.Poser3DTag;
using Screenshot = SightKeeper.Data.Binary.DataSets.Screenshot;
using Tag = SightKeeper.Data.Binary.DataSets.Tag;

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

	internal Poser3DDataSet Convert(
		Domain.Model.DataSets.Poser3D.Poser3DDataSet dataSet,
		ConversionSession session)
	{
		Poser3DDataSet serializableDataSet = new(
			dataSet,
			GamesConverter.GetGameId(dataSet.Game, session),
			_screenshotsConverter.Convert(dataSet.Screenshots),
			Poser3DTagsConverter.Convert(dataSet.Tags, session),
			_assetsConverter.Convert(dataSet.Assets, session),
			_weightsConverter.Convert(dataSet.Weights, session));
		return serializableDataSet;
	}

	internal Domain.Model.DataSets.Poser3D.Poser3DDataSet ConvertBack(
		Poser3DDataSet raw,
		ReverseConversionSession session)
	{
		Guard.IsNotNull(session.Games);
		Domain.Model.DataSets.Poser3D.Poser3DDataSet dataSet = new()
		{
			Name = raw.Name,
			Description = raw.Description,
			Game = null
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

	[UnsafeAccessor(UnsafeAccessorKind.Method)]
	private static extern Weights<TTag, TKeyPoint> CreateWeights<TTag, TKeyPoint>(
		WeightsLibrary<TTag, TKeyPoint> library,
		ModelSize size,
		WeightsMetrics metrics,
		ImmutableDictionary<Domain.Model.DataSets.Poser3D.Poser3DTag, ImmutableHashSet<KeyPointTag3D>> tags)
		where TTag : PoserTag
		where TKeyPoint : KeyPointTag<TTag>;

	private static void CreateTags(Domain.Model.DataSets.Poser3D.Poser3DDataSet dataSet, ImmutableArray<Poser3DTag> tags, ReverseConversionSession session)
	{
		foreach (var rawTag in tags)
			CreateTag(dataSet, session, rawTag);
	}

	private static void CreateTag(Domain.Model.DataSets.Poser3D.Poser3DDataSet dataSet, ReverseConversionSession session, Poser3DTag rawTag)
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

	private static void CreateKeyPoints(ReverseConversionSession session, ImmutableArray<Tag> keyPoints, Domain.Model.DataSets.Poser3D.Poser3DTag tag)
	{
		foreach (var rawKeyPoint in keyPoints)
			CreateKeyPoint(session, tag, rawKeyPoint);
	}

	private static void CreateKeyPoint(ReverseConversionSession session, Domain.Model.DataSets.Poser3D.Poser3DTag tag, Tag rawKeyPoint)
	{
		var keyPoint = tag.CreateKeyPoint(rawKeyPoint.Name);
		keyPoint.Color = rawKeyPoint.Color;
		session.Tags.Add(rawKeyPoint.Id, keyPoint);
	}

	private void CreateScreenshots(Domain.Model.DataSets.Poser3D.Poser3DDataSet dataSet, ImmutableArray<Screenshot> screenshots, ReverseConversionSession session)
	{
		throw new NotImplementedException();
		// foreach (var rawScreenshot in screenshots)
		// {
		// 	var screenshot = dataSet.Screenshots.CreateScreenshot(rawScreenshot.CreationDate, out var removedScreenshots);
		// 	Guard.IsTrue(removedScreenshots.IsEmpty);
		// 	session.Screenshots.Add(rawScreenshot.Id, screenshot);
		// 	_screenshotsDataAccess.AssociateId(screenshot, rawScreenshot.Id);
		// }
	}

	private static void CreateAssets(Domain.Model.DataSets.Poser3D.Poser3DDataSet dataSet, ImmutableArray<Poser3DAsset> assets, ReverseConversionSession session)
	{
		foreach (var rawAsset in assets)
		{
			var screenshot = (Screenshot<Domain.Model.DataSets.Poser3D.Poser3DAsset>)session.Screenshots[rawAsset.ScreenshotId];
			var asset = dataSet.Assets.MakeAsset(screenshot);
			foreach (var rawItem in rawAsset.Items)
				asset.CreateItem(
					(Domain.Model.DataSets.Poser3D.Poser3DTag)session.Tags[rawItem.TagId],
					rawItem.Bounding,
					rawItem.KeyPoints.Select(keyPoint => new KeyPoint3D(keyPoint.Position, keyPoint.IsVisible)).ToImmutableList(),
					rawItem.NumericProperties,
					rawItem.BooleanProperties);
		}
	}

	private void CreateWeights(Domain.Model.DataSets.Poser3D.Poser3DDataSet dataSet, ImmutableArray<PoserWeights> raw, ReverseConversionSession session)
	{
		foreach (var rawWeights in raw)
		{
			var weights = CreateWeights(dataSet.Weights, rawWeights.Size, rawWeights.Metrics, ConvertBack(rawWeights.Tags, session));
			_weightsDataAccess.AssociateId(weights, rawWeights.Id);
			session.Weights.Add(rawWeights.Id, weights);
		}
	}

	private ImmutableDictionary<Domain.Model.DataSets.Poser3D.Poser3DTag, ImmutableHashSet<KeyPointTag3D>> ConvertBack(
		ImmutableArray<(Id Id, ImmutableArray<Id> KeyPointIds)> tags,
		ReverseConversionSession session)
	{
		return tags.ToImmutableDictionary(
			t => (Domain.Model.DataSets.Poser3D.Poser3DTag)session.Tags[t.Id],
			t => t.KeyPointIds.Select(id => (KeyPointTag3D)session.Tags[id]).ToImmutableHashSet());
	}
}