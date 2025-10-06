using System.Buffers;
using FluentAssertions;
using MemoryPack;
using NSubstitute;
using SightKeeper.Data.DataSets.Tags;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.Tests.DataSets;

public sealed class PlainTagsFormatterTests
{
	[Fact]
	public void ShouldPersistTagName()
	{
		const string tagName = "The tag";
		var tag = Substitute.For<Tag>();
		tag.Name.Returns(tagName);
		var persistedTag = Persist(tag);
		persistedTag.Name.Should().Be(tagName);
	}

	[Fact]
	public void ShouldPersistTagColor()
	{
		const uint tagColor = 1234;
		var tag = Substitute.For<Tag>();
		tag.Color.Returns(tagColor);
		var persistedTag = Persist(tag);
		persistedTag.Color.Should().Be(tagColor);
	}

	private static Tag Persist(Tag tag)
	{
		var buffer = Serialize(tag);
		var persistedTag = Deserialize(buffer);
		return persistedTag.Single();
	}

	private static byte[] Serialize(params IReadOnlyCollection<Tag> tags)
	{
		var formatter = new PlainTagsFormatter();
		ArrayBufferWriter<byte> bufferWriter = new();
		using var state = MemoryPackWriterOptionalStatePool.Rent(MemoryPackSerializerOptions.Default);
		var writer = new MemoryPackWriter<ArrayBufferWriter<byte>>(ref bufferWriter, state);
		formatter.WriteTags(ref writer, tags);
		writer.Flush();
		return bufferWriter.WrittenSpan.ToArray();
	}

	private static IReadOnlyCollection<Tag> Deserialize(ReadOnlySpan<byte> buffer)
	{
		var formatter = new PlainTagsFormatter();
		using var state = MemoryPackReaderOptionalStatePool.Rent(MemoryPackSerializerOptions.Default);
		var reader = new MemoryPackReader(buffer, state);
		InMemoryTagsLibrary<Tag> library = new(new FakeTagFactory());
		formatter.ReadTags(ref reader, library);
		return library.Tags;
	}
}