using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Xaml.Interactivity;
using CommunityToolkit.Diagnostics;
using SightKeeper.Avalonia.Annotation.Drawing.BoundingTransform.Corners;
using SightKeeper.Avalonia.Annotation.Drawing.BoundingTransform.Sides;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets.Assets;

namespace SightKeeper.Avalonia.Annotation.Drawing.BoundingTransform;

internal sealed class DragableBoundingBehavior : Behavior<Control>
{
	public static readonly StyledProperty<Control?> ContainerProperty =
		AvaloniaProperty.Register<DragableBoundingBehavior, Control?>(nameof(Container));

	public static readonly StyledProperty<Thumb?> LeftThumbProperty =
		AvaloniaProperty.Register<DragableBoundingBehavior, Thumb?>(nameof(LeftThumb));

	public static readonly StyledProperty<Thumb?> TopLeftThumbProperty =
		AvaloniaProperty.Register<DragableBoundingBehavior, Thumb?>(nameof(TopLeftThumb));

	public static readonly StyledProperty<Thumb?> TopThumbProperty =
		AvaloniaProperty.Register<DragableBoundingBehavior, Thumb?>(nameof(TopThumb));

	public static readonly StyledProperty<Thumb?> TopRightThumbProperty =
		AvaloniaProperty.Register<DragableBoundingBehavior, Thumb?>(nameof(TopRightThumb));

	public static readonly StyledProperty<Thumb?> RightThumbProperty =
		AvaloniaProperty.Register<DragableBoundingBehavior, Thumb?>(nameof(RightThumb));

	public static readonly StyledProperty<Thumb?> BottomRightThumbProperty =
		AvaloniaProperty.Register<DragableBoundingBehavior, Thumb?>(nameof(BottomRightThumb));

	public static readonly StyledProperty<Thumb?> BottomThumbProperty =
		AvaloniaProperty.Register<DragableBoundingBehavior, Thumb?>(nameof(BottomThumb));

	public static readonly StyledProperty<Thumb?> BottomLeftThumbProperty =
		AvaloniaProperty.Register<DragableBoundingBehavior, Thumb?>(nameof(BottomLeftThumb));

	public static readonly StyledProperty<Thumb?> HorizontalMoveThumbProperty =
		AvaloniaProperty.Register<DragableBoundingBehavior, Thumb?>(nameof(HorizontalMoveThumb));

	public static readonly StyledProperty<Thumb?> VerticalMoveThumbProperty =
		AvaloniaProperty.Register<DragableBoundingBehavior, Thumb?>(nameof(VerticalMoveThumb));

	public static readonly StyledProperty<Thumb?> MoveThumbProperty =
		AvaloniaProperty.Register<DragableBoundingBehavior, Thumb?>(nameof(MoveThumb));

	public static readonly StyledProperty<Bounding> ActualBoundingProperty =
		AvaloniaProperty.Register<DragableBoundingBehavior, Bounding>(nameof(ActualBounding),
			defaultBindingMode: BindingMode.TwoWay);

	public static readonly StyledProperty<Bounding> DisplayBoundingProperty =
		AvaloniaProperty.Register<DragableBoundingBehavior, Bounding>(nameof(DisplayBounding),
			defaultBindingMode: BindingMode.OneWayToSource);

	[ResolveByName]
	public Control? Container
	{
		get => GetValue(ContainerProperty);
		set => SetValue(ContainerProperty, value);
	}

	[ResolveByName]
	public Thumb? LeftThumb
	{
		get => GetValue(LeftThumbProperty);
		set => SetValue(LeftThumbProperty, value);
	}

	[ResolveByName]
	public Thumb? TopLeftThumb
	{
		get => GetValue(TopLeftThumbProperty);
		set => SetValue(TopLeftThumbProperty, value);
	}

	[ResolveByName]
	public Thumb? TopThumb
	{
		get => GetValue(TopThumbProperty);
		set => SetValue(TopThumbProperty, value);
	}

	[ResolveByName]
	public Thumb? TopRightThumb
	{
		get => GetValue(TopRightThumbProperty);
		set => SetValue(TopRightThumbProperty, value);
	}

	[ResolveByName]
	public Thumb? RightThumb
	{
		get => GetValue(RightThumbProperty);
		set => SetValue(RightThumbProperty, value);
	}

	[ResolveByName]
	public Thumb? BottomRightThumb
	{
		get => GetValue(BottomRightThumbProperty);
		set => SetValue(BottomRightThumbProperty, value);
	}

	[ResolveByName]
	public Thumb? BottomThumb
	{
		get => GetValue(BottomThumbProperty);
		set => SetValue(BottomThumbProperty, value);
	}

	[ResolveByName]
	public Thumb? BottomLeftThumb
	{
		get => GetValue(BottomLeftThumbProperty);
		set => SetValue(BottomLeftThumbProperty, value);
	}

	[ResolveByName]
	public Thumb? HorizontalMoveThumb
	{
		get => GetValue(HorizontalMoveThumbProperty);
		set => SetValue(HorizontalMoveThumbProperty, value);
	}

	[ResolveByName]
	public Thumb? VerticalMoveThumb
	{
		get => GetValue(VerticalMoveThumbProperty);
		set => SetValue(VerticalMoveThumbProperty, value);
	}

	[ResolveByName]
	public Thumb? MoveThumb
	{
		get => GetValue(MoveThumbProperty);
		set => SetValue(MoveThumbProperty, value);
	}

	public Bounding ActualBounding
	{
		get => GetValue(ActualBoundingProperty);
		set => SetValue(ActualBoundingProperty, value);
	}

	public Bounding DisplayBounding
	{
		get => GetValue(DisplayBoundingProperty);
		set => SetValue(DisplayBoundingProperty, value);
	}

	protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
	{
		base.OnPropertyChanged(change);
		if (change is AvaloniaPropertyChangedEventArgs<Thumb?> thumbChange)
		{
			var oldThumb = thumbChange.OldValue.Value;
			if (oldThumb != null)
				oldThumb.DragStarted -= OnThumbDragStarted;
			var newThumb = thumbChange.NewValue.Value;
			if (newThumb != null)
				newThumb.DragStarted += OnThumbDragStarted;
		}
		if (change.Property == ActualBoundingProperty)
			DisplayBounding = ActualBounding;
	}

	private void OnThumbDragStarted(object? sender, VectorEventArgs e)
	{
		Guard.IsNotNull(sender);
		var thumb = (Thumb)sender;
		thumb.DragDelta += OnThumbDragDelta;
		thumb.DragCompleted += OnThumbDragCompleted;
		Guard.IsNull(_transformer);
		_transformer = CreateTransformer(ActualBounding, thumb.HorizontalAlignment, thumb.VerticalAlignment);
		var containerSize = Container.Bounds.Size;
		_transformer.MinimumSize = new Vector2<double>(1 / containerSize.Width * 20, 1 / containerSize.Height * 20);
	}

	private void OnThumbDragDelta(object? sender, VectorEventArgs e)
	{
		Guard.IsNotNull(_transformer);
		Guard.IsNotNull(Container);
		var containerSize = Container.Bounds.Size;
		DisplayBounding = _transformer.Transform(new Vector(e.Vector.X / containerSize.Width, e.Vector.Y / containerSize.Height));
	}

	private void OnThumbDragCompleted(object? sender, VectorEventArgs e)
	{
		Guard.IsNotNull(sender);
		var thumb = (Thumb)sender;
		thumb.DragDelta -= OnThumbDragDelta;
		thumb.DragCompleted -= OnThumbDragCompleted;
		ActualBounding = DisplayBounding;
		_transformer = null;
	}

	private static BoundingTransformer CreateTransformer(
		Bounding bounding,
		HorizontalAlignment horizontalAlignment,
		VerticalAlignment verticalAlignment) =>
		(horizontalAlignment, verticalAlignment) switch
		{
			(HorizontalAlignment.Left, VerticalAlignment.Top) => new TopLeftCornerBoundingTransformer(bounding),
			(HorizontalAlignment.Right, VerticalAlignment.Top) => new TopRightCornerBoundingTransformer(bounding),
			(HorizontalAlignment.Left, VerticalAlignment.Bottom) => new BottomLeftCornerBoundingTransformer(bounding),
			(HorizontalAlignment.Right, VerticalAlignment.Bottom) => new BottomRightCornerBoundingTransformer(bounding),
			(HorizontalAlignment.Left, VerticalAlignment.Center) => new LeftSideBoundingTransformer(bounding),
			(HorizontalAlignment.Center, VerticalAlignment.Top) => new TopSideBoundingTransformer(bounding),
			(HorizontalAlignment.Right, VerticalAlignment.Center) => new RightSideBoundingTransformer(bounding),
			(HorizontalAlignment.Center, VerticalAlignment.Bottom) => new BottomSideBoundingTransformer(bounding),
			(HorizontalAlignment.Stretch, VerticalAlignment.Top or VerticalAlignment.Center or VerticalAlignment.Bottom) => new HorizontalMoveBoundingTransformer(bounding),
			(HorizontalAlignment.Left or HorizontalAlignment.Center or HorizontalAlignment.Right, VerticalAlignment.Stretch) => new VerticalMoveBoundingTransformer(bounding),
			(HorizontalAlignment.Center,VerticalAlignment.Center) or (HorizontalAlignment.Stretch, VerticalAlignment.Stretch) => new MoveBoundingTransformer(bounding),
			_ => throw new ArgumentOutOfRangeException()
		};

	private BoundingTransformer? _transformer;
}