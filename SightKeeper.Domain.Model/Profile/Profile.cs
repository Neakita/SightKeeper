using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Model.Detector;

namespace SightKeeper.Domain.Model;

public sealed class Profile
{
	public string Name { get; set; }
	public string Description { get; set; } = string.Empty;
	public Game? Game { get; set; }
	public DetectorDataSet DetectorDataSet { get; set; }
	
	public Profile(string name, DetectorDataSet detectorDataSet)
	{
		Name = name;
		DetectorDataSet = detectorDataSet;
	}
	
	private Profile(string name, string description)
	{
		Name = name;
		Description = description;
		DetectorDataSet = null!;
	}
}