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
using SightKeeper.Domain.Model.DataSets.Weights;

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
			screenshots,
			tags.CastArray<PackablePoser3DTag>(),
			assets.CastArray<PackableItemsAsset<PackablePoser3DItem>>(),
			weights.CastArray<PackablePoserWeights>());
	}

	protected override ImmutableArray<PackableTag> ConvertTags(IReadOnlyCollection<Tag> tags, out ImmutableDictionary<Tag, byte> lookup)
	{
		var lookupBuilder = ImmutableDictionary.CreateBuilder<Tag, byte>();
		var resultBuilder = ImmutableArray.CreateBuilder<PackableTag>(tags.Count);
		var keyPointTagsBuilder = ImmutableArray.CreateBuilder<PackableTag>();
		var numericItemPropertiesBuilder = ImmutableArray.CreateBuilder<PackableNumericItemProperty>();
		var booleanItemPropertiesBuilder = ImmutableArray.CreateBuilder<PackableBooleanItemProperty>();
		byte tagIndex = 0;
		foreach (var tag in tags.Cast<Poser3DTag>())
		{
			keyPointTagsBuilder.Capacity = tag.KeyPoints.Count;
			numericItemPropertiesBuilder.Capacity = tag.NumericProperties.Count;
			booleanItemPropertiesBuilder.Capacity = tag.BooleanProperties.Count;
			byte keyPointTagIndex = 0;
			foreach (var keyPointTag in tag.KeyPoints)
			{
				var keyPointTagId = keyPointTagIndex++;
				lookupBuilder.Add(keyPointTag, keyPointTagId);
				keyPointTagsBuilder.Add(ConvertPlainTag(keyPointTagId, keyPointTag));
			}
			foreach (var property in tag.NumericProperties)
			{
				PackableNumericItemProperty convertedProperty = new(
					property.Name,
					property.MinimumValue,
					property.MaximumValue);
				numericItemPropertiesBuilder.Add(convertedProperty);
			}
			foreach (var property in tag.BooleanProperties)
			{
				PackableBooleanItemProperty convertedProperty = new(
					property.Name);
				booleanItemPropertiesBuilder.Add(convertedProperty);
			}
			var tagId = tagIndex++;
			PackablePoser2DTag convertedTag = new(
				tagId,
				tag.Name,
				tag.Color,
				keyPointTagsBuilder.DrainToImmutable(),
				numericItemPropertiesBuilder.DrainToImmutable());
			resultBuilder.Add(convertedTag);
			lookupBuilder.Add(tag, tagId);
		}
		lookup = lookupBuilder.ToImmutable();
		return resultBuilder.ToImmutable();
	}

	protected override ImmutableArray<PackableAsset> ConvertAssets(IReadOnlyCollection<Asset> assets, Func<Tag, byte> getTagId)
	{
		return assets.Cast<Poser3DAsset>().Select(ConvertAsset).ToImmutableArray();
		PackableAsset ConvertAsset(Poser3DAsset asset) =>
			new PackableItemsAsset<PackablePoser3DItem>(asset.Usage, ScreenshotsDataAccess.GetId(asset.Screenshot), ConvertItems(asset.Items));
		ImmutableArray<PackablePoser3DItem> ConvertItems(IEnumerable<Poser3DItem> items) => items.Select(ConvertItem).ToImmutableArray();
		PackablePoser3DItem ConvertItem(Poser3DItem item) => new(getTagId(item.Tag), item.Bounding, ConvertKeyPoints(item.KeyPoints), item.NumericProperties, item.BooleanProperties);
		ImmutableArray<PackableKeyPoint3D> ConvertKeyPoints(IEnumerable<KeyPoint3D> keyPoints) =>
			keyPoints.Select(keyPoint => new PackableKeyPoint3D(keyPoint.Position, keyPoint.IsVisible)).ToImmutableArray();
	}

	protected override ImmutableArray<PackableWeights> ConvertWeights(IReadOnlyCollection<Weights> weights, Func<Tag, byte> getTagId)
	{
		return weights.Cast<Weights<Poser3DTag, KeyPointTag3D>>().Select(ConvertWeightsItem).ToImmutableArray();
		PackableWeights ConvertWeightsItem(Weights<Poser3DTag, KeyPointTag3D> item) =>
			new PackablePoserWeights(item.CreationDate, item.ModelSize, item.Metrics, item.Resolution,
				ConvertWeightsTags(item.Tags));
		ImmutableDictionary<byte, ImmutableArray<byte>> ConvertWeightsTags(
			IReadOnlyDictionary<Poser3DTag, ImmutableHashSet<KeyPointTag3D>> tags) =>
			tags.ToImmutableDictionary(
				pair => getTagId(pair.Key),
				pair => ConvertWeightsKeyPointTags(pair.Value));
		ImmutableArray<byte> ConvertWeightsKeyPointTags(IEnumerable<KeyPointTag3D> tags) =>
			tags.Select(tag => getTagId(tag)).ToImmutableArray();
	}
}