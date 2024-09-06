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

internal sealed class DetectorDataSetConverter : DataSetConverter
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
		ImmutableArray<PackableTag> tags,
		ImmutableArray<PackableAsset> assets,
		ImmutableArray<PackableWeights> weights)
	{
		return new PackableDetectorDataSet(
			name,
			description,
			gameId,
			composition,
			screenshots,
			tags,
			assets.CastArray<PackableItemsAsset<PackableDetectorItem>>(),
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
		return assets.Cast<DetectorAsset>().Select(ConvertAsset).ToImmutableArray();
		PackableAsset ConvertAsset(DetectorAsset asset) => new PackableItemsAsset<PackableDetectorItem>(
			asset.Usage,
			ScreenshotsDataAccess.GetId(asset.Screenshot),
			asset.Items.Select(ConvertItem).ToImmutableArray());
		PackableDetectorItem ConvertItem(DetectorItem item) => new(getTagId(item.Tag), item.Bounding);
	}

	protected override ImmutableArray<PackableWeights> ConvertWeights(IReadOnlyCollection<Weights> weights, Func<Tag, byte> getTagId)
	{
		return weights.Cast<Weights<DetectorTag>>().Select(ConvertWeightsItem).ToImmutableArray();
		PackableWeights ConvertWeightsItem(Weights<DetectorTag> item) =>
			new PackablePlainWeights(item.CreationDate, item.ModelSize, item.Metrics, item.Resolution,
				ConvertWeightsTags(item.Tags));
		ImmutableArray<byte> ConvertWeightsTags(IEnumerable<Tag> tags) => tags.Select(getTagId).ToImmutableArray();
	}
}