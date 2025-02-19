using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Xaml.Interactivity;
using SightKeeper.Domain;

namespace SightKeeper.Avalonia.Annotation.Drawing.Poser;

internal sealed class DraggableKeyPointBehavior : Behavior
{
	public static readonly StyledProperty<Thumb?> ThumbProperty =
		AvaloniaProperty.Register<DraggableKeyPointBehavior, Thumb?>(nameof(Thumb));

	public static readonly StyledProperty<Size> CanvasSizeProperty =
		AvaloniaProperty.Register<DraggableKeyPointBehavior, Size>(nameof(CanvasSize));

	public static readonly StyledProperty<ListBoxItem?> ContainerProperty =
		AvaloniaProperty.Register<DraggableKeyPointBehavior, ListBoxItem?>(nameof(Container));

	public static readonly StyledProperty<Vector2<double>> PositionProperty =
		AvaloniaProperty.Register<DraggableKeyPointBehavior, Vector2<double>>(nameof(Position), defaultBindingMode: BindingMode.TwoWay);

	public Thumb? Thumb
	{
		get => GetValue(ThumbProperty);
		set => SetValue(ThumbProperty, value);
	}

	public Size CanvasSize
	{
		get => GetValue(CanvasSizeProperty);
		set => SetValue(CanvasSizeProperty, value);
	}

	public ListBoxItem? Container
	{
		get => GetValue(ContainerProperty);
		set => SetValue(ContainerProperty, value);
	}

	public Vector2<double> Position
	{
		get => GetValue(PositionProperty);
		set => SetValue(PositionProperty, value);
	}

	protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
	{
		base.OnPropertyChanged(change);
		if (change.Property == ThumbProperty)
		{
			var (oldThumb, newThumb) = change.GetOldAndNewValue<Thumb?>();
			if (oldThumb != null)
			{
				oldThumb.DragStarted -= OnDragStarted;
				oldThumb.DragDelta -= OnDragDelta;
				oldThumb.DragCompleted -= OnDragCompleted;
			}
			if (newThumb != null)
			{
				newThumb.DragStarted += OnDragStarted;
				newThumb.DragDelta += OnDragDelta;
				newThumb.DragCompleted += OnDragCompleted;
			}
		}
	}

	private Vector2<double> _position;

	private void OnDragStarted(object? sender, VectorEventArgs args)
	{
		_position = Position * new Vector2<double>(CanvasSize.Width, CanvasSize.Height);
	}

	private void OnDragDelta(object? sender, VectorEventArgs args)
	{
		var delta = args.Vector;
		_position = new Vector2<double>(
			Math.Clamp(_position.X + delta.X, 0, CanvasSize.Width),
			Math.Clamp(_position.Y + delta.Y, 0, CanvasSize.Height));
		UpdatePreview();
	}

	private void UpdatePreview()
	{
		if (Container == null)
			return;
		Canvas.SetLeft(Container, _position.X);
		Canvas.SetTop(Container, _position.Y);
	}

	private void OnDragCompleted(object? sender, VectorEventArgs args)
	{
		Vector2<double> canvasSize = new(CanvasSize.Width, CanvasSize.Height);
		var normalizedPosition = _position / canvasSize;
		SetCurrentValue(PositionProperty, normalizedPosition);
		ClearPreview();
	}

	private void ClearPreview()
	{
		if (Container == null)
			return;
		Container.ClearValue(Canvas.LeftProperty);
		Container.ClearValue(Canvas.TopProperty);
	}
}