using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
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

    public bool CanChangeSelectedModel { get; } = true;
    public AnnotatorScreenshotsViewModel Screenshots => new(new MockScreenshotsDataAccess());

    public ScreenshoterViewModel Screenshoter => new(new MockStreamModelScreenshoter());
    
    private sealed class MockStreamModelScreenshoter : StreamModelScreenshoter
    {
        public IObservable<Screenshot> Screenshoted { get; } = new Subject<Screenshot>();
        public IObservable<Screenshot> ScreenshotRemoved { get; } = new Subject<Screenshot>();
        public Model? Model { get; set; }
        public bool IsEnabled { get; set; }
        public byte ScreenshotsPerSecond { get; set; }
        public ushort? MaxImages { get; set; }
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

