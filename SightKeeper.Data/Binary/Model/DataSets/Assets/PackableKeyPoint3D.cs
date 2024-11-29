using MemoryPack;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets.Poser3D;

namespace SightKeeper.Data.Binary.Model.DataSets.Assets;

/// <summary>
/// MemoryPackable version of <see cref="KeyPoint3D"/>
/// </summary>
[MemoryPackable]
internal sealed partial class PackableKeyPoint3D : PackableKeyPoint
{
	public bool IsVisible { get; }

	public PackableKeyPoint3D(int index, Vector2<double> position, bool isVisible) : base(index, position)
	{
		IsVisible = isVisible;
	}
}