using System;
using System.Reactive.Disposables;
using Avalonia.Input;
using Avalonia.ReactiveUI;
using ReactiveUI;
using SightKeeper.Avalonia.ViewModels.Annotating;

namespace SightKeeper.Avalonia.Views.Annotating;

internal sealed partial class AnnotatorTools : ReactiveUserControl<AnnotatorToolsViewModel>, IDisposable
{
    public AnnotatorTools()
    {
        _disposable = this.WhenActivated(OnActivated);
        InitializeComponent();
    }

    public void Dispose() => _disposable.Dispose();

    private readonly IDisposable _disposable;

    private void OnActivated(CompositeDisposable disposable)
    {
        Disposable.Create(OnDeactivated).DisposeWith(disposable);
        TagsListBox.PointerWheelChanged += OnTagsListBoxWheelScrolled;
    }

    private void OnDeactivated()
    {
        TagsListBox.PointerWheelChanged -= OnTagsListBoxWheelScrolled;
    }

    private static void OnTagsListBoxWheelScrolled(object? sender, PointerWheelEventArgs e)
    {
        e.Handled = true;
    }
}