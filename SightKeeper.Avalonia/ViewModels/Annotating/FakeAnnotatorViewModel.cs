using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using SightKeeper.Application.Annotating;
using SightKeeper.Avalonia.ViewModels.Elements;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.Detector;
using SightKeeper.Domain.Services;

namespace SightKeeper.Avalonia.ViewModels.Annotating;

public sealed class FakeAnnotatorViewModel : IAnnotatingViewModel
{
    public ReadOnlyObservableCollection<DataSetViewModel> DataSets => new(new ObservableCollection<DataSetViewModel>
    {
        new DataSetViewModel<DetectorAsset>(new DataSet<DetectorAsset>("Some model")),
        new DataSetViewModel<DetectorAsset>(new DataSet<DetectorAsset>("Another model"))
    });

    public DataSetViewModel? SelectedDataSet { get; set; }

    public bool CanChangeSelectedDataSet => true;
    public AnnotatorScreenshotsViewModel Screenshots => new(new MockScreenshotImageLoader(), new MockScreenshotsDataAccess());

    public ScreenshoterViewModel Screenshoter => new(new MockStreamDataSetScreenshoter());
    public AnnotatorEnvironmentHolder EnvironmentHolder { get; } = new(null);
    public AnnotatorEnvironment? Environment => null;

    private sealed class MockStreamDataSetScreenshoter : StreamDataSetScreenshoter
    {
        public DataSet? DataSet { get; set; }
        public bool IsEnabled { get; set; }
        public byte ScreenshotsPerSecond { get; set; }
    }
    
    private sealed class MockScreenshotImageLoader : ScreenshotImageLoader
    {
        public ScreenshotImage Load(Screenshot screenshot)
        {
            return null!;
        }
    }
    
    private sealed class MockScreenshotsDataAccess : ScreenshotsDataAccess
    {
        public void Load(ScreenshotsLibrary library)
        {
        }

        public Task LoadAsync(ScreenshotsLibrary library, CancellationToken cancellationToken = default)
        {
            throw new NotSupportedException();
        }

        public void SaveChanges(ScreenshotsLibrary library)
        {
            throw new NotSupportedException();
        }
    }
}

