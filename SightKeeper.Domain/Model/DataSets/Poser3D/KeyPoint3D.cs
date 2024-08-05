namespace SightKeeper.Domain.Model.DataSets.Poser3D;

public sealed class KeyPoint3D
{
	public Vector2<double> Position { get; set; }
	public KeyPointTag3D Tag { get; }
	public bool IsVisible { get; set; }

	internal KeyPoint3D(Vector2<double> position, KeyPointTag3D tag, bool isVisible)
	{
		Position = position;
		Tag = tag;
		IsVisible = isVisible;
	}
}