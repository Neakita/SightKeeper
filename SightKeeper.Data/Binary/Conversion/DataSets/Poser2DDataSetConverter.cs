using System.Collections;
using System.Collections.Immutable;
using SightKeeper.Data.Binary.Model.DataSets;
using SightKeeper.Data.Binary.Model.DataSets.Assets;
using SightKeeper.Data.Binary.Model.DataSets.Compositions;
using SightKeeper.Data.Binary.Model.DataSets.Tags;
using SightKeeper.Data.Binary.Model.DataSets.Weights;
using SightKeeper.Data.Binary.Services;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets.Assets;
using SightKeeper.Domain.Model.DataSets.Poser2D;
using SightKeeper.Domain.Model.DataSets.Tags;

namespace SightKeeper.Data.Binary.Conversion.DataSets;

internal sealed class Poser2DDataSetConverter : PoserDataSetConverter
{
	public Poser2DDataSetConverter(FileSystemScreenshotsDataAccess screenshotsDataAccess) : base(screenshotsDataAccess)
	{
	}

	protected override PackablePoser2DDataSet CreatePackableDataSet(
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
		return new PackablePoser2DDataSet(
			name,
			description,
			gameId,
			composition,
			maxScreenshotsWithoutAsset,
			screenshots,
			tags.CastArray<PackablePoser2DTag>(),
			assets.CastArray<PackableItemsAsset<PackablePoser2DItem>>(),
			weights.CastArray<PackablePoserWeights>());
	}

	protected override ImmutableArray<PackableTag> ConvertTags(IReadOnlyCollection<Tag> tags, ConversionSession session)
	{
		var resultBuilder = ImmutableArray.CreateBuilder<PackablePoser2DTag>(tags.Count);
		var keyPointTagsBuilder = ImmutableArray.CreateBuilder<PackableTag>();
		var numericItemPropertiesBuilder = ImmutableArray.CreateBuilder<PackableNumericItemProperty>();
		byte tagCounter = 0;
		foreach (var tag in tags.Cast<Poser2DTag>())
		{
			var tagId = tagCounter++;
			session.TagsIds.Add(tag, tagId);
			keyPointTagsBuilder.Capacity = tag.KeyPoints.Count;
			numericItemPropertiesBuilder.Capacity = tag.Properties.Count;
			BuildKeyPoints(tag.KeyPoints, ref tagCounter, session, keyPointTagsBuilder);
			BuildNumericProperties(tag.Properties, numericItemPropertiesBuilder);
			PackablePoser2DTag convertedTag = new(
				tagId,
				tag.Name,
				tag.Color,
				keyPointTagsBuilder.DrainToImmutable(),
				numericItemPropertiesBuilder.DrainToImmutable());
			resultBuilder.Add(convertedTag);
		}
		return ImmutableArray<PackableTag>.CastUp(resultBuilder.ToImmutable());
	}

	protected override ImmutableArray<PackableAsset> ConvertAssets(IReadOnlyCollection<Asset> assets, ConversionSession session)
	{
		var convertedAssets = assets.Cast<Poser2DAsset>().Select(ConvertAsset).ToImmutableArray();
		return ImmutableArray<PackableAsset>.CastUp(convertedAssets);
		PackableItemsAsset<PackablePoser2DItem> ConvertAsset(Poser2DAsset asset) =>
			new(asset.Usage, ScreenshotsDataAccess.GetId(asset.Screenshot), ConvertItems(asset.Items));
		ImmutableArray<PackablePoser2DItem> ConvertItems(IEnumerable<Poser2DItem> items) => items.Select(ConvertItem).ToImmutableArray();
		PackablePoser2DItem ConvertItem(Poser2DItem item) => new(session.TagsIds[item.Tag], item.Bounding, ConvertKeyPoints(item.KeyPoints), item.Properties);
		ImmutableArray<PackableKeyPoint2D> ConvertKeyPoints(IEnumerable<Vector2<double>> keyPoints) =>
			keyPoints.Select(position => new PackableKeyPoint2D(position)).ToImmutableArray();
	}

	protected override ImmutableDictionary<byte, ImmutableArray<byte>> ConvertWeightsTags(IDictionary tags, ConversionSession session)
	{
		return tags
			.Cast<KeyValuePair<Poser2DTag, ImmutableHashSet<KeyPointTag2D>>>()
			.ToImmutableDictionary(
				pair => session.TagsIds[pair.Key],
				pair => pair.Value.Select(tag => session.TagsIds[tag]).ToImmutableArray());
	}
}