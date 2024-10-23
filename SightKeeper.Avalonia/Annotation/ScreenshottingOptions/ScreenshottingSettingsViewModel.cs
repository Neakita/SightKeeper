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

	public float? FPS
	{
		get => _screenshotter.MaximumFPS;
		set => SetProperty(_screenshotter.MaximumFPS, value, _screenshotter, static (screenshotter, newValue) => screenshotter.MaximumFPS = newValue);
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