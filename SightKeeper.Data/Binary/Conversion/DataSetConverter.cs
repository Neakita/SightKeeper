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

namespace SightKeeper.Data.Binary.Conversion;

internal abstract class DataSetConverter<TDataSet> where TDataSet : DataSet
{
	protected DataSetConverter(FileSystemScreenshotsDataAccess screenshotsDataAccess)
	{
		ScreenshotsDataAccess = screenshotsDataAccess;
	}

	public PackableDataSet Convert(TDataSet dataSet, ConversionSession session)
	{
		var tagsDictionary = ConvertTagsToDictionary(dataSet.Tags);
		Guard.IsNotNull(session.GameIds);

		ushort? gameId = dataSet.Game == null ? null : session.GameIds[dataSet.Game];
		PackableComposition? composition = ConvertComposition(dataSet.Composition);
		ImmutableArray<PackableScreenshot> screenshots = ConvertScreenshots(dataSet.Screenshots);
		IEnumerable<PackableTag> tags = tagsDictionary.Values;
		IEnumerable<PackableAsset> assets = ConvertAssets(dataSet.Assets, GetTagId);
		IEnumerable<PackableWeights> weights = ConvertWeights(dataSet.Weights, GetTagId);

		return CreatePackableDataSet(dataSet.Name, dataSet.Description, gameId, composition, screenshots, tags, assets, weights);

		byte GetTagId(Tag tag) => tagsDictionary[tag].Id;
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
		IEnumerable<PackableTag> tags,
		IEnumerable<PackableAsset> assets,
		IEnumerable<PackableWeights> weights);
	protected abstract IEnumerable<PackableTag> ConvertTags(TagsLibrary tags);
	protected abstract IEnumerable<PackableAsset> ConvertAssets(AssetsLibrary assets, Func<Tag, byte> getTagId);
	protected abstract IEnumerable<PackableWeights> ConvertWeights(WeightsLibrary weights, Func<Tag, byte> getTagId);

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

	private ImmutableDictionary<Tag, PackableTag> ConvertTagsToDictionary(TagsLibrary tagsLibrary)
	{
		return ConvertTags(tagsLibrary).Zip(tagsLibrary).ToImmutableDictionary(tuple => tuple.Second, tuple => tuple.First);
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