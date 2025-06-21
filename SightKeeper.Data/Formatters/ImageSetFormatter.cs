using CommunityToolkit.Diagnostics;
using FlakeId;
using MemoryPack;
using SightKeeper.Data.Model.Images;
using SightKeeper.Data.Services;
using SightKeeper.Domain;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.Formatters;

internal sealed class ImageSetFormatter : MemoryPackFormatter<ImageSet>
{
	public ImageSetFormatter(ChangeListener changeListener, Lock editingLock)
	{
		var dataAccess = new CompressedFileSystemDataAccess();
		dataAccess.DirectoryPath = Path.Combine(dataAccess.DirectoryPath, "Images");
		_imageWrapper = new ImageWrapper(dataAccess);
		_setWrapper = new ImageSetWrapper(changeListener, editingLock);
	}

	public override void Serialize<TBufferWriter>(ref MemoryPackWriter<TBufferWriter> writer, scoped ref ImageSet? set)
	{
		if (set == null)
		{
			writer.WriteNullObjectHeader();
			return;
		}
		writer.WriteString(set.Name);
		writer.WriteString(set.Description);
		writer.WriteCollectionHeader(set.Images.Count);
		foreach (var image in set.Images)
		{
			var inMemoryImage = image.UnWrapDecorator<InMemoryImage>();
			writer.WriteUnmanaged(inMemoryImage.Id, inMemoryImage.CreationTimestamp, inMemoryImage.Size);
		}
	}

	public override void Deserialize(ref MemoryPackReader reader, scoped ref ImageSet? set)
	{
		if (reader.PeekIsNull())
		{
			set = null;
			return;
		}
		var name = reader.ReadString();
		Guard.IsNotNull(name);
		var description = reader.ReadString();
		Guard.IsNotNull(description);
		Guard.IsTrue(reader.TryReadCollectionHeader(out var imagesCount));
		var inMemorySet = new InMemoryImageSet(_imageWrapper, imagesCount)
		{
			Name = name,
			Description = description
		};
		for (int i = 0; i < imagesCount; i++)
		{
			reader.ReadUnmanaged(out Id id, out DateTimeOffset creationTimestamp, out Vector2<ushort> size);
			InMemoryImage image = new(id, creationTimestamp, size);
			inMemorySet.AddImage(image);
		}

		set = _setWrapper.Wrap(inMemorySet);
	}

	private readonly ImageSetWrapper _setWrapper;
	private readonly ImageWrapper _imageWrapper;
}