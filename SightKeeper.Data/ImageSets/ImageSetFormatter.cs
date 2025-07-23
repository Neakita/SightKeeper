using System.Buffers;
using CommunityToolkit.Diagnostics;
using FlakeId;
using MemoryPack;
using SightKeeper.Application.ImageSets.Creating;
using SightKeeper.Data.ImageSets.Images;
using SightKeeper.Domain;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.ImageSets;

public sealed class ImageSetFormatter(
	ImageSetWrapper setWrapper,
	ImageSetFactory<InMemoryImageSet> imageSetFactory,
	ImageLookupperPopulator imageLookupperPopulator)
    : MemoryPackFormatter<StorableImageSet>, IMemoryPackFormatter<ImageSet>
{
    public override void Serialize<TBufferWriter>(ref MemoryPackWriter<TBufferWriter> writer, scoped ref StorableImageSet? set)
	{
		if (set == null)
		{
			writer.WriteNullObjectHeader();
			return;
		}
		WriteGeneralMembers(ref writer, set);
		WriteImages(ref writer, set);
	}

	public override void Deserialize(ref MemoryPackReader reader, scoped ref StorableImageSet? set)
	{
		if (reader.PeekIsNull())
		{
			set = null;
			return;
		}
		var inMemorySet = ReadGeneralMembers(ref reader);
		ReadImages(ref reader, inMemorySet);
		set = setWrapper.Wrap(inMemorySet);
		imageLookupperPopulator.AddImages(set.Images);
	}

    private static void WriteGeneralMembers<TBufferWriter>(ref MemoryPackWriter<TBufferWriter> writer, ImageSet set)
		where TBufferWriter : IBufferWriter<byte>
	{
		writer.WriteString(set.Name);
		writer.WriteString(set.Description);
	}

	private static void WriteImages<TBufferWriter>(ref MemoryPackWriter<TBufferWriter> writer, StorableImageSet set)
		where TBufferWriter : IBufferWriter<byte>
	{
		writer.WriteCollectionHeader(set.Images.Count);
		foreach (var image in set.Images)
			WriteImage(ref writer, image);
	}

	private static void WriteImage<TBufferWriter>(ref MemoryPackWriter<TBufferWriter> writer, StorableImage image)
		where TBufferWriter : IBufferWriter<byte>
	{
		writer.WriteUnmanaged(image.Id, image.CreationTimestamp, image.Size);
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

	public void Serialize<TBufferWriter>(ref MemoryPackWriter<TBufferWriter> writer, scoped ref ImageSet? set) where TBufferWriter : IBufferWriter<byte>
	{
		var storableSet = (StorableImageSet?)set;
		Serialize(ref writer, ref storableSet);
	}

	public void Deserialize(ref MemoryPackReader reader, scoped ref ImageSet? set)
	{
		StorableImageSet? storableSet = null;
		Deserialize(ref reader, ref storableSet);
		set = storableSet;
	}
}