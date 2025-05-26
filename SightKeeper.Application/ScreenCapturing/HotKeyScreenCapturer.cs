using System.Diagnostics.CodeAnalysis;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using CommunityToolkit.Diagnostics;
using HotKeys;
using HotKeys.Handlers;
using SharpHook.Native;
using SightKeeper.Domain;
using SightKeeper.Domain.Images;

namespace SightKeeper.Application.ScreenCapturing;

public abstract class HotKeyScreenCapturer : ImageCapturer
{
	public required ScreenBoundsProvider ScreenBoundsProvider { get; init; }
	public required ImagesCleaner ImagesCleaner { get; init; }

	public required BindingsManager BindingsManager
	{
		[MemberNotNull(nameof(_binding))] init
		{
			var handler = GetHandler();
			_binding = value.CreateBinding(handler);
			_binding.Gesture = new Gesture(MouseButton.Button1);
		}
	}

	public ImageSet? Set
	{
		get;
		set
		{
			field = value;
			_binding.IsEnabled = value != null;
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
		get => _resolution;
		set
		{
			Guard.IsGreaterThanOrEqualTo(value.X, MinimumResolutionDimension);
			Guard.IsGreaterThanOrEqualTo(value.Y, MinimumResolutionDimension);
			_resolution = value;
		}
	}

	public Gesture Gesture
	{
		get => _binding.Gesture;
		set => _binding.Gesture = value;
	}

	protected Vector2<ushort> Offset => ScreenBoundsProvider.MainScreenCenter - _resolution / 2;

	protected abstract void MakeImage();

	protected virtual void ClearExceedImages()
	{
		if (Set == null)
			return;
		ImagesCleaner.RemoveExceedUnusedImages(Set);
	}

	private const ushort MinimumResolutionDimension = 32;
	private readonly Subject<ImageSet?> _setChanged = new();
	private readonly Binding _binding;
	private Vector2<ushort> _resolution = new(320, 320);

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
}