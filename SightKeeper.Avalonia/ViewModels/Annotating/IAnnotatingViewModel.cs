using System.Collections.ObjectModel;
using SightKeeper.Avalonia.ViewModels.Elements;

namespace SightKeeper.Avalonia.ViewModels.Annotating;

public interface IAnnotatingViewModel
{
    ReadOnlyObservableCollection<DataSetViewModel> DataSets { get; }
    DataSetViewModel? SelectedDataSet { get; set; }
    bool CanChangeSelectedDataSet { get; }
    AnnotatorScreenshotsViewModel Screenshots { get; }
    ScreenshoterViewModel Screenshoter { get; }
    AnnotatorEnvironmentHolder EnvironmentHolder { get; }
}