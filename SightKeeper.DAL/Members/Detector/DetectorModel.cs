using System.ComponentModel.DataAnnotations.Schema;
using SightKeeper.DAL.Members.Abstract;
using SightKeeper.DAL.Members.Common;

namespace SightKeeper.DAL.Members.Detector;

[Table("DetectorModels")]
public class DetectorModel : Model
{
	public override IEnumerable<Screenshot> Screenshots => DetectorScreenshots.Cast<Screenshot>().ToList();
	
	public virtual List<DetectorScreenshot> DetectorScreenshots { get; private set; } = new();

	
	public DetectorModel(string name) : this(name, new Resolution()) { }
	public DetectorModel(string name, Resolution resolution) : base(name, resolution) { }


	private DetectorModel(Guid id, string name) : base(id, name) { }
}