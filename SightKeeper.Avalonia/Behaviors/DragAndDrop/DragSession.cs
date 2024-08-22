using System.Collections.Immutable;
using Avalonia;
using Avalonia.Controls;

namespace SightKeeper.Avalonia.Behaviors.DragAndDrop;

internal sealed class DragSession
{
	public DragDecorations? Decorations { get; set; }
	public Control DraggingItemContainer { get; }
	public object DraggingItem { get; }
	public int DraggingItemIndex { get; }
	public ImmutableArray<double> InsertLinePositions { get; }
	public Point InitialPosition { get; }
	public bool IsThresholdCrossed { get; set; }

	public DragSession(
		Control draggingItemContainer,
		object draggingItem,
		ImmutableArray<double> insertLinePositions,
		int draggingItemIndex,
		Point initialPosition)
	{
		DraggingItemContainer = draggingItemContainer;
		DraggingItem = draggingItem;
		InsertLinePositions = insertLinePositions;
		DraggingItemIndex = draggingItemIndex;
		InitialPosition = initialPosition;
	}
}