using Avalonia.Media;

namespace SightKeeper.Avalonia.DataSets.Dialogs.Tags.Plain;

public interface EditableTagDataContext
{
	string Name { get; set; }
	Color Color { get; set; }
}