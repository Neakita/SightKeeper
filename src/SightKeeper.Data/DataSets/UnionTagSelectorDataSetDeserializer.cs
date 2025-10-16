using CommunityToolkit.Diagnostics;
using MemoryPack;
using SightKeeper.Data.Services;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.DataSets;

internal sealed class UnionTagSelectorDataSetDeserializer(IReadOnlyList<Deserializer<DataSet<Tag, Asset>>> deserializersByUnionTag) : Deserializer<DataSet<Tag, Asset>>
{
	public DataSet<Tag, Asset> Deserialize(ref MemoryPackReader reader)
	{
		Guard.IsTrue(reader.TryReadUnionHeader(out var unionTag));
		var deserializer = deserializersByUnionTag[unionTag];
		return deserializer.Deserialize(ref reader);
	}
}