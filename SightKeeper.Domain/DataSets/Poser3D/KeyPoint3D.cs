using SightKeeper.Domain.DataSets.Poser;

namespace SightKeeper.Domain.DataSets.Poser3D;

public interface KeyPoint3D : KeyPoint
{
	bool IsVisible { get; set; }
}