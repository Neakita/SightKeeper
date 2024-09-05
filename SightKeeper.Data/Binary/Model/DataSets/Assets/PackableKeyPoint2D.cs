using MemoryPack;
using SightKeeper.Domain.Model;

namespace SightKeeper.Data.Binary.Model.DataSets.Assets;

// TODO ISSUE KeyPoint2D actually doesn't exist right now
/// <summary>
/// MemoryPackable version of <see cref="KeyPoint2D"/>
/// </summary>
[MemoryPackable]
internal sealed partial class PackableKeyPoint2D
{
	public Vector2<double> Position { get; }

	public PackableKeyPoint2D(Vector2<double> position)
	{
		Position = position;
	}
}