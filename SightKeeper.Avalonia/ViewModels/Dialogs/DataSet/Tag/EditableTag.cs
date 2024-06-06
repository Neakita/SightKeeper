using Avalonia.Media;
using SightKeeper.Application.DataSets;

namespace SightKeeper.Avalonia.ViewModels.Dialogs.DataSet.Tag;

public interface EditableTag
{
    string Name { get; set; }
    Color Color { get; set; }

    TagInfo ToTagInfo();
}