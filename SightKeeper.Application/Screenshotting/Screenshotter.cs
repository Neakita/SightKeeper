using CommunityToolkit.Diagnostics;
using HotKeys.Bindings;
using HotKeys.Gestures;
using HotKeys.Handlers.Contextual;
using SharpHook.Native;
using SightKeeper.Domain;
using SightKeeper.Domain.Images;

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

	public Gesture Gesture { get; set; } = new(MouseButton.Button1);

	protected TimeSpan Timeout => FPSLimit == null ? TimeSpan.Zero : TimeSpan.FromSeconds(1) / FPSLimit.Value;
	protected Vector2<ushort> Offset => _screenBoundsProvider.MainScreenCenter - _resolution / 2;

	protected Screenshotter(
		ScreenBoundsProvider screenBoundsProvider,
		BindingsManager bindingsManager)
	{
		_screenBoundsProvider = screenBoundsProvider;
		_bindingsManager = bindingsManager;
		_handler = new ContextualizedHandler(MakeScreenshots);
	}

	protected abstract void MakeScreenshots(ActionContext context);

	private const ushort MinimumResolutionDimension = 32;
	private readonly ScreenBoundsProvider _screenBoundsProvider;
	private readonly BindingsManager _bindingsManager;
	private readonly ContextualizedHandler _handler;
	private float? _fpsLimit;
	private Vector2<ushort> _resolution = new(320, 320);
	private bool _isEnabled;
	private IDisposable? _binding;
	private ScreenshotsLibrary? _library;

	protected virtual void Enable()
	{
		Guard.IsNull(_binding);
		_binding = _bindingsManager.Bind(Gesture, _handler);
	}

	protected virtual void Disable()
	{
		Guard.IsNotNull(_binding);
		_binding.Dispose();
		_binding = null;
	}
}