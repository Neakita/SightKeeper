using Avalonia;
using Avalonia.Controls;

namespace SightKeeper.Avalonia.Behaviors.DragAndDrop;

internal sealed class DragDecorations
{
	public Canvas Canvas { get; }
	public Control ItemGhost { get; }
	public Border InsertLine { get; }

	public DragDecorations(Canvas canvas, Control itemGhost, Border insertLine)
	{
		Canvas = canvas;
		ItemGhost = itemGhost;
		InsertLine = insertLine;
	}

	public void MoveItemGhost(Point position)
	{
		SetPositionAtCanvas(ItemGhost, position);
	}

	public void MoveInsertLine(Point position)
	{
		SetPositionAtCanvas(InsertLine, position - new Vector(0, DragAndDropOrderBehavior.InsertLineHalfHeight));
	}

	private static void SetPositionAtCanvas(AvaloniaObject element, Point position)
	{
		Canvas.SetLeft(element, position.X);
		Canvas.SetTop(element, position.Y);
	}
}