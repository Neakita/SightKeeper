using MemoryPack;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets.Poser2D;

namespace SightKeeper.Data.Binary.Model.DataSets.Assets;

/// <summary>
/// MemoryPackable version of <see cref="KeyPoint2D"/>
/// </summary>
[MemoryPackable]
internal sealed partial class PackableKeyPoint2D : PackableKeyPoint
{
	public PackableKeyPoint2D(int index, Vector2<double> position) : base(index, position)
	{
	}
}