using SightKeeper.Domain.Model.DataSets.Poser;

namespace SightKeeper.Domain.Model.DataSets.Poser2D;

public sealed class KeyPoint2D : KeyPoint
{
	public override Poser2DItem Item { get; }
	public override KeyPointTag2D Tag { get; }

	public KeyPoint2D(Vector2<double> position, Poser2DItem item, KeyPointTag2D tag) : base(position)
	{
		Item = item;
		Tag = tag;
	}
}