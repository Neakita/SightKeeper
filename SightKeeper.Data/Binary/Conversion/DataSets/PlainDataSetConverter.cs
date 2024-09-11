using System.Collections.Immutable;
using SightKeeper.Data.Binary.Model.DataSets;
using SightKeeper.Data.Binary.Model.DataSets.Assets;
using SightKeeper.Data.Binary.Model.DataSets.Tags;
using SightKeeper.Data.Binary.Model.DataSets.Weights;
using SightKeeper.Data.Binary.Services;
using SightKeeper.Domain.Model.DataSets.Tags;
using SightKeeper.Domain.Model.DataSets.Weights;

namespace SightKeeper.Data.Binary.Conversion.DataSets;

internal abstract class PlainDataSetConverter<TAsset, TDataSet> : DataSetConverter<PackableTag, TAsset, TDataSet>
	where TAsset : PackableAsset
	where TDataSet : PackablePlainDataSet<TAsset>, new()
{
	protected PlainDataSetConverter(FileSystemScreenshotsDataAccess screenshotsDataAccess, ConversionSession session) : base(screenshotsDataAccess, session)
	{
	}

	protected ImmutableArray<PackableTag> ConvertPlainTags(IReadOnlyCollection<Tag> tags)
	{
		var builder = ImmutableArray.CreateBuilder<PackableTag>(tags.Count);
		byte index = 0;
		foreach (var tag in tags)
		{
			Session.TagsIds.Add(tag, index);
			builder.Add(ConvertPlainTag(index, tag));
			index++;
		}
		return builder.DrainToImmutable();
	}

	protected sealed override ImmutableArray<PackableWeights> ConvertWeights(IReadOnlyCollection<Weights> weights)
	{
		var resultBuilder = ImmutableArray.CreateBuilder<PackableWeights>(weights.Count);
		foreach (var item in weights.Cast<PlainWeights>())
		{
			var id = Session.WeightsIdCounter++;
			resultBuilder.Add(ConvertWeights(id, item, ConvertWeightsTags(item.Tags)));
			Session.WeightsIds.Add(item, id);
		}
		return resultBuilder.DrainToImmutable();
		ImmutableArray<byte> ConvertWeightsTags(IEnumerable<Tag> tags) => tags
			.Select(tag => Session.TagsIds[tag])
			.ToImmutableArray();
	}

	private static PackableWeights ConvertWeights(ushort id, Weights item, ImmutableArray<byte> tagIds) =>
		new(id, item.CreationDate, item.ModelSize, item.Metrics, item.Resolution, tagIds);
}