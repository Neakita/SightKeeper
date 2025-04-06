using Avalonia.Media;

namespace SightKeeper.Avalonia.DataSets.Dialogs.Tags;

public interface EditableTagDataContext
{
	string Name { get; set; }
	Color Color { get; set; }
}