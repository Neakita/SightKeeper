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
using SightKeeper.Application.Training;
using SightKeeper.Application.Training.Assets.Distribution;
using SightKeeper.Application.Training.COCO;
using SightKeeper.Application.Training.Data;
using SightKeeper.Application.Training.Data.Transforming;
using SightKeeper.Application.Training.DFINE;
using SightKeeper.Avalonia.Misc;
using SightKeeper.Data.DataSets;
using SightKeeper.Domain.DataSets;
using SixLabors.ImageSharp.PixelFormats;
#if OS_WINDOWS
using SightKeeper.Application.Windows;
#elif OS_LINUX
using SightKeeper.Application.Linux.X11;
#endif

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
		.To<SustainableScreenCapturer<Bgra32, DX11ScreenCapturer>>()
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

		.Bind<DataSetExporter<DataSet>>()
		.To<ZippedMemoryPackDataSetExporter>()

		.Bind<DataSetImporter>()
		.To<ZippedMemoryPackDataSetImporter>()

		.Bind<Trainer<AssetData>>()
		.To<TypeSwitchTrainer>()

		.Bind<Trainer<ItemsAssetData<AssetItemData>>>()
		.To<DFineTrainer>()

		.Bind<CondaEnvironmentManager>()
#if OS_WINDOWS
		.To<StatelessWindowsCondaEnvironmentManager>()
#elif OS_LINUX

#endif
		.Bind<TrainDataExporter<ItemsAssetData<AssetItemData>>>()
		.To(context =>
		{
			TrainDataExporter<ItemsAssetData<AssetItemData>> exporter = new COCODetectorDataSetExporter();
			exporter = new DistributedTrainDataExporter<ItemsAssetData<AssetItemData>>(exporter);
			context.Inject(out TrainDataTransformer<ItemsAssetData<AssetItemData>> transformer);
			exporter = new TransformingTrainDataExporter<ItemsAssetData<AssetItemData>>(exporter, transformer);
			return exporter;
		})

		.Bind<CommandRunner>()
#if OS_WINDOWS
		.To(_ =>
		{
			CommandRunner commandRunner = new ArgumentCommandRunner("cmd.exe");
			commandRunner = new WindowsArgumentCarryCommandRunner(commandRunner);
			return commandRunner;
		})
#elif OS_LINUX

#endif

		.Bind<TrainDataTransformer<ItemsAssetData<AssetItemData>>>()
		.To<CropTransformer<ItemsAssetData<AssetItemData>>>()

		.Bind<CropRectanglesProvider<ItemsAssetData<AssetItemData>>>()
		.To<RandomItemsCropRectanglesProvider<ItemsAssetData<AssetItemData>, AssetItemData>>()
	
		.Bind<AssetCropper<ItemsAssetData<AssetItemData>>>()
		.To<ItemsAssetCropper<AssetItemData>>()
	
		.Bind<ItemCropper<AssetItemData>>()
		.To<AssetItemCropper>();
}