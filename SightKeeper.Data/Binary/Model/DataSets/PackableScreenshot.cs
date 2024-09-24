using FlakeId;
using MemoryPack;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets.Screenshots;

namespace SightKeeper.Data.Binary.Model.DataSets;

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