using System.Collections.ObjectModel;
using SightKeeper.Avalonia.ViewModels.Elements;

namespace SightKeeper.Avalonia.ViewModels.Annotating;

public interface IAnnotatingViewModel
{
    ReadOnlyObservableCollection<DataSetViewModel> Models { get; }
    DataSetViewModel? SelectedModel { get; set; }
    bool CanChangeSelectedModel { get; }
    AnnotatorScreenshotsViewModel Screenshots { get; }
    ScreenshoterViewModel Screenshoter { get; }
    AnnotatorTools? Tools { get; }
    AnnotatorWorkSpace? WorkSpace { get; }
}