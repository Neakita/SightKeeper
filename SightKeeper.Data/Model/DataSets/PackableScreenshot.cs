using FlakeId;
using MemoryPack;
using SightKeeper.Domain;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.Model.DataSets;

/// <summary>
/// MemoryPackable version of <see cref="Screenshot"/>
/// </summary>
[MemoryPackable]
internal sealed partial class PackableScreenshot
{
	public Id Id { get; }
	public DateTimeOffset CreationTimestamp { get; }
	public Vector2<ushort> Resolution { get; }

	public PackableScreenshot(Id id, DateTimeOffset creationTimestamp, Vector2<ushort> resolution)
	{
		Id = id;
		CreationTimestamp = creationTimestamp;
		Resolution = resolution;
	}
}