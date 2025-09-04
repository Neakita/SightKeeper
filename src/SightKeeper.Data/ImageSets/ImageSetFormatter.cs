using System.Buffers;
using CommunityToolkit.Diagnostics;
using MemoryPack;
using SightKeeper.Application.ImageSets.Creating;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.ImageSets;

public sealed class ImageSetFormatter(
	ImageSetWrapper setWrapper,
	ImageSetFactory<InMemoryImageSet> imageSetFactory,
	ImageLookupperPopulator imageLookupperPopulator)
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
		imageLookupperPopulator.AddImages(set.Images);
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
		writer.WriteValue<IReadOnlyCollection<ManagedImage>>(set.Images);
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
		var images = reader.ReadValue<IReadOnlyCollection<ManagedImage>>();
		Guard.IsNotNull(images);
		inMemorySet.EnsureCapacity(images.Count);
		foreach (var image in images)
			inMemorySet.WrapAndInsertImage(image);
	}
}