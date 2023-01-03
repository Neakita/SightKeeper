namespace SightKeeper.Backend;

public sealed class ScreenshotAreaInfo
{
	public ushort HorizontalOffset { get; set; }
	public ushort VerticalOffset { get; set; }
	private byte ScreenIndex { get; set; }
}