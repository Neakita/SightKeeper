using CommunityToolkit.Diagnostics;
using FlakeId;
using MemoryPack;
using SightKeeper.Domain;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.ImageSets.Images;

public sealed class ImagesFormatter : MemoryPackFormatter<IReadOnlyCollection<ManagedImage>>
{
	public override void Serialize<TBufferWriter>(ref MemoryPackWriter<TBufferWriter> writer, scoped ref IReadOnlyCollection<ManagedImage>? images)
	{
		if (images == null)
		{
			writer.WriteCollectionHeader(0);
			return;
		}
		writer.WriteCollectionHeader(images.Count);
		foreach (var image in images)
			writer.WriteUnmanaged(image.Id, image.CreationTimestamp, image.Size);
	}

	public override void Deserialize(ref MemoryPackReader reader, scoped ref IReadOnlyCollection<ManagedImage>? images)
	{
		Guard.IsTrue(reader.TryReadCollectionHeader(out var imagesCount));
		var imagesArray = new ManagedImage[imagesCount];
		for (int i = 0; i < imagesCount; i++)
		{
			var image = ReadImage(ref reader);
			imagesArray[i] = image;
		}
		images = imagesArray;
	}

	private static InMemoryImage ReadImage(ref MemoryPackReader reader)
	{
		reader.ReadUnmanaged(out Id id, out DateTimeOffset creationTimestamp, out Vector2<ushort> size);
		return new InMemoryImage(id, creationTimestamp, size);
	}
}