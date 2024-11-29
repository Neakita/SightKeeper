using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets.Poser;

namespace SightKeeper.Data.Binary.Model.DataSets.Assets;

/// <summary>
/// MemoryPackable version of <see cref="KeyPoint"/>
/// </summary>
internal abstract class PackableKeyPoint
{
	public int Index { get; }
	public Vector2<double> Position { get; }

	protected PackableKeyPoint(int index, Vector2<double> position)
	{
		Index = index;
		Position = position;
	}
}