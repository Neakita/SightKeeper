using Avalonia;
using ReactiveUI;
using SightKeeper.Application.Annotating;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Model.Detector;

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

	public void BeginDrawing(Point startPosition)
	{
		if (_drawing) throw new Exception();
		if (Screenshot == null) throw new Exception();
		if (ItemClass == null) throw new Exception();
		_drawing = true;
		_startPosition = startPosition;
		_item = new DetectorItem(ItemClass, new BoundingBox(startPosition.X, startPosition.Y, 0, 0));
		Screenshot.Items.Add(_item);
	}

	public void UpdateDrawing(Point currentPosition)
	{
		if (!_drawing) throw new Exception();
		if (_item == null) throw new Exception();
		_item.BoundingBox.SetFromTwoPositions(_startPosition, currentPosition);
	}

	public void EndDrawing(Point finishPosition)
	{
		if (!_drawing) throw new Exception();
		if (_item == null) throw new Exception();
		_drawing = false;
		_item.BoundingBox.SetFromTwoPositions(_startPosition, finishPosition);
		Drawn?.Invoke(_item);
		_item = null;
	}

	private bool _drawing;
	private Point _startPosition;
	private DetectorItem? _item;
}
