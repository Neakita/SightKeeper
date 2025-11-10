using FlakeId;
using SightKeeper.Application;
using SightKeeper.Data.Services;
using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.Images;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace SightKeeper.Data.ImageSets.Images;

internal sealed class StreamableDataImage(ManagedImage inner, FileSystemDataAccess dataAccess)
	: ManagedImage, DeletableData, FileExtensionProvider, StreamableData, LoadableImage, Decorator<ManagedImage>
{
	public DateTimeOffset CreationTimestamp => inner.CreationTimestamp;
	public Vector2<ushort> Size => inner.Size;
	public IReadOnlyCollection<Asset> Assets => inner.Assets;
	public string FileExtension => dataAccess.FileExtension;
	public ManagedImage Inner => inner;

	public async Task<Image> LoadAsync(CancellationToken cancellationToken)
	{
		await using var stream = dataAccess.OpenRead(Id);
		return await Image.LoadAsync(stream, cancellationToken);
	}

	public async Task<Image<TPixel>> LoadAsync<TPixel>(CancellationToken cancellationToken) where TPixel : unmanaged, IPixel<TPixel>
	{
		await using var stream = dataAccess.OpenRead(Id);
		return await Image.LoadAsync<TPixel>(stream, cancellationToken);
	}

	public Stream OpenWriteStream()
	{
		return dataAccess.OpenWrite(Id);
	}

	public Stream OpenReadStream()
	{
		return dataAccess.OpenRead(Id);
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