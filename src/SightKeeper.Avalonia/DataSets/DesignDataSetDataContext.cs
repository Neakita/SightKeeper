namespace SightKeeper.Avalonia.DataSets;

internal sealed class DesignDataSetDataContext : DataSetDataContext
{
	public string Name { get; }

	public DesignDataSetDataContext(string name)
	{
		Name = name;
	}
}