using SerilogTimings;
using SightKeeper.Application.Annotating;
using SightKeeper.Data;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Model.Detector;

namespace SightKeeper.Services;

public sealed class AnnotatorDrawerImplementation : AnnotatorDrawer
{
	private readonly AppDbContextFactory _dbContextFactory;
	private DetectorAsset? _screenshot;
	private ItemClass? _itemClass;

	public event Action<DetectorItem>? Drawn;

	public DetectorAsset? Screenshot
	{
		get => _screenshot;
		set
		{
			throw new NotImplementedException();
			if (_drawing) throw new Exception();
			// if (_screenshot != null) _screenshot.Items = null!;
			// this.RaiseAndSetIfChanged(ref _screenshot, value);
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
			throw new NotImplementedException();
			if (_drawing) throw new Exception();
			// this.RaiseAndSetIfChanged(ref _itemClass, value);
		}
	}

	public AnnotatorDrawerImplementation(AppDbContextFactory dbContextFactory)
	{
		_dbContextFactory = dbContextFactory;
	}

	public bool BeginDrawing(Point startPosition)
	{
		throw new NotImplementedException();
		if (_drawing) return false;
		if (ItemClass == null) return false;
		//Screenshot.ThrowIfNull(nameof(Screenshot));
		_drawing = true;
		startPosition = ClampToNormalized(startPosition);
		_startPosition = startPosition;
		_dbContext = _dbContextFactory.CreateDbContext();
		_dbContext.Attach(Screenshot!);
		_item = new DetectorItem(ItemClass!, new BoundingBox(startPosition.X, startPosition.Y, 0, 0));
		//Screenshot!.Items.Add(_item);
		return true;
	}

	public void UpdateDrawing(Point currentPosition)
	{
		throw new NotImplementedException();
		//ThrowHelper.ThrowIf(!_drawing, "Cannot update drawing because no currently drawing");
		//_item.ThrowIfNull(nameof(_item));
		currentPosition = ClampToNormalized(currentPosition);
		//_item!.BoundingBox.SetFromTwoPositions(_startPosition, currentPosition);
	}

	public void EndDrawing(Point finishPosition)
	{
		throw new NotImplementedException();
		Operation operation = Operation.Begin("Сохранение аннотации");
		//ThrowHelper.ThrowIf(!_drawing, "Cannot update drawing because no currently drawing");
		//_item.ThrowIfNull(nameof(_item));
		//_dbContext.ThrowIfNull(nameof(_dbContext));
		_drawing = false;
		finishPosition = ClampToNormalized(finishPosition);
		//_item!.BoundingBox.SetFromTwoPositions(_startPosition, finishPosition);
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
