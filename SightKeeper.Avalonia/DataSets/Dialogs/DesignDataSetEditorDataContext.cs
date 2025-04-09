namespace SightKeeper.Avalonia.DataSets.Dialogs;

internal sealed class DesignDataSetEditorDataContext : DataSetEditorDataContext
{
	public string Name { get; init; } = string.Empty;
	public string Description { get; init; } = string.Empty;
}