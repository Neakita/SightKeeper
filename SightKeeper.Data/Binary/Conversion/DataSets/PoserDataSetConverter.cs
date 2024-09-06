using System.Collections.Immutable;
using SightKeeper.Data.Binary.Model.DataSets.Tags;
using SightKeeper.Data.Binary.Model.DataSets.Weights;
using SightKeeper.Data.Binary.Services;
using SightKeeper.Domain.Model.DataSets.Poser;
using SightKeeper.Domain.Model.DataSets.Tags;
using SightKeeper.Domain.Model.DataSets.Weights;

namespace SightKeeper.Data.Binary.Conversion.DataSets;

internal abstract class PoserDataSetConverter : DataSetConverter
{
	protected PoserDataSetConverter(FileSystemScreenshotsDataAccess screenshotsDataAccess) : base(screenshotsDataAccess)
	{
	}

	protected static void BuildKeyPoints(
		IEnumerable<KeyPointTag> keyPointTags,
		ref byte indexCounter,
		ImmutableDictionary<Tag, byte>.Builder lookupBuilder,
		ImmutableArray<PackableTag>.Builder keyPointTagsBuilder)
	{
		foreach (var keyPointTag in keyPointTags)
		{
			var keyPointTagId = indexCounter++;
			lookupBuilder.Add(keyPointTag, keyPointTagId);
			keyPointTagsBuilder.Add(ConvertPlainTag(keyPointTagId, keyPointTag));
		}
	}

	protected static void BuildNumericProperties(
		IEnumerable<NumericItemProperty> properties,
		ImmutableArray<PackableNumericItemProperty>.Builder builder)
	{
		foreach (var property in properties)
		{
			PackableNumericItemProperty convertedProperty = new(
				property.Name,
				property.MinimumValue,
				property.MaximumValue);
			builder.Add(convertedProperty);
		}
	}
	
	protected static PackableWeights ConvertWeightsItem<TTag, TKeyPointTag>(Weights<TTag, TKeyPointTag> item, Func<Tag, byte> getTagId)
		where TTag : PoserTag
		where TKeyPointTag : KeyPointTag<TTag>
	{
		return new PackablePoserWeights(
			item.CreationDate,
			item.ModelSize,
			item.Metrics,
			item.Resolution,
			ConvertWeightsTags(item.Tags));

		ImmutableDictionary<byte, ImmutableArray<byte>> ConvertWeightsTags(
			IReadOnlyDictionary<TTag, ImmutableHashSet<TKeyPointTag>> tags) =>
			tags.ToImmutableDictionary(
				pair => getTagId(pair.Key),
				pair => ConvertWeightsKeyPointTags(pair.Value));

		ImmutableArray<byte> ConvertWeightsKeyPointTags(IEnumerable<KeyPointTag> source) =>
			source.Select(tag => getTagId(tag)).ToImmutableArray();
	}
}