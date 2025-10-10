using System.Buffers;
using CommunityToolkit.Diagnostics;
using MemoryPack;
using SightKeeper.Data.DataSets.Assets.Items;
using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.DataSets.Poser.Items;

internal sealed class PoserItemsFormatter : ItemsFormatter<PoserItem>
{
	public void WriteItems<TBufferWriter>(
		ref MemoryPackWriter<TBufferWriter> writer,
		IReadOnlyCollection<PoserItem> items,
		IReadOnlyDictionary<Tag, byte> tagIndexes)
		where TBufferWriter : IBufferWriter<byte>
	{
		writer.WriteCollectionHeader(items.Count);
		foreach (var item in items)
		{
			var tagIndex = tagIndexes[item.Tag];
			writer.WriteUnmanaged(tagIndex, item.Bounding);
			WriteKeyPoints(writer, tagIndexes, item);
		}
	}

	public void ReadItems(ref MemoryPackReader reader, IReadOnlyList<Tag> tags, ItemsOwner<PoserItem> itemsOwner)
	{
		Guard.IsTrue(reader.TryReadCollectionHeader(out var itemsCount));
		for (int i = 0; i < itemsCount; i++)
		{
			reader.ReadUnmanaged(out byte tagIndex, out Bounding bounding);
			var item = itemsOwner.MakeItem();
			var innermostItem = item.GetInnermost<PoserItem>();
			var tag = tags[tagIndex];
			var poserTag = (PoserTag)tag;
			innermostItem.Tag = poserTag;
			tag.AddUser(item);
			innermostItem.Bounding = bounding;
			Guard.IsTrue(reader.TryReadCollectionHeader(out var keyPointsCount));
			for (int j = 0; j < keyPointsCount; j++)
			{
				reader.ReadUnmanaged(out byte keyPointTagIndex, out Vector2<double> position);
				var keyPointTag = poserTag.KeyPointTags[keyPointTagIndex];
				var keyPoint = item.MakeKeyPoint(keyPointTag);
				keyPointTag.AddUser(keyPoint);
				keyPoint.Position = position;
			}
		}
	}

	private static void WriteKeyPoints<TBufferWriter>(
		MemoryPackWriter<TBufferWriter> writer,
		IReadOnlyDictionary<Tag, byte> tagIndexes,
		PoserItem item)
		where TBufferWriter : IBufferWriter<byte>
	{
		writer.WriteCollectionHeader(item.KeyPoints.Count);
		foreach (var keyPoint in item.KeyPoints)
		{
			var keyPointTagIndex = tagIndexes[item.Tag];
			writer.WriteUnmanaged(keyPointTagIndex, keyPoint.Position);
		}
	}
}