using System.Buffers;
using MemoryPack;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.DataSets.Tags;

internal interface TagsFormatter<in TTag>
{
	void WriteTags<TBufferWriter>(
		ref MemoryPackWriter<TBufferWriter> writer,
		IReadOnlyCollection<TTag> tags)
		where TBufferWriter : IBufferWriter<byte>;

	void ReadTags(ref MemoryPackReader reader, TagsOwner<TTag> tagsLibrary);
}