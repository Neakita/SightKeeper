using FlakeId;
using SightKeeper.Data.Services;
using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.Images;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace SightKeeper.Data.ImageSets.Images;

internal sealed class StreamableDataImage(ManagedImage inner, FileSystemDataAccess dataAccess) : ManagedImage, DeletableImageData, FileExtensionProvider
{
	public DateTimeOffset CreationTimestamp => inner.CreationTimestamp;
	public Vector2<ushort> Size => inner.Size;
	public IReadOnlyCollection<Asset> Assets => inner.Assets;
	public string FileExtension => dataAccess.FileExtension;

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
		return dataAccess.OpenWrite(Id);
	}

	public Stream? OpenReadStream()
	{
		return dataAccess.OpenRead(Id);
	}

	public bool TryCopyTo(string filePath)
	{
		return dataAccess.TryCopyTo(Id, filePath);
	}

	public void DeleteData()
	{
		dataAccess.Delete(Id);
	}

	public override string? ToString()
	{
		return inner.ToString();
	}

	private Id Id => inner.GetFirst<IdHolder>().Id;
}