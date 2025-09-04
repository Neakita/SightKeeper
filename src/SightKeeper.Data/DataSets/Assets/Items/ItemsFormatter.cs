using System.Buffers;
using MemoryPack;
using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.DataSets.Assets.Items;

public interface ItemsFormatter<in TItem>
{
	void WriteItems<TBufferWriter>(
		ref MemoryPackWriter<TBufferWriter> writer,
		IReadOnlyCollection<TItem> items,
		Dictionary<Tag, byte> tagIndexes)
		where TBufferWriter : IBufferWriter<byte>;

	void ReadItems(ref MemoryPackReader reader, IReadOnlyList<Tag> tags, ItemsOwner<TItem> itemsOwner);
}