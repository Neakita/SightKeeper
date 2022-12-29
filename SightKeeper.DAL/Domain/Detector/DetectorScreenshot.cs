using System.ComponentModel.DataAnnotations.Schema;
using SightKeeper.DAL.Domain.Abstract;

namespace SightKeeper.DAL.Domain.Detector;

[Table("DetectorScreenshots")]
public class DetectorScreenshot : Screenshot
{
	public DetectorScreenshot(DetectorModel model)
	{
		Model = model;
	}


	private DetectorScreenshot(int id, DateTime creationDate) : base(id, creationDate)
	{
		Model = null!;
	}

	public virtual List<DetectorItem> Items { get; } = new();
	
	public DetectorModel Model { get; private set; }
}