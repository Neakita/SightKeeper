using System.Collections.ObjectModel;
using SightKeeper.Avalonia.ViewModels.Elements;

namespace SightKeeper.Avalonia.ViewModels.Annotating;

public interface IAnnotatingViewModel
{
    ReadOnlyObservableCollection<ModelViewModel> Models { get; }
    ModelViewModel? SelectedModel { get; set; }
    bool CanChangeSelectedModel { get; }
    AnnotatorScreenshotsViewModel Screenshots { get; }
    ScreenshoterViewModel Screenshoter { get; }
    AnnotatorTools? Tools { get; }
    AnnotatorWorkSpace? WorkSpace { get; }
}