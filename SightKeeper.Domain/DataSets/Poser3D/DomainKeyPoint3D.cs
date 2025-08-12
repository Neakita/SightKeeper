using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Domain.DataSets.Poser3D;

public sealed class DomainKeyPoint3D(KeyPoint3D inner) : KeyPoint3D
{
	public Tag Tag => inner.Tag;

	public Vector2<double> Position
	{
		get => inner.Position;
		set
		{
			KeyPointPositionConstraintException.ThrowIfNotNormalized(this, value);
			inner.Position = value;
		}
	}

	public bool IsVisible
	{
		get => inner.IsVisible;
		set => inner.IsVisible = value;
	}
}