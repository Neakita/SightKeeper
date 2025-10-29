using CommunityToolkit.Diagnostics;
using FlakeId;
using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.ImageSets.Images;

internal sealed class InMemoryImage(Id id, DateTimeOffset creationTimestamp, Vector2<ushort> size)
	: ManagedImage, EditableImageAssets, IdHolder
{
	public Id Id => id;
	public DateTimeOffset CreationTimestamp => creationTimestamp;
	public Vector2<ushort> Size => size;
	public IReadOnlyCollection<Asset> Assets => _assets;

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

	public override string ToString()
	{
		return $"#{Id.ToString()}";
	}

	private readonly HashSet<Asset> _assets = new();
}