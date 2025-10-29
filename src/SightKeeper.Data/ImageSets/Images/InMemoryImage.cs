using CommunityToolkit.Diagnostics;
using FlakeId;
using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.Images;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace SightKeeper.Data.ImageSets.Images;

internal sealed class InMemoryImage(Id id, DateTimeOffset creationTimestamp, Vector2<ushort> size)
	: ManagedImage, EditableImageAssets, IdHolder
{
	public Id Id => id;
	public DateTimeOffset CreationTimestamp => creationTimestamp;
	public Vector2<ushort> Size => size;
	public IReadOnlyCollection<Asset> Assets => _assets;

	public Task<Image?> LoadAsync(CancellationToken cancellationToken)
	{
		return Task.FromResult<Image?>(null);
	}

	public Task<Image<TPixel>?> LoadAsync<TPixel>(CancellationToken cancellationToken) where TPixel : unmanaged, IPixel<TPixel>
	{
		return Task.FromResult<Image<TPixel>?>(null);
	}

	public void Add(Asset asset)
	{
		bool isAdded = _assets.Add(asset);
		Guard.IsTrue(isAdded);
	}

	public void Remove(Asset asset)
	{
		bool isRemoved = _assets.Remove(asset);
		Guard.IsTrue(isRemoved);
	}

	public void DeleteData()
	{
	}

	public override string ToString()
	{
		return $"#{Id.ToString()}";
	}

	private readonly HashSet<Asset> _assets = new();
}