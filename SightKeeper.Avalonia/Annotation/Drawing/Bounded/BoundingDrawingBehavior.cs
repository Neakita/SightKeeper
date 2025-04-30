using System;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Xaml.Interactivity;
using CommunityToolkit.Diagnostics;
using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Assets.Items;

namespace SightKeeper.Avalonia.Annotation.Drawing.Bounded;

public sealed class BoundingDrawingBehavior : Behavior<Canvas>
{
	private const string HideItemsStyleClass = "hide-items";

	public static readonly StyledProperty<ICommand?> CommandProperty =
		AvaloniaProperty.Register<BoundingDrawingBehavior, ICommand?>(nameof(Command));

	public static readonly StyledProperty<ITemplate<Control>?> DrawingItemTemplateProperty =
		AvaloniaProperty.Register<BoundingDrawingBehavior, ITemplate<Control>?>(nameof(DrawingItemTemplate));

	public static readonly StyledProperty<double> MinimumBoundingSizeProperty =
		AvaloniaProperty.Register<BoundingDrawingBehavior, double>(nameof(MinimumBoundingSize), 20);

	public static readonly StyledProperty<ListBox?> ListBoxProperty =
		AvaloniaProperty.Register<BoundingDrawingBehavior, ListBox?>(nameof(ListBox));

	public ICommand? Command
	{
		get => GetValue(CommandProperty);
		set => SetValue(CommandProperty, value);
	}

	public ITemplate<Control>? DrawingItemTemplate
	{
		get => GetValue(DrawingItemTemplateProperty);
		set => SetValue(DrawingItemTemplateProperty, value);
	}

	public double MinimumBoundingSize
	{
		get => GetValue(MinimumBoundingSizeProperty);
		set => SetValue(MinimumBoundingSizeProperty, value);
	}

	public ListBox? ListBox
	{
		get => GetValue(ListBoxProperty);
		set => SetValue(ListBoxProperty, value);
	}

	protected override void OnAttached()
	{
		Guard.IsNotNull(AssociatedObject);
		AssociatedObject.PointerPressed += OnAssociatedObjectPointerPressed;
	}

	protected override void OnDetaching()
	{
		Guard.IsNotNull(AssociatedObject);
		AssociatedObject.PointerPressed -= OnAssociatedObjectPointerPressed;
	}

	private Point _initialPosition;
	private Control? _drawingItem;

	private void OnAssociatedObjectPointerPressed(object? sender, PointerPressedEventArgs e)
	{
		if (!e.GetCurrentPoint(null).Properties.IsLeftButtonPressed)
			return;
		Guard.IsNotNull(AssociatedObject);
		if (!IsEnabled)
			return;
		Guard.IsNotNull(Command);
		if (!Command.CanExecute(default(Bounding)))
			return;
		AssociatedObject.PointerMoved += OnAssociatedObjectPointerMoved;
		AssociatedObject.PointerReleased += OnAssociatedObjectPointerReleased;
		_initialPosition = e.GetPosition(AssociatedObject);
		ListBox?.Classes.Add(HideItemsStyleClass);
		ShowPreview();
		e.Handled = true;
	}

	private void ShowPreview()
	{
		if (DrawingItemTemplate == null)
			return;
		Guard.IsNotNull(AssociatedObject);
		_drawingItem = DrawingItemTemplate.Build();
		AssociatedObject.Children.Add(_drawingItem);
		UpdateDrawingItemBounding(_initialPosition);
	}

	private void OnAssociatedObjectPointerMoved(object? sender, PointerEventArgs e)
	{
		if (_drawingItem == null)
			return;
		Guard.IsNotNull(AssociatedObject);
		var position = e.GetPosition(AssociatedObject);
		position = new Point(
			Math.Clamp(position.X, 0, AssociatedObject.Bounds.Width),
			Math.Clamp(position.Y, 0, AssociatedObject.Bounds.Height));
		UpdateDrawingItemBounding(position);
	}

	private void OnAssociatedObjectPointerReleased(object? sender, PointerReleasedEventArgs e)
	{
		Guard.IsNotNull(AssociatedObject);
		RemovePreview();
		ListBox?.Classes.Remove(HideItemsStyleClass);
		AssociatedObject.PointerMoved -= OnAssociatedObjectPointerMoved;
		AssociatedObject.PointerReleased -= OnAssociatedObjectPointerReleased;
		var finalPosition = e.GetPosition(AssociatedObject);
		finalPosition = new Point(
			Math.Clamp(finalPosition.X, 0, AssociatedObject.Bounds.Width),
			Math.Clamp(finalPosition.Y, 0, AssociatedObject.Bounds.Height));
		var bounding = CreateBounding(_initialPosition, finalPosition);
		if (!ShouldAnItemBeCreated(bounding))
			return;
		var associatedObjectSize = new Vector2<double>(AssociatedObject.Bounds.Width, AssociatedObject.Bounds.Height);
		var normalizedBounding = bounding / associatedObjectSize;
		Guard.IsNotNull(Command);
		Guard.IsTrue(Command.CanExecute(normalizedBounding));
		Command.Execute(normalizedBounding);
	}

	private void RemovePreview()
	{
		if (_drawingItem == null)
			return;
		Guard.IsNotNull(AssociatedObject);
		bool isRemoved = AssociatedObject.Children.Remove(_drawingItem);
		Guard.IsTrue(isRemoved);
		_drawingItem = null;
	}

	private bool ShouldAnItemBeCreated(Bounding bounding)
	{
		return bounding.Width > MinimumBoundingSize && bounding.Height > MinimumBoundingSize;
	}

	private void UpdateDrawingItemBounding(Point position)
	{
		if (_drawingItem == null)
			return;
		var bounding = CreateBounding(_initialPosition, position);
		Canvas.SetLeft(_drawingItem, bounding.Left);
		Canvas.SetTop(_drawingItem, bounding.Top);
		_drawingItem.Width = bounding.Width;
		_drawingItem.Height = bounding.Height;
	}

	private static Bounding CreateBounding(Point point1, Point point2)
	{
		return new Bounding(point1.X, point1.Y, point2.X, point2.Y);
	}
}