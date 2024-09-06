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

namespace SightKeeper.Data.Binary.Conversion;

internal sealed class ClassifierDataSetConverter : DataSetConverter<ClassifierDataSet>
{
	public ClassifierDataSetConverter(FileSystemScreenshotsDataAccess screenshotsDataAccess) : base(screenshotsDataAccess)
	{
	}

	protected override PackableClassifierDataSet CreatePackableDataSet(string name,
		string description,
		ushort? gameId,
		PackableComposition? composition,
		ImmutableArray<PackableScreenshot> screenshots,
		IEnumerable<PackableTag> tags,
		IEnumerable<PackableAsset> assets, IEnumerable<PackableWeights> weights)
	{
		return new PackableClassifierDataSet(name, description, gameId, composition, screenshots, tags, assets, weights);
	}

	protected override IEnumerable<PackableTag> ConvertTags(TagsLibrary tags)
	{
		return tags.Select((tag, index) => ConvertPlainTag((byte)index, tag));
	}

	protected override IEnumerable<PackableClassifierAsset> ConvertAssets(AssetsLibrary assets, Func<Tag, byte> getTagId)
	{
		return ((IReadOnlyCollection<ClassifierAsset>)assets).Select(ConvertAsset);
		PackableClassifierAsset ConvertAsset(ClassifierAsset asset) => new(
			asset.Usage,
			ScreenshotsDataAccess.GetId(asset.Screenshot),
			getTagId(asset.Tag));
	}

	protected override IEnumerable<PackablePlainWeights> ConvertWeights(WeightsLibrary weights, Func<Tag, byte> getTagId)
	{
		return ((IReadOnlyCollection<Weights<ClassifierTag>>)weights).Select(ConvertWeightsItem);
		PackablePlainWeights ConvertWeightsItem(Weights<ClassifierTag> item) =>
			new(item.CreationDate, item.ModelSize, item.Metrics, item.Resolution, ConvertWeightsTags(item.Tags));
		ImmutableArray<byte> ConvertWeightsTags(IEnumerable<Tag> tags) => tags.Select(getTagId).ToImmutableArray();
	}
}