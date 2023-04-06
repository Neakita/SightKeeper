using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Domain.Model.Detector;

[Table("DetectorModels")]
public class DetectorModel : Abstract.Model
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

	public ObservableCollection<DetectorScreenshot> DetectorScreenshots {  get; private set; } = new();
}