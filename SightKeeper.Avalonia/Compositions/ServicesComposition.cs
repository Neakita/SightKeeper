#if OS_WINDOWS
using SightKeeper.Application.Windows;
#elif OS_LINUX
using SightKeeper.Application.Linux.X11;
#endif
using System.Linq;
using CommunityToolkit.Diagnostics;
using FluentValidation;
using HotKeys;
using HotKeys.SharpHook;
using Pure.DI;
using Serilog;
using SharpHook.Reactive;
using SightKeeper.Application;
using SightKeeper.Application.ImageSets;
using SightKeeper.Application.ImageSets.Creating;
using SightKeeper.Application.ImageSets.Editing;
using SightKeeper.Application.ScreenCapturing;
using SightKeeper.Application.ScreenCapturing.Saving;
using SightKeeper.Avalonia.Misc;
using SixLabors.ImageSharp.PixelFormats;

namespace SightKeeper.Avalonia.Compositions;

public sealed class ServicesComposition
{
	private void Setup() => DI.Setup(nameof(ServicesComposition), CompositionKind.Internal)

		.Bind<IValidator<ImageSetData>>()
		.To<NewImageSetDataValidator>()

		.Bind<IValidator<ExistingImageSetData>>()
		.To<ExistingImageSetDataValidator>()

		.Bind<ObservableListRepository<TT>>()
		.To<ComposeObservableListRepository<TT>>()

		.Bind<ImageCapturer>()
		.As(Lifetime.Singleton)
		.To<HotKeyScreenCapturer<Bgra32>>()

		.Bind<ScreenBoundsProvider>()
		.To<SharpHookScreenBoundsProvider>()

		.Bind<SelfActivityProvider>()
		.To<AvaloniaSelfActivityProvider>()

		.Bind<ScreenCapturer<Bgra32>>()
#if OS_WINDOWS
		.To<DX11ScreenCapturer>()
#elif OS_LINUX
		.To<X11ScreenCapturer>()
#endif

		.Bind<ImageSaver<Bgra32>>()
		.To<ImmediateImageSaver<Bgra32>>()

		.Bind<PixelConverter<Bgra32, Rgba32>>()
		.To<Bgra32ToRgba32PixelConverter>()

		.Bind<ImageDataConverterMiddleware<Bgra32, Rgba32>>()
		.To(context =>
		{
			context.Inject(out PixelConverter<Bgra32, Rgba32> converter);
			context.Inject(out ImageDataWriter<Rgba32> dataWriter);
			return new ImageDataConverterMiddleware<Bgra32, Rgba32>
			{
				Converter = converter,
				Next = dataWriter
			};
		})

		.Bind<ImageDataSaver<Bgra32>>()
		.To(context =>
		{
			context.Inject(out ImageDataConverterMiddleware<Bgra32, Rgba32> converter);
			return new BufferedImageDataSaverMiddleware<Bgra32>
			{
				Next = converter
			};
		})

		.Bind<IReactiveGlobalHook>()
		.To(_ =>
		{
			var hook = new SimpleReactiveGlobalHook(true);
			hook.RunAsync();
			return hook;
		})

		.Bind<BindingsManager>()
		.To(context =>
		{
			context.Inject(out IReactiveGlobalHook hook);
			var gestures = hook.ObserveInputStates().Filter().ToGesture();
			return new BindingsManager(gestures);
		})

		.Bind<ILogger>()
		.To(context =>
		{
			var consumer = context.ConsumerTypes.Single();
			Guard.IsNotNull(consumer);
			return Log.ForContext(consumer);
		});
}