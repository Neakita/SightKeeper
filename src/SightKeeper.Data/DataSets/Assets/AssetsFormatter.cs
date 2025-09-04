using System.Buffers;
using MemoryPack;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.DataSets.Assets;

public interface AssetsFormatter<in TAsset>
{
	void Serialize<TBufferWriter>(
		ref MemoryPackWriter<TBufferWriter> writer,
		IReadOnlyCollection<TAsset> assets,
		Dictionary<Tag, byte> tagIndexes)
		where TBufferWriter : IBufferWriter<byte>;

	void Deserialize(ref MemoryPackReader reader, DataSet<TAsset> set);
}