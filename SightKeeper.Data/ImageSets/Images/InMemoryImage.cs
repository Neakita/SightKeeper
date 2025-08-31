using CommunityToolkit.Diagnostics;
using FlakeId;
using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Assets;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace SightKeeper.Data.ImageSets.Images;

internal sealed class InMemoryImage : StorableImage
{
	public Id Id { get; }
	public DateTimeOffset CreationTimestamp { get; }
	public Vector2<ushort> Size { get; }
	public IReadOnlyCollection<Asset> Assets => _assets;
	public string? DataFormat => null;

	public InMemoryImage(Id id, DateTimeOffset creationTimestamp, Vector2<ushort> size)
	{
		Id = id;
		CreationTimestamp = creationTimestamp;
		Size = size;
	}

	public Image? Load(CancellationToken cancellationToken)
	{
		return null;
	}

	public Image<TPixel>? Load<TPixel>(CancellationToken cancellationToken) where TPixel : unmanaged, IPixel<TPixel>
	{
		return null;
	}

	public Task<Image?> LoadAsync(CancellationToken cancellationToken)
	{
		return Task.FromResult<Image?>(null);
	}

	public Task<Image<TPixel>?> LoadAsync<TPixel>(CancellationToken cancellationToken) where TPixel : unmanaged, IPixel<TPixel>
	{
		return Task.FromResult<Image<TPixel>?>(null);
	}

	public Stream? OpenWriteStream()
	{
		return null;
	}

	public Stream? OpenReadStream()
	{
		return null;
	}

	public bool TryCopyTo(string filePath)
	{
		return false;
	}

	public void AddAsset(Asset asset)
	{
		bool isAdded = _assets.Add(asset);
		Guard.IsTrue(isAdded);
	}

	public void RemoveAsset(Asset asset)
	{
		bool isRemoved = _assets.Remove(asset);
		Guard.IsTrue(isRemoved);
	}

	public void DeleteData()
	{
	}

	public void Dispose()
	{
	}

	public override string ToString()
	{
		return $"#{Id.ToString()}";
	}

	private readonly HashSet<Asset> _assets = new();
}