using System.ComponentModel.DataAnnotations.Schema;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Domain.Model.Detector;

[Table("DetectorModels")]
public class DetectorModel : Abstract.Model
{
	public ICollection<DetectorAsset> DetectorScreenshots {  get; private set; }
	
	public DetectorModel(string name) : this(name, new Resolution())
	{
	}

	public DetectorModel(string name, ushort width, ushort height) : this(name, new Resolution(width, height))
	{
	}

	public DetectorModel(string name, Resolution resolution) : base(name, resolution)
	{
		DetectorScreenshots = new List<DetectorAsset>();
	}


	private DetectorModel(int id, string name, string description) : base(id, name, description)
	{
		DetectorScreenshots = null!;
	}
}