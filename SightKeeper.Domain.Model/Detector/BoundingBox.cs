using Microsoft.EntityFrameworkCore;

namespace SightKeeper.Domain.Model.Detector;

[Owned]
public class BoundingBox
{
	public BoundingBox(float x, float y, float width, float height)
	{
		X = x;
		Y = y;
		Width = width;
		Height = height;
	}

	public float X { get; set; }
	public float Y { get; set; }
	public float Width { get; set; }
	public float Height { get; set; }
}