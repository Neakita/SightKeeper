using SightKeeper.Domain.Model.Common;
using Image = SightKeeper.Domain.Model.Common.Image;

namespace SightKeeper.Application;

public interface ScreenCapture
{
	Image Capture();
	Task<Image> CaptureAsync(CancellationToken cancellationToken = default);

	Game? Game { get; set; }
	public Resolution? Resolution { get; set; }
	public ushort XOffset { get; set; }
	public ushort YOffset { get; set; }
	public bool CanCapture { get; }
}