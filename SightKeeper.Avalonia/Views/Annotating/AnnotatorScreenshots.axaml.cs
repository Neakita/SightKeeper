using System;
using System.Reactive.Disposables;
using Avalonia.Controls;
using Avalonia.Input;
using SightKeeper.Application.Extensions;

namespace SightKeeper.Avalonia.Views.Annotating;

internal sealed partial class AnnotatorScreenshots : UserControl, IDisposable
{
    public AnnotatorScreenshots()
    {
	    throw new NotImplementedException();
	    // _disposable = this.WhenActivated(OnActivated);
	    // InitializeComponent();
    }

    public void Dispose() => _disposable.Dispose();

    private readonly IDisposable _disposable = null!;

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