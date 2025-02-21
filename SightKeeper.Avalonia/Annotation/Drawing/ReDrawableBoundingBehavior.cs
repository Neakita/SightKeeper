using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Xaml.Interactivity;
using CommunityToolkit.Diagnostics;
using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Assets;

namespace SightKeeper.Avalonia.Annotation.Drawing;

internal sealed class ReDrawableBoundingBehavior : Behavior<Control>
{
	private const string HideItemsStyleClass = "hide-items";

	public static readonly StyledProperty<Panel?> DrawingCanvasProperty =
		AvaloniaProperty.Register<ReDrawableBoundingBehavior, Panel?>(nameof(DrawingCanvas));

	public static readonly StyledProperty<Panel?> ThumbsPanelProperty =
		AvaloniaProperty.Register<ReDrawableBoundingBehavior, Panel?>(nameof(ThumbsPanel));

	public static readonly StyledProperty<Bounding> BoundingProperty =
		AvaloniaProperty.Register<ReDrawableBoundingBehavior, Bounding>(nameof(Bounding),
			defaultBindingMode: BindingMode.TwoWay);

	public static readonly StyledProperty<ITemplate<Control>?> PreviewTemplateProperty =
		AvaloniaProperty.Register<ReDrawableBoundingBehavior, ITemplate<Control>?>(nameof(PreviewTemplate));

	public static readonly StyledProperty<ListBox?> ListBoxProperty =
		AvaloniaProperty.Register<ReDrawableBoundingBehavior, ListBox?>(nameof(ListBox));

	public Panel? DrawingCanvas
	{
		get => GetValue(DrawingCanvasProperty);
		set => SetValue(DrawingCanvasProperty, value);
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

	public ITemplate<Control>? PreviewTemplate
	{
		get => GetValue(PreviewTemplateProperty);
		set => SetValue(PreviewTemplateProperty, value);
	}

	public ListBox? ListBox
	{
		get => GetValue(ListBoxProperty);
		set => SetValue(ListBoxProperty, value);
	}

	protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
	{
		base.OnPropertyChanged(change);
		if (change.Property == ThumbsPanelProperty)
			HandleThumbsPanelChange(change);
	}

	private Vector2<double> _fixedPoint1;
	private double? _fixedX2;
	private double? _fixedY2;
	private Control? _preview;
	private bool _pointerMoved;

	private void HandleThumbsPanelChange(AvaloniaPropertyChangedEventArgs args)
	{
		var (oldValue, newValue) = args.GetOldAndNewValue<Panel?>();
		if (oldValue != null)
		{
			oldValue.Children.CollectionChanged -= OnThumbsPanelChildrenChanged;
			UnsubscribeFromThumbs(oldValue.Children);
		}

		if (newValue != null)
		{
			newValue.Children.CollectionChanged += OnThumbsPanelChildrenChanged;
			SubscribeOnThumbs(newValue.Children);
		}
	}

	private void OnThumbsPanelChildrenChanged(object? sender, NotifyCollectionChangedEventArgs e)
	{
		if (e.OldItems != null)
			UnsubscribeFromThumbs(e.OldItems.Cast<InputElement>());
		if (e.NewItems != null)
			SubscribeOnThumbs(e.NewItems.Cast<InputElement>());
	}

	private void SubscribeOnThumbs(IEnumerable<InputElement> elements)
	{
		foreach (var element in elements)
			element.PointerPressed += OnThumbPointerPressed;
	}

	private void UnsubscribeFromThumbs(IEnumerable<InputElement> elements)
	{
		foreach (var element in elements)
			element.PointerPressed -= OnThumbPointerPressed;
	}

	private void OnThumbPointerPressed(object? sender, PointerPressedEventArgs e)
	{
		if (sender == null || DrawingCanvas == null)
			return;
		e.Pointer.Capture(DrawingCanvas);
		DrawingCanvas.PointerMoved += OnDrawingCanvasPointerMoved;
		DrawingCanvas.PointerReleased += OnDrawingCanvasPointerReleased;
		var canvasSize = DrawingCanvas.Bounds.Size.ToVector();
		var sizedBounding = Bounding * canvasSize;
		SetFixedPoints((Layoutable)sender, sizedBounding);
		InitializePreview(sizedBounding);
		ListBox?.Classes.Add(HideItemsStyleClass);
	}

	private void SetFixedPoints(Layoutable thumb, Bounding sizedBounding)
	{
		if (!TryGetOppositeSide(sizedBounding, thumb.HorizontalAlignment, out var fixedX1))
		{
			fixedX1 = sizedBounding.Left;
			_fixedX2 = sizedBounding.Right;
		}
		if (!TryGetOppositeSide(sizedBounding, thumb.VerticalAlignment, out var fixedY1))
		{
			fixedY1 = sizedBounding.Top;
			_fixedY2 = sizedBounding.Bottom;
		}
		_fixedPoint1 = new Vector2<double>(fixedX1, fixedY1);
	}

	private static bool TryGetOppositeSide(Bounding bounding, HorizontalAlignment alignment, out double value)
	{
		if (alignment == HorizontalAlignment.Left)
		{
			value = bounding.Right;
			return true;
		}
		if (alignment == HorizontalAlignment.Right)
		{
			value = bounding.Left;
			return true;
		}
		value = 0;
		return false;
	}

	private static bool TryGetOppositeSide(Bounding bounding, VerticalAlignment alignment, out double value)
	{
		if (alignment == VerticalAlignment.Top)
		{
			value = bounding.Bottom;
			return true;
		}
		if (alignment == VerticalAlignment.Bottom)
		{
			value = bounding.Top;
			return true;
		}
		value = 0;
		return false;
	}

	private void InitializePreview(Bounding sizedBounding)
	{
		if (PreviewTemplate == null || DrawingCanvas == null)
			return;
		_preview = PreviewTemplate.Build();
		SetPreviewBounds(sizedBounding);
		DrawingCanvas.Children.Add(_preview);
	}

	private void OnDrawingCanvasPointerMoved(object? sender, PointerEventArgs e)
	{
		_pointerMoved = true;
		var position = e.GetPosition(DrawingCanvas);
		UpdatePreview(position);
	}

	private void UpdatePreview(Point position)
	{
		if (_preview == null || DrawingCanvas == null)
			return;
		var canvasSize = DrawingCanvas.Bounds.Size;
		var x2 = _fixedX2 ?? Math.Clamp(position.X, 0, canvasSize.Width);
		var y2 = _fixedY2 ?? Math.Clamp(position.Y, 0, canvasSize.Height);
		Vector2<double> point2 = new(x2, y2);
		var bounding = Bounding.FromPoints(_fixedPoint1, point2);
		SetPreviewBounds(bounding);
	}

	private void SetPreviewBounds(Bounding bounding)
	{
		if (_preview == null)
			return;
		Canvas.SetLeft(_preview, bounding.Left);
		Canvas.SetTop(_preview, bounding.Top);
		_preview.Width = bounding.Width;
		_preview.Height = bounding.Height;
	}

	private void OnDrawingCanvasPointerReleased(object? sender, PointerReleasedEventArgs e)
	{
		Guard.IsNotNull(DrawingCanvas);
		DrawingCanvas.PointerMoved -= OnDrawingCanvasPointerMoved;
		DrawingCanvas.PointerReleased -= OnDrawingCanvasPointerReleased;
		RemovePreview();
		var canvasSize = DrawingCanvas.Bounds.Size.ToVector();
		var position = e.GetPosition(DrawingCanvas).ToVector().Clamp(Vector2<double>.Zero, canvasSize);
		Vector2<double> point2 = new(_fixedX2 ?? position.X, _fixedY2 ?? position.Y);
		_fixedX2 = null;
		_fixedY2 = null;
		var bounding = Bounding.FromPoints(_fixedPoint1, point2);
		var normalizedBounding = bounding / canvasSize;
		ListBox?.Classes.Remove(HideItemsStyleClass);
		if (!_pointerMoved)
			return;
		_pointerMoved = false;
		SetCurrentValue(BoundingProperty, normalizedBounding);
	}

	private void RemovePreview()
	{
		if (_preview == null || DrawingCanvas == null)
			return;
		DrawingCanvas.Children.Remove(_preview);
		_preview = null;
	}
}