using System.ComponentModel.DataAnnotations.Schema;
using SightKeeper.Backend.Data.Members.Abstract;

namespace SightKeeper.Backend.Data.Members.Detector;

public sealed class DetectorModel : Model
{
	[NotMapped] public override List<Screenshot> Screenshots { get; } = new();

	public bool Tracking { get; set; } = false;
	public List<DetectorScreenshot> DetectorScreenshots { get; set; } = new();
}