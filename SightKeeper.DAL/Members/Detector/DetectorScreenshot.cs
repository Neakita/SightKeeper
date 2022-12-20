using System.ComponentModel.DataAnnotations.Schema;
using SightKeeper.DAL.Members.Abstract;

namespace SightKeeper.DAL.Members.Detector;

[Table("DetectorScreenshots")]
public class DetectorScreenshot : Screenshot
{
	public virtual List<DetectorItem> Items { get; private set; } = new();


	public DetectorScreenshot() { }


	private DetectorScreenshot(Guid id, DateTime creationDate) : base(id, creationDate) { }
}