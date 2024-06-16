using SightKeeper.Domain.Model.DataSets.Detector;

namespace SightKeeper.Domain.Model.DataSets;

public abstract class DataSet
{
	public string Name { get; set; }
	public string Description { get; set; }
	public Game? Game { get; set; }
	public ushort Resolution { get; }
	public ScreenshotsLibrary Screenshots { get; }
	public DetectorWeightsLibrary Weights { get; }

	public override string ToString() => Name;

	protected DataSet(string name, ushort resolution)
	{
		Name = name;
		Description = string.Empty;
		Resolution = resolution;
		Screenshots = new ScreenshotsLibrary();
		Weights = new DetectorWeightsLibrary();
	}
}