using System.Collections.Generic;
using System.Threading.Tasks;
using SightKeeper.Application.Annotating;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.Detector;
using SightKeeper.Domain.Services;

namespace SightKeeper.Avalonia.ViewModels.Annotating;

public sealed class FakeAnnotatingViewModel : IAnnotatingViewModel
{
    public Task<IReadOnlyCollection<Model>> Models => Task.FromResult((IReadOnlyCollection<Model>)new[]
    {
        new DetectorModel("Some model"),
        new DetectorModel("Another model")
    });

    public Model? SelectedModel { get; set; }

    public bool CanChangeSelectedModel => true;
    public AnnotatorScreenshotsViewModel Screenshots => new(new MockScreenshotsDataAccess());

    public ScreenshoterViewModel Screenshoter => new(new MockStreamModelScreenshoter());
    public AnnotatorTools? Tools => null;
    public AnnotatorWorkSpace? WorkSpace => null;

    private sealed class MockStreamModelScreenshoter : StreamModelScreenshoter
    {
        public Model? Model { get; set; }
        public bool IsEnabled { get; set; }
        public byte ScreenshotsPerSecond { get; set; }
    }

    private sealed class MockScreenshotsDataAccess : ScreenshotsDataAccess
    {
        public void Load(ScreenshotsLibrary library)
        {
        }

        public void SaveChanges(ScreenshotsLibrary library)
        {
        }
    }
}

