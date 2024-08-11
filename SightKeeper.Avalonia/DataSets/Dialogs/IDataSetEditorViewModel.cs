using System.Collections.Generic;
using System.Windows.Input;
using SightKeeper.Avalonia.DataSets.Dialogs.Tag;
using SightKeeper.Domain.Model;

namespace SightKeeper.Avalonia.DataSets.Dialogs;

public interface IDataSetEditorViewModel
{
    string Name { get; set; }
    string Description { get; set; }
    int? Resolution { get; set; }
    IReadOnlyCollection<EditableTag> Tags { get; }
    string NewTagName { get; set; }
    Game? Game { get; set; }
    IReadOnlyCollection<Game> Games { get; }
    
    ICommand AddTagCommand { get; }
    ICommand DeleteTagCommand { get; }
    ICommand ApplyCommand { get; }
    ICommand CancelCommand { get; }
}