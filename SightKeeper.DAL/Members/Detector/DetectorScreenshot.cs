using SightKeeper.DAL.Members.Abstract;

namespace SightKeeper.DAL.Members.Detector;

public sealed class DetectorScreenshot : Screenshot
{
	public DetectorModel Model { get; private set; }
	public ICollection<DetectorItem> Items { get; private set; } = new List<DetectorItem>();


	private DetectorScreenshot(Guid id) : base(id)
	{
		Model = null!;
	}
}