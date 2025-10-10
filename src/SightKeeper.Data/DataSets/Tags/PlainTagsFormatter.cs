using System.Buffers;
using CommunityToolkit.Diagnostics;
using MemoryPack;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.DataSets.Tags;

internal sealed class PlainTagsFormatter : TagsFormatter<Tag>
{
	public void WriteTags<TBufferWriter>(ref MemoryPackWriter<TBufferWriter> writer, IReadOnlyCollection<Tag> tags)
		where TBufferWriter : IBufferWriter<byte>
	{
		writer.WriteCollectionHeader(tags.Count);
		foreach (var tag in tags)
			WriteTag(ref writer, tag);
	}

	public void ReadTags(ref MemoryPackReader reader, TagsOwner<Tag> tagsLibrary)
	{
		Guard.IsTrue(reader.TryReadCollectionHeader(out var tagsCount));
		for (var i = 0; i < tagsCount; i++)
			ReadTag(ref reader, tagsLibrary);
	}

	public static void WriteTag<TBufferWriter>(ref MemoryPackWriter<TBufferWriter> writer, Tag tag)
		where TBufferWriter : IBufferWriter<byte>
	{
		writer.WriteString(tag.Name);
		writer.WriteUnmanaged(tag.Color);
	}

	public static TTag ReadTag<TTag>(ref MemoryPackReader reader, TagsOwner<TTag> tagsLibrary) where TTag : Tag
	{
		var tagName = reader.ReadString();
		Guard.IsNotNull(tagName);
		var tag = tagsLibrary.CreateTag(tagName);
		var innermostTag = tag.GetInnermost<TTag>();
		innermostTag.Color = reader.ReadUnmanaged<uint>();
		return tag;
	}
}