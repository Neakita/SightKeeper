using System.Buffers;
using CommunityToolkit.Diagnostics;
using MemoryPack;
using SightKeeper.Data.DataSets.Assets.Items;
using SightKeeper.Data.DataSets.Tags;
using SightKeeper.Data.Services;
using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.DataSets.Detector;

internal sealed class DetectorItemsFormatter(TagIndexProvider tagIndexProvider) : ItemsFormatter<DetectorItem>
{
	public void WriteItems<TBufferWriter>(
		ref MemoryPackWriter<TBufferWriter> writer,
		IReadOnlyCollection<DetectorItem> items)
		where TBufferWriter : IBufferWriter<byte>
	{
		writer.WriteCollectionHeader(items.Count);
		foreach (var item in items)
		{
			var tagIndex = tagIndexProvider.GetTagIndex(item.Tag);
			writer.WriteUnmanaged(tagIndex, item.Bounding);
		}
	}

	public void ReadItems(ref MemoryPackReader reader, IReadOnlyList<Tag> tags, ItemsOwner<DetectorItem> itemsOwner)
	{
		Guard.IsTrue(reader.TryReadCollectionHeader(out var itemsCount));
		for (int i = 0; i < itemsCount; i++)
		{
			reader.ReadUnmanaged(out byte tagIndex, out Bounding bounding);
			var item = itemsOwner.MakeItem();
			var innermostItem = item.GetInnermost<DetectorItem>();
			var tag = tags[tagIndex];
			innermostItem.Tag = tag;
			tag.GetFirst<EditableTagUsers>().AddUser(item);
			innermostItem.Bounding = bounding;
		}
	}
}