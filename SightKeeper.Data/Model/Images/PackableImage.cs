using FlakeId;
using MemoryPack;
using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.Model.Images;

internal sealed class PackableImage : Image
{
	static PackableImage()
	{
		MemoryPackFormatterProvider.Register(new PackableImageFormatter());
	}

	public DateTimeOffset CreationTimestamp { get; }
	public Vector2<ushort> Size { get; }

	[MemoryPackIgnore] public IReadOnlyCollection<Asset> Assets => _assets;

	public Id Id { get; }

	public PackableImage(DateTimeOffset creationTimestamp, Vector2<ushort> size, Id id)
	{
		CreationTimestamp = creationTimestamp;
		Size = size;
		Id = id;
	}

	private readonly HashSet<Asset> _assets = new();
}