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
	public DateTime CreationDate { get; }
	public Vector2<ushort> Resolution { get; }

	public PackableScreenshot(DateTime creationDate, Vector2<ushort> resolution)
	{
		CreationDate = creationDate;
		Resolution = resolution;
	}
}