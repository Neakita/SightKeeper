using System.Reactive.Disposables;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.ReactiveUI;
using CommunityToolkit.Diagnostics;
using ReactiveUI;
using SightKeeper.Avalonia.Extensions;
using SightKeeper.Avalonia.ViewModels.Annotating;

namespace SightKeeper.Avalonia.Views.Annotating;

public sealed partial class DetectorDrawer : ReactiveUserControl<DetectorDrawerViewModel>
{
	public DetectorDrawer()
    {
        InitializeComponent();
        this.WhenActivated(OnActivated);
    }

    protected override void OnInitialized()
    {
	    Image.PointerPressed += ImageOnPointerPressed;
    }
    
    private TopLevel? _topLevel;

    private void ImageOnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
	    if (ViewModel == null || !ViewModel.CanBeginDrawing)
		    return;
	    var normalizedPosition = GetNormalizedPosition(e);
	    ViewModel.BeginDrawing(normalizedPosition);
	    Image.PointerPressed -= ImageOnPointerPressed;
	    var topLevel = this.GetTopLevel();
	    topLevel.PointerReleased += OnPointerReleased;
	    topLevel.PointerMoved += OnPointerMoved;
    }
    
    private void OnPointerMoved(object? sender, PointerEventArgs e)
    {
	    Guard.IsNotNull(ViewModel);
	    var normalizedPosition = GetNormalizedPosition(e);
	    ViewModel.UpdateDrawing(normalizedPosition);
    }

    private void OnPointerReleased(object? sender, PointerReleasedEventArgs e)
    {
	    Guard.IsNotNull(ViewModel);
	    this.GetTopLevel().PointerReleased -= OnPointerReleased;
	    this.GetTopLevel().PointerMoved -= OnPointerMoved;
	    Image.PointerPressed += ImageOnPointerPressed;
	    var normalizedPosition = GetNormalizedPosition(e);
	    ViewModel.EndDrawing(normalizedPosition);
    }

    private Point GetNormalizedPosition(PointerEventArgs e)
    {
	    var position = e.GetPosition(Image);
	    Point normalizedPosition = new(position.X / Image.Bounds.Width, position.Y / Image.Bounds.Height);
	    return normalizedPosition;
    }

    private void OnActivated(CompositeDisposable disposable)
    {
	    Disposable.Create(OnDeactivated).DisposeWith(disposable);
	    _topLevel = this.GetTopLevel();
	    _topLevel.KeyDown += OnTopLevelKeyDown;
	    _topLevel.KeyUp += OnTopLevelKeyUp;
    }

    private void OnDeactivated()
    {
	    Guard.IsNotNull(_topLevel);
	    _topLevel.KeyDown -= OnTopLevelKeyDown;
	    _topLevel.KeyUp -= OnTopLevelKeyUp;
    }

    private void OnTopLevelKeyDown(object? sender, KeyEventArgs e)
    {
	    if (ViewModel != null && e.Key == Key.LeftCtrl)
		    ViewModel.IsItemSelectionEnabled = true;
    }

    private void OnTopLevelKeyUp(object? sender, KeyEventArgs e)
    {
	    if (ViewModel != null && e.Key == Key.LeftCtrl)
		    ViewModel.IsItemSelectionEnabled = false;
    }
}