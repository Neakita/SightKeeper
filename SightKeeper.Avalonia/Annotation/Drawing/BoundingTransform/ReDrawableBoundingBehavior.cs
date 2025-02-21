using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Xaml.Interactivity;
using CommunityToolkit.Diagnostics;
using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Assets;

namespace SightKeeper.Avalonia.Annotation.Drawing.BoundingTransform;

internal sealed class ReDrawableBoundingBehavior : Behavior<Control>
{
	public static readonly StyledProperty<Panel?> CanvasProperty =
		AvaloniaProperty.Register<ReDrawableBoundingBehavior, Panel?>(nameof(Canvas));

	public static readonly StyledProperty<Panel?> ThumbsPanelProperty =
		AvaloniaProperty.Register<ReDrawableBoundingBehavior, Panel?>(nameof(ThumbsPanel));

	public static readonly StyledProperty<Bounding> BoundingProperty =
		AvaloniaProperty.Register<ReDrawableBoundingBehavior, Bounding>(nameof(Bounding), defaultBindingMode: BindingMode.TwoWay);

	public static readonly StyledProperty<ITemplate<Control>?> PreviewTemplateProperty =
		AvaloniaProperty.Register<ReDrawableBoundingBehavior, ITemplate<Control>?>(nameof(PreviewTemplate));

	public Panel? Canvas
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

	protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
	{
		base.OnPropertyChanged(change);
		if (change.Property == ThumbsPanelProperty)
			HandleThumbsPanelChange(change);
	}

	private double _fixedX1;
	private double _fixedY1;
	private double? _fixedX2;
	private double? _fixedY2;
	private Control? _preview;
	private bool _pointerMoved;

	private void HandleThumbsPanelChange(AvaloniaPropertyChangedEventArgs change)
	{
		var (oldValue, newValue) = change.GetOldAndNewValue<Panel?>();
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
		if (sender == null || Canvas == null)
			return;
		e.Pointer.Capture(Canvas);
		Canvas.PointerMoved += OnCanvasPointerMoved;
		Canvas.PointerReleased += OnCanvasPointerReleased;
		var thumb = (InputElement)sender;
		var changingHorizontalSide = thumb.HorizontalAlignment.ToOptionalSide();
		var changingVerticalSide = thumb.VerticalAlignment.ToOptionalSide();
		var canvasSize = Canvas.Bounds.Size.ToVector();
		var sizedBounding = Bounding * canvasSize;
		if (changingHorizontalSide == null)
		{
			_fixedX1 = sizedBounding.Left;
			_fixedX2 = sizedBounding.Right;
		}
		else
		{
			_fixedX1 = sizedBounding.Get(changingHorizontalSide.Value.Opposite());
		}
		if (changingVerticalSide == null)
		{
			_fixedY1 = sizedBounding.Top;
			_fixedY2 = sizedBounding.Bottom;
		}
		else
		{
			_fixedY1 = sizedBounding.Get(changingVerticalSide.Value.Opposite());
		}
		InitializePreview(sizedBounding);
	}

	private void InitializePreview(Bounding sizedBounding)
	{
		if (PreviewTemplate == null || Canvas == null)
			return;
		_preview = PreviewTemplate.Build();
		global::Avalonia.Controls.Canvas.SetLeft(_preview, sizedBounding.Left);
		global::Avalonia.Controls.Canvas.SetTop(_preview, sizedBounding.Top);
		_preview.Width = sizedBounding.Width;
		_preview.Height = sizedBounding.Height;
		Canvas.Children.Add(_preview);
	}

	private void OnCanvasPointerMoved(object? sender, PointerEventArgs e)
	{
		_pointerMoved = true;
		var position = e.GetPosition(Canvas);
		UpdatePreview(position);
	}

	private void UpdatePreview(Point position)
	{
		if (_preview == null || Canvas == null)
			return;
		var x1 = _fixedX1;
		var y1 = _fixedY1;
		var canvasSize = Canvas.Bounds.Size;
		var x2 = _fixedX2 ?? Math.Clamp(position.X, 0, canvasSize.Width);
		var y2 = _fixedY2 ?? Math.Clamp(position.Y, 0, canvasSize.Height);
		Sort(ref x1, ref x2);
		Sort(ref y1, ref y2);
		global::Avalonia.Controls.Canvas.SetLeft(_preview, x1);
		global::Avalonia.Controls.Canvas.SetTop(_preview, y1);
		_preview.Width = x2 - x1;
		_preview.Height = y2 - y1;
	}

	private static void Sort(ref double lesser, ref double greater)
	{
		if (lesser > greater)
			(lesser, greater) = (greater, lesser);
	}

	private void OnCanvasPointerReleased(object? sender, PointerReleasedEventArgs e)
	{
		Guard.IsNotNull(Canvas);
		Canvas.PointerMoved -= OnCanvasPointerMoved;
		Canvas.PointerReleased -= OnCanvasPointerReleased;
		RemovePreview();
		_fixedX2 = null;
		_fixedY2 = null;
		var canvasSize = Canvas.Bounds.Size.ToVector();
		var position = e.GetPosition(Canvas).ToVector().Clamp(Vector2<double>.Zero, canvasSize);
		Vector2<double> point1 = new(_fixedX1, _fixedY1);
		Vector2<double> point2 = new(_fixedX2 ?? position.X, _fixedY2 ?? position.Y);
		var bounding = Bounding.FromPoints(point1, point2);
		var normalizedBounding = bounding / canvasSize;
		if (!_pointerMoved)
			return;
		_pointerMoved = false;
		SetCurrentValue(BoundingProperty, normalizedBounding);
	}

	private void RemovePreview()
	{
		if (_preview == null || Canvas == null)
			return;
		Canvas.Children.Remove(_preview);
		_preview = null;
	}
}