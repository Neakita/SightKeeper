using System.ComponentModel.DataAnnotations.Schema;
using SightKeeper.Domain.Abstract;
using SightKeeper.Domain.Common;

namespace SightKeeper.Domain.Detector;

[Table("DetectorModels")]
public class DetectorModel : Model
{
	public DetectorModel(string name) : this(name, new Resolution())
	{
	}

	public DetectorModel(string name, Resolution resolution) : base(name, resolution)
	{
	}


	private DetectorModel(int id, string name) : base(id, name)
	{
	}

	public virtual List<DetectorScreenshot> DetectorScreenshots { get; } = new();
}