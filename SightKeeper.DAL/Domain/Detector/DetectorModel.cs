using System.ComponentModel.DataAnnotations.Schema;
using SightKeeper.DAL.Domain.Abstract;
using SightKeeper.DAL.Domain.Common;

namespace SightKeeper.DAL.Domain.Detector;

[Table("DetectorModels")]
public class DetectorModel : Model
{
	public override IEnumerable<Screenshot> Screenshots => DetectorScreenshots.Cast<Screenshot>().ToList();
	
	public virtual List<DetectorScreenshot> DetectorScreenshots { get; private set; } = new();

	
	public DetectorModel(string name) : this(name, new Resolution()) { }
	public DetectorModel(string name, Resolution resolution) : base(name, resolution) { }


	private DetectorModel(int id, string name) : base(id, name) { }
}