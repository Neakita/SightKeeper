using SightKeeper.Data.Services;
using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.ImageSets.Images;

internal sealed class StreamableDataImage(InMemoryImage inner, FileSystemDataAccess dataAccess) : Image, Decorator<InMemoryImage>, Decorator<Image>
{
	public DateTimeOffset CreationTimestamp => inner.CreationTimestamp;

	public Vector2<ushort> Size => inner.Size;

	public IReadOnlyCollection<Asset> Assets => inner.Assets;

	InMemoryImage Decorator<InMemoryImage>.Inner => inner;
	Image Decorator<Image>.Inner => inner;

	public Stream OpenWriteStream()
	{
		return dataAccess.OpenWrite(inner.Id);
	}

	public Stream OpenReadStream()
	{
		return dataAccess.OpenRead(inner.Id);
	}

	public void AddAsset(Asset asset)
	{
		inner.AddAsset(asset);
	}

	public void RemoveAsset(Asset asset)
	{
		inner.RemoveAsset(asset);
	}

	internal void DeleteData()
	{
		dataAccess.Delete(inner.Id);
	}
}