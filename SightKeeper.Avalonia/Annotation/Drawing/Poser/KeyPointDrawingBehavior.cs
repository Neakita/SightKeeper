using System;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Xaml.Interactivity;
using CommunityToolkit.Diagnostics;
using SightKeeper.Domain;

namespace SightKeeper.Avalonia.Annotation.Drawing.Poser;

public sealed class KeyPointDrawingBehavior : Behavior<Canvas>
{
	private const string HideItemsStyleClass = "hide-items";

	public static readonly StyledProperty<ITemplate<Control>?> KeyPointTemplateProperty =
		AvaloniaProperty.Register<KeyPointDrawingBehavior, ITemplate<Control>?>(nameof(KeyPointTemplate));

	public static readonly StyledProperty<ICommand?> CreateKeyPointCommandProperty =
		AvaloniaProperty.Register<KeyPointDrawingBehavior, ICommand?>(nameof(CreateKeyPointCommand));

	public static readonly StyledProperty<KeyPointViewModel?> ExistingKeyPointProperty =
		AvaloniaProperty.Register<KeyPointDrawingBehavior, KeyPointViewModel?>(nameof(ExistingKeyPoint));

	public static readonly StyledProperty<ListBox?> ListBoxProperty =
		AvaloniaProperty.Register<KeyPointDrawingBehavior, ListBox?>(nameof(ListBox));

	public ITemplate<Control>? KeyPointTemplate
	{
		get => GetValue(KeyPointTemplateProperty);
		set => SetValue(KeyPointTemplateProperty, value);
	}

	public ICommand? CreateKeyPointCommand
	{
		get => GetValue(CreateKeyPointCommandProperty);
		set => SetValue(CreateKeyPointCommandProperty, value);
	}

	public KeyPointViewModel? ExistingKeyPoint
	{
		get => GetValue(ExistingKeyPointProperty);
		set => SetValue(ExistingKeyPointProperty, value);
	}

	public ListBox? ListBox
	{
		get => GetValue(ListBoxProperty);
		set => SetValue(ListBoxProperty, value);
	}

	protected override void OnAttached()
	{
		Guard.IsNotNull(AssociatedObject);
		AssociatedObject.PointerPressed += OnAssociatedObjectPointerPressed;
	}

	protected override void OnDetaching()
	{
		Guard.IsNotNull(AssociatedObject);
		AssociatedObject.PointerPressed -= OnAssociatedObjectPointerPressed;
	}

	private Control? _keyPointPreview;

	private void OnAssociatedObjectPointerPressed(object? sender, PointerPressedEventArgs e)
	{
		if (!e.GetCurrentPoint(null).Properties.IsLeftButtonPressed)
			return;
		Guard.IsNotNull(CreateKeyPointCommand);
		if (!CreateKeyPointCommand.CanExecute(default(Vector2<double>)))
			return;
		ListBox?.Classes.Add(HideItemsStyleClass);
		ShowPreview();
		Guard.IsNotNull(AssociatedObject);
		AssociatedObject.PointerMoved += OnAssociatedObjectPointerMoved;
		AssociatedObject.PointerReleased += OnAssociatedObjectPointerReleased;
		var position = e.GetPosition(AssociatedObject);
		UpdatePreviewPosition(position);
		e.Handled = true;
	}

	private void ShowPreview()
	{
		Guard.IsNotNull(AssociatedObject);
		if (KeyPointTemplate == null)
			return;
		_keyPointPreview = KeyPointTemplate.Build();
		AssociatedObject.Children.Add(_keyPointPreview);
	}

	private void OnAssociatedObjectPointerMoved(object? sender, PointerEventArgs e)
	{
		Guard.IsNotNull(AssociatedObject);
		var position = e.GetPosition(AssociatedObject);
		var associatedObjectSize = AssociatedObject.Bounds.Size;
		position = new Point(
			Math.Clamp(position.X, 0, associatedObjectSize.Width),
			Math.Clamp(position.Y, 0, associatedObjectSize.Height));
		UpdatePreviewPosition(position);
	}

	private void UpdatePreviewPosition(Point position)
	{
		if (_keyPointPreview == null)
			return;
		Canvas.SetLeft(_keyPointPreview, position.X);
		Canvas.SetTop(_keyPointPreview, position.Y);
	}

	private void OnAssociatedObjectPointerReleased(object? sender, PointerReleasedEventArgs e)
	{
		Guard.IsNotNull(AssociatedObject);
		AssociatedObject.PointerMoved -= OnAssociatedObjectPointerMoved;
		AssociatedObject.PointerReleased -= OnAssociatedObjectPointerReleased;
		if (_keyPointPreview != null)
		{
			bool isRemoved = AssociatedObject.Children.Remove(_keyPointPreview);
			Guard.IsTrue(isRemoved);
			_keyPointPreview = null;
		}
		ListBox?.Classes.Remove(HideItemsStyleClass);
		var position = e.GetPosition(AssociatedObject);
		var associatedObjectSize = AssociatedObject.Bounds.Size;
		Vector2<double> normalizedPosition = new(position.X / associatedObjectSize.Width, position.Y / associatedObjectSize.Height);
		normalizedPosition = normalizedPosition.Clamp(Vector2<double>.Zero, Vector2<double>.One);
		if (ExistingKeyPoint != null)
		{
			ExistingKeyPoint.Position = normalizedPosition;
			return;
		}
		Guard.IsNotNull(CreateKeyPointCommand);
		Guard.IsTrue(CreateKeyPointCommand.CanExecute(normalizedPosition));
		CreateKeyPointCommand.Execute(normalizedPosition);
	}
}