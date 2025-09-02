using System.Buffers;
using CommunityToolkit.Diagnostics;
using MemoryPack;
using SightKeeper.Application.ImageSets.Creating;
using SightKeeper.Data.ImageSets.Images;
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
		writer.WriteValue<IReadOnlyCollection<StorableImage>>(set.Images);
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
		var images = reader.ReadValue<IReadOnlyCollection<StorableImage>>();
		Guard.IsNotNull(images);
		inMemorySet.EnsureCapacity(images.Count);
		foreach (var image in images)
			inMemorySet.WrapAndInsertImage(image);
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