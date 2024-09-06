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
using SightKeeper.Domain.Model.DataSets.Weights;

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
			screenshots,
			tags.CastArray<PackablePoser2DTag>(),
			assets.CastArray<PackableItemsAsset<PackablePoser2DItem>>(),
			weights.CastArray<PackablePoserWeights>());
	}

	protected override ImmutableArray<PackableTag> ConvertTags(IReadOnlyCollection<Tag> tags, out ImmutableDictionary<Tag, byte> lookup)
	{
		var lookupBuilder = ImmutableDictionary.CreateBuilder<Tag, byte>();
		var resultBuilder = ImmutableArray.CreateBuilder<PackableTag>(tags.Count);
		var keyPointTagsBuilder = ImmutableArray.CreateBuilder<PackableTag>();
		var numericItemPropertiesBuilder = ImmutableArray.CreateBuilder<PackableNumericItemProperty>();
		byte tagIndex = 0;
		foreach (var tag in tags.Cast<Poser2DTag>())
		{
			keyPointTagsBuilder.Capacity = tag.KeyPoints.Count;
			numericItemPropertiesBuilder.Capacity = tag.Properties.Count;
			byte keyPointTagIndex = 0;
			foreach (var keyPointTag in tag.KeyPoints)
			{
				var keyPointTagId = keyPointTagIndex++;
				lookupBuilder.Add(keyPointTag, keyPointTagId);
				keyPointTagsBuilder.Add(ConvertPlainTag(keyPointTagId, keyPointTag));
			}
			foreach (var property in tag.Properties)
			{
				PackableNumericItemProperty convertedProperty = new(
					property.Name,
					property.MinimumValue,
					property.MaximumValue);
				numericItemPropertiesBuilder.Add(convertedProperty);
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
		return assets.Cast<Poser2DAsset>().Select(ConvertAsset).ToImmutableArray();
		PackableAsset ConvertAsset(Poser2DAsset asset) =>
			new PackableItemsAsset<PackablePoser2DItem>(asset.Usage, ScreenshotsDataAccess.GetId(asset.Screenshot),
				ConvertItems(asset.Items));
		ImmutableArray<PackablePoser2DItem> ConvertItems(IEnumerable<Poser2DItem> items) => items.Select(ConvertItem).ToImmutableArray();
		PackablePoser2DItem ConvertItem(Poser2DItem item) => new(getTagId(item.Tag), item.Bounding, ConvertKeyPoints(item.KeyPoints), item.Properties);
		ImmutableArray<PackableKeyPoint2D> ConvertKeyPoints(IEnumerable<Vector2<double>> keyPoints) =>
			keyPoints.Select(position => new PackableKeyPoint2D(position)).ToImmutableArray();
	}

	protected override ImmutableArray<PackableWeights> ConvertWeights(IReadOnlyCollection<Weights> weights, Func<Tag, byte> getTagId)
	{
		return weights.Cast<Weights<Poser2DTag, KeyPointTag2D>>().Select(ConvertWeightsItem).ToImmutableArray();
		PackableWeights ConvertWeightsItem(Weights<Poser2DTag, KeyPointTag2D> item) =>
			new PackablePoserWeights(item.CreationDate, item.ModelSize, item.Metrics, item.Resolution,
				ConvertWeightsTags(item.Tags));
		ImmutableDictionary<byte, ImmutableArray<byte>> ConvertWeightsTags(
			IReadOnlyDictionary<Poser2DTag, ImmutableHashSet<KeyPointTag2D>> tags) =>
			tags.ToImmutableDictionary(
				pair => getTagId(pair.Key),
				pair => ConvertWeightsKeyPointTags(pair.Value));
		ImmutableArray<byte> ConvertWeightsKeyPointTags(IEnumerable<KeyPointTag2D> tags) =>
			tags.Select(tag => getTagId(tag)).ToImmutableArray();
	}
}