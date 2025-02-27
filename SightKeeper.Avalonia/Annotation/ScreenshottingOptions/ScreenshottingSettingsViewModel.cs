using HotKeys.Gestures;
using SightKeeper.Application;
using SightKeeper.Application.ScreenCapturing;
using SightKeeper.Domain.Images;

namespace SightKeeper.Avalonia.Annotation.ScreenshottingOptions;

public sealed class ScreenshottingSettingsViewModel : ViewModel
{
	public ImageSet? Library
	{
		get => _hotKeyScreenCapture.Library;
		set => SetProperty(_hotKeyScreenCapture.Library, value, _hotKeyScreenCapture, static (screenshotter, newValue) => screenshotter.Library = newValue);
	}

	public bool IsEnabled
	{
		get => _hotKeyScreenCapture.IsEnabled;
		set => SetProperty(_hotKeyScreenCapture.IsEnabled, value, _hotKeyScreenCapture, static (screenshotter, newValue) => screenshotter.IsEnabled = newValue);
	}

	public float? FPSLimit
	{
		get => _hotKeyScreenCapture.FPSLimit;
		set => SetProperty(_hotKeyScreenCapture.FPSLimit, value, _hotKeyScreenCapture, static (screenshotter, newValue) => screenshotter.FPSLimit = newValue);
	}

	public ushort ResolutionWidth
	{
		get => _hotKeyScreenCapture.ImageSize.X;
		set => SetProperty(_hotKeyScreenCapture.ImageSize.X, value, _hotKeyScreenCapture, static (screenshotter, newValue) => screenshotter.ImageSize = screenshotter.ImageSize.WithX(newValue));
	}

	public ushort ResolutionHeight
	{
		get => _hotKeyScreenCapture.ImageSize.Y;
		set => SetProperty(_hotKeyScreenCapture.ImageSize.Y, value, _hotKeyScreenCapture, static (screenshotter, newValue) => screenshotter.ImageSize = screenshotter.ImageSize.WithY(newValue));
	}

	public Gesture Gesture
	{
		get => _hotKeyScreenCapture.Gesture;
		set => SetProperty(_hotKeyScreenCapture.Gesture, value, _hotKeyScreenCapture, static (screenshotter, newValue) => screenshotter.Gesture = newValue);
	}

	public ushort MaximumWidth => _screenBoundsProvider.MainScreenSize.X;
	public ushort MaximumHeight => _screenBoundsProvider.MainScreenSize.Y;

	public ScreenshottingSettingsViewModel(ScreenBoundsProvider screenBoundsProvider, HotKeyScreenCapture hotKeyScreenCapture)
	{
		_screenBoundsProvider = screenBoundsProvider;
		_hotKeyScreenCapture = hotKeyScreenCapture;
		ResolutionWidth = MaximumWidth;
		ResolutionHeight = MaximumHeight;
	}

	private readonly ScreenBoundsProvider _screenBoundsProvider;
	private readonly HotKeyScreenCapture _hotKeyScreenCapture;
}