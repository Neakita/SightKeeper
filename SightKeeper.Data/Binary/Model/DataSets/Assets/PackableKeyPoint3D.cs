using MemoryPack;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets.Poser3D;

namespace SightKeeper.Data.Binary.Model.DataSets.Assets;

/// <summary>
/// MemoryPackable version of <see cref="KeyPoint3D"/>
/// </summary>
[MemoryPackable]
internal sealed partial class PackableKeyPoint3D
{
	public Vector2<double> Position { get; }
	public bool IsVisible { get; }

	public PackableKeyPoint3D(Vector2<double> position, bool isVisible)
	{
		Position = position;
		IsVisible = isVisible;
	}
}