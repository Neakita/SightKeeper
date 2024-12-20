using CommunityToolkit.Diagnostics;
using HotKeys;
using HotKeys.ActionRunners;
using HotKeys.Bindings;
using HotKeys.Gestures;
using HotKeys.SharpHook;
using SharpHook.Native;
using SightKeeper.Domain;
using SightKeeper.Domain.Screenshots;

namespace SightKeeper.Application.Screenshotting;

public abstract class Screenshotter
{
	public bool IsEnabled
	{
		get => _isEnabled;
		set
		{
			if (_isEnabled == value)
				return;
			Guard.IsNotNull(Library);
			_isEnabled = value;
			if (value) Enable();
			else Disable();
		}
	}

	public ScreenshotsLibrary? Library
	{
		get => _library;
		set
		{
			Guard.IsFalse(IsEnabled);
			_library = value;
		}
	}

	public float? FPSLimit
	{
		get => _fpsLimit;
		set
		{
			if (value != null)
				Guard.IsGreaterThan(value.Value, 0);
			_fpsLimit = value;
		}
	}

	public virtual Vector2<ushort> ImageSize
	{
		get => _resolution;
		set
		{
			Guard.IsGreaterThanOrEqualTo(value.X, MinimumResolutionDimension);
			Guard.IsGreaterThanOrEqualTo(value.Y, MinimumResolutionDimension);
			_resolution = value;
		}
	}

	public Gesture Gesture { get; set; } = new([new FormattedButton(MouseButton.Button1)]);

	protected TimeSpan? Timeout => TimeSpan.FromSeconds(1) / FPSLimit;
	protected Vector2<ushort> Offset => _screenBoundsProvider.MainScreenCenter - _resolution / 2;

	protected Screenshotter(
		ScreenBoundsProvider screenBoundsProvider,
		BindingsManager bindingsManager)
	{
		_screenBoundsProvider = screenBoundsProvider;
		_bindingsManager = bindingsManager;
	}

	protected abstract void MakeScreenshots(ActionContext context);

	private const ushort MinimumResolutionDimension = 32;
	private readonly ScreenBoundsProvider _screenBoundsProvider;
	private readonly BindingsManager _bindingsManager;
	private float? _fpsLimit;
	private Vector2<ushort> _resolution = new(320, 320);
	private bool _isEnabled;
	private Binding? _binding;
	private ScreenshotsLibrary? _library;

	protected virtual void Enable()
	{
		Guard.IsNull(_binding);
		_binding = _bindingsManager.CreateBinding(MakeScreenshots, InputTypes.Hold);
		_bindingsManager.SetGesture(_binding, Gesture);
	}

	protected virtual void Disable()
	{
		Guard.IsNotNull(_binding);
		_binding.Dispose();
		_binding = null;
	}
}