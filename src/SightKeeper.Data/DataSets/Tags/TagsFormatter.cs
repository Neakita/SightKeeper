using System.Buffers;
using CommunityToolkit.Diagnostics;
using MemoryPack;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.DataSets.Tags;

internal static class TagsFormatter
{
	public static void WriteTags<TBufferWriter>(ref MemoryPackWriter<TBufferWriter> writer, IReadOnlyCollection<Tag> tags)
		where TBufferWriter : IBufferWriter<byte>
	{
		writer.WriteCollectionHeader(tags.Count);
		foreach (var tag in tags)
		{
			writer.WriteString(tag.Name);
			writer.WriteUnmanaged(tag.Color);
		}
	}

	public static void ReadTags(ref MemoryPackReader reader, StorableTagsOwner<StorableTag> tagsLibrary)
	{
		Guard.IsTrue(reader.TryReadCollectionHeader(out var tagsCount));
		tagsLibrary.EnsureCapacity(tagsCount);
		for (int i = 0; i < tagsCount; i++)
		{
			var tagName = reader.ReadString();
			Guard.IsNotNull(tagName);
			var color = reader.ReadUnmanaged<uint>();
			var tag = tagsLibrary.CreateTag(tagName);
			tag.Color = color;
		}
	}
}