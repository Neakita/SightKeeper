using FlakeId;
using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.Model.Images;

internal sealed class InMemoryImage : Image
{
	public DateTimeOffset CreationTimestamp { get; }
	public Vector2<ushort> Size { get; }

	public IReadOnlyCollection<Asset> Assets => _assets;
	public Id Id { get; }

	public InMemoryImage(DateTimeOffset creationTimestamp, Vector2<ushort> size, Id id)
	{
		CreationTimestamp = creationTimestamp;
		Size = size;
		Id = id;
	}

	public Stream? OpenWriteStream()
	{
		return null;
	}

	public Stream? OpenReadStream()
	{
		return null;
	}


	private readonly HashSet<Asset> _assets = new();
}