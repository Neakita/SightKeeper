using Microsoft.EntityFrameworkCore;

namespace SightKeeper.DAL.Domain.Detector;

[Owned]
public class BoundingBox
{
	public float X { get; set; }
	public float Y { get; set; }
	public float Width { get; set; }
	public float Height { get; set; }
	
	
	public BoundingBox(float x, float y, float width, float height)
	{
		X = x;
		Y = y;
		Width = width;
		Height = height;
	}
}