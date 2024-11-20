using System;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Input;
using Avalonia.Xaml.Interactivity;
using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Model.DataSets.Assets;

namespace SightKeeper.Avalonia.Annotation.Drawing;

internal sealed class DrawingBehavior : Behavior<Canvas>
{
	public static readonly StyledProperty<ICommand?> CommandProperty =
		AvaloniaProperty.Register<DrawingBehavior, ICommand?>(nameof(Command));

	public static readonly StyledProperty<IDataTemplate?> DrawingItemTemplateProperty =
		AvaloniaProperty.Register<DrawingBehavior, IDataTemplate?>(nameof(DrawingItemTemplate));

	public ICommand? Command
	{
		get => GetValue(CommandProperty);
		set => SetValue(CommandProperty, value);
	}

	public IDataTemplate? DrawingItemTemplate
	{
		get => GetValue(DrawingItemTemplateProperty);
		set => SetValue(DrawingItemTemplateProperty, value);
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
		Guard.IsNotNull(AssociatedObject);
		AssociatedObject.PointerMoved += OnAssociatedObjectPointerMoved;
		AssociatedObject.PointerReleased += OnAssociatedObjectPointerReleased;
		_initialPosition = e.GetPosition(AssociatedObject);
		_drawingItem = DrawingItemTemplate?.Build(null);
		if (_drawingItem == null)
			return;
		AssociatedObject.Children.Add(_drawingItem);
		UpdateDrawingItemBounding(_initialPosition);
	}

	private void OnAssociatedObjectPointerMoved(object? sender, PointerEventArgs e)
	{
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
		AssociatedObject.PointerMoved -= OnAssociatedObjectPointerMoved;
		AssociatedObject.PointerReleased -= OnAssociatedObjectPointerReleased;
		if (_drawingItem == null)
			return;
		bool isRemoved = AssociatedObject.Children.Remove(_drawingItem);
		Guard.IsTrue(isRemoved);
		_drawingItem = null;
		var finalPosition = e.GetPosition(AssociatedObject);
		Bounding bounding = CreateBounding(_initialPosition, finalPosition);
		if (Command?.CanExecute(bounding) == true)
			Command.Execute(bounding);
	}

	private void UpdateDrawingItemBounding(Point position)
	{
		var bounding = CreateBounding(_initialPosition, position);
		Guard.IsNotNull(_drawingItem);
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