namespace SightKeeper.Domain.Model.Common;

public sealed class Resolution
{
	public int Width { get; private set; }
	public int Height { get; private set; }
	
	public Resolution()
	{
		Width = 320;
		Height = 320;
	}
	
	public Resolution(int width, int height)
	{
		Width = width;
		Height = height;
	}
}