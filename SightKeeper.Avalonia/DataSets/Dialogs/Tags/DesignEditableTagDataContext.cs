using Avalonia.Media;

namespace SightKeeper.Avalonia.DataSets.Dialogs.Tags;

internal sealed class DesignEditableTagDataContext : EditableTagDataContext
{
	public string Name { get; set; }
	public Color Color { get; set; }

	public DesignEditableTagDataContext(string name)
	{
		Name = name;
		Color = Colors.Transparent;
	}

	public DesignEditableTagDataContext(string name, Color color)
	{
		Name = name;
		Color = color;
	}
}