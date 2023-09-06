using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.ReactiveUI;
using CommunityToolkit.Diagnostics;
using SightKeeper.Avalonia.ViewModels.Annotating;

namespace SightKeeper.Avalonia.Views.Annotating;

public sealed partial class DetectorItem : ReactiveUserControl<DetectorItemViewModel>
{
	public DetectorItem()
	{
		InitializeComponent();
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

    private Vector GetNormalizedVector(VectorEventArgs args)
    {
	    Guard.IsNotNull(ViewModel);
	    var drawer = ViewModel.Drawer;
	    Guard.IsNotNull(drawer.ImageSize);
	    return new Vector(args.Vector.X / drawer.ImageSize.Value.Width, args.Vector.Y / drawer.ImageSize.Value.Height);
    }

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