using HotKeys.Gestures;
using SightKeeper.Application;
using SightKeeper.Application.Screenshotting;
using SightKeeper.Domain.Screenshots;

namespace SightKeeper.Avalonia.Annotation.ScreenshottingOptions;

public sealed class ScreenshottingSettingsViewModel : ViewModel
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

	public float? FPSLimit
	{
		get => _screenshotter.FPSLimit;
		set => SetProperty(_screenshotter.FPSLimit, value, _screenshotter, static (screenshotter, newValue) => screenshotter.FPSLimit = newValue);
	}

	public ushort ResolutionWidth
	{
		get => _screenshotter.ImageSize.X;
		set => SetProperty(_screenshotter.ImageSize.X, value, _screenshotter, static (screenshotter, newValue) => screenshotter.ImageSize = screenshotter.ImageSize.WithX(newValue));
	}

	public ushort ResolutionHeight
	{
		get => _screenshotter.ImageSize.Y;
		set => SetProperty(_screenshotter.ImageSize.Y, value, _screenshotter, static (screenshotter, newValue) => screenshotter.ImageSize = screenshotter.ImageSize.WithY(newValue));
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
		ResolutionWidth = MaximumWidth;
		ResolutionHeight = MaximumHeight;
	}

	private readonly ScreenBoundsProvider _screenBoundsProvider;
	private readonly Screenshotter _screenshotter;
}