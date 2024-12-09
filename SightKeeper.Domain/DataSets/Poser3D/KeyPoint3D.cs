using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Domain.DataSets.Poser3D;

public sealed class KeyPoint3D : KeyPoint
{
	public bool IsVisible { get; set; }

	internal KeyPoint3D(Tag tag, Vector2<double> position, bool isVisible) : base(tag, position)
	{
		IsVisible = isVisible;
	}
}