using System.Collections;
using System.Collections.Immutable;
using SightKeeper.Data.Binary.Model.DataSets;
using SightKeeper.Data.Binary.Model.DataSets.Assets;
using SightKeeper.Data.Binary.Model.DataSets.Tags;
using SightKeeper.Data.Binary.Services;
using SightKeeper.Domain.Model.DataSets.Assets;
using SightKeeper.Domain.Model.DataSets.Poser3D;
using SightKeeper.Domain.Model.DataSets.Tags;

namespace SightKeeper.Data.Binary.Conversion.DataSets;

internal sealed class Poser3DDataSetConverter : PoserDataSetConverter<PackablePoser3DTag, PackableItemsAsset<PackablePoser3DItem>, PackablePoser3DDataSet>
{
	public Poser3DDataSetConverter(FileSystemScreenshotsDataAccess screenshotsDataAccess, ConversionSession session) : base(screenshotsDataAccess, session)
	{
	}

	private ImmutableArray<PackablePoser3DTag> ConvertPoserTags(IReadOnlyCollection<Tag> tags)
	{
		var resultBuilder = ImmutableArray.CreateBuilder<PackablePoser3DTag>(tags.Count);
		var keyPointTagsBuilder = ImmutableArray.CreateBuilder<PackableTag>();
		var numericItemPropertiesBuilder = ImmutableArray.CreateBuilder<PackableNumericItemProperty>();
		var booleanItemPropertiesBuilder = ImmutableArray.CreateBuilder<PackableBooleanItemProperty>();
		byte tagCounter = 0;
		foreach (var tag in tags.Cast<Poser3DTag>())
		{
			var tagId = tagCounter++;
			Session.TagsIds.Add(tag, tagId);
			keyPointTagsBuilder.Capacity = tag.KeyPoints.Count;
			numericItemPropertiesBuilder.Capacity = tag.NumericProperties.Count;
			booleanItemPropertiesBuilder.Capacity = tag.BooleanProperties.Count;
			BuildKeyPoints(tag.KeyPoints, ref tagCounter, keyPointTagsBuilder);
			BuildNumericProperties(tag.NumericProperties, numericItemPropertiesBuilder);
			foreach (var property in tag.BooleanProperties)
			{
				PackableBooleanItemProperty convertedProperty = new(
					property.Name);
				booleanItemPropertiesBuilder.Add(convertedProperty);
			}
			PackablePoser3DTag convertedTag = new(
				tagId,
				tag.Name,
				tag.Color,
				keyPointTagsBuilder.DrainToImmutable(),
				numericItemPropertiesBuilder.DrainToImmutable(),
				booleanItemPropertiesBuilder.DrainToImmutable());
			resultBuilder.Add(convertedTag);
		}
		return resultBuilder.ToImmutable();
	}

	protected override ImmutableArray<PackablePoser3DTag> ConvertTags(IReadOnlyCollection<Tag> tags)
	{
		return ConvertPoserTags(tags);
	}

	protected override ImmutableArray<PackableItemsAsset<PackablePoser3DItem>> ConvertAssets(IReadOnlyCollection<Asset> assets)
	{
		return assets.Cast<Poser3DAsset>().Select(ConvertAsset).ToImmutableArray();
		PackableItemsAsset<PackablePoser3DItem> ConvertAsset(Poser3DAsset asset) =>
			new(asset.Usage, ScreenshotsDataAccess.GetId(asset.Screenshot), ConvertItems(asset.Items));
		ImmutableArray<PackablePoser3DItem> ConvertItems(IEnumerable<Poser3DItem> items) => items.Select(ConvertItem).ToImmutableArray();
		PackablePoser3DItem ConvertItem(Poser3DItem item) => new(Session.TagsIds[item.Tag], item.Bounding, ConvertKeyPoints(item.KeyPoints), item.NumericProperties, item.BooleanProperties);
		ImmutableArray<PackableKeyPoint3D> ConvertKeyPoints(IEnumerable<KeyPoint3D> keyPoints) =>
			keyPoints.Select(keyPoint => new PackableKeyPoint3D(keyPoint.Position, keyPoint.IsVisible)).ToImmutableArray();
	}

	protected override ImmutableDictionary<byte, ImmutableArray<byte>> ConvertWeightsTags(IDictionary tags)
	{
		return tags
			.Cast<KeyValuePair<Poser3DTag, ImmutableHashSet<KeyPointTag3D>>>()
			.ToImmutableDictionary(
				pair => Session.TagsIds[pair.Key],
				pair => pair.Value.Select(tag => Session.TagsIds[tag]).ToImmutableArray());
	}
}