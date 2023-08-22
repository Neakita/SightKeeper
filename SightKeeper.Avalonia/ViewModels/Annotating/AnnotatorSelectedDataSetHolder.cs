using CommunityToolkit.Diagnostics;
using SightKeeper.Avalonia.ViewModels.Elements;
using SightKeeper.Domain.Model.Extensions;

namespace SightKeeper.Avalonia.ViewModels.Annotating;

public sealed class AnnotatorSelectedDataSetHolder
{
    public DataSetViewModel? SelectedDataSetViewModel
    {
        get => _selectedDataSetViewModel;
        set
        {
            if (_selectedDataSetViewModel == value)
                return;
            _selectedDataSetViewModel = value;
            _screenshoterViewModel.DataSet = value?.DataSet;
            _screenshotsViewModel.DataSet = value?.DataSet;
            _annotatorEnvironmentHolder.DataSetModelType = value?.DataSet.GetModelType();
            if (value == null)
                return;
            var currentEnvironment = _annotatorEnvironmentHolder.CurrentEnvironment;
            Guard.IsNotNull(currentEnvironment);
            currentEnvironment.DataSetViewModel = value;
        }
    }

    public AnnotatorSelectedDataSetHolder(AnnotatorEnvironmentHolder annotatorEnvironmentHolder, ScreenshoterViewModel screenshoterViewModel, AnnotatorScreenshotsViewModel screenshotsViewModel)
    {
        _annotatorEnvironmentHolder = annotatorEnvironmentHolder;
        _screenshoterViewModel = screenshoterViewModel;
        _screenshotsViewModel = screenshotsViewModel;
    }
    
    private readonly AnnotatorEnvironmentHolder _annotatorEnvironmentHolder;
    private readonly ScreenshoterViewModel _screenshoterViewModel;
    private readonly AnnotatorScreenshotsViewModel _screenshotsViewModel;
    private DataSetViewModel? _selectedDataSetViewModel;
}