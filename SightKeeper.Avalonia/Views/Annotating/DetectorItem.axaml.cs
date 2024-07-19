using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Layout;
using CommunityToolkit.Diagnostics;
using SightKeeper.Avalonia.ViewModels.Annotating.Drawer;
using DetectorItemViewModel = SightKeeper.Avalonia.ViewModels.Annotating.Drawer.DetectorItemViewModel;

namespace SightKeeper.Avalonia.Views.Annotating;

internal sealed partial class DetectorItem : UserControl
{
	public DetectorItem()
	{
		InitializeComponent();
	}

	// ReSharper disable UnusedParameter.Local
	private void OnThumbDragStarted(object? sender, VectorEventArgs e)
	{
		throw new NotImplementedException();
		// if (ViewModel == null)
		//  return;
		// var thumb = GetThumb(sender);
		// var item = GetItem(thumb);
		// var resizeMode = GetResizeDirection(thumb);
		// ViewModel.Resizer.BeginResize(item, resizeMode);
	}

    private void OnThumbDragDelta(object? sender, VectorEventArgs e)
    {
	    throw new NotImplementedException();
	    // Guard.IsNotNull(ViewModel);
	    // ViewModel.Resizer.UpdateResize(GetNormalizedVector(e));
    }

    private void OnThumbDragCompleted(object? sender, VectorEventArgs e)
    {
	    throw new NotImplementedException();
	    // Guard.IsNotNull(ViewModel);
	    // ViewModel.Resizer.EndResize();
    }
    // ReSharper restore UnusedParameter.Local

    private Vector GetNormalizedVector(VectorEventArgs args)
    {
	    throw new NotImplementedException();
	    // Guard.IsNotNull(ViewModel);
	    // var drawer = ViewModel.Drawer;
	    // Guard.IsNotNull(drawer.ImageSize);
	    // return new Vector(args.Vector.X / drawer.ImageSize.Value.Width, args.Vector.Y / drawer.ImageSize.Value.Height);
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