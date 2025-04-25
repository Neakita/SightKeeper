namespace SightKeeper.Avalonia.ImageSets.Capturing;

public interface CapturingSettingsDataContext
{
	ushort MaximumWidth { get; }
	ushort MaximumHeight { get; }
	ushort Width { get; set; }
	ushort Height { get; set; }
	double? FrameRateLimit { get; set; }
}