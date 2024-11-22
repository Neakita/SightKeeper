using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Xaml.Interactivity;
using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets.Assets;

namespace SightKeeper.Avalonia.Annotation.Drawing.BoundingTransform;

internal sealed class DragableBoundingBehavior : Behavior<Control>
{
	public static readonly StyledProperty<Control?> CanvasProperty =
		AvaloniaProperty.Register<DragableBoundingBehavior, Control?>(nameof(Canvas));

	public static readonly StyledProperty<Panel?> ThumbsPanelProperty =
		AvaloniaProperty.Register<DragableBoundingBehavior, Panel?>(nameof(ThumbsPanel));

	public static readonly StyledProperty<Bounding> ActualBoundingProperty =
		AvaloniaProperty.Register<DragableBoundingBehavior, Bounding>(nameof(ActualBounding),
			defaultBindingMode: BindingMode.TwoWay);

	public static readonly StyledProperty<Bounding> DisplayBoundingProperty =
		AvaloniaProperty.Register<DragableBoundingBehavior, Bounding>(nameof(DisplayBounding),
			defaultBindingMode: BindingMode.OneWayToSource);

	protected override void OnAttachedToVisualTree()
	{
		GenerateThumbs();
	}

	protected override void OnDetachedFromVisualTree()
	{
		foreach (var thumb in ThumbsPanel.Children.OfType<Thumb>())
		{
			thumb.DragStarted -= OnThumbDragStarted;
		}
	}

	[ResolveByName]
	public Control? Canvas
	{
		get => GetValue(CanvasProperty);
		set => SetValue(CanvasProperty, value);
	}

	[ResolveByName]
	public Panel? ThumbsPanel
	{
		get => GetValue(ThumbsPanelProperty);
		set => SetValue(ThumbsPanelProperty, value);
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
		_transformer = CreateTransformer(thumb.HorizontalAlignment, thumb.VerticalAlignment);
		var containerSize = Canvas.Bounds.Size;
		_transformer.MinimumSize = new Vector2<double>(1 / containerSize.Width * 20, 1 / containerSize.Height * 20);
	}

	private void OnThumbDragDelta(object? sender, VectorEventArgs e)
	{
		Guard.IsNotNull(_transformer);
		Guard.IsNotNull(Canvas);
		var containerSize = Canvas.Bounds.Size;
		DisplayBounding = _transformer.Transform(DisplayBounding,
			new Vector2<double>(e.Vector.X / containerSize.Width, e.Vector.Y / containerSize.Height));
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
		HorizontalAlignment horizontalAlignment,
		VerticalAlignment verticalAlignment)
	{
		List<BoundingTransformer> transformers = new(2);
		if (horizontalAlignment == HorizontalAlignment.Stretch)
			transformers.Add(new HorizontalMoveBoundingTransformer());
		if (verticalAlignment == VerticalAlignment.Stretch)
			transformers.Add(new VerticalMoveBoundingTransformer());
		if (transformers.Count > 0)
			return new AggregateBoundingTransformer(transformers);
		var horizontalSide = horizontalAlignment.ToOptionalSide();
		var verticalSide = verticalAlignment.ToOptionalSide();
		if (horizontalSide != null)
			transformers.Add(new BoundingSideTransformer(horizontalSide.Value));
		if (verticalSide != null)
			transformers.Add(new BoundingSideTransformer(verticalSide.Value));
		if (transformers.Count > 1)
			return new AggregateBoundingTransformer(transformers);
		return transformers.Single();
	}

	private BoundingTransformer? _transformer;

	private void GenerateThumbs()
	{
		ReadOnlySpan<HorizontalAlignment> horizontalAlignments =
		[
			HorizontalAlignment.Stretch, HorizontalAlignment.Left, HorizontalAlignment.Center, HorizontalAlignment.Right
		];
		ReadOnlySpan<VerticalAlignment> verticalAlignments =
		[
			VerticalAlignment.Stretch, VerticalAlignment.Top, VerticalAlignment.Center, VerticalAlignment.Bottom
		];
		foreach (var horizontalAlignment in horizontalAlignments)
		foreach (var verticalAlignment in verticalAlignments)
		{
			if (horizontalAlignment == HorizontalAlignment.Center && verticalAlignment == VerticalAlignment.Center)
				continue;
			if (horizontalAlignment == HorizontalAlignment.Stretch && verticalAlignment is VerticalAlignment.Center or VerticalAlignment.Bottom)
				continue;
			if (verticalAlignment == VerticalAlignment.Stretch && horizontalAlignment is HorizontalAlignment.Center or HorizontalAlignment.Right)
				continue;
			Thumb thumb = new();
			thumb.DragStarted += OnThumbDragStarted;
			thumb.HorizontalAlignment = horizontalAlignment;
			thumb.VerticalAlignment = verticalAlignment;
			ThumbsPanel.Children.Add(thumb);
		}
	}
}