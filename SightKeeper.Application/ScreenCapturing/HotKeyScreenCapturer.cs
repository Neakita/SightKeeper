using System.Diagnostics.CodeAnalysis;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using CommunityToolkit.Diagnostics;
using HotKeys;
using HotKeys.Handlers;
using SharpHook.Native;
using SightKeeper.Application.ScreenCapturing.Saving;
using SightKeeper.Domain;
using SightKeeper.Domain.Images;

namespace SightKeeper.Application.ScreenCapturing;

public sealed class HotKeyScreenCapturer<TPixel> : ImageCapturer, IDisposable
{
	public required ScreenBoundsProvider ScreenBoundsProvider { get; init; }
	public required ImagesCleaner ImagesCleaner { get; init; }
	public required ScreenCapturer<TPixel> ScreenCapturer { get; init; }
	public required ImageSaverFactory<TPixel> ImageSaverFactory { get; init; }
	public required SelfActivityProvider SelfActivityProvider { get; init; }

	public required BindingsManager BindingsManager
	{
		[MemberNotNull(nameof(_binding))] init
		{
			var handler = GetHandler();
			_binding = value.CreateBinding(handler);
			_binding.IsEnabled = false;
			_binding.Gesture = new Gesture(MouseButton.Button1);
		}
	}

	public ImageSet? Set
	{
		get;
		set
		{
			field = value;
			IsEnabled = value != null;
			_binding.IsEnabled = IsEnabled;
			_setChanged.OnNext(value);
		}
	}

	public IObservable<ImageSet?> SetChanged => _setChanged.AsObservable();

	public double? FrameRateLimit
	{
		get;
		set
		{
			field = value;
			_binding.Handler = GetHandler();
		}
	}

	public Vector2<ushort> ImageSize
	{
		get;
		set
		{
			Guard.IsGreaterThanOrEqualTo(value.X, MinimumResolutionDimension);
			Guard.IsGreaterThanOrEqualTo(value.Y, MinimumResolutionDimension);
			field = value;
		}
	} = new(320, 320);

	public Gesture Gesture
	{
		get => _binding.Gesture;
		set => _binding.Gesture = value;
	}

	public void Dispose()
	{
		_setChanged.Dispose();
		_binding.Dispose();
	}

	private bool IsEnabled
	{
		get;
		set
		{
			if (field == value)
				return;
			_imageSaver = value ? ImageSaverFactory.CreateImageSaver() : null;
			field = value;
		}
	}

	private const ushort MinimumResolutionDimension = 32;
	private readonly Subject<ImageSet?> _setChanged = new();
	private readonly Binding _binding;
	private Vector2<ushort> Offset => ScreenBoundsProvider.MainScreenCenter - ImageSize / 2;
	private ImageSaver<TPixel>? _imageSaver;

	private ContinuousHandler GetHandler()
	{
		return FrameRateLimit switch
		{
			null => new RepeatedHandler(new ActionHandler(MakeImage), new ActionHandler(ClearExceedImages)),
			0 => new BeginAsyncHandler(new ActionHandler(() =>
			{
				MakeImage();
				ClearExceedImages();
			})),
			_ => new TimedHandler(new ActionHandler(MakeImage), new ActionHandler(ClearExceedImages))
			{
				Period = TimeSpan.FromSeconds(1) / FrameRateLimit.Value
			}
		};
	}

	private void MakeImage()
	{
		if (SelfActivityProvider.IsOwnWindowActive)
			return;
		if (_imageSaver is LimitedSaver { IsLimitReached: true })
			return;
		var set = Set;
		Guard.IsNotNull(set);
		var imageData = ScreenCapturer.Capture(ImageSize, Offset);
		Guard.IsNotNull(_imageSaver);
		_imageSaver.SaveImage(set, imageData, DateTimeOffset.Now);
	}

	private void ClearExceedImages()
	{
		var set = Set;
		if (set == null)
			return;
		if (_imageSaver is LimitedSaver limitedSaver)
			limitedSaver.Processing.Wait();
		ImagesCleaner.RemoveExceedUnusedImages(set);
	}
}