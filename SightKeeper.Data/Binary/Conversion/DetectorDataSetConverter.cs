using System.Collections.Immutable;
using SightKeeper.Data.Binary.Model.DataSets;
using SightKeeper.Data.Binary.Model.DataSets.Assets;
using SightKeeper.Data.Binary.Model.DataSets.Compositions;
using SightKeeper.Data.Binary.Model.DataSets.Tags;
using SightKeeper.Data.Binary.Model.DataSets.Weights;
using SightKeeper.Data.Binary.Services;
using SightKeeper.Domain.Model.DataSets.Assets;
using SightKeeper.Domain.Model.DataSets.Detector;
using SightKeeper.Domain.Model.DataSets.Tags;
using SightKeeper.Domain.Model.DataSets.Weights;

namespace SightKeeper.Data.Binary.Conversion;

internal sealed class DetectorDataSetConverter : DataSetConverter<DetectorDataSet>
{
	public DetectorDataSetConverter(FileSystemScreenshotsDataAccess screenshotsDataAccess) : base(screenshotsDataAccess)
	{
	}

	protected override PackableDetectorDataSet CreatePackableDataSet(
		string name,
		string description,
		ushort? gameId,
		PackableComposition? composition,
		ImmutableArray<PackableScreenshot> screenshots,
		IEnumerable<PackableTag> tags,
		IEnumerable<PackableAsset> assets,
		IEnumerable<PackableWeights> weights)
	{
		return new PackableDetectorDataSet(name, description, gameId, composition, screenshots, tags, assets, weights);
	}

	protected override IEnumerable<PackableTag> ConvertTags(TagsLibrary tags)
	{
		return tags.Select((tag, index) => ConvertPlainTag((byte)index, tag));
	}

	protected override IEnumerable<PackableItemsAsset<PackableDetectorItem>> ConvertAssets(AssetsLibrary assets, Func<Tag, byte> getTagId)
	{
		return ((IReadOnlyCollection<DetectorAsset>)assets).Select(ConvertAsset);
		PackableItemsAsset<PackableDetectorItem> ConvertAsset(DetectorAsset asset) => new(
			asset.Usage,
			ScreenshotsDataAccess.GetId(asset.Screenshot),
			asset.Items.Select(ConvertItem).ToImmutableArray());
		PackableDetectorItem ConvertItem(DetectorItem item) => new(getTagId(item.Tag), item.Bounding);
	}

	protected override IEnumerable<PackablePlainWeights> ConvertWeights(WeightsLibrary weights, Func<Tag, byte> getTagId)
	{
		return ((IReadOnlyCollection<Weights<DetectorTag>>)weights).Select(ConvertWeightsItem);
		PackablePlainWeights ConvertWeightsItem(Weights<DetectorTag> item) =>
			new(item.CreationDate, item.ModelSize, item.Metrics, item.Resolution, ConvertWeightsTags(item.Tags));
		ImmutableArray<byte> ConvertWeightsTags(IEnumerable<Tag> tags) => tags.Select(getTagId).ToImmutableArray();
	}
}