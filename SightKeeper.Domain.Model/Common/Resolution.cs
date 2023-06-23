namespace SightKeeper.Domain.Model.Common;

public sealed class Resolution : IResolution
{
	public Resolution(int width = 320, int height = 320)
	{
		Width = width;
		Height = height;
	}

	public int Width { get; set; }
	public int Height { get; set; }
}