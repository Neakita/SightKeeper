using System;
using Avalonia;
using CommunityToolkit.Diagnostics;
using SightKeeper.Application.Annotating;

namespace SightKeeper.Avalonia.ViewModels.Annotating;

public sealed class DetectorItemResizer
{
    private sealed class ResizingData
    {
        public DetectorItemViewModel Item { get; }
        public ResizeDirection Direction { get; }
        
        public ResizingData(DetectorItemViewModel item, ResizeDirection direction, Action<BoundingBoxViewModel, Vector> updateBoundingDelegate)
        {
            _updateBoundingDelegate = updateBoundingDelegate;
            Item = item;
            Direction = direction;
        }

        public void UpdateBounding(Vector delta) => _updateBoundingDelegate(Item.Bounding, delta);

        private readonly Action<BoundingBoxViewModel, Vector> _updateBoundingDelegate;
    }
    
    public DetectorItemResizer(DetectorAnnotator annotator)
    {
        _annotator = annotator;
    }
    
    public void BeginResize(DetectorItemViewModel item, ResizeDirection direction)
    {
        Guard.IsNull(_data);
        _data = new ResizingData(item, direction, GetBoundingUpdateDelegate(direction));
    }

    public void UpdateResize(Vector delta)
    {
        Guard.IsNotNull(_data);
        _data.UpdateBounding(delta);
    }

    public void EndResize()
    {
        Guard.IsNotNull(_data);
        var item = _data.Item.Item;
        Guard.IsNotNull(item);
        _data.Item.Bounding.Synchronize();
        _annotator.Move(item, _data.Item.Bounding.Bounding);
        _data = null;
    }
    
    private readonly DetectorAnnotator _annotator;
    private ResizingData? _data;

    private Action<BoundingBoxViewModel, Vector> GetBoundingUpdateDelegate(ResizeDirection direction) => direction switch
    {
        ResizeDirection.Left => UpdateLeft,
        ResizeDirection.Top => UpdateTop,
        ResizeDirection.Right => UpdateRight,
        ResizeDirection.Bottom => UpdateBottom,
        ResizeDirection.TopLeft => UpdateTopLeft,
        ResizeDirection.TopRight => UpdateTopRight,
        ResizeDirection.BottomRight => UpdateBottomRight,
        ResizeDirection.BottomLeft => UpdateBottomLeft,
        ResizeDirection.All => UpdateAll,
        _ => ThrowHelper.ThrowArgumentOutOfRangeException<Action<BoundingBoxViewModel, Vector>>(nameof(direction), direction, null)
    };

    private static void UpdateLeft(BoundingBoxViewModel bounding, Vector delta)
    {
        var maximum = Math.Max(0, bounding.Right - DetectorDrawerViewModel.MinimumDimensionSize);
        bounding.Left = Math.Clamp(bounding.Left + delta.X, 0, maximum);
    }

    private static void UpdateTop(BoundingBoxViewModel bounding, Vector delta)
    {
        var maximum = Math.Max(0, bounding.Bottom - DetectorDrawerViewModel.MinimumDimensionSize);
        bounding.Top = Math.Clamp(bounding.Top + delta.Y, 0, maximum);
    }

    private static void UpdateRight(BoundingBoxViewModel bounding, Vector delta)
    {
        var minimum = Math.Min(1, bounding.Left + DetectorDrawerViewModel.MinimumDimensionSize);
        bounding.Right = Math.Clamp(bounding.Right + delta.X, minimum, 1);
    }

    private static void UpdateBottom(BoundingBoxViewModel bounding, Vector delta)
    {
        var minimum = Math.Min(1, bounding.Top + DetectorDrawerViewModel.MinimumDimensionSize);
        bounding.Bottom = Math.Clamp(bounding.Bottom + delta.Y, minimum, 1);
    }

    private static void UpdateTopLeft(BoundingBoxViewModel bounding, Vector delta)
    {
        UpdateTop(bounding, delta);
        UpdateLeft(bounding, delta);
    }

    private static void UpdateTopRight(BoundingBoxViewModel bounding, Vector delta)
    {
        UpdateTop(bounding, delta);
        UpdateRight(bounding, delta);
    }

    private static void UpdateBottomLeft(BoundingBoxViewModel bounding, Vector delta)
    {
        UpdateBottom(bounding, delta);
        UpdateLeft(bounding, delta);
    }

    private static void UpdateBottomRight(BoundingBoxViewModel bounding, Vector delta)
    {
        UpdateBottom(bounding, delta);
        UpdateRight(bounding, delta);
    }

    private static void UpdateAll(BoundingBoxViewModel bounding, Vector delta)
    {

        bounding.SetFromTwoPositions(
            Math.Clamp(bounding.Left + delta.X, 0, 1 - bounding.Width),
            Math.Clamp(bounding.Top + delta.Y, 0, 1 - bounding.Height),
            Math.Clamp(bounding.Right + delta.X, bounding.Width, 1),
            Math.Clamp(bounding.Bottom + delta.Y, bounding.Height, 1));
    }
}