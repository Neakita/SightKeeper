using FlakeId;
using SightKeeper.Data.Services;
using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.Images;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace SightKeeper.Data.ImageSets.Images;

internal sealed class StreamableDataImage(ManagedImage inner, FileSystemDataAccess dataAccess) : ManagedImage
{
	public Id Id => inner.Id;
	public DateTimeOffset CreationTimestamp => inner.CreationTimestamp;
	public Vector2<ushort> Size => inner.Size;
	public IReadOnlyCollection<Asset> Assets => inner.Assets;
	public string DataFormat => dataAccess.FileExtension;

	public Image? Load(CancellationToken cancellationToken)
	{
		return dataAccess.LoadImage(Id, cancellationToken);
	}

	public Image<TPixel>? Load<TPixel>(CancellationToken cancellationToken) where TPixel : unmanaged, IPixel<TPixel>
	{
		return dataAccess.LoadImage<TPixel>(Id, cancellationToken);
	}

	public Task<Image?> LoadAsync(CancellationToken cancellationToken)
	{
		return dataAccess.LoadImageAsync(Id, cancellationToken);
	}

	public Task<Image<TPixel>?> LoadAsync<TPixel>(CancellationToken cancellationToken) where TPixel : unmanaged, IPixel<TPixel>
	{
		return dataAccess.LoadImageAsync<TPixel>(Id, cancellationToken);
	}

	public Stream OpenWriteStream()
	{
		return dataAccess.OpenWrite(inner.Id);
	}

	public Stream? OpenReadStream()
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