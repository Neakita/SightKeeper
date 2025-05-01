using System.Reactive.Linq;
using System.Reactive.Subjects;
using CommunityToolkit.Diagnostics;
using HotKeys.Bindings;
using HotKeys.Gestures;
using HotKeys.Handlers.Contextual;
using SharpHook.Native;
using SightKeeper.Domain;
using SightKeeper.Domain.Images;

namespace SightKeeper.Application.ScreenCapturing;

public abstract class HotKeyScreenCapture : ImageCapturer
{
	public bool IsEnabled
	{
		get;
		private set
		{
			if (field == value)
				return;
			Guard.IsNotNull(Set);
			field = value;
			if (value) Enable();
			else Disable();
		}
	}

	public ImageSet? Set
	{
		get;
		set
		{
			if (field != null)
				IsEnabled = false;
			field = value;
			if (value != null)
				IsEnabled = true;
			_setChanged.OnNext(value);
		}
	}

	public IObservable<ImageSet?> SetChanged => _setChanged.AsObservable();

	public double? FrameRateLimit
	{
		get;
		set
		{
			if (value != null)
				Guard.IsGreaterThan(value.Value, 0);
			field = value;
		}
	}

	public Vector2<ushort> ImageSize
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

	protected TimeSpan Timeout => FrameRateLimit == null ? TimeSpan.Zero : TimeSpan.FromSeconds(1) / FrameRateLimit.Value;
	protected Vector2<ushort> Offset => _screenBoundsProvider.MainScreenCenter - _resolution / 2;

	protected HotKeyScreenCapture(
		ScreenBoundsProvider screenBoundsProvider,
		BindingsManager bindingsManager)
	{
		_screenBoundsProvider = screenBoundsProvider;
		_bindingsManager = bindingsManager;
		_handler = new ContextualizedHandler(MakeImages);
	}

	protected abstract void MakeImages(ActionContext context);

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

	private const ushort MinimumResolutionDimension = 32;
	private readonly ScreenBoundsProvider _screenBoundsProvider;
	private readonly BindingsManager _bindingsManager;
	private readonly ContextualizedHandler _handler;
	private readonly Subject<ImageSet?> _setChanged = new();
	private Vector2<ushort> _resolution = new(320, 320);
	private IDisposable? _binding;
}