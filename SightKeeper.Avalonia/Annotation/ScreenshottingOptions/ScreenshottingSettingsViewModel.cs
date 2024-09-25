using HotKeys.Gestures;
using SightKeeper.Application;
using SightKeeper.Domain.Model.DataSets.Screenshots;

namespace SightKeeper.Avalonia.Annotation.ScreenshottingOptions;

internal sealed class ScreenshottingSettingsViewModel : ViewModel
{
	public ScreenshotsLibrary? Library
	{
		get => _screenshoter.Library;
		set => SetProperty(_screenshoter.Library, value, _screenshoter, static (screenshoter, newValue) => screenshoter.Library = newValue);
	}

	public bool IsEnabled
	{
		get => _screenshoter.IsEnabled;
		set => SetProperty(_screenshoter.IsEnabled, value, _screenshoter, static (screenshoter, newValue) => screenshoter.IsEnabled = newValue);
	}

	public float FPS
	{
		get => _screenshoter.FPS;
		set => SetProperty(_screenshoter.FPS, value, _screenshoter, static (screenshoter, newValue) => screenshoter.FPS = newValue);
	}

	public ushort ResolutionWidth
	{
		get => _screenshoter.Resolution.X;
		set => SetProperty(_screenshoter.Resolution.X, value, _screenshoter, static (screenshoter, newValue) => screenshoter.Resolution = screenshoter.Resolution.WithX(newValue));
	}

	public ushort ResolutionHeight
	{
		get => _screenshoter.Resolution.Y;
		set => SetProperty(_screenshoter.Resolution.Y, value, _screenshoter, static (screenshoter, newValue) => screenshoter.Resolution = screenshoter.Resolution.WithY(newValue));
	}

	public Gesture Gesture
	{
		get => _screenshoter.Gesture;
		set => SetProperty(_screenshoter.Gesture, value, _screenshoter, static (screenshoter, newValue) => screenshoter.Gesture = newValue);
	}

	public ushort MaximumWidth => _screenBoundsProvider.MainScreenSize.X;
	public ushort MaximumHeight => _screenBoundsProvider.MainScreenSize.Y;

	public ScreenshottingSettingsViewModel(ScreenBoundsProvider screenBoundsProvider, Screenshoter screenshoter)
	{
		_screenBoundsProvider = screenBoundsProvider;
		_screenshoter = screenshoter;
	}

	private readonly ScreenBoundsProvider _screenBoundsProvider;
	private readonly Screenshoter _screenshoter;
}