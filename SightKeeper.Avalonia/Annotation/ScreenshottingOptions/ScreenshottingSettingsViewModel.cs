using HotKeys.Gestures;
using SightKeeper.Application;
using SightKeeper.Application.Screenshotting;
using SightKeeper.Domain.Model.DataSets.Screenshots;

namespace SightKeeper.Avalonia.Annotation.ScreenshottingOptions;

internal sealed class ScreenshottingSettingsViewModel : ViewModel
{
	public ScreenshotsLibrary? Library
	{
		get => _screenshotter.Library;
		set => SetProperty(_screenshotter.Library, value, _screenshotter, static (screenshotter, newValue) => screenshotter.Library = newValue);
	}

	public bool IsEnabled
	{
		get => _screenshotter.IsEnabled;
		set => SetProperty(_screenshotter.IsEnabled, value, _screenshotter, static (screenshotter, newValue) => screenshotter.IsEnabled = newValue);
	}

	public float FPS
	{
		get => _screenshotter.FPS;
		set => SetProperty(_screenshotter.FPS, value, _screenshotter, static (screenshotter, newValue) => screenshotter.FPS = newValue);
	}

	public ushort ResolutionWidth
	{
		get => _screenshotter.Resolution.X;
		set => SetProperty(_screenshotter.Resolution.X, value, _screenshotter, static (screenshotter, newValue) => screenshotter.Resolution = screenshotter.Resolution.WithX(newValue));
	}

	public ushort ResolutionHeight
	{
		get => _screenshotter.Resolution.Y;
		set => SetProperty(_screenshotter.Resolution.Y, value, _screenshotter, static (screenshotter, newValue) => screenshotter.Resolution = screenshotter.Resolution.WithY(newValue));
	}

	public Gesture Gesture
	{
		get => _screenshotter.Gesture;
		set => SetProperty(_screenshotter.Gesture, value, _screenshotter, static (screenshotter, newValue) => screenshotter.Gesture = newValue);
	}

	public ushort MaximumWidth => _screenBoundsProvider.MainScreenSize.X;
	public ushort MaximumHeight => _screenBoundsProvider.MainScreenSize.Y;

	public ScreenshottingSettingsViewModel(ScreenBoundsProvider screenBoundsProvider, Screenshotter screenshotter)
	{
		_screenBoundsProvider = screenBoundsProvider;
		_screenshotter = screenshotter;
	}

	private readonly ScreenBoundsProvider _screenBoundsProvider;
	private readonly Screenshotter _screenshotter;
}