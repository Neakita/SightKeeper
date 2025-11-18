using CommunityToolkit.Diagnostics;
using MemoryPack;
using SightKeeper.Application.Misc;
using SightKeeper.Data.DataSets.Assets;
using SightKeeper.Data.DataSets.Tags;
using SightKeeper.Data.DataSets.Weights;
using SightKeeper.Data.Services;
using SightKeeper.Domain;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.DataSets;

internal sealed class DataSetDeserializer<TTag, TAsset>(
	Factory<DataSet<TTag, TAsset>> dataSetFactory,
	TagsFormatter<TTag> tagsFormatter,
	AssetsFormatter<TAsset> assetsFormatter,
	WeightsFormatter weightsFormatter) :
	Deserializer<DataSet<Tag, Asset>>
	where TTag : Tag
	where TAsset : Asset
{
	public DataSet<Tag, Asset> Deserialize(ref MemoryPackReader reader)
	{
		var set = dataSetFactory.Create();
		var innerSet = set.GetInnermost<DataSet<TTag, TAsset>>();
		ReadGeneralData(ref reader, (DataSet<Tag, Asset>)innerSet);
		tagsFormatter.ReadTags(ref reader, innerSet.TagsLibrary);
		assetsFormatter.Deserialize(ref reader, (DataSet<Tag, TAsset>)innerSet);
		weightsFormatter.ReadWeights(ref reader, innerSet.WeightsLibrary);
		return (DataSet<Tag, Asset>)set;
	}

	private static void ReadGeneralData(ref MemoryPackReader reader, DataSet<Tag, Asset> dataSet)
	{
		var name = reader.ReadString();
		Guard.IsNotNull(name);
		var description = reader.ReadString();
		Guard.IsNotNull(description);
		dataSet.Name = name;
		dataSet.Description = description;
	}
}