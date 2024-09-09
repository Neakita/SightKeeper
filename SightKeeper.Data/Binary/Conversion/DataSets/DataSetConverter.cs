using System.Collections.Immutable;
using CommunityToolkit.Diagnostics;
using SightKeeper.Data.Binary.Model.DataSets;
using SightKeeper.Data.Binary.Model.DataSets.Assets;
using SightKeeper.Data.Binary.Model.DataSets.Compositions;
using SightKeeper.Data.Binary.Model.DataSets.Tags;
using SightKeeper.Data.Binary.Model.DataSets.Weights;
using SightKeeper.Data.Binary.Services;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Model.DataSets.Assets;
using SightKeeper.Domain.Model.DataSets.Screenshots;
using SightKeeper.Domain.Model.DataSets.Tags;
using SightKeeper.Domain.Model.DataSets.Weights;

namespace SightKeeper.Data.Binary.Conversion.DataSets;

internal abstract class DataSetConverter
{
	protected DataSetConverter(FileSystemScreenshotsDataAccess screenshotsDataAccess)
	{
		ScreenshotsDataAccess = screenshotsDataAccess;
	}

	public PackableDataSet Convert(
		DataSet dataSet,
		ConversionSession session,
		ImmutableDictionary<Weights, ushort>.Builder weightsLookupBuilder)
	{
		Guard.IsNotNull(session.GameIds);

		ushort? gameId = dataSet.Game == null ? null : session.GameIds[dataSet.Game];
		PackableComposition? composition = ConvertComposition(dataSet.Composition);
		ImmutableArray<PackableScreenshot> screenshots = ConvertScreenshots(dataSet.ScreenshotsLibrary);
		ImmutableArray<PackableTag> tags = ConvertTags(dataSet.TagsLibrary.Tags, out var lookup);
		ImmutableArray<PackableAsset> assets = ConvertAssets(dataSet.AssetsLibrary.Assets, GetTagId);
		ImmutableArray<PackableWeights> weights = ConvertWeights(dataSet.WeightsLibrary.Weights, session, weightsLookupBuilder, GetTagId);
		return CreatePackableDataSet(
			dataSet.Name,
			dataSet.Description,
			gameId,
			composition,
			dataSet.ScreenshotsLibrary.MaxQuantity,
			screenshots,
			tags,
			assets,
			weights);

		byte GetTagId(Tag tag) => lookup[tag];
	}

	protected FileSystemScreenshotsDataAccess ScreenshotsDataAccess { get; }

	protected static PackableTag ConvertPlainTag(byte id, Tag tag)
	{
		return new PackableTag(id, tag.Name, tag.Color);
	}

	protected abstract PackableDataSet CreatePackableDataSet(string name,
		string description,
		ushort? gameId,
		PackableComposition? composition,
		ushort? maxScreenshotsWithoutAsset,
		ImmutableArray<PackableScreenshot> screenshots,
		ImmutableArray<PackableTag> tags,
		ImmutableArray<PackableAsset> assets,
		ImmutableArray<PackableWeights> weights);

	protected abstract ImmutableArray<PackableTag> ConvertTags(IReadOnlyCollection<Tag> tags, out ImmutableDictionary<Tag, byte> lookup);
	protected abstract ImmutableArray<PackableAsset> ConvertAssets(IReadOnlyCollection<Asset> assets, Func<Tag, byte> getTagId);

	protected virtual ImmutableArray<PackableWeights> ConvertWeights(
		IReadOnlyCollection<Weights> weights,
		ConversionSession session,
		ImmutableDictionary<Weights, ushort>.Builder lookupBuilder,
		Func<Tag, byte> getTagId)
	{
		var resultBuilder = ImmutableArray.CreateBuilder<PackableWeights>(weights.Count);
		foreach (var item in weights.Cast<PlainWeights>())
		{
			var id = session.WeightsIdCounter++;
			resultBuilder.Add(ConvertWeights(id, item, ConvertWeightsTags(item.Tags)));
			lookupBuilder.Add(item, id);
		}
		return ImmutableArray<PackableWeights>.CastUp(resultBuilder.DrainToImmutable());
		ImmutableArray<byte> ConvertWeightsTags(IEnumerable<Tag> tags) => tags.Select(getTagId).ToImmutableArray();
	}

	private static PackablePlainWeights ConvertWeights(ushort id, Weights item, ImmutableArray<byte> tagIds) =>
		new(id, item.CreationDate, item.ModelSize, item.Metrics, item.Resolution, tagIds);

	private static PackableTransparentComposition? ConvertComposition(Composition? composition)
	{
		return composition switch
		{
			null => null,
			TransparentComposition transparentComposition => new PackableTransparentComposition(
				transparentComposition.MaximumScreenshotsDelay,
				transparentComposition.Opacities),
			_ => throw new ArgumentOutOfRangeException()
		};
	}

	private ImmutableArray<PackableScreenshot> ConvertScreenshots(ScreenshotsLibrary library)
	{
		return library.Screenshots.Select(ConvertScreenshot).ToImmutableArray();
		PackableScreenshot ConvertScreenshot(Screenshot screenshot) => new(
			ScreenshotsDataAccess.GetId(screenshot),
			screenshot.CreationDate,
			screenshot.Resolution);
	}
}