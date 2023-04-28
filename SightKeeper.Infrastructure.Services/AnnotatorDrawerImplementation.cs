using Avalonia;
using ReactiveUI;
using SerilogTimings;
using SightKeeper.Application.Annotating;
using SightKeeper.Common;
using SightKeeper.Data;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Model.Detector;

namespace SightKeeper.Infrastructure.Services;

public sealed class AnnotatorDrawerImplementation : ReactiveObject, AnnotatorDrawer
{
	private readonly AppDbContextFactory _dbContextFactory;
	private DetectorScreenshot? _screenshot;
	private ItemClass? _itemClass;

	public event Action<DetectorItem>? Drawn;

	public DetectorScreenshot? Screenshot
	{
		get => _screenshot;
		set
		{
			if (_drawing) throw new Exception();
			if (_screenshot != null) _screenshot.Items = null!;
			this.RaiseAndSetIfChanged(ref _screenshot, value);
			if (_screenshot != null)
			{
				using AppDbContext dbContext = _dbContextFactory.CreateDbContext();
				dbContext.Attach(_screenshot);
				dbContext.Entry(_screenshot).Collection(shot => shot.Items).Load();
			}
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

	public AnnotatorDrawerImplementation(AppDbContextFactory dbContextFactory)
	{
		_dbContextFactory = dbContextFactory;
	}

	public bool BeginDrawing(Point startPosition)
	{
		if (_drawing) return false;
		if (ItemClass == null) return false;
		Screenshot.ThrowIfNull(nameof(Screenshot));
		_drawing = true;
		startPosition = ClampToNormalized(startPosition);
		_startPosition = startPosition;
		_dbContext = _dbContextFactory.CreateDbContext();
		_dbContext.Attach(Screenshot!);
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
		Operation operation = Operation.Begin("Сохранение аннотации");
		ThrowHelper.ThrowIf(!_drawing, "Cannot update drawing because no currently drawing");
		_item.ThrowIfNull(nameof(_item));
		_dbContext.ThrowIfNull(nameof(_dbContext));
		_drawing = false;
		finishPosition = ClampToNormalized(finishPosition);
		_item!.BoundingBox.SetFromTwoPositions(_startPosition, finishPosition);
		_dbContext!.SaveChanges();
		Drawn?.Invoke(_item);
		_item = null;
		operation.Complete();
	}

	private bool _drawing;
	private Point _startPosition;
	private DetectorItem? _item;
	private AppDbContext? _dbContext;

	private static Point ClampToNormalized(Point position) =>
		new(Math.Clamp(position.X, 0, 1), 
			Math.Clamp(position.Y, 0, 1));
}
