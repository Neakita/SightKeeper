using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.VisualTree;
using Avalonia.Xaml.Interactivity;
using CommunityToolkit.Diagnostics;
using SightKeeper.Avalonia.Annotation.Drawing.BoundingTransform.Transformers;
using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Assets;

namespace SightKeeper.Avalonia.Annotation.Drawing.BoundingTransform;

internal sealed class DragableBoundingBehavior : Behavior<Control>
{
	public static readonly StyledProperty<Size> CanvasSizeProperty =
		AvaloniaProperty.Register<DragableBoundingBehavior, Size>(nameof(CanvasSize));

	public static readonly StyledProperty<Panel?> ThumbsPanelProperty =
		AvaloniaProperty.Register<DragableBoundingBehavior, Panel?>(nameof(ThumbsPanel));

	public static readonly StyledProperty<Bounding> BoundingProperty =
		AvaloniaProperty.Register<DragableBoundingBehavior, Bounding>(nameof(Bounding),
			defaultBindingMode: BindingMode.TwoWay);

	public static readonly StyledProperty<double> MinimumBoundingSizeProperty =
		AvaloniaProperty.Register<DragableBoundingBehavior, double>(nameof(MinimumBoundingSize), 20);

	public static readonly AttachedProperty<ThumbSideMode> SideModeProperty =
		AvaloniaProperty.RegisterAttached<Thumb, ThumbSideMode>("SideMode", typeof(DragableBoundingBehavior));

	public static void SetSideMode(Thumb element, ThumbSideMode value)
	{
		element.SetValue(SideModeProperty, value);
	}

	public static ThumbSideMode GetSideMode(Thumb element)
	{
		return element.GetValue(SideModeProperty);
	}

	protected override void OnAttachedToVisualTree()
	{
		Guard.IsNotNull(ThumbsPanel);
		foreach (var thumb in ThumbsPanel.Children.OfType<Thumb>())
			SubscribeOnThumb(thumb);
		ThumbsPanel.Children.CollectionChanged += OnThumbsPanelChildrenChanged;
	}

	protected override void OnDetachedFromVisualTree()
	{
		Guard.IsNotNull(ThumbsPanel);
		ThumbsPanel.Children.CollectionChanged -= OnThumbsPanelChildrenChanged;
		foreach (var thumb in ThumbsPanel.Children.OfType<Thumb>())
			UnsubscribeFromThumb(thumb);
	}

	public Size CanvasSize
	{
		get => GetValue(CanvasSizeProperty);
		set => SetValue(CanvasSizeProperty, value);
	}

	[ResolveByName]
	public Panel? ThumbsPanel
	{
		get => GetValue(ThumbsPanelProperty);
		set => SetValue(ThumbsPanelProperty, value);
	}

	public Bounding Bounding
	{
		get => GetValue(BoundingProperty);
		set => SetValue(BoundingProperty, value);
	}

	public double MinimumBoundingSize
	{
		get => GetValue(MinimumBoundingSizeProperty);
		set => SetValue(MinimumBoundingSizeProperty, value);
	}

	private Bounding _bounding;
	private BoundingTransformer? _transformer;

	private void OnThumbDragStarted(object? sender, VectorEventArgs e)
	{
		Guard.IsNotNull(sender);
		var thumb = (Thumb)sender;
		thumb.DragDelta += OnThumbDragDelta;
		thumb.DragCompleted += OnThumbDragCompleted;
		Guard.IsNull(_transformer);
		var sideMode = GetSideMode(thumb);
		_transformer = CreateTransformer(thumb.HorizontalAlignment, thumb.VerticalAlignment, sideMode);
		_transformer.MinimumSize = new Vector2<double>(1 / CanvasSize.Width * MinimumBoundingSize,
			1 / CanvasSize.Height * MinimumBoundingSize);
		_bounding = Bounding;
	}

	private void OnThumbDragDelta(object? sender, VectorEventArgs e)
	{
		Guard.IsNotNull(_transformer);
		_bounding = _transformer.Transform(_bounding,
			new Vector2<double>(e.Vector.X / CanvasSize.Width, e.Vector.Y / CanvasSize.Height));
		UpdateItemPreview();
	}

	private void UpdateItemPreview()
	{
		var item = AssociatedObject.FindAncestorOfType<ListBoxItem>();
		Guard.IsNotNull(item);
		item.Width = _bounding.Width * CanvasSize.Width;
		item.Height = _bounding.Height * CanvasSize.Height;
		Canvas.SetLeft(item, _bounding.Left * CanvasSize.Width);
		Canvas.SetTop(item, _bounding.Top * CanvasSize.Height);
	}

	private void OnThumbDragCompleted(object? sender, VectorEventArgs e)
	{
		Guard.IsNotNull(sender);
		var thumb = (Thumb)sender;
		thumb.DragDelta -= OnThumbDragDelta;
		thumb.DragCompleted -= OnThumbDragCompleted;
		Bounding = _bounding;
		_transformer = null;
		ClearItemDisplayValues();
	}

	private void ClearItemDisplayValues()
	{
		var item = AssociatedObject.FindAncestorOfType<ListBoxItem>();
		Guard.IsNotNull(item);
		item.ClearValue(Layoutable.WidthProperty);
		item.ClearValue(Layoutable.HeightProperty);
		item.ClearValue(Canvas.LeftProperty);
		item.ClearValue(Canvas.TopProperty);
	}

	private static BoundingTransformer CreateTransformer(
		HorizontalAlignment horizontalAlignment,
		VerticalAlignment verticalAlignment,
		ThumbSideMode sideMode)
	{
		List<BoundingTransformer> transformers = new(2);
		if (sideMode == ThumbSideMode.MoveAlong)
		{
			if (horizontalAlignment == HorizontalAlignment.Stretch)
				transformers.Add(new HorizontalMoveBoundingTransformer());
			if (verticalAlignment == VerticalAlignment.Stretch)
				transformers.Add(new VerticalMoveBoundingTransformer());
			if (transformers.Count > 1)
				return new AggregateBoundingTransformer(transformers);
			return transformers.Single();
		}
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

	private void OnThumbsPanelChildrenChanged(object? sender, NotifyCollectionChangedEventArgs e)
	{
		Guard.IsFalse(e.Action == NotifyCollectionChangedAction.Reset);
		if (e.NewItems != null)
			foreach (var thumb in e.NewItems.OfType<Thumb>())
				SubscribeOnThumb(thumb);
		if (e.OldItems != null)
			foreach (var thumb in e.OldItems.OfType<Thumb>())
				UnsubscribeFromThumb(thumb);
	}

	private void SubscribeOnThumb(Thumb thumb)
	{
		thumb.DragStarted += OnThumbDragStarted;
	}

	private void UnsubscribeFromThumb(Thumb thumb)
	{
		thumb.DragStarted -= OnThumbDragStarted;
	}
}