using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Application;

public interface ScreenCapture
{
	byte[] Capture();
	
	bool IsEnabled { get; set; }
	Game? Game { get; set; }
	public Resolution? Resolution { get; set; }
	public ushort XOffset { get; set; }
	public ushort YOffset { get; set; }
}