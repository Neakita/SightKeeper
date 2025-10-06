using MemoryPack;
using SightKeeper.Application.DataSets.Creating;
using SightKeeper.Data.DataSets.Assets;
using SightKeeper.Data.DataSets.Tags;
using SightKeeper.Data.DataSets.Weights;
using SightKeeper.Domain;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.DataSets;

public sealed class MemoryPackDataSetDeserializer<TTag, TAsset>(
	DataSetFactory<TTag, TAsset> dataSetFactory,
	TagsFormatter<TTag> tagsFormatter,
	AssetsFormatter<TAsset> assetsFormatter)
	: MemoryPackDataSetDeserializer
	where TTag : Tag
	where TAsset : Asset
{
	public DataSet<Tag, Asset> Deserialize(ref MemoryPackReader reader)
	{
		var set = dataSetFactory.CreateDataSet();
		var innerSet = set.GetInnermost<DataSet<TTag, TAsset>>();
		DataSetGeneralDataFormatter.ReadGeneralData(ref reader, (DataSet<Tag, Asset>)innerSet);
		tagsFormatter.ReadTags(ref reader, innerSet.TagsLibrary);
		assetsFormatter.Deserialize(ref reader, (DataSet<Tag, TAsset>)innerSet);
		WeightsFormatter.ReadWeights(ref reader, innerSet.WeightsLibrary, (IReadOnlyList<Tag>)innerSet.TagsLibrary.Tags);
		return (DataSet<Tag, Asset>)set;
	}
}