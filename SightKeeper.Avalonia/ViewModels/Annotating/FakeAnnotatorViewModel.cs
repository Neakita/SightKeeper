using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using SightKeeper.Application.Annotating;
using SightKeeper.Avalonia.ViewModels.Elements;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Services;

namespace SightKeeper.Avalonia.ViewModels.Annotating;

public sealed class FakeAnnotatorViewModel : IAnnotatingViewModel
{
    public ReadOnlyObservableCollection<DataSetViewModel> DataSets => new(new ObservableCollection<DataSetViewModel>
    {
        new(new DataSet("Some data set")),
        new(new DataSet("Another data set"))
    });

    public DataSetViewModel? SelectedDataSet { get; set; }

    public bool CanChangeSelectedDataSet => true;
    public AnnotatorScreenshotsViewModel Screenshots => new(new MockScreenshotImageLoader(), new MockScreenshotsDataAccess());

    public ScreenshoterViewModel Screenshoter => new(new MockStreamDataSetScreenshoter());
    public AnnotatorToolsViewModel ToolsViewModel { get; } = null;
    public DrawerViewModel DrawerViewModel { get; } = null;

    private sealed class MockStreamDataSetScreenshoter : StreamDataSetScreenshoter
    {
        public DataSet? DataSet { get; set; }
        public bool IsEnabled { get; set; }
        public byte ScreenshotsPerSecond { get; set; }
    }
    
    private sealed class MockScreenshotImageLoader : ScreenshotImageLoader
    {
        public Image Load(Screenshot screenshot)
        {
            return null;
        }
    }
    
    private sealed class MockScreenshotsDataAccess : ScreenshotsDataAccess
    {
        public Task Load(ScreenshotsLibrary library, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task LoadAsync(ScreenshotsLibrary library, CancellationToken cancellationToken = default)
        {
            throw new NotSupportedException();
        }

        public Task SaveChanges(ScreenshotsLibrary library, CancellationToken cancellationToken)
        {
            throw new NotSupportedException();
        }
    }
}

