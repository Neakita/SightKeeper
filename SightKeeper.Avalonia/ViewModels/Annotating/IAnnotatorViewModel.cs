using System.Collections.Generic;
using SightKeeper.Avalonia.ViewModels.Elements;

namespace SightKeeper.Avalonia.ViewModels.Annotating;

public interface IAnnotatorViewModel
{
    IReadOnlyCollection<DataSetViewModel> DataSetViewModels { get; }
    SelectedDataSetViewModel? SelectedDataSet { get; }
    bool CanChangeSelectedDataSet { get; }
    AnnotatorScreenshotsViewModel Screenshots { get; }
    ScreenshoterViewModel Screenshoter { get; }
    AnnotatorToolsViewModel ToolsViewModel { get; }
    DrawerViewModel DrawerViewModel { get; }
}