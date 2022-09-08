using System.ComponentModel.DataAnnotations.Schema;
using SightKeeper.Backend.Data.Members.Abstract;

namespace SightKeeper.Backend.Data.Members.Detector;

public sealed class DetectorModel : Model
{
	[NotMapped] public override IEnumerable<Screenshot> Screenshots => DetectorScreenshots.Cast<Screenshot>().ToList();
	
	public List<DetectorScreenshot> DetectorScreenshots { get; set; } = new();
}