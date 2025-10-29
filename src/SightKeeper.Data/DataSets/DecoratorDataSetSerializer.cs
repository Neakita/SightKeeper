using System.Buffers;
using MemoryPack;
using SightKeeper.Data.Services;
using SightKeeper.Domain;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.DataSets;

internal sealed class DecoratorDataSetSerializer : Serializer<DataSet<Tag, Asset>>
{
	public void Serialize<TBufferWriter>(ref MemoryPackWriter<TBufferWriter> writer, DataSet<Tag, Asset> set) where TBufferWriter : IBufferWriter<byte>
	{
		var serializable = set.GetFirst<MemoryPackSerializable>();
		serializable.Serialize(ref writer);
	}
}