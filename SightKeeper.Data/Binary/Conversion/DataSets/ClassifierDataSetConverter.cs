using System.Collections.Immutable;
using SightKeeper.Data.Binary.Model.DataSets;
using SightKeeper.Data.Binary.Model.DataSets.Assets;
using SightKeeper.Data.Binary.Model.DataSets.Compositions;
using SightKeeper.Data.Binary.Model.DataSets.Tags;
using SightKeeper.Data.Binary.Model.DataSets.Weights;
using SightKeeper.Data.Binary.Services;
using SightKeeper.Domain.Model.DataSets.Assets;
using SightKeeper.Domain.Model.DataSets.Classifier;
using SightKeeper.Domain.Model.DataSets.Tags;
using SightKeeper.Domain.Model.DataSets.Weights;

namespace SightKeeper.Data.Binary.Conversion.DataSets;

internal sealed class ClassifierDataSetConverter : DataSetConverter
{
	public ClassifierDataSetConverter(FileSystemScreenshotsDataAccess screenshotsDataAccess) : base(screenshotsDataAccess)
	{
	}

	protected override PackableClassifierDataSet CreatePackableDataSet(
		string name,
		string description,
		ushort? gameId,
		PackableComposition? composition,
		ImmutableArray<PackableScreenshot> screenshots,
		ImmutableArray<PackableTag> tags,
		ImmutableArray<PackableAsset> assets,
		ImmutableArray<PackableWeights> weights)
	{
		return new PackableClassifierDataSet(
			name,
			description,
			gameId,
			composition,
			screenshots,
			tags,
			assets.CastArray<PackableClassifierAsset>(),
			weights.CastArray<PackablePlainWeights>());
	}

	protected override ImmutableArray<PackableTag> ConvertTags(IReadOnlyCollection<Tag> tags, out ImmutableDictionary<Tag, byte> lookup)
	{
		lookup = tags.Select((tag, index) => (tag, index))
			.ToImmutableDictionary(tuple => tuple.tag, tuple => (byte)tuple.index);
		return tags.Select((tag, index) => ConvertPlainTag((byte)index, tag)).ToImmutableArray();
	}

	protected override ImmutableArray<PackableAsset> ConvertAssets(IReadOnlyCollection<Asset> assets, Func<Tag, byte> getTagId)
	{
		return assets.Cast<ClassifierAsset>().Select(ConvertAsset).ToImmutableArray();
		PackableAsset ConvertAsset(ClassifierAsset asset) => new PackableClassifierAsset(
			asset.Usage,
			ScreenshotsDataAccess.GetId(asset.Screenshot),
			getTagId(asset.Tag));
	}

	protected override ImmutableArray<PackableWeights> ConvertWeights(IReadOnlyCollection<Weights> weights, Func<Tag, byte> getTagId)
	{
		return weights.Cast<Weights<ClassifierTag>>().Select(ConvertWeightsItem).ToImmutableArray();
		PackableWeights ConvertWeightsItem(Weights<ClassifierTag> item) =>
			new PackablePlainWeights(item.CreationDate, item.ModelSize, item.Metrics, item.Resolution, ConvertWeightsTags(item.Tags));
		ImmutableArray<byte> ConvertWeightsTags(IEnumerable<Tag> tags) => tags.Select(getTagId).ToImmutableArray();
	}
}