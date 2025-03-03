﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Xaml.Interactivity;
using CommunityToolkit.Diagnostics;

namespace SightKeeper.Avalonia.Behaviors.DragAndDrop;

internal abstract class DragAndDropOrderBehavior : Behavior<ItemsControl>
{
	internal const double InsertLineHeight = 4;
	internal const double InsertLineHalfHeight = InsertLineHeight / 2;

	public static readonly StyledProperty<double> DragMoveThresholdProperty =
		AvaloniaProperty.Register<DragAndDropOrderBehavior, double>(nameof(DragMoveThreshold));

	public static readonly StyledProperty<Brush?> InsertLineFillProperty =
		AvaloniaProperty.Register<DragAndDropOrderBehavior, Brush?>(nameof(InsertLineFill));

	public double DragMoveThreshold
	{
		get => GetValue(DragMoveThresholdProperty);
		set => SetValue(DragMoveThresholdProperty, value);
	}

	public Brush? InsertLineFill
	{
		get => GetValue(InsertLineFillProperty);
		set => SetValue(InsertLineFillProperty, value);
	}

	protected override void OnAttachedToVisualTree()
	{
		Guard.IsNotNull(AssociatedObject);
		_overlayLayer = OverlayLayer.GetOverlayLayer(AssociatedObject);
		AssociatedObject.AddHandler(InputElement.PointerPressedEvent, OnPointerPressed, RoutingStrategies.Tunnel);
		AssociatedObject.Background ??= Brushes.Transparent;
	}

	protected override void OnDetachedFromVisualTree()
	{
		Guard.IsNotNull(AssociatedObject);
		_overlayLayer = null;
		AssociatedObject.RemoveHandler(InputElement.PointerPressedEvent, OnPointerPressed);
	}

	protected abstract Control? FindAncestorItemContainer(Visual visual);

	private DragSession? _dragSession;
	private OverlayLayer? _overlayLayer;

	private void OnPointerPressed(object? sender, PointerPressedEventArgs args)
	{
		Guard.IsNotNull(AssociatedObject);
		Guard.IsNotNull(args.Source);
		var visual = (Visual)args.Source;
		var itemContainer = FindAncestorItemContainer(visual);
		if (itemContainer == null)
			return;
		Guard.IsNotNull(AssociatedObject.ItemFromContainer(itemContainer));
		Guard.IsNull(_dragSession);
		_dragSession = InitializeDragSession(itemContainer, args.GetPosition(null));
		if (DragMoveThreshold == 0)
			_dragSession.Decorations = InitializeDragDecorations(itemContainer, args.GetPosition);
		BeginDragging();
	}

	private void OnPointerMoved(object? sender, PointerEventArgs args)
	{
		Guard.IsNotNull(AssociatedObject);
		Guard.IsNotNull(_dragSession);

		if (!_dragSession.IsThresholdCrossed)
		{
			var currentPosition = args.GetPosition(null);
			if (Point.Distance(_dragSession.InitialPosition, currentPosition) < DragMoveThreshold)
				return;
			_dragSession.IsThresholdCrossed = true;
		}
		
		if (_dragSession.Decorations == null)
			_dragSession.Decorations ??=
				InitializeDragDecorations(_dragSession.DraggingItemContainer, args.GetPosition);
		else
		{
			_dragSession.Decorations.MoveItemGhost(args.GetPosition(_dragSession.Decorations.Canvas));
			
			var insertLineRelativeToAssociatedObject = new Point(0, Closest(args.GetPosition(AssociatedObject).Y, _dragSession.InsertLinePositions));
			var translatedPoint = AssociatedObject.TranslatePoint(insertLineRelativeToAssociatedObject, _dragSession.Decorations.Canvas);
			Guard.IsNotNull(translatedPoint);
			_dragSession.Decorations.MoveInsertLine(translatedPoint.Value);
		}
	}

	private void OnPointerReleased(object? sender, PointerReleasedEventArgs args)
	{
		Guard.IsNotNull(_dragSession);
		Guard.IsNotNull(AssociatedObject);
		Guard.IsNotNull(AssociatedObject.ItemsSource);
		var dragSession = _dragSession;
		if (_dragSession.Decorations != null)
			RemoveDragDecorations();
		EndDragging();
		if (!dragSession.IsThresholdCrossed)
			return;
		var originalIndex = dragSession.DraggingItemIndex;
		var targetIndex = IndexOfClosest(args.GetPosition(AssociatedObject).Y, dragSession.InsertLinePositions);
		var item = dragSession.DraggingItem;
		if (targetIndex > originalIndex)
			targetIndex--;
		if (targetIndex == originalIndex)
			return;
		var list = (IList)AssociatedObject.ItemsSource;
		list.RemoveAt(originalIndex);
		list.Insert(targetIndex, item);
	}

	private DragSession InitializeDragSession(Control itemContainer, Point initialPosition)
	{
		Guard.IsNotNull(AssociatedObject);
		var itemIndex = AssociatedObject.IndexFromContainer(itemContainer);
		var item = AssociatedObject.ItemFromContainer(itemContainer);
		Guard.IsNotNull(item);
		DragSession session = new(itemContainer, item, GetInsertLinePositions(AssociatedObject).ToImmutableArray(), itemIndex, initialPosition);
		return session;
	}

	protected abstract Control CreateItemGhost(Control itemContainer);

	private DragDecorations InitializeDragDecorations(
		Control itemContainer,
		Func<Visual, Point> pointerPositionFactory)
	{
		Guard.IsNotNull(AssociatedObject);
		Guard.IsNotNull(_dragSession);
		Canvas canvas = new();
		var itemGhost = CreateItemGhost(itemContainer);
		Border insertLine = new()
		{
			Width = AssociatedObject.Bounds.Width,
			Height = InsertLineHeight,
			CornerRadius = new CornerRadius(2),
			Background = InsertLineFill
		};
		var item = AssociatedObject.ItemFromContainer(itemContainer);
		Guard.IsNotNull(item);
		DragDecorations decorations = new(canvas, itemGhost, insertLine);
		canvas.Children.Add(itemGhost);
		canvas.Children.Add(insertLine);
		Guard.IsNotNull(_overlayLayer);
		_overlayLayer.Children.Add(canvas);
		var pointerPosition = pointerPositionFactory(canvas);
		decorations.MoveItemGhost(pointerPosition);
		
		var insertLineRelativeToAssociatedObject = new Point(0, Closest(pointerPositionFactory(AssociatedObject).Y, _dragSession.InsertLinePositions));
		var translatedPoint = AssociatedObject.TranslatePoint(insertLineRelativeToAssociatedObject, canvas);
		Guard.IsNotNull(translatedPoint);
		decorations.MoveInsertLine(translatedPoint.Value);
		
		return decorations;
	}

	private void BeginDragging()
	{
		Guard.IsNotNull(AssociatedObject);
		AssociatedObject.AddHandler(InputElement.PointerMovedEvent, OnPointerMoved);
		AssociatedObject.AddHandler(InputElement.PointerReleasedEvent, OnPointerReleased);
	}

	private void EndDragging()
	{
		Guard.IsNotNull(AssociatedObject);
		AssociatedObject.RemoveHandler(InputElement.PointerMovedEvent, OnPointerMoved);
		AssociatedObject.RemoveHandler(InputElement.PointerReleasedEvent, OnPointerReleased);
		_dragSession = null;
	}

	private void RemoveDragDecorations()
	{
		Guard.IsNotNull(_overlayLayer);
		Guard.IsNotNull(_dragSession);
		Guard.IsNotNull(_dragSession.Decorations);
		_overlayLayer.Children.Remove(_dragSession.Decorations.Canvas);
		_dragSession.Decorations.Canvas.Children.Remove(_dragSession.Decorations.ItemGhost);
		_dragSession.Decorations = null;
	}

	private static IEnumerable<double> GetInsertLinePositions(ItemsControl itemsControl)
	{
		Guard.IsNotNull(itemsControl.Presenter);
		Guard.IsNotNull(itemsControl.Presenter.Panel);
		var panel = (StackPanel)itemsControl.Presenter.Panel;
		Guard.IsTrue(panel.Orientation == Orientation.Vertical);
		var halfSpacing = panel.Spacing / 2;
		yield return -halfSpacing;
		foreach (var child in panel.Children)
			yield return child.Bounds.Bottom + halfSpacing;
	}

	private static int IndexOfClosest(double pointerPosition, ImmutableArray<double> positions)
	{
		Guard.IsGreaterThanOrEqualTo(positions.Length, 2);
		for (int i = 0; i < positions.Length - 1; i++)
		{
			var currentItem = positions[i];
			var nextItem = positions[i + 1];
			var currentItemDistance = Math.Abs(currentItem - pointerPosition);
			var nextItemDistance = Math.Abs(nextItem - pointerPosition);
			if (nextItemDistance > currentItemDistance)
				return i;
		}
		return positions.Length - 1;
	}

	private static double Closest(double pointerPosition, ImmutableArray<double> positions)
	{
		var index = IndexOfClosest(pointerPosition, positions);
		return positions[index];
	}
}