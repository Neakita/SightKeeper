using System.Buffers;
using CommunityToolkit.Diagnostics;
using MemoryPack;
using SightKeeper.Data.DataSets.Assets.Items;
using SightKeeper.Data.DataSets.Tags;
using SightKeeper.Data.Services;
using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.DataSets.Poser.Items.Decorators;

internal sealed class PoserItemsFormatter(TagIndexProvider tagIndexProvider) : ItemsFormatter<PoserItem>
{
	public void WriteItems<TBufferWriter>(
		ref MemoryPackWriter<TBufferWriter> writer,
		IReadOnlyCollection<PoserItem> items)
		where TBufferWriter : IBufferWriter<byte>
	{
		writer.WriteCollectionHeader(items.Count);
		foreach (var item in items)
		{
			var tagIndex = tagIndexProvider.GetTagIndex(item.Tag);
			writer.WriteUnmanaged(tagIndex, item.Bounding);
			WriteKeyPoints(ref writer, item);
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
			tag.GetFirst<EditableTagUsers>().AddUser(item);
			innermostItem.Bounding = bounding;
			ReadKeyPoints(ref reader, poserTag.KeyPointTags, innermostItem);
		}
	}

	private void WriteKeyPoints<TBufferWriter>(
		ref MemoryPackWriter<TBufferWriter> writer,
		PoserItem item)
		where TBufferWriter : IBufferWriter<byte>
	{
		writer.WriteCollectionHeader(item.KeyPoints.Count);
		foreach (var keyPoint in item.KeyPoints)
		{
			var keyPointTagIndex = tagIndexProvider.GetTagIndex(keyPoint.Tag);
			writer.WriteUnmanaged(keyPointTagIndex, keyPoint.Position);
		}
	}

	private static void ReadKeyPoints(ref MemoryPackReader reader, IReadOnlyList<Tag> keyPointTags, PoserItem item)
	{
		Guard.IsTrue(reader.TryReadCollectionHeader(out var keyPointsCount));
		for (int j = 0; j < keyPointsCount; j++)
		{
			reader.ReadUnmanaged(out byte keyPointTagIndex, out Vector2<double> position);
			var keyPointTag = keyPointTags[keyPointTagIndex];
			var keyPoint = item.MakeKeyPoint(keyPointTag);
			var innermostKeyPoint = keyPoint.GetInnermost<KeyPoint>();
			keyPointTag.GetFirst<EditableTagUsers>().AddUser(keyPoint);
			innermostKeyPoint.Position = position;
		}
	}
}