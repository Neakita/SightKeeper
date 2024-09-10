using System.Collections;
using System.Collections.Immutable;
using SightKeeper.Data.Binary.Model.DataSets;
using SightKeeper.Data.Binary.Model.DataSets.Assets;
using SightKeeper.Data.Binary.Model.DataSets.Compositions;
using SightKeeper.Data.Binary.Model.DataSets.Tags;
using SightKeeper.Data.Binary.Model.DataSets.Weights;
using SightKeeper.Data.Binary.Services;
using SightKeeper.Domain.Model.DataSets.Assets;
using SightKeeper.Domain.Model.DataSets.Poser3D;
using SightKeeper.Domain.Model.DataSets.Tags;

namespace SightKeeper.Data.Binary.Conversion.DataSets;

internal sealed class Poser3DDataSetConverter : PoserDataSetConverter
{
	public Poser3DDataSetConverter(FileSystemScreenshotsDataAccess screenshotsDataAccess) : base(screenshotsDataAccess)
	{
	}

	protected override PackablePoser3DDataSet CreatePackableDataSet(
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
		return new PackablePoser3DDataSet(
			name,
			description,
			gameId,
			composition,
			maxScreenshotsWithoutAsset,
			screenshots,
			tags.CastArray<PackablePoser3DTag>(),
			assets.CastArray<PackableItemsAsset<PackablePoser3DItem>>(),
			weights.CastArray<PackablePoserWeights>());
	}

	protected override ImmutableArray<PackableTag> ConvertTags(IReadOnlyCollection<Tag> tags, ConversionSession session)
	{
		var resultBuilder = ImmutableArray.CreateBuilder<PackablePoser3DTag>(tags.Count);
		var keyPointTagsBuilder = ImmutableArray.CreateBuilder<PackableTag>();
		var numericItemPropertiesBuilder = ImmutableArray.CreateBuilder<PackableNumericItemProperty>();
		var booleanItemPropertiesBuilder = ImmutableArray.CreateBuilder<PackableBooleanItemProperty>();
		byte tagCounter = 0;
		foreach (var tag in tags.Cast<Poser3DTag>())
		{
			var tagId = tagCounter++;
			session.TagsIds.Add(tag, tagId);
			keyPointTagsBuilder.Capacity = tag.KeyPoints.Count;
			numericItemPropertiesBuilder.Capacity = tag.NumericProperties.Count;
			booleanItemPropertiesBuilder.Capacity = tag.BooleanProperties.Count;
			BuildKeyPoints(tag.KeyPoints, ref tagCounter, session, keyPointTagsBuilder);
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
		return ImmutableArray<PackableTag>.CastUp(resultBuilder.ToImmutable());
	}

	protected override ImmutableArray<PackableAsset> ConvertAssets(IReadOnlyCollection<Asset> assets, ConversionSession session)
	{
		var convertedAssets = assets.Cast<Poser3DAsset>().Select(ConvertAsset).ToImmutableArray();
		return ImmutableArray<PackableAsset>.CastUp(convertedAssets);
		PackableItemsAsset<PackablePoser3DItem> ConvertAsset(Poser3DAsset asset) =>
			new(asset.Usage, ScreenshotsDataAccess.GetId(asset.Screenshot), ConvertItems(asset.Items));
		ImmutableArray<PackablePoser3DItem> ConvertItems(IEnumerable<Poser3DItem> items) => items.Select(ConvertItem).ToImmutableArray();
		PackablePoser3DItem ConvertItem(Poser3DItem item) => new(session.TagsIds[item.Tag], item.Bounding, ConvertKeyPoints(item.KeyPoints), item.NumericProperties, item.BooleanProperties);
		ImmutableArray<PackableKeyPoint3D> ConvertKeyPoints(IEnumerable<KeyPoint3D> keyPoints) =>
			keyPoints.Select(keyPoint => new PackableKeyPoint3D(keyPoint.Position, keyPoint.IsVisible)).ToImmutableArray();
	}

	protected override ImmutableDictionary<byte, ImmutableArray<byte>> ConvertWeightsTags(IDictionary tags, ConversionSession session)
	{
		return tags
			.Cast<KeyValuePair<Poser3DTag, ImmutableHashSet<KeyPointTag3D>>>()
			.ToImmutableDictionary(
				pair => session.TagsIds[pair.Key],
				pair => pair.Value.Select(tag => session.TagsIds[tag]).ToImmutableArray());
	}
}