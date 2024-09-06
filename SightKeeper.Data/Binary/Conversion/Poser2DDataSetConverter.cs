using System.Collections.Immutable;
using SightKeeper.Data.Binary.Model.DataSets;
using SightKeeper.Data.Binary.Model.DataSets.Assets;
using SightKeeper.Data.Binary.Model.DataSets.Compositions;
using SightKeeper.Data.Binary.Model.DataSets.Tags;
using SightKeeper.Data.Binary.Model.DataSets.Weights;
using SightKeeper.Data.Binary.Services;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets.Assets;
using SightKeeper.Domain.Model.DataSets.Poser;
using SightKeeper.Domain.Model.DataSets.Poser2D;
using SightKeeper.Domain.Model.DataSets.Tags;
using SightKeeper.Domain.Model.DataSets.Weights;

namespace SightKeeper.Data.Binary.Conversion;

internal sealed class Poser2DDataSetConverter : DataSetConverter<Poser2DDataSet>
{
	public Poser2DDataSetConverter(FileSystemScreenshotsDataAccess screenshotsDataAccess) : base(screenshotsDataAccess)
	{
	}

	protected override PackablePoser2DDataSet CreatePackableDataSet(
		string name,
		string description,
		ushort? gameId,
		PackableComposition? composition,
		ImmutableArray<PackableScreenshot> screenshots,
		IEnumerable<PackableTag> tags,
		IEnumerable<PackableAsset> assets,
		IEnumerable<PackableWeights> weights)
	{
		return new PackablePoser2DDataSet(name, description, gameId, composition, screenshots, tags, assets, weights);
	}

	protected override IEnumerable<PackablePoser2DTag> ConvertTags(TagsLibrary tags)
	{
		return ((IReadOnlyCollection<Poser2DTag>)tags).Select((tag, index) => ConvertTag((byte)index, tag));
		PackablePoser2DTag ConvertTag(byte id, Poser2DTag tag) => new(
			id,
			tag.Name,
			tag.Color,
			ConvertKeyPointTags(tag.KeyPoints),
			ConvertNumericItemProperties(tag.Properties));
		ImmutableArray<PackableTag> ConvertKeyPointTags(IEnumerable<KeyPointTag> keyPointTags) =>
			keyPointTags.Select((tag, index) => ConvertKeyPointTag((byte)index, tag)).ToImmutableArray();
		PackableTag ConvertKeyPointTag(byte id, KeyPointTag tag) => new(id, tag.Name, tag.Color);
		ImmutableArray<PackableNumericItemProperty> ConvertNumericItemProperties(
			IEnumerable<NumericItemProperty> properties) =>
			properties.Select(ConvertNumericItemProperty).ToImmutableArray();
		PackableNumericItemProperty ConvertNumericItemProperty(NumericItemProperty property) =>
			new(property.Name, property.MinimumValue, property.MaximumValue);
	}

	protected override IEnumerable<PackableItemsAsset<PackablePoser2DItem>> ConvertAssets(AssetsLibrary assets, Func<Tag, byte> getTagId)
	{
		return ((IReadOnlyCollection<Poser2DAsset>)assets).Select(ConvertAsset);
		PackableItemsAsset<PackablePoser2DItem> ConvertAsset(Poser2DAsset asset) => new(
			asset.Usage,
			ScreenshotsDataAccess.GetId(asset.Screenshot),
			ConvertItems(asset.Items));
		ImmutableArray<PackablePoser2DItem> ConvertItems(IEnumerable<Poser2DItem> items) => items.Select(ConvertItem).ToImmutableArray();
		PackablePoser2DItem ConvertItem(Poser2DItem item) => new(getTagId(item.Tag), item.Bounding, ConvertKeyPoints(item.KeyPoints));
		ImmutableArray<PackableKeyPoint2D> ConvertKeyPoints(IEnumerable<Vector2<double>> keyPoints) =>
			keyPoints.Select(position => new PackableKeyPoint2D(position)).ToImmutableArray();
	}

	protected override IEnumerable<PackablePoserWeights> ConvertWeights(WeightsLibrary weights, Func<Tag, byte> getTagId)
	{
		return ((IReadOnlyCollection<Weights<Poser2DTag, KeyPointTag2D>>)weights).Select(ConvertWeightsItem);
		PackablePoserWeights ConvertWeightsItem(Weights<Poser2DTag, KeyPointTag2D> item) => new(
			item.CreationDate,
			item.ModelSize,
			item.Metrics,
			item.Resolution,
			ConvertWeightsTags(item.Tags));
		ImmutableDictionary<byte, ImmutableArray<byte>> ConvertWeightsTags(
			IReadOnlyDictionary<Poser2DTag, ImmutableHashSet<KeyPointTag2D>> tags) =>
			tags.ToImmutableDictionary(
				pair => getTagId(pair.Key),
				pair => ConvertKeyPointTags(pair.Value));
		ImmutableArray<byte> ConvertKeyPointTags(IEnumerable<KeyPointTag2D> tags) =>
			tags.Select(tag => getTagId(tag)).ToImmutableArray();
	}
}