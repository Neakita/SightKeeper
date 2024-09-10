using System.Collections;
using System.Collections.Immutable;
using SightKeeper.Data.Binary.Model.DataSets;
using SightKeeper.Data.Binary.Model.DataSets.Tags;
using SightKeeper.Data.Binary.Model.DataSets.Weights;
using SightKeeper.Data.Binary.Services;
using SightKeeper.Domain.Model.DataSets.Poser;
using SightKeeper.Domain.Model.DataSets.Weights;

namespace SightKeeper.Data.Binary.Conversion.DataSets;

internal abstract class PoserDataSetConverter<TPackableDataSet> : DataSetConverter<TPackableDataSet>
	where TPackableDataSet : PackableDataSet, new()
{
	protected PoserDataSetConverter(FileSystemScreenshotsDataAccess screenshotsDataAccess, ConversionSession session) : base(screenshotsDataAccess, session)
	{
	}

	protected void BuildKeyPoints(
		IEnumerable<KeyPointTag> keyPointTags,
		ref byte indexCounter,
		ImmutableArray<PackableTag>.Builder builder)
	{
		foreach (var keyPointTag in keyPointTags)
		{
			Session.TagsIds.Add(keyPointTag, indexCounter);
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

	protected ImmutableArray<PackablePoserWeights> ConvertPoserWeights(
		IReadOnlyCollection<Weights> weights)
	{
		var resultBuilder = ImmutableArray.CreateBuilder<PackablePoserWeights>();
		foreach (var item in weights.Cast<PoserWeights>())
		{
			resultBuilder.Add(ConvertWeights(Session.WeightsIdCounter, item));
			Session.WeightsIds.Add(item, Session.WeightsIdCounter);
			Session.WeightsIdCounter++;
		}
		return resultBuilder.DrainToImmutable();
	}
	
	private PackablePoserWeights ConvertWeights(ushort id, PoserWeights item)
	{
		return new PackablePoserWeights(
			id,
			item.CreationDate,
			item.ModelSize,
			item.Metrics,
			item.Resolution,
			ConvertWeightsTags(item.Tags));
	}

	protected abstract ImmutableDictionary<byte, ImmutableArray<byte>> ConvertWeightsTags(IDictionary tags);
}