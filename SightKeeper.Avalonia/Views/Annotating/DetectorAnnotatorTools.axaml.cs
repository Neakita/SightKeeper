using System;
using System.Reactive.Disposables;
using Avalonia.Controls;
using Avalonia.Input;
using SightKeeper.Application.Extensions;

namespace SightKeeper.Avalonia.Views.Annotating;

internal sealed partial class AnnotatorTools : UserControl, IDisposable
{
    public AnnotatorTools()
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