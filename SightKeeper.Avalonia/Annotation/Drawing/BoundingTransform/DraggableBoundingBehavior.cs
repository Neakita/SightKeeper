using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Xaml.Interactivity;
using CommunityToolkit.Diagnostics;
using SightKeeper.Avalonia.Annotation.Drawing.BoundingTransform.Transformers;
using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Assets;

namespace SightKeeper.Avalonia.Annotation.Drawing.BoundingTransform;

internal sealed class DraggableBoundingBehavior : Behavior<Control>
{
	private const string DraggingStyleClass = "dragging";

	public static readonly StyledProperty<Size> CanvasSizeProperty =
		AvaloniaProperty.Register<DraggableBoundingBehavior, Size>(nameof(CanvasSize));

	public static readonly StyledProperty<Panel?> ThumbsPanelProperty =
		AvaloniaProperty.Register<DraggableBoundingBehavior, Panel?>(nameof(ThumbsPanel));

	public static readonly StyledProperty<Bounding> BoundingProperty =
		AvaloniaProperty.Register<DraggableBoundingBehavior, Bounding>(nameof(Bounding),
			defaultBindingMode: BindingMode.TwoWay);

	public static readonly StyledProperty<double> MinimumBoundingSizeProperty =
		AvaloniaProperty.Register<DraggableBoundingBehavior, double>(nameof(MinimumBoundingSize), 20);

	public static readonly StyledProperty<Layoutable?> ItemContainerProperty =
		AvaloniaProperty.Register<DraggableBoundingBehavior, Layoutable?>(nameof(ItemContainer));

	public static readonly StyledProperty<Control?> ItemProperty =
		AvaloniaProperty.Register<DraggableBoundingBehavior, Control?>(nameof(Item));

	public static readonly StyledProperty<ListBox?> ListBoxProperty =
		AvaloniaProperty.Register<DraggableBoundingBehavior, ListBox?>(nameof(ListBox));

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

	public Layoutable? ItemContainer
	{
		get => GetValue(ItemContainerProperty);
		set => SetValue(ItemContainerProperty, value);
	}

	public Control? Item
	{
		get => GetValue(ItemProperty);
		set => SetValue(ItemProperty, value);
	}

	public ListBox? ListBox
	{
		get => GetValue(ListBoxProperty);
		set => SetValue(ListBoxProperty, value);
	}

	private Bounding _bounding;
	private BoundingTransformer? _transformer;
	private Cursor? _hiddenCursor;

	private void OnThumbDragStarted(object? sender, VectorEventArgs e)
	{
		Guard.IsNotNull(sender);
		var thumb = (Thumb)sender;
		thumb.DragDelta += OnThumbDragDelta;
		thumb.DragCompleted += OnThumbDragCompleted;
		Guard.IsNull(_transformer);
		_transformer = CreateTransformer(thumb.HorizontalAlignment, thumb.VerticalAlignment);
		_transformer.MinimumSize = new Vector2<double>(1 / CanvasSize.Width * MinimumBoundingSize,
			1 / CanvasSize.Height * MinimumBoundingSize);
		_bounding = Bounding;
		HideThumbs();
		_hiddenCursor = thumb.Cursor;
		thumb.Cursor = new Cursor(StandardCursorType.None);
		HideOtherItems();
		Item?.Classes.Add(DraggingStyleClass);
		ItemContainer?.Classes.Add(DraggingStyleClass);
	}

	private static BoundingTransformer CreateTransformer(
		HorizontalAlignment horizontalAlignment,
		VerticalAlignment verticalAlignment)
	{
		List<BoundingTransformer> transformers = new(2);
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

	private void HideOtherItems()
	{
		if (ListBox == null)
			return;
		for (int i = 0; i < ListBox.Items.Count; i++)
		{
			var container = ListBox.ContainerFromIndex(i);
			Guard.IsNotNull(container);
			if (container == ItemContainer)
				continue;
			container.IsVisible = false;
		}
	}

	private void HideThumbs()
	{
		Guard.IsNotNull(ThumbsPanel);
		ThumbsPanel.Opacity = 0;
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
		var container = ItemContainer;
		Guard.IsNotNull(container);
		Canvas.SetLeft(container, _bounding.Left * CanvasSize.Width);
		Canvas.SetTop(container, _bounding.Top * CanvasSize.Height);
		var item = Item;
		Guard.IsNotNull(item);
		item.Width = _bounding.Width * CanvasSize.Width;
		item.Height = _bounding.Height * CanvasSize.Height;
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
		ShowThumbs();
		thumb.Cursor = _hiddenCursor;
		_hiddenCursor = null;
		ShowHiddenItems();
		Item?.Classes.Remove(DraggingStyleClass);
		ItemContainer?.Classes.Remove(DraggingStyleClass);
	}

	private void ClearItemDisplayValues()
	{
		var container = ItemContainer;
		Guard.IsNotNull(container);
		container.ClearValue(Canvas.LeftProperty);
		container.ClearValue(Canvas.TopProperty);
		var item = Item;
		Guard.IsNotNull(item);
		item.ClearValue(Layoutable.WidthProperty);
		item.ClearValue(Layoutable.HeightProperty);
	}

	private void ShowThumbs()
	{
		Guard.IsNotNull(ThumbsPanel);
		ThumbsPanel.ClearValue(Visual.OpacityProperty);
	}

	private void ShowHiddenItems()
	{
		if (ListBox == null)
			return;
		for (int i = 0; i < ListBox.Items.Count; i++)
		{
			var container = ListBox.ContainerFromIndex(i);
			Guard.IsNotNull(container);
			if (container == ItemContainer)
				continue;
			container.ClearValue(Visual.IsVisibleProperty);
		}
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