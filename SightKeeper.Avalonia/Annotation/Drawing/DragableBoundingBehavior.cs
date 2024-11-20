using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Xaml.Interactivity;
using SightKeeper.Domain.Model.DataSets.Assets;

namespace SightKeeper.Avalonia.Annotation.Drawing;

internal sealed class DragableBoundingBehavior : Behavior<Control>
{
	public static readonly StyledProperty<Canvas> CanvasProperty =
		AvaloniaProperty.Register<DragableBoundingBehavior, Canvas>(nameof(Canvas));

	public static readonly StyledProperty<Thumb?> TopLeftThumbProperty =
		AvaloniaProperty.Register<DragableBoundingBehavior, Thumb?>(nameof(TopLeftThumb));

	public static readonly StyledProperty<Thumb?> TopRightThumbProperty =
		AvaloniaProperty.Register<DragableBoundingBehavior, Thumb?>(nameof(TopRightThumb));

	public static readonly StyledProperty<Thumb?> BottomLeftThumbProperty =
		AvaloniaProperty.Register<DragableBoundingBehavior, Thumb?>(nameof(BottomLeftThumb));

	public static readonly StyledProperty<Thumb?> BottomRightThumbProperty =
		AvaloniaProperty.Register<DragableBoundingBehavior, Thumb?>(nameof(BottomRightThumb));

	public static readonly StyledProperty<Bounding> BoundingProperty =
		AvaloniaProperty.Register<DragableBoundingBehavior, Bounding>(nameof(Bounding));

	[ResolveByName]
	public Canvas Canvas
	{
		get => GetValue(CanvasProperty);
		set => SetValue(CanvasProperty, value);
	}

	[ResolveByName]
	public Thumb? TopLeftThumb
	{
		get => GetValue(TopLeftThumbProperty);
		set => SetValue(TopLeftThumbProperty, value);
	}

	[ResolveByName]
	public Thumb? TopRightThumb
	{
		get => GetValue(TopRightThumbProperty);
		set => SetValue(TopRightThumbProperty, value);
	}

	[ResolveByName]
	public Thumb? BottomLeftThumb
	{
		get => GetValue(BottomLeftThumbProperty);
		set => SetValue(BottomLeftThumbProperty, value);
	}

	[ResolveByName]
	public Thumb? BottomRightThumb
	{
		get => GetValue(BottomRightThumbProperty);
		set => SetValue(BottomRightThumbProperty, value);
	}

	public Bounding Bounding
	{
		get => GetValue(BoundingProperty);
		set => SetValue(BoundingProperty, value);
	}
}