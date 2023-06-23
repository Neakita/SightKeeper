using SightKeeper.Domain.Model.Abstract;
using SightKeeper.Domain.Model.Detector;

namespace SightKeeper.Domain.Model.Common;

public sealed class Profile : Entity
{
	public string Name { get; set; }
	public string Description { get; set; } = string.Empty;
	public Game? Game { get; set; }
	public DetectorModel DetectorModel { get; set; }
	
	public Profile(string name, DetectorModel detectorModel)
	{
		Name = name;
		DetectorModel = detectorModel;
	}
	
	private Profile(int id, string name, string description) : base(id)
	{
		Name = name;
		Description = description;
		DetectorModel = null!;
	}
}