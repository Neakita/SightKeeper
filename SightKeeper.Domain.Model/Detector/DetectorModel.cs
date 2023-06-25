using System.ComponentModel.DataAnnotations.Schema;
using SightKeeper.Domain.Model.Abstract;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Domain.Model.Detector;

[Table("DetectorModels")]
public class DetectorModel : Abstract.Model
{
	public ICollection<Screenshot> Screenshots {  get; set; }
	public ICollection<DetectorAsset> Assets { get; set; }

	public DetectorModel(string name) : this(name, new Resolution())
	{
	}

	public DetectorModel(string name, ushort width, ushort height) : this(name, new Resolution(width, height))
	{
	}

	public DetectorModel(string name, Resolution resolution) : base(name, resolution)
	{
		Screenshots = new List<Screenshot>();
		Assets = new List<DetectorAsset>();
	}


	private DetectorModel(int id, string name, string description) : base(id, name, description)
	{
		Screenshots = null!;
		Assets = null!;
	}
}