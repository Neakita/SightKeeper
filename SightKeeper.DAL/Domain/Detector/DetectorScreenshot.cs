using System.ComponentModel.DataAnnotations.Schema;
using SightKeeper.DAL.Domain.Abstract;

namespace SightKeeper.DAL.Domain.Detector;

[Table("DetectorScreenshots")]
public class DetectorScreenshot : Screenshot
{
	public DetectorScreenshot()
	{
	}


	private DetectorScreenshot(int id, DateTime creationDate) : base(id, creationDate)
	{
	}

	public virtual List<DetectorItem> Items { get; } = new();
}