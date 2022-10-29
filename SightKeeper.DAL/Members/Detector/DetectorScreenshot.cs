using SightKeeper.DAL.Members.Abstract;

namespace SightKeeper.DAL.Members.Detector;

public sealed class DetectorScreenshot : Screenshot
{
	public List<DetectorItem> Items { get; private set; } = new();


	public DetectorScreenshot() { }


	private DetectorScreenshot(Guid id, DateTime creationDate) : base(id, creationDate) { }
}