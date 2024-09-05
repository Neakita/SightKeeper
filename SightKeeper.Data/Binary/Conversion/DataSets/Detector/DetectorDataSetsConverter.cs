using System.Collections.Immutable;
using CommunityToolkit.Diagnostics;
using SightKeeper.Data.Binary.DataSets;
using SightKeeper.Data.Binary.Services;
using SightKeeper.Domain.Model.DataSets.Detector;
using SightKeeper.Domain.Model.DataSets.Screenshots;
using DetectorAsset = SightKeeper.Data.Binary.DataSets.Detector.DetectorAsset;
using DetectorDataSet = SightKeeper.Data.Binary.DataSets.Detector.DetectorDataSet;
using Screenshot = SightKeeper.Data.Binary.DataSets.Screenshot;
using Tag = SightKeeper.Data.Binary.DataSets.Tag;

namespace SightKeeper.Data.Binary.Conversion.DataSets.Detector;

internal sealed class DetectorDataSetsConverter
{
	public DetectorDataSetsConverter(
		FileSystemScreenshotsDataAccess screenshotsDataAccess,
		FileSystemWeightsDataAccess weightsDataAccess,
		WeightsConverter weightsConverter)
	{
		_screenshotsDataAccess = screenshotsDataAccess;
		_weightsDataAccess = weightsDataAccess;
		_weightsConverter = weightsConverter;
		_screenshotsConverter = new ScreenshotsConverter(screenshotsDataAccess);
		_assetsConverter = new DetectorAssetsConverter(screenshotsDataAccess);
	}

	internal DetectorDataSet Convert(
		Domain.Model.DataSets.Detector.DetectorDataSet dataSet,
		ConversionSession session)
	{
		DetectorDataSet serializableDataSet = new(
			dataSet,
			GamesConverter.GetGameId(dataSet.Game, session),
			TagsConverter.Convert(dataSet.Tags, session),
			_screenshotsConverter.Convert(dataSet.Screenshots),
			_assetsConverter.Convert(dataSet.Assets, session),
			_weightsConverter.Convert(dataSet.Weights, session));
		return serializableDataSet;
	}

	internal Domain.Model.DataSets.Detector.DetectorDataSet ConvertBack(
		DetectorDataSet raw,
		ReverseConversionSession session)
	{
		Guard.IsNotNull(session.Games);
		Domain.Model.DataSets.Detector.DetectorDataSet dataSet = new()
		{
			Name = raw.Name,
			Description = raw.Description
		};
		if (raw.GameId != null)
			dataSet.Game = session.Games[raw.GameId.Value];
		dataSet.Screenshots.MaxQuantity = raw.MaxScreenshots;
		AddTags(dataSet, raw.Tags, session);
		AddScreenshots(dataSet, raw.Screenshots, session);
		AddAssets(dataSet, raw.Assets, session);
		AddWeights(dataSet, raw.Weights, session);
		return dataSet;
	}

	private readonly ScreenshotsConverter _screenshotsConverter;
	private readonly FileSystemScreenshotsDataAccess _screenshotsDataAccess;
	private readonly FileSystemWeightsDataAccess _weightsDataAccess;
	private readonly WeightsConverter _weightsConverter;
	private readonly DetectorAssetsConverter _assetsConverter;

	private static void AddTags(Domain.Model.DataSets.Detector.DetectorDataSet dataSet, ImmutableArray<Tag> tags, ReverseConversionSession session)
	{
		foreach (var rawTag in tags)
		{
			var tag = dataSet.Tags.CreateTag(rawTag.Name);
			tag.Color = rawTag.Color;
			session.Tags.Add(rawTag.Id, tag);
		}
	}

	private void AddScreenshots(Domain.Model.DataSets.Detector.DetectorDataSet dataSet, ImmutableArray<Screenshot> screenshots, ReverseConversionSession session)
	{
		throw new NotImplementedException();
		// foreach (var rawScreenshot in screenshots)
		// {
		// 	var screenshot = dataSet.Screenshots.CreateScreenshot(rawScreenshot.CreationDate, new Vector2<ushort>(), out var removedScreenshots);
		// 	Guard.IsTrue(removedScreenshots.IsEmpty);
		// 	session.Screenshots.Add(rawScreenshot.Id, screenshot);
		// 	_screenshotsDataAccess.AssociateId(screenshot, rawScreenshot.Id);
		// }
	}

	private static void AddAssets(Domain.Model.DataSets.Detector.DetectorDataSet dataSet, ImmutableArray<DetectorAsset> assets, ReverseConversionSession session)
	{
		foreach (var rawAsset in assets)
		{
			var screenshot = (Screenshot<Domain.Model.DataSets.Detector.DetectorAsset>)session.Screenshots[rawAsset.ScreenshotId];
			var asset = dataSet.Assets.MakeAsset(screenshot);
			foreach (var rawItem in rawAsset.Items)
				asset.CreateItem((DetectorTag)session.Tags[rawItem.TagId], rawItem.Bounding);
		}
	}

	private void AddWeights(Domain.Model.DataSets.Detector.DetectorDataSet dataSet, ImmutableArray<WeightsWithTags> raw, ReverseConversionSession session)
	{
		throw new NotImplementedException();
		// foreach (var rawWeights in raw)
		// {
		// 	var weights = dataSet.Weights.CreateWeights(rawWeights.CreationDate, rawWeights.Size, rawWeights.Metrics, new Vector2<ushort>(), rawWeights.Tags.Select(tagId => (DetectorTag)session.Tags[tagId]));
		// 	_weightsDataAccess.AssociateId(weights, rawWeights.Id);
		// 	session.Weights.Add(rawWeights.Id, weights);
		// }
	}
}