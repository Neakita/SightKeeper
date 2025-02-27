using HotKeys.Gestures;
using SightKeeper.Application;
using SightKeeper.Application.ScreenCapturing;
using SightKeeper.Domain.Images;

namespace SightKeeper.Avalonia.Annotation;

public sealed class ScreenCapturingSettingsViewModel : ViewModel
{
	public ImageSet? Set
	{
		get => _hotKeyScreenCapture.Set;
		set => SetProperty(_hotKeyScreenCapture.Set, value, _hotKeyScreenCapture, static (capture, newValue) => capture.Set = newValue);
	}

	public bool IsEnabled
	{
		get => _hotKeyScreenCapture.IsEnabled;
		set => SetProperty(_hotKeyScreenCapture.IsEnabled, value, _hotKeyScreenCapture, static (capture, newValue) => capture.IsEnabled = newValue);
	}

	public float? FPSLimit
	{
		get => _hotKeyScreenCapture.FPSLimit;
		set => SetProperty(_hotKeyScreenCapture.FPSLimit, value, _hotKeyScreenCapture, static (capture, newValue) => capture.FPSLimit = newValue);
	}

	public ushort ResolutionWidth
	{
		get => _hotKeyScreenCapture.ImageSize.X;
		set => SetProperty(_hotKeyScreenCapture.ImageSize.X, value, _hotKeyScreenCapture, static (capture, newValue) => capture.ImageSize = capture.ImageSize.WithX(newValue));
	}

	public ushort ResolutionHeight
	{
		get => _hotKeyScreenCapture.ImageSize.Y;
		set => SetProperty(_hotKeyScreenCapture.ImageSize.Y, value, _hotKeyScreenCapture, static (capture, newValue) => capture.ImageSize = capture.ImageSize.WithY(newValue));
	}

	public Gesture Gesture
	{
		get => _hotKeyScreenCapture.Gesture;
		set => SetProperty(_hotKeyScreenCapture.Gesture, value, _hotKeyScreenCapture, static (capture, newValue) => capture.Gesture = newValue);
	}

	public ushort MaximumWidth => _screenBoundsProvider.MainScreenSize.X;
	public ushort MaximumHeight => _screenBoundsProvider.MainScreenSize.Y;

	public ScreenCapturingSettingsViewModel(ScreenBoundsProvider screenBoundsProvider, HotKeyScreenCapture hotKeyScreenCapture)
	{
		_screenBoundsProvider = screenBoundsProvider;
		_hotKeyScreenCapture = hotKeyScreenCapture;
		ResolutionWidth = MaximumWidth;
		ResolutionHeight = MaximumHeight;
	}

	private readonly ScreenBoundsProvider _screenBoundsProvider;
	private readonly HotKeyScreenCapture _hotKeyScreenCapture;
}