using System.Collections;
using System.Collections.Immutable;
using SightKeeper.Data.Binary.Model.DataSets.Tags;
using SightKeeper.Data.Binary.Model.DataSets.Weights;
using SightKeeper.Data.Binary.Services;
using SightKeeper.Domain.Model.DataSets.Poser;
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
		ConversionSession session,
		ImmutableArray<PackableTag>.Builder builder)
	{
		foreach (var keyPointTag in keyPointTags)
		{
			session.TagsIds.Add(keyPointTag, indexCounter);
			builder.Add(ConvertPlainTag(indexCounter, keyPointTag));
			indexCounter++;
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
		ConversionSession session)
	{
		var resultBuilder = ImmutableArray.CreateBuilder<PackablePoserWeights>();
		foreach (var item in weights.Cast<PoserWeights>())
		{
			resultBuilder.Add(ConvertWeights(session.WeightsIdCounter, item, session));
			session.WeightsIds.Add(item, session.WeightsIdCounter);
			session.WeightsIdCounter++;
		}
		return ImmutableArray<PackableWeights>.CastUp(resultBuilder.DrainToImmutable());
	}
	
	private PackablePoserWeights ConvertWeights(ushort id, PoserWeights item, ConversionSession session)
	{
		return new PackablePoserWeights(
			id,
			item.CreationDate,
			item.ModelSize,
			item.Metrics,
			item.Resolution,
			ConvertWeightsTags(item.Tags, session));
	}

	protected abstract ImmutableDictionary<byte, ImmutableArray<byte>> ConvertWeightsTags(
		IDictionary tags,
		ConversionSession session);
}