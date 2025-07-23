using CommunityToolkit.Diagnostics;
using FlakeId;
using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Assets;

namespace SightKeeper.Data.ImageSets.Images;

internal sealed class InMemoryImage : StorableImage
{
	public Id Id { get; }
	public DateTimeOffset CreationTimestamp { get; }
	public Vector2<ushort> Size { get; }
	public IReadOnlyCollection<Asset> Assets => _assets;

	public InMemoryImage(Id id, DateTimeOffset creationTimestamp, Vector2<ushort> size)
	{
		Id = id;
		CreationTimestamp = creationTimestamp;
		Size = size;
	}

	public Stream? OpenWriteStream()
	{
		return null;
	}

	public Stream? OpenReadStream()
	{
		return null;
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

	private readonly HashSet<Asset> _assets = new();
}