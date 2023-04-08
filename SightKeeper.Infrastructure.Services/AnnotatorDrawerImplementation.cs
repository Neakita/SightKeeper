using Avalonia;
using ReactiveUI;
using SightKeeper.Application.Annotating;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Model.Detector;
using SightKeeper.Infrastructure.Common;

namespace SightKeeper.Infrastructure.Services;

public sealed class AnnotatorDrawerImplementation : ReactiveObject, AnnotatorDrawer
{
	private DetectorScreenshot? _screenshot;
	private ItemClass? _itemClass;

	public event Action<DetectorItem>? Drawn;

	public DetectorScreenshot? Screenshot
	{
		get => _screenshot;
		set
		{
			if (_drawing) throw new Exception();
			this.RaiseAndSetIfChanged(ref _screenshot, value);
		}
	}

	public ItemClass? ItemClass
	{
		get => _itemClass;
		set
		{
			if (_drawing) throw new Exception();
			this.RaiseAndSetIfChanged(ref _itemClass, value);
		}
	}

	public bool BeginDrawing(Point startPosition)
	{
		if (_drawing) return false;
		if (ItemClass == null) return false;
		Screenshot.ThrowIfNull(nameof(Screenshot));
		_drawing = true;
		startPosition = ClampToNormalized(startPosition);
		_startPosition = startPosition;
		_item = new DetectorItem(ItemClass!, new BoundingBox(startPosition.X, startPosition.Y, 0, 0));
		Screenshot!.Items.Add(_item);
		return true;
	}

	public void UpdateDrawing(Point currentPosition)
	{
		ThrowHelper.ThrowIf(!_drawing, "Cannot update drawing because no currently drawing");
		_item.ThrowIfNull(nameof(_item));
		currentPosition = ClampToNormalized(currentPosition);
		_item!.BoundingBox.SetFromTwoPositions(_startPosition, currentPosition);
	}

	public void EndDrawing(Point finishPosition)
	{
		ThrowHelper.ThrowIf(!_drawing, "Cannot update drawing because no currently drawing");
		_item.ThrowIfNull(nameof(_item));
		_drawing = false;
		finishPosition = ClampToNormalized(finishPosition);
		_item!.BoundingBox.SetFromTwoPositions(_startPosition, finishPosition);
		Drawn?.Invoke(_item);
		_item = null;
	}

	private bool _drawing;
	private Point _startPosition;
	private DetectorItem? _item;

	private static Point ClampToNormalized(Point position) =>
		new(Math.Clamp(position.X, 0, 1), 
			Math.Clamp(position.Y, 0, 1));
}
