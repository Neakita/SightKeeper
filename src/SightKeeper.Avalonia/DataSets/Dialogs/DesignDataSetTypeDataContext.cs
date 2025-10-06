namespace SightKeeper.Avalonia.DataSets.Dialogs;

internal sealed class DesignDataSetTypeDataContext(string name) : DataSetTypeDataContext
{
	public string Name => name;
}