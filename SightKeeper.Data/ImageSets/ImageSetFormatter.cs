using System.Buffers;
using CommunityToolkit.Diagnostics;
using FlakeId;
using MemoryPack;
using SightKeeper.Application.ImageSets.Creating;
using SightKeeper.Data.ImageSets.Images;
using SightKeeper.Domain;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.ImageSets;

internal sealed class ImageSetFormatter(ImageSetWrapper setWrapper, ImageSetFactory<InMemoryImageSet> imageSetFactory)
    : MemoryPackFormatter<ImageSet>
{
    public override void Serialize<TBufferWriter>(ref MemoryPackWriter<TBufferWriter> writer, scoped ref ImageSet? set)
	{
		if (set == null)
		{
			writer.WriteNullObjectHeader();
			return;
		}
		WriteGeneralMembers(ref writer, set);
		WriteImages(ref writer, set);
	}

	public override void Deserialize(ref MemoryPackReader reader, scoped ref ImageSet? set)
	{
		if (reader.PeekIsNull())
		{
			set = null;
			return;
		}
		var inMemorySet = ReadGeneralMembers(ref reader);
		ReadImages(ref reader, inMemorySet);
		set = setWrapper.Wrap(inMemorySet);
	}

    private static void WriteGeneralMembers<TBufferWriter>(ref MemoryPackWriter<TBufferWriter> writer, ImageSet set)
		where TBufferWriter : IBufferWriter<byte>
	{
		writer.WriteString(set.Name);
		writer.WriteString(set.Description);
	}

	private static void WriteImages<TBufferWriter>(ref MemoryPackWriter<TBufferWriter> writer, ImageSet set)
		where TBufferWriter : IBufferWriter<byte>
	{
		writer.WriteCollectionHeader(set.Images.Count);
		foreach (var image in set.Images)
			WriteImage(ref writer, image);
	}

	private static void WriteImage<TBufferWriter>(ref MemoryPackWriter<TBufferWriter> writer, Image image)
		where TBufferWriter : IBufferWriter<byte>
	{
		var inMemoryImage = image.UnWrapDecorator<InMemoryImage>();
		writer.WriteUnmanaged(inMemoryImage.Id, inMemoryImage.CreationTimestamp, inMemoryImage.Size);
	}

	private InMemoryImageSet ReadGeneralMembers(ref MemoryPackReader reader)
	{
		var name = reader.ReadString();
		Guard.IsNotNull(name);
		var description = reader.ReadString();
		Guard.IsNotNull(description);
        var inMemorySet = imageSetFactory.CreateImageSet();
        inMemorySet.Name = name;
        inMemorySet.Description = description;
		return inMemorySet;
	}

	private static void ReadImages(ref MemoryPackReader reader, InMemoryImageSet inMemorySet)
	{
		Guard.IsTrue(reader.TryReadCollectionHeader(out var imagesCount));
		inMemorySet.EnsureCapacity(imagesCount);
		for (int i = 0; i < imagesCount; i++)
		{
			var image = ReadImage(ref reader);
			inMemorySet.AddImage(image);
		}
	}

	private static InMemoryImage ReadImage(ref MemoryPackReader reader)
	{
		reader.ReadUnmanaged(out Id id, out DateTimeOffset creationTimestamp, out Vector2<ushort> size);
		InMemoryImage image = new(id, creationTimestamp, size);
		return image;
	}
}