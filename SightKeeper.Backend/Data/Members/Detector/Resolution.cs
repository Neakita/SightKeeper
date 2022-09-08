namespace SightKeeper.Backend.Data.Members.Detector;

public sealed class Resolution
{
	public Guid Id { get; set; }
	public ushort Width { get; set; }
	public ushort Height { get; set; }


	public Resolution(ushort width = 320, ushort height = 320)
	{
		Width = width;
		Height = height;
	}
}
