using System.ComponentModel.DataAnnotations.Schema;
using SightKeeper.DAL.Domain.Abstract;

namespace SightKeeper.DAL.Domain.Detector;

[Table("DetectorScreenshots")]
public class DetectorScreenshot : Screenshot
{
	public DetectorScreenshot(DetectorModel model, Image image) : base(image)
	{
		Model = model;
		Items = new List<DetectorItem>();
	}


	private DetectorScreenshot(int id) : base(id)
	{
		Model = null!;
		Items = null!;
	}

	public virtual List<DetectorItem> Items { get; private set; }
	public DetectorModel Model { get; private set; }
}