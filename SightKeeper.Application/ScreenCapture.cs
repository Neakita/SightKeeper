using Avalonia.Media.Imaging;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Application;

public interface ScreenCapture
{
	IBitmap Capture();
	
	bool IsEnabled { get; set; }
	Game? Game { get; set; }
	public ushort Width { get; set; }
	public ushort Height { get; set; }
	public ushort XOffset { get; set; }
	public ushort YOffset { get; set; }
}