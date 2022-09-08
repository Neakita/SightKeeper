using SightKeeper.Backend.Data.Members.Abstract;

namespace SightKeeper.Backend.Data.Members.Detector;

public sealed class DetectorScreenshot : Screenshot
{
	public List<DetectorItem> Items { get; set; } = new();
}
