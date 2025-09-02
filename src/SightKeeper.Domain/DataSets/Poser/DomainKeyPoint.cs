using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Domain.DataSets.Poser;

public sealed class DomainKeyPoint(KeyPoint inner) : KeyPoint
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
}