#if OS_WINDOWS
using SightKeeper.Application.Windows;
#elif OS_LINUX
using SightKeeper.Application.Linux.X11;
#endif
using System;
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

		.Bind<PixelConverter<Bgra32, Rgba32>>()
		.To<Bgra32ToRgba32PixelConverter>()
		
		.Bind<ImageSaverFactory<Bgra32>>()
		.To(context =>
		{
			context.Inject(out PixelConverter<Bgra32, Rgba32> pixelConverter);
			context.Inject(out ImageDataWriter<Rgba32> imageDataWriter);
			return new BufferedConvertingImageSaverFactory<Bgra32, Rgba32>(Array.MaxLength, 20, pixelConverter, imageDataWriter);
		})

		.Bind<IReactiveGlobalHook>()
		.To(_ =>
		{
			var hook = new SimpleReactiveGlobalHook(runAsyncOnBackgroundThread: true);
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
		.To(_ => Log.Logger);
}