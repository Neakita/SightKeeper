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

namespace SightKeeper.Data.Binary.Conversion.DataSets;

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
		ushort? maxScreenshotsWithoutAsset,
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
			maxScreenshotsWithoutAsset,
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
		var convertedAssets = assets.Cast<DetectorAsset>().Select(ConvertAsset).ToImmutableArray();
		return ImmutableArray<PackableAsset>.CastUp(convertedAssets);
		PackableItemsAsset<PackableDetectorItem> ConvertAsset(DetectorAsset asset) => new(
			asset.Usage,
			ScreenshotsDataAccess.GetId(asset.Screenshot),
			asset.Items.Select(ConvertItem).ToImmutableArray());
		PackableDetectorItem ConvertItem(DetectorItem item) => new(getTagId(item.Tag), item.Bounding);
	}
}