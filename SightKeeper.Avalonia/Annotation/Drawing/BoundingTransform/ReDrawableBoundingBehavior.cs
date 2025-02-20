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
	public static readonly StyledProperty<InputElement?> DrawingCanvasProperty =
		AvaloniaProperty.Register<ReDrawableBoundingBehavior, InputElement?>(nameof(DrawingCanvas));

	public static readonly StyledProperty<Panel?> ThumbsPanelProperty =
		AvaloniaProperty.Register<ReDrawableBoundingBehavior, Panel?>(nameof(ThumbsPanel));

	public static readonly StyledProperty<Bounding> BoundingProperty =
		AvaloniaProperty.Register<ReDrawableBoundingBehavior, Bounding>(nameof(Bounding), defaultBindingMode: BindingMode.TwoWay);

	public InputElement? DrawingCanvas
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
		if (sender == null || DrawingCanvas == null)
			return;
		e.Pointer.Capture(DrawingCanvas);
		DrawingCanvas.PointerMoved += OnCanvasPointerMoved;
		DrawingCanvas.PointerReleased += OnCanvasPointerReleased;
		var thumb = (InputElement)sender;
		var changingHorizontalSide = thumb.HorizontalAlignment.ToOptionalSide();
		var changingVerticalSide = thumb.VerticalAlignment.ToOptionalSide();
		var sizedBounding = Bounding * DrawingCanvas.Bounds.Size.ToVector();
		if (changingHorizontalSide != null)
		{
			_fixedX1 = sizedBounding.Get(changingHorizontalSide.Value.Opposite());
		}
		else
		{
			_fixedX1 = sizedBounding.Left;
			_fixedX2 = sizedBounding.Right;
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
	}

	private void OnCanvasPointerMoved(object? sender, PointerEventArgs e)
	{
		
	}

	private void OnCanvasPointerReleased(object? sender, PointerReleasedEventArgs e)
	{
		Guard.IsNotNull(DrawingCanvas);
		DrawingCanvas.PointerMoved -= OnCanvasPointerMoved;
		DrawingCanvas.PointerReleased -= OnCanvasPointerReleased;
		var canvasSize = DrawingCanvas.Bounds.Size.ToVector();
		var position = e.GetPosition(DrawingCanvas).ToVector().Clamp(Vector2<double>.Zero, canvasSize);
		Vector2<double> point1 = new(_fixedX1, _fixedY1);
		Vector2<double> point2 = new(_fixedX2 ?? position.X, _fixedY2 ?? position.Y);
		var bounding = Bounding.FromPoints(point1, point2);
		var normalizedBounding = bounding / canvasSize;
		SetCurrentValue(BoundingProperty, normalizedBounding);
	}
}