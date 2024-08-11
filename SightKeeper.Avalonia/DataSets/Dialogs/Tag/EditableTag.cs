using Avalonia.Media;
using SightKeeper.Application.DataSets;

namespace SightKeeper.Avalonia.DataSets.Dialogs.Tag;

public interface EditableTag
{
    string Name { get; set; }
    Color Color { get; set; }

    TagInfo ToTagInfo();
}