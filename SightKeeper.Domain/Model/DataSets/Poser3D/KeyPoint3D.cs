namespace SightKeeper.Domain.Model.DataSets.Poser3D;

public sealed class KeyPoint3D
{
	public Vector2<double> Position { get; set; }
	public bool IsVisible { get; set; }

	public KeyPoint3D(Vector2<double> position, bool isVisible)
	{
		Position = position;
		IsVisible = isVisible;
	}
}