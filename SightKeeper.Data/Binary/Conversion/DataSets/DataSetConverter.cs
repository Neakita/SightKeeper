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

	public PackableDataSet Convert(DataSet dataSet, ConversionSession session)
	{
		Guard.IsNotNull(session.GameIds);

		ushort? gameId = dataSet.Game == null ? null : session.GameIds[dataSet.Game];
		PackableComposition? composition = ConvertComposition(dataSet.Composition);
		ImmutableArray<PackableScreenshot> screenshots = ConvertScreenshots(dataSet.Screenshots);
		ImmutableArray<PackableTag> tags = ConvertTags(dataSet.Tags, out var lookup);
		ImmutableArray<PackableAsset> assets = ConvertAssets(dataSet.Assets, GetTagId);
		ImmutableArray<PackableWeights> weights = ConvertWeights(dataSet.Weights, GetTagId);
		return CreatePackableDataSet(dataSet.Name, dataSet.Description, gameId, composition, screenshots, tags, assets, weights);

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
		ImmutableArray<PackableScreenshot> screenshots,
		ImmutableArray<PackableTag> tags,
		ImmutableArray<PackableAsset> assets,
		ImmutableArray<PackableWeights> weights);

	protected abstract ImmutableArray<PackableTag> ConvertTags(IReadOnlyCollection<Tag> tags, out ImmutableDictionary<Tag, byte> lookup);
	protected abstract ImmutableArray<PackableAsset> ConvertAssets(IReadOnlyCollection<Asset> assets, Func<Tag, byte> getTagId);
	protected abstract ImmutableArray<PackableWeights> ConvertWeights(IReadOnlyCollection<Weights> weights, Func<Tag, byte> getTagId);
	protected static PackablePlainWeights ConvertWeights(Weights item, ImmutableArray<byte> tagIds) =>
		new(item.CreationDate, item.ModelSize, item.Metrics, item.Resolution, tagIds);

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

	private ImmutableArray<PackableScreenshot> ConvertScreenshots(ScreenshotsLibrary screenshots)
	{
		return screenshots.Select(ConvertScreenshot).ToImmutableArray();
		PackableScreenshot ConvertScreenshot(Screenshot screenshot) => new(
			ScreenshotsDataAccess.GetId(screenshot),
			screenshot.CreationDate,
			screenshot.Resolution);
	}
}