using SightKeeper.Application;
using SightKeeper.Application.ScreenCapturing;

namespace SightKeeper.Avalonia.ImageSets.Capturing;

public sealed class CapturingSettingsViewModel : ViewModel, CapturingSettingsDataContext
{
	public ushort MaximumWidth => _screenBoundsProvider.MainScreenSize.X;
	public ushort MaximumHeight => _screenBoundsProvider.MainScreenSize.Y;

	public ushort Width
	{
		get => _capturer.ImageSize.X;
		set
		{
			OnPropertyChanging();
			_capturer.ImageSize = _capturer.ImageSize.WithX(value);
			OnPropertyChanged();
		}
	}

	public ushort Height
	{
		get => _capturer.ImageSize.Y;
		set
		{
			OnPropertyChanging();
			_capturer.ImageSize = _capturer.ImageSize.WithY(value);
			OnPropertyChanged();
		}
	}

	public double? FrameRateLimit
	{
		get => _capturer.FrameRateLimit;
		set
		{
			OnPropertyChanging();
			_capturer.FrameRateLimit = value;
			OnPropertyChanged();
		}
	}

	public CapturingSettingsViewModel(ScreenBoundsProvider screenBoundsProvider, ImageCapturer capturer)
	{
		_screenBoundsProvider = screenBoundsProvider;
		_capturer = capturer;
	}

	private readonly ScreenBoundsProvider _screenBoundsProvider;
	private readonly ImageCapturer _capturer;
}