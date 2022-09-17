using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SightKeeper.DAL.Members.Detector;

public sealed class BoundingBox
{
	public float X { get; private set; }
	public float Y { get; private set; }
	public float Width { get; private set; }
	public float Height { get; private set; }
	
	
	public BoundingBox(float x, float y, float width, float height)
	{
		X = x;
		Y = y;
		Width = width;
		Height = height;
	}
}