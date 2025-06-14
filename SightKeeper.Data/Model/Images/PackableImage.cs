using FlakeId;
using MemoryPack;
using SightKeeper.Domain;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.Model.Images;

internal sealed class PackableImage : Image
{
	static PackableImage()
	{
		MemoryPackFormatterProvider.Register(new PackableImageFormatter());
	}

	public Id Id { get; }

	public PackableImage(DateTimeOffset creationTimestamp, Vector2<ushort> size, Id id) : base(creationTimestamp, size)
	{
		Id = id;
	}
}