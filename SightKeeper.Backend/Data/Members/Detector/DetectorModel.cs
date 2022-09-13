using SightKeeper.Backend.Data.Members.Abstract;
using SightKeeper.Backend.Data.Members.Common;

namespace SightKeeper.Backend.Data.Members.Detector;

public sealed class DetectorModel : Model
{
	public override IEnumerable<Screenshot> Screenshots => DetectorScreenshots.Cast<Screenshot>().ToList();
	
	public List<DetectorScreenshot> DetectorScreenshots { get; set; } = new();
	
	public DetectorModel() { }
	
	public DetectorModel(string name, Resolution resolution, Game? game = null) : base(name, resolution) { }
}