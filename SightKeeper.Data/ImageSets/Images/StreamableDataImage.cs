using FlakeId;
using SightKeeper.Data.Services;
using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Assets;

namespace SightKeeper.Data.ImageSets.Images;

internal sealed class StreamableDataImage(StorableImage inner, FileSystemDataAccess dataAccess) : StorableImage
{
	public Id Id => inner.Id;
	public DateTimeOffset CreationTimestamp => inner.CreationTimestamp;
	public Vector2<ushort> Size => inner.Size;
	public IReadOnlyCollection<Asset> Assets => inner.Assets;
	public string DataFormat => dataAccess.FileExtension;

	public Stream OpenWriteStream()
	{
		return dataAccess.OpenWrite(inner.Id);
	}

	public Stream OpenReadStream()
	{
		return dataAccess.OpenRead(inner.Id);
	}

	public bool TryCopyTo(string filePath)
	{
		return dataAccess.TryCopyTo(Id, filePath);
	}

	public void AddAsset(Asset asset)
	{
		inner.AddAsset(asset);
	}

	public void RemoveAsset(Asset asset)
	{
		inner.RemoveAsset(asset);
	}

	public void DeleteData()
	{
		dataAccess.Delete(inner.Id);
	}

	public void Dispose()
	{
		inner.Dispose();
	}

	public override string? ToString()
	{
		return inner.ToString();
	}
}