using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.ReactiveUI;
using CommunityToolkit.Diagnostics;
using SightKeeper.Avalonia.Extensions;
using SightKeeper.Avalonia.ViewModels.Annotating;

namespace SightKeeper.Avalonia.Views.Annotating;

public sealed partial class DetectorDrawer : ReactiveUserControl<DrawerViewModel>
{
	public DetectorDrawer()
    {
        InitializeComponent();
    }

	protected override void OnLoaded(RoutedEventArgs e)
	{
		base.OnLoaded(e);
		Image.PointerPressed += ImageOnPointerPressed;
		Image.SizeChanged += OnImageSizeChanged;
		_topLevel = this.GetTopLevel();
		_topLevel.KeyDown += OnTopLevelKeyDown;
		_topLevel.KeyUp += OnTopLevelKeyUp;
		if (ViewModel == null)
			return;
		ViewModel.ImageSize = Image.Bounds.Size;
	}

	private void OnImageSizeChanged(object? sender, SizeChangedEventArgs e)
	{
		if (ViewModel == null)
			return;
		ViewModel.ImageSize = e.NewSize;
	}

	protected override void OnUnloaded(RoutedEventArgs e)
	{
		base.OnUnloaded(e);
		Image.SizeChanged += OnImageSizeChanged;
		Guard.IsNotNull(_topLevel);
		_topLevel.KeyDown -= OnTopLevelKeyDown;
		_topLevel.KeyUp -= OnTopLevelKeyUp;
		if (ViewModel == null)
			return;
		ViewModel.ImageSize = null;
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