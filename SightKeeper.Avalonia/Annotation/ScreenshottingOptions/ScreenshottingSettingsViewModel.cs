using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using SightKeeper.Application;

namespace SightKeeper.Avalonia.Annotation.ScreenshottingOptions;

internal sealed partial class ScreenshottingSettingsViewModel : ViewModel
{
	public float FPS
	{
		get => _fps;
		set
		{
			Guard.IsGreaterThan(value, 0);
			SetProperty(ref _fps, value);
		}
	}

	public ushort ResolutionWidth
	{
		get => _resolutionWidth;
		set
		{
			Guard.IsGreaterThan(value, MinimumResolution);
			SetProperty(ref _resolutionWidth, value);
		}
	}

	public ushort ResolutionHeight
	{
		get => _resolutionHeight;
		set
		{
			Guard.IsGreaterThan(value, MinimumResolution);
			SetProperty(ref _resolutionHeight, value);
		}
	}

	public ushort MaximumWidth => (ushort)_screenBoundsProvider.MainScreenSize.X;
	public ushort MaximumHeight => (ushort)_screenBoundsProvider.MainScreenSize.Y;

	public ScreenshottingSettingsViewModel(ScreenBoundsProvider screenBoundsProvider)
	{
		_screenBoundsProvider = screenBoundsProvider;
	}

	private const ushort MinimumResolution = 32;
	private readonly ScreenBoundsProvider _screenBoundsProvider;
	[ObservableProperty] private bool _isEnabled;
	[ObservableProperty] private PassiveScalingOptionsViewModel? _scalingOptions;
	[ObservableProperty] private PassiveWalkingOptionsViewModel? _walkingOptions;
	private float _fps;
	private ushort _resolutionWidth;
	private ushort _resolutionHeight;
}