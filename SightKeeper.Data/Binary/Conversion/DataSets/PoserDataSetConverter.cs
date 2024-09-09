using System.Collections;
using System.Collections.Immutable;
using CommunityToolkit.Diagnostics;
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

	protected sealed override ImmutableArray<PackableWeights> ConvertWeights(
		IReadOnlyCollection<Weights> weights,
		ConversionSession session,
		ImmutableDictionary<Weights, ushort>.Builder lookupBuilder,
		Func<Tag, byte> getTagId)
	{
		var resultBuilder = ImmutableArray.CreateBuilder<PackablePoserWeights>();
		foreach (var item in weights.Cast<PoserWeights>())
		{
			resultBuilder.Add(ConvertWeights(session.WeightsIdCounter++, item, getTagId));
			lookupBuilder.Add(item, session.WeightsIdCounter++);
		}
		return ImmutableArray<PackableWeights>.CastUp(resultBuilder.DrainToImmutable());
	}
	
	private static PackablePoserWeights ConvertWeights(ushort id, PoserWeights item, Func<Tag, byte> getTagId)
	{
		return new PackablePoserWeights(
			id,
			item.CreationDate,
			item.ModelSize,
			item.Metrics,
			item.Resolution,
			ConvertWeightsTags(item.Tags));

		ImmutableDictionary<byte, ImmutableArray<byte>> ConvertWeightsTags(
			IDictionary tags) =>
			tags.Cast<DictionaryEntry>().ToImmutableDictionary(entry => getTagId((PoserTag)entry.Key), entry =>
			{
				Guard.IsNotNull(entry.Value);
				return ((IReadOnlyCollection<KeyPointTag>)entry.Value).Select(getTagId).ToImmutableArray();
			});
	}
}