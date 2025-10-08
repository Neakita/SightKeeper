using System.Buffers;
using CommunityToolkit.Diagnostics;
using MemoryPack;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.DataSets.Tags;

internal sealed class PoserTagsFormatter : TagsFormatter<PoserTag>
{
	public void WriteTags<TBufferWriter>(ref MemoryPackWriter<TBufferWriter> writer, IReadOnlyCollection<PoserTag> tags)
		where TBufferWriter : IBufferWriter<byte>
	{
		writer.WriteCollectionHeader(tags.Count);
		foreach (var tag in tags)
		{
			PlainTagsFormatter.WriteTag(ref writer, tag);
			writer.WriteCollectionHeader(tag.KeyPointTags.Count);
			foreach (var keyPointTag in tag.KeyPointTags)
				PlainTagsFormatter.WriteTag(ref writer, keyPointTag);
		}
	}

	public void ReadTags(ref MemoryPackReader reader, TagsOwner<PoserTag> tagsLibrary)
	{
		Guard.IsTrue(reader.TryReadCollectionHeader(out var tagsCount));
		for (int i = 0; i < tagsCount; i++)
		{
			var poserTag = PlainTagsFormatter.ReadTag(ref reader, tagsLibrary);
			Guard.IsTrue(reader.TryReadCollectionHeader(out var keyPointTagsCount));
			for (int j = 0; j < keyPointTagsCount; j++)
				PlainTagsFormatter.ReadTag(ref reader, poserTag);
		}
	}
}