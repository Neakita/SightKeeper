using System.Buffers;
using CommunityToolkit.Diagnostics;
using MemoryPack;
using SightKeeper.Data.DataSets.Assets.Items;
using SightKeeper.Data.DataSets.Detector.Items;
using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Detector;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.DataSets.Detector;

internal sealed class DetectorItemsFormatter : ItemsFormatter<DetectorItem>
{
	public void WriteItems<TBufferWriter>(ref MemoryPackWriter<TBufferWriter> writer, IReadOnlyCollection<DetectorItem> items, Dictionary<Tag, byte> tagIndexes) where TBufferWriter : IBufferWriter<byte>
	{
		writer.WriteCollectionHeader(items.Count);
		foreach (var item in items)
		{
			var tagIndex = tagIndexes[item.Tag];
			writer.WriteUnmanaged(tagIndex, item.Bounding);
		}
	}

	public void ReadItems(ref MemoryPackReader reader, IReadOnlyList<Tag> tags, ItemsOwner<DetectorItem> itemsOwner)
	{
		Guard.IsTrue(reader.TryReadCollectionHeader(out var itemsCount));
		for (int i = 0; i < itemsCount; i++)
		{
			reader.ReadUnmanaged(out byte tagIndex, out Bounding bounding);
			var tag = tags[tagIndex];
			var item = itemsOwner.MakeItem(tag);
			tag.AddUser(item);
			var innermostItem = item.UnWrapDecorator<InMemoryDetectorItem>();
			innermostItem.Bounding = bounding;
		}
	}
}