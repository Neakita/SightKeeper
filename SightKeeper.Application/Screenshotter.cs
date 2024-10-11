using CommunityToolkit.Diagnostics;
using CommunityToolkit.HighPerformance;
using HotKeys;
using HotKeys.ActionRunners;
using HotKeys.Bindings;
using HotKeys.Gestures;
using HotKeys.SharpHook;
using SharpHook.Native;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets.Screenshots;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.PixelFormats;

namespace SightKeeper.Application;

public sealed class Screenshotter
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

	public float FPS
	{
		get => _fps;
		set
		{
			Guard.IsGreaterThan(value, 0);
			_fps = value;
		}
	}

	public Vector2<ushort> Resolution
	{
		get => _resolution;
		set
		{
			Guard.IsGreaterThanOrEqualTo(value.X, MinimumResolutionDimension);
			Guard.IsGreaterThanOrEqualTo(value.Y, MinimumResolutionDimension);
			_resolution = value;
		}
	}

	public Gesture Gesture { get; set; } = new([new FormattedSharpButton(MouseButton.Button1)]);

	public Screenshotter(
		ScreenCapture screenCapture,
		ScreenshotsDataAccess screenshotsDataAccess,
		ScreenBoundsProvider screenBoundsProvider,
		BindingsManager bindingsManager)
	{
		_screenCapture = screenCapture;
		_screenshotsDataAccess = screenshotsDataAccess;
		_screenBoundsProvider = screenBoundsProvider;
		_bindingsManager = bindingsManager;
	}

	private const ushort MinimumResolutionDimension = 32;
	private readonly ScreenCapture _screenCapture;
	private readonly ScreenshotsDataAccess _screenshotsDataAccess;
	private readonly ScreenBoundsProvider _screenBoundsProvider;
	private readonly BindingsManager _bindingsManager;
	private TimeSpan Timeout => TimeSpan.FromSeconds(1) / FPS;
	private Vector2<ushort> Offset => _screenBoundsProvider.MainScreenCenter - _resolution / 2;
	private float _fps = 10;
	private Vector2<ushort> _resolution = new(320, 320);
	private bool _isEnabled;
	private Binding? _binding;
	private ScreenshotsLibrary? _library;

	private void Enable()
	{
		Guard.IsNull(_binding);
		_binding = _bindingsManager.CreateBinding(MakeScreenshots, InputTypes.Hold);
		_bindingsManager.SetGesture(_binding, Gesture);
	}

	private void Disable()
	{
		Guard.IsNotNull(_binding);
		_binding.Dispose();
		_binding = null;
	}

	private void MakeScreenshots(ActionContext context)
	{
		var contextEliminated = false;
		while (!contextEliminated && IsEnabled)
		{
			Guard.IsNotNull(Library);
			var imageData = _screenCapture.Capture(Resolution, Offset);
			using var image = LoadImage(imageData);
			_screenshotsDataAccess.CreateScreenshot(Library, image, DateTimeOffset.Now, Resolution);
			contextEliminated = context.WaitForElimination(Timeout);
		}
	}

	private static Image<Bgra32> LoadImage(ReadOnlySpan2D<Bgra32> imageData)
	{
		if (imageData.TryGetSpan(out var span))
			return Image.LoadPixelData(span, imageData.Width, imageData.Height);
		Image<Bgra32> image = new(imageData.Width, imageData.Height);
		for (int i = 0; i < imageData.Height; i++)
		{
			var source = imageData.GetRowSpan(i);
			var target = image.DangerousGetPixelRowMemory(i);
			source.CopyTo(target.Span);
		}
		return image;
	}
}