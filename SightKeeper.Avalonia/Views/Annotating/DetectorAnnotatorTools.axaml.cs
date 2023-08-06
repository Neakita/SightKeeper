using System;
using System.Reactive.Disposables;
using Avalonia.Input;
using Avalonia.ReactiveUI;
using ReactiveUI;
using SightKeeper.Avalonia.ViewModels.Annotating;

namespace SightKeeper.Avalonia.Views.Annotating;

public sealed partial class DetectorAnnotatorTools : ReactiveUserControl<DetectorAnnotatorToolsViewModel>, IDisposable
{
    public DetectorAnnotatorTools()
    {
        _disposable = this.WhenActivated(OnActivated);
        InitializeComponent();
    }

    public void Dispose() => _disposable.Dispose();

    private readonly IDisposable _disposable;

    private void OnActivated(CompositeDisposable disposable)
    {
        Disposable.Create(OnDeactivated).DisposeWith(disposable);
        ItemClassesListBox.PointerWheelChanged += OnItemClassesListBoxWheelScrolled;
    }

    private void OnDeactivated()
    {
        ItemClassesListBox.PointerWheelChanged -= OnItemClassesListBoxWheelScrolled;
    }

    private static void OnItemClassesListBoxWheelScrolled(object? sender, PointerWheelEventArgs e)
    {
        e.Handled = true;
    }
}