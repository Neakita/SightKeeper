using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Threading;
using Avalonia.Xaml.Interactivity;
using CommunityToolkit.Diagnostics;
using HotKeys.Handlers;
using Sightful.Avalonia.Controls.ZoomViewer;

namespace SightKeeper.Avalonia.Behaviors;

internal sealed class DrawingZoomHotKeysBinding : Behavior<ZoomViewer>
{
	public static readonly StyledProperty<Key?> ZoomInHotKeyProperty =
		AvaloniaProperty.Register<DrawingZoomHotKeysBinding, Key?>(nameof(ZoomInHotKey));

	public static readonly StyledProperty<Key?> ZoomOutHotKeyProperty =
		AvaloniaProperty.Register<DrawingZoomHotKeysBinding, Key?>(nameof(ZoomOutHotKey));

	public Key? ZoomInHotKey
	{
		get => GetValue(ZoomInHotKeyProperty);
		set => SetValue(ZoomInHotKeyProperty, value);
	}

	public Key? ZoomOutHotKey
	{
		get => GetValue(ZoomOutHotKeyProperty);
		set => SetValue(ZoomOutHotKeyProperty, value);
	}

	public DrawingZoomHotKeysBinding()
	{
		_zoomInHandler = new TimedHandler(new ActionHandler(ZoomIn))
		{
			Period = TimeSpan.FromMilliseconds(50)
		};
		_zoomOutHandler = new TimedHandler(new ActionHandler(ZoomOut))
		{
			Period = TimeSpan.FromMilliseconds(50)
		};
	}

	protected override void OnAttachedToVisualTree()
	{
		_topLevel = TopLevel.GetTopLevel(AssociatedObject);
		Guard.IsNotNull(_topLevel);
		_topLevel.KeyDown += OnKeyDown;
		_topLevel.KeyUp += OnKeyUp;
	}

	protected override void OnDetachedFromVisualTree()
	{
		Guard.IsNotNull(_topLevel);
		_topLevel.KeyDown -= OnKeyDown;
		_topLevel.KeyUp -= OnKeyUp;
		_zoomInHandler.End();
		_zoomOutHandler.End();
	}

	protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
	{
		base.OnPropertyChanged(change);
		_zoomInHandler.End();
		_zoomOutHandler.End();
	}

	private readonly TimedHandler _zoomInHandler;
	private readonly TimedHandler _zoomOutHandler;
	private TopLevel? _topLevel;

	private void OnKeyDown(object? sender, KeyEventArgs e)
	{
		if (e.Key == ZoomInHotKey)
			_zoomInHandler.Begin();
		else if (e.Key == ZoomOutHotKey)
			_zoomOutHandler.Begin();
	}

	private void OnKeyUp(object? sender, KeyEventArgs e)
	{
		if (e.Key == ZoomInHotKey)
			_zoomInHandler.End();
		else if (e.Key == ZoomOutHotKey)
			_zoomOutHandler.End();
	}

	private void ZoomIn()
	{
		Dispatcher.UIThread.Invoke(() =>
		{
			Guard.IsNotNull(AssociatedObject);
			return AssociatedObject.Zoom += 0.5;
		});
	}

	private void ZoomOut()
	{
		Dispatcher.UIThread.Invoke(() =>
		{
			Guard.IsNotNull(AssociatedObject);
			return AssociatedObject.Zoom -= 0.5;
		});
	}
}