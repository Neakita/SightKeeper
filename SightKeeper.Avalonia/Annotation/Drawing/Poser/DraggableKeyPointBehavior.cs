using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Xaml.Interactivity;
using CommunityToolkit.Diagnostics;
using SightKeeper.Avalonia.Extensions;
using SightKeeper.Domain;

namespace SightKeeper.Avalonia.Annotation.Drawing.Poser;

internal sealed class DraggableKeyPointBehavior : Behavior
{
	private const string HideItemsStyleClass = "hide-items";

	public static readonly StyledProperty<Vector2<double>> PositionProperty =
		AvaloniaProperty.Register<DraggableKeyPointBehavior, Vector2<double>>(nameof(Position), defaultBindingMode: BindingMode.TwoWay);

	public static readonly StyledProperty<ListBox?> ListBoxProperty =
		AvaloniaProperty.Register<DraggableKeyPointBehavior, ListBox?>(nameof(ListBox));

	public static readonly StyledProperty<Panel?> DrawingCanvasProperty =
		AvaloniaProperty.Register<DraggableKeyPointBehavior, Panel?>(nameof(DrawingCanvas));

	public static readonly StyledProperty<ITemplate<Control>?> PreviewTemplateProperty =
		AvaloniaProperty.Register<DraggableKeyPointBehavior, ITemplate<Control>?>(nameof(PreviewTemplate));

	public static readonly StyledProperty<InputElement?> ThumbProperty =
		AvaloniaProperty.Register<DraggableKeyPointBehavior, InputElement?>(nameof(Thumb));

	public Vector2<double> Position
	{
		get => GetValue(PositionProperty);
		set => SetValue(PositionProperty, value);
	}

	public ListBox? ListBox
	{
		get => GetValue(ListBoxProperty);
		set => SetValue(ListBoxProperty, value);
	}

	public Panel? DrawingCanvas
	{
		get => GetValue(DrawingCanvasProperty);
		set => SetValue(DrawingCanvasProperty, value);
	}

	public ITemplate<Control>? PreviewTemplate
	{
		get => GetValue(PreviewTemplateProperty);
		set => SetValue(PreviewTemplateProperty, value);
	}

	[ResolveByName]
	public InputElement? Thumb
	{
		get => GetValue(ThumbProperty);
		set => SetValue(ThumbProperty, value);
	}

	protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
	{
		base.OnPropertyChanged(change);
		if (change.Property == ThumbProperty)
			HandleThumbChange(change);
	}

	private Control? _preview;
	private bool _pointerMoved;

	private void HandleThumbChange(AvaloniaPropertyChangedEventArgs change)
	{
		var (oldValue, newValue) = change.GetOldAndNewValue<InputElement?>();
		if (oldValue != null)
			oldValue.PointerPressed -= OnPointerPressed;
		if (newValue != null)
			newValue.PointerPressed += OnPointerPressed;
	}

	private void OnPointerPressed(object? sender, PointerPressedEventArgs e)
	{
		if (DrawingCanvas == null)
			return;
		e.Pointer.Capture(DrawingCanvas);
		DrawingCanvas.PointerMoved += OnPointerMoved;
		DrawingCanvas.PointerReleased += OnPointerReleased;
		ListBox?.Classes.Add(HideItemsStyleClass);
		InitializePreview();
		var canvasSize = DrawingCanvas.Bounds.Size.ToVector();
		var sizedPosition = Position * canvasSize;
		SetPreviewPosition(sizedPosition);
	}

	private void InitializePreview()
	{
		if (PreviewTemplate == null || DrawingCanvas == null)
			return;
		_preview = PreviewTemplate.Build();
		DrawingCanvas.Children.Add(_preview);
	}

	private void OnPointerMoved(object? sender, PointerEventArgs e)
	{
		Guard.IsNotNull(DrawingCanvas);
		var position = e.GetPosition(DrawingCanvas).ToVector();
		var canvasSize = DrawingCanvas.Bounds.Size.ToVector();
		position = position.Clamp(Vector2<double>.Zero, canvasSize);
		SetPreviewPosition(position);
		_pointerMoved = true;
	}

	private void SetPreviewPosition(Vector2<double> position)
	{
		if (_preview == null)
			return;
		Canvas.SetLeft(_preview, position.X);
		Canvas.SetTop(_preview, position.Y);
	}

	private void OnPointerReleased(object? sender, PointerReleasedEventArgs e)
	{
		Guard.IsNotNull(DrawingCanvas);
		DrawingCanvas.PointerMoved -= OnPointerMoved;
		DrawingCanvas.PointerReleased -= OnPointerReleased;
		RemovePreview();
		ListBox?.Classes.Remove(HideItemsStyleClass);
		if (!_pointerMoved)
			return;
		_pointerMoved = false;
		var canvasSize = DrawingCanvas.Bounds.Size.ToVector();
		var position = e.GetPosition(DrawingCanvas).ToVector();
		var normalizedPosition = position / canvasSize;
		normalizedPosition = normalizedPosition.Clamp(Vector2<double>.Zero, Vector2<double>.One);
		SetCurrentValue(PositionProperty, normalizedPosition);
	}

	private void RemovePreview()
	{
		if (DrawingCanvas == null || _preview == null)
			return;
		DrawingCanvas.Children.Remove(_preview);
	}
}