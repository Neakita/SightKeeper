using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain.Model.DataSets.Poser;

public abstract class KeyPoint
{
	public abstract PoserItem Item { get; }
	public abstract KeyPointTag Tag { get; }
	public int Index => Tag.Index;
	public Vector2<double> Position
	{
		get => field;
		set
		{
			Guard.IsBetweenOrEqualTo(value.X, 0, 1);
			Guard.IsBetweenOrEqualTo(value.Y, 0, 1);
			field = value;
		}
	}

	protected KeyPoint(Vector2<double> position)
	{
		Position = position;
	}
}