using System;
using System.Reactive.Disposables;
using Avalonia.Input;
using Avalonia.ReactiveUI;
using ReactiveUI;
using SightKeeper.Avalonia.ViewModels.Annotating;

namespace SightKeeper.Avalonia.Views.Annotating;

public sealed partial class AnnotatorScreenshots : ReactiveUserControl<AnnotatorScreenshotsViewModel>, IDisposable
{
    public AnnotatorScreenshots()
    {
        _disposable = this.WhenActivated(OnActivated);
        InitializeComponent();
    }

    public void Dispose() => _disposable.Dispose();

    private readonly IDisposable _disposable;

    private void OnActivated(CompositeDisposable disposable)
    {
        Disposable.Create(OnDeactivated).DisposeWith(disposable);
        ScreenshotsListBox.PointerWheelChanged += OnScreenshotsListBoxWheelScrolled;
    }

    private void OnDeactivated()
    {
        ScreenshotsListBox.PointerWheelChanged -= OnScreenshotsListBoxWheelScrolled;
    }

    private static void OnScreenshotsListBoxWheelScrolled(object? sender, PointerWheelEventArgs e)
    {
        e.Handled = true;
    }
}