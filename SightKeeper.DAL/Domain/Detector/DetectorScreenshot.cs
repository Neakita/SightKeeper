using System.ComponentModel.DataAnnotations.Schema;
using SightKeeper.DAL.Domain.Abstract;

namespace SightKeeper.DAL.Domain.Detector;

[Table("DetectorScreenshots")]
public class DetectorScreenshot : Screenshot
{
	public virtual List<DetectorItem> Items { get; private set; } = new();


	public DetectorScreenshot() { }


	private DetectorScreenshot(int id, DateTime creationDate) : base(id, creationDate) { }
}