using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Application;

public interface ScreenCapture
{
	byte[] Capture();
	Task<byte[]> CaptureAsync(CancellationToken cancellationToken = default);

	Game? Game { get; set; }
	public Resolution? Resolution { get; set; }
	public ushort XOffset { get; set; }
	public ushort YOffset { get; set; }
	public bool CanCapture { get; }
}