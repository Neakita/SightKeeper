using SightKeeper.Domain.Model;

namespace SightKeeper.Data.Binary.Model.DataSets.Assets;

internal sealed class KeyPoint2D
{
	public Vector2<double> Position { get; }

	public KeyPoint2D(Vector2<double> position)
	{
		Position = position;
	}
}