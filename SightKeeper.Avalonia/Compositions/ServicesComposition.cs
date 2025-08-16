#if OS_WINDOWS
using SightKeeper.Application.Windows;
#elif OS_LINUX
using SightKeeper.Application.Linux.X11;
#endif
using System;
using System.Buffers;
using FluentValidation;
using HotKeys;
using HotKeys.SharpHook;
using Pure.DI;
using Serilog;
using SharpHook.Reactive;
using SightKeeper.Application;
using SightKeeper.Application.DataSets;
using SightKeeper.Application.DataSets.Creating;
using SightKeeper.Application.DataSets.Editing;
using SightKeeper.Application.ImageSets;
using SightKeeper.Application.ImageSets.Creating;
using SightKeeper.Application.ImageSets.Editing;
using SightKeeper.Application.ScreenCapturing;
using SightKeeper.Application.ScreenCapturing.Saving;
using SightKeeper.Avalonia.Misc;
using SightKeeper.Data.DataSets;
using SixLabors.ImageSharp.PixelFormats;

namespace SightKeeper.Avalonia.Compositions;

public sealed class ServicesComposition
{
	private void Setup() => DI.Setup(nameof(ServicesComposition), CompositionKind.Internal)

		.Bind<IValidator<ImageSetData>>()
		.To<NewImageSetDataValidator>()

		.Bind<IValidator<ExistingImageSetData>>()
		.To<ExistingImageSetDataValidator>()

		.Bind<IValidator<ExistingDataSetData>>()
		.To<ExistingDataSetDataValidator>()

		.Bind<IValidator<NewDataSetData>>()
		.To<NewDataSetDataValidator>()

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
			return new FuncImageSaverFactory<Bgra32>(() =>
			{
				var arrayPool = ArrayPool<Bgra32>.Create(Array.MaxLength, 20);
				context.Inject(out ImageDataSaver<Rgba32> imageDataSaver);
				return new ImmediateImageSaver<Bgra32>
				{
					DataSaver = new BufferedImageDataSaverMiddleware<Bgra32>
					{
						Next = new ImageDataConverterMiddleware<Bgra32, Rgba32>
						{
							Converter = pixelConverter,
							Next = imageDataSaver
						},
						ArrayPool = arrayPool
					}
				};
			});
		})

		.Bind<IReactiveGlobalHook>()
		.As(Lifetime.Singleton)
		.To(_ =>
		{
			var hook = new ReactiveGlobalHook(runAsyncOnBackgroundThread: true);
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
		.To(_ => Log.Logger)
	
		.Bind<DataSetExporter>()
		.To<ZippedMemoryPackDataSetExporter>();
}