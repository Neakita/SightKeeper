using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using CommunityToolkit.Diagnostics;
using SightKeeper.Data.Binary.DataSets;
using SightKeeper.Data.Binary.Services;
using SightKeeper.Domain.Model.DataSets.Classifier;
using SightKeeper.Domain.Model.DataSets.Screenshots;
using SightKeeper.Domain.Model.DataSets.Tags;
using SightKeeper.Domain.Model.DataSets.Weights;
using ClassifierAsset = SightKeeper.Data.Binary.DataSets.Classifier.ClassifierAsset;
using ClassifierDataSet = SightKeeper.Data.Binary.DataSets.Classifier.ClassifierDataSet;
using Screenshot = SightKeeper.Data.Binary.DataSets.Screenshot;
using Tag = SightKeeper.Data.Binary.DataSets.Tag;

namespace SightKeeper.Data.Binary.Conversion.DataSets.Classifier;

internal sealed class ClassifierDataSetsConverter
{
	public ClassifierDataSetsConverter(
		FileSystemScreenshotsDataAccess screenshotsDataAccess,
		FileSystemWeightsDataAccess weightsDataAccess,
		WeightsConverter weightsConverter)
	{
		_screenshotsDataAccess = screenshotsDataAccess;
		_weightsDataAccess = weightsDataAccess;
		_weightsConverter = weightsConverter;
		_screenshotsConverter = new ScreenshotsConverter(screenshotsDataAccess);
		_assetsConverter = new ClassifierAssetsConverter(screenshotsDataAccess);
	}

	public ClassifierDataSet Convert(Domain.Model.DataSets.Classifier.ClassifierDataSet dataSet, ConversionSession session)
	{
		ClassifierDataSet serializableDataSet = new(
			dataSet,
			GamesConverter.GetGameId(dataSet.Game, session),
			TagsConverter.Convert(dataSet.Tags, session),
			_screenshotsConverter.Convert(dataSet.Screenshots),
			_assetsConverter.Convert(dataSet.Assets, session),
			_weightsConverter.Convert(dataSet.Weights, session));
		return serializableDataSet;
	}

	public Domain.Model.DataSets.Classifier.ClassifierDataSet ConvertBack(
		ClassifierDataSet raw,
		ReverseConversionSession session)
	{
		Guard.IsNotNull(session.Games);
		Domain.Model.DataSets.Classifier.ClassifierDataSet dataSet = new()
		{
			Name = raw.Name,
			Description = raw.Description,
			Resolution = raw.Resolution
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

	private readonly FileSystemScreenshotsDataAccess _screenshotsDataAccess;
	private readonly FileSystemWeightsDataAccess _weightsDataAccess;
	private readonly WeightsConverter _weightsConverter;
	private readonly ScreenshotsConverter _screenshotsConverter;
	private readonly ClassifierAssetsConverter _assetsConverter;

	[UnsafeAccessor(UnsafeAccessorKind.Method)]
	private static extern Weights<TTag> CreateWeights<TTag>(
		WeightsLibrary<TTag> library,
		ModelSize size,
		WeightsMetrics metrics,
		IEnumerable<ClassifierTag> tags)
		where TTag : Domain.Model.DataSets.Tags.Tag, MinimumTagsCount;

	private static void AddTags(Domain.Model.DataSets.Classifier.ClassifierDataSet dataSet, ImmutableArray<Tag> tags, ReverseConversionSession session)
	{
		foreach (var rawTag in tags)
		{
			var tag = dataSet.Tags.CreateTag(rawTag.Name);
			tag.Color = rawTag.Color;
			session.Tags.Add(rawTag.Id, tag);
		}
	}

	private void AddScreenshots(Domain.Model.DataSets.Classifier.ClassifierDataSet dataSet, ImmutableArray<Screenshot> screenshots, ReverseConversionSession session)
	{
		foreach (var rawScreenshot in screenshots)
		{
			var screenshot = dataSet.Screenshots.AddScreenshot(rawScreenshot.CreationDate);
			session.Screenshots.Add(rawScreenshot.Id, screenshot);
			_screenshotsDataAccess.AssociateId(screenshot, rawScreenshot.Id);
		}
	}

	private static void AddAssets(Domain.Model.DataSets.Classifier.ClassifierDataSet dataSet, ImmutableArray<ClassifierAsset> assets, ReverseConversionSession session)
	{
		foreach (var rawAsset in assets)
		{
			var screenshot = (Screenshot<Domain.Model.DataSets.Classifier.ClassifierAsset>)session.Screenshots[rawAsset.ScreenshotId];
			var asset = dataSet.Assets.MakeAsset(screenshot);
			asset.Tag = (ClassifierTag)session.Tags[rawAsset.TagId];
		}
	}

	private void AddWeights(Domain.Model.DataSets.Classifier.ClassifierDataSet dataSet, ImmutableArray<WeightsWithTags> raw, ReverseConversionSession session)
	{
		foreach (var rawWeights in raw)
		{
			var weights = CreateWeights(dataSet.Weights, rawWeights.Size, rawWeights.Metrics, rawWeights.Tags.Select(tagId => (ClassifierTag)session.Tags[tagId]));
			_weightsDataAccess.AssociateId(weights, rawWeights.Id);
			session.Weights.Add(rawWeights.Id, weights);
		}
	}
}