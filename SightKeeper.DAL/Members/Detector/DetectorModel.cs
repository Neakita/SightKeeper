using SightKeeper.DAL.Members.Abstract;
using SightKeeper.DAL.Members.Common;

namespace SightKeeper.DAL.Members.Detector;

public sealed class DetectorModel : Model
{
	public override IEnumerable<Screenshot> Screenshots => DetectorScreenshots.Cast<Screenshot>().ToList();
	
	public ICollection<DetectorScreenshot> DetectorScreenshots { get; } = new List<DetectorScreenshot>();

	
	public DetectorModel(string name, Resolution resolution, Game? game = null) : base(name, resolution, game) { }


	private DetectorModel(Guid id, string name) : base(id, name) { }
}