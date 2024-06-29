using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain.Model.DataSets;

public abstract class DataSet
{
	public string Name { get; set; }
	public string Description { get; set; }
	public Game? Game { get; set; }
	public ushort Resolution { get; }

	public override string ToString() => Name;

	protected DataSet(string name, ushort resolution)
	{
		Name = name;
		Description = string.Empty;
		Guard.IsGreaterThan<int>(resolution, 0);
		Guard.IsEqualTo(resolution % 32, 0);
		
		Resolution = resolution;
	}
}