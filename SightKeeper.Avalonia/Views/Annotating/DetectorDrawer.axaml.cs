using System.Collections.Generic;
using System.Reactive.Disposables;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Layout;
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
	    return GetNormalizedPosition(position);
    }

    private Point GetNormalizedPosition(Point position) => new(position.X / Image.Bounds.Width, position.Y / Image.Bounds.Height);

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

    private void OnThumbDragStarted(object? sender, VectorEventArgs e)
    {
	    if (ViewModel == null)
		    return;
	    var thumb = GetThumb(sender);
	    var item = GetItem(thumb);
	    var resizeMode = GetResizeDirection(thumb);
	    ViewModel.Resizer.BeginResize(item, resizeMode);
    }

    private void OnThumbDragDelta(object? sender, VectorEventArgs e)
    {
	    Guard.IsNotNull(ViewModel);
	    ViewModel.Resizer.UpdateResize(GetNormalizedVector(e));
    }

    private void OnThumbDragCompleted(object? sender, VectorEventArgs e)
    {
	    Guard.IsNotNull(ViewModel);
	    ViewModel.Resizer.EndResize();
    }

    private Vector GetNormalizedVector(VectorEventArgs args) =>
	    new(args.Vector.X / Image.Bounds.Width, args.Vector.Y / Image.Bounds.Height);

    private static Thumb GetThumb(object? sender)
    {
	    Guard.IsNotNull(sender);
	    return (Thumb)sender;
    }

    private static DetectorItemViewModel GetItem(IDataContextProvider thumb)
    {
	    var thumbDataContext = thumb.DataContext;
	    Guard.IsNotNull(thumbDataContext);
	    Guard.IsOfType<DetectorItemViewModel>(thumbDataContext);
	    return (DetectorItemViewModel)thumbDataContext;
    }

    private static ResizeDirection GetResizeDirection(Layoutable thumb) =>
	    ResizeDirections[(thumb.VerticalAlignment, thumb.HorizontalAlignment)];

    private static readonly Dictionary<(VerticalAlignment, HorizontalAlignment), ResizeDirection> ResizeDirections = new()
    {
	    { (VerticalAlignment.Top, HorizontalAlignment.Left), ResizeDirection.TopLeft },
	    { (VerticalAlignment.Top, HorizontalAlignment.Center), ResizeDirection.Top },
	    { (VerticalAlignment.Top, HorizontalAlignment.Right), ResizeDirection.TopRight },
	    { (VerticalAlignment.Bottom, HorizontalAlignment.Left), ResizeDirection.BottomLeft },
	    { (VerticalAlignment.Bottom, HorizontalAlignment.Center), ResizeDirection.Bottom },
	    { (VerticalAlignment.Bottom, HorizontalAlignment.Right), ResizeDirection.BottomRight },
	    { (VerticalAlignment.Center, HorizontalAlignment.Left), ResizeDirection.Left },
	    { (VerticalAlignment.Center, HorizontalAlignment.Center), ResizeDirection.All },
	    { (VerticalAlignment.Center, HorizontalAlignment.Right), ResizeDirection.Right }
    };
}