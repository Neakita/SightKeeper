using Avalonia;
using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Model.Detector;

namespace SightKeeper.Avalonia.ViewModels.Annotating;

public sealed class AnnotatorDrawerViewModel : ViewModel
{
    public bool CanBeginDrawing => _drawingData == null;
    public bool CanUpdateDrawing => _drawingData != null;
    public bool CanEndDrawing => _drawingData != null;
    
    public void BeginDrawing(Point startPosition)
    {
        Guard.IsNull(_drawingData);
        _drawingData = new DrawingData(startPosition, new BoundingBoxViewModel(new BoundingBox()));
        _drawingData.UpdateBounding(startPosition);
    }

    public void UpdateDrawing(Point intermediatePosition)
    {
        Guard.IsNotNull(_drawingData);
        _drawingData.UpdateBounding(intermediatePosition);
    }

    public void EndDrawing(Point finishPosition)
    {
        Guard.IsNotNull(_drawingData);
        _drawingData.UpdateBounding(finishPosition);
    }

    private DrawingData? _drawingData;

    private sealed class DrawingData
    {
        public Point StartPosition { get; set; }
        public BoundingBoxViewModel EditableBounding { get; }

        public DrawingData(Point startPosition, BoundingBoxViewModel editableBounding)
        {
            StartPosition = startPosition;
            EditableBounding = editableBounding;
        }

        public void UpdateBounding(Point currentPosition) =>
            EditableBounding.SetFromTwoPositions(StartPosition.X, StartPosition.Y, currentPosition.X, currentPosition.Y);
    }
}