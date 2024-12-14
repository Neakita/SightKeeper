using FlakeId;
using MemoryPack;
using SightKeeper.Domain;
using SightKeeper.Domain.Screenshots;

namespace SightKeeper.Data.Model.DataSets;

/// <summary>
/// MemoryPackable version of <see cref="Screenshot"/>
/// </summary>
[MemoryPackable]
internal sealed partial class PackableScreenshot
{
	public Id Id { get; }
	public DateTimeOffset CreationDate { get; }
	public Vector2<ushort> Resolution { get; }

	public PackableScreenshot(Id id, DateTimeOffset creationDate, Vector2<ushort> resolution)
	{
		Id = id;
		CreationDate = creationDate;
		Resolution = resolution;
	}
}