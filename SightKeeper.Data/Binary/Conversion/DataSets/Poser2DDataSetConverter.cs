using System.Collections;
using System.Collections.Immutable;
using SightKeeper.Data.Binary.Model.DataSets;
using SightKeeper.Data.Binary.Model.DataSets.Assets;
using SightKeeper.Data.Binary.Model.DataSets.Tags;
using SightKeeper.Data.Binary.Services;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Model.DataSets.Assets;
using SightKeeper.Domain.Model.DataSets.Poser2D;
using SightKeeper.Domain.Model.DataSets.Tags;

namespace SightKeeper.Data.Binary.Conversion.DataSets;

internal sealed class Poser2DDataSetConverter : PoserDataSetConverter<PackablePoser2DDataSet>
{
	public Poser2DDataSetConverter(FileSystemScreenshotsDataAccess screenshotsDataAccess, ConversionSession session) : base(screenshotsDataAccess, session)
	{
	}

	public override PackablePoser2DDataSet Convert(DataSet dataSet)
	{
		var packable = base.Convert(dataSet);
		packable.Tags = ConvertTags(dataSet.TagsLibrary.Tags);
		packable.Assets = ConvertAssets(dataSet.AssetsLibrary.Assets);
		packable.Weights = ConvertPoserWeights(dataSet.WeightsLibrary.Weights);
		return packable;
	}

	private ImmutableArray<PackablePoser2DTag> ConvertTags(IReadOnlyCollection<Tag> tags)
	{
		var resultBuilder = ImmutableArray.CreateBuilder<PackablePoser2DTag>(tags.Count);
		var keyPointTagsBuilder = ImmutableArray.CreateBuilder<PackableTag>();
		var numericItemPropertiesBuilder = ImmutableArray.CreateBuilder<PackableNumericItemProperty>();
		byte tagCounter = 0;
		foreach (var tag in tags.Cast<Poser2DTag>())
		{
			var tagId = tagCounter++;
			Session.TagsIds.Add(tag, tagId);
			keyPointTagsBuilder.Capacity = tag.KeyPoints.Count;
			numericItemPropertiesBuilder.Capacity = tag.Properties.Count;
			BuildKeyPoints(tag.KeyPoints, ref tagCounter, keyPointTagsBuilder);
			BuildNumericProperties(tag.Properties, numericItemPropertiesBuilder);
			PackablePoser2DTag convertedTag = new(
				tagId,
				tag.Name,
				tag.Color,
				keyPointTagsBuilder.DrainToImmutable(),
				numericItemPropertiesBuilder.DrainToImmutable());
			resultBuilder.Add(convertedTag);
		}
		return resultBuilder.DrainToImmutable();
	}

	private ImmutableArray<PackableItemsAsset<PackablePoser2DItem>> ConvertAssets(IReadOnlyCollection<Asset> assets)
	{
		return assets.Cast<Poser2DAsset>().Select(ConvertAsset).ToImmutableArray();
		PackableItemsAsset<PackablePoser2DItem> ConvertAsset(Poser2DAsset asset) =>
			new(asset.Usage, ScreenshotsDataAccess.GetId(asset.Screenshot), ConvertItems(asset.Items));
		ImmutableArray<PackablePoser2DItem> ConvertItems(IEnumerable<Poser2DItem> items) => items.Select(ConvertItem).ToImmutableArray();
		PackablePoser2DItem ConvertItem(Poser2DItem item) => new(Session.TagsIds[item.Tag], item.Bounding, ConvertKeyPoints(item.KeyPoints), item.Properties);
		ImmutableArray<PackableKeyPoint2D> ConvertKeyPoints(IEnumerable<Vector2<double>> keyPoints) =>
			keyPoints.Select(position => new PackableKeyPoint2D(position)).ToImmutableArray();
	}

	protected override ImmutableDictionary<byte, ImmutableArray<byte>> ConvertWeightsTags(IDictionary tags)
	{
		return tags
			.Cast<KeyValuePair<Poser2DTag, ImmutableHashSet<KeyPointTag2D>>>()
			.ToImmutableDictionary(
				pair => Session.TagsIds[pair.Key],
				pair => pair.Value.Select(tag => Session.TagsIds[tag]).ToImmutableArray());
	}
}