using System.Buffers;
using Autofac;
using FluentValidation;
using HotKeys;
using HotKeys.SharpHook;
using SharpHook.Reactive;
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
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Tags;
using SixLabors.ImageSharp.PixelFormats;

namespace SightKeeper.Application;

public static class ApplicationServicesExtensions
{
	public static void AddApplicationServices(this ContainerBuilder builder)
	{
		builder.AddValidators();
		builder.AddImageCapturing();
		builder.AddTraining();
		builder.AddHotKeys();

		builder.RegisterGeneric(typeof(ComposeObservableListRepository<>))
			.As(typeof(ObservableListRepository<>));

		builder.RegisterType<ImageSetCreator>();
		builder.RegisterType<ImageSetEditor>();
		builder.RegisterType<DataSetCreator>();
		builder.RegisterType<DataSetEditor>();
	}

	private static void AddValidators(this ContainerBuilder builder)
	{
		builder.RegisterType<NewImageSetDataValidator>()
			.As<IValidator<ImageSetData>>();

		builder.RegisterType<ExistingImageSetDataValidator>()
			.As<IValidator<ExistingImageSetData>>();

		builder.RegisterType<ExistingDataSetDataValidator>()
			.As<IValidator<ExistingDataSetData>>();

		builder.RegisterType<NewDataSetDataValidator>()
			.As<IValidator<NewDataSetData>>();
	}

	private static void AddImageCapturing(this ContainerBuilder builder)
	{
		builder.RegisterType<HotKeyScreenCapturer<Bgra32>>()
			.SingleInstance()
			.As<ImageCapturer>();

		builder.RegisterType<SharpHookScreenBoundsProvider>()
			.As<ScreenBoundsProvider>();

		builder.RegisterType<Bgra32ToRgba32PixelConverter>()
			.As<PixelConverter<Bgra32, Rgba32>>();

		builder.Register(context =>
		{
			var pixelConverter = context.Resolve<PixelConverter<Bgra32, Rgba32>>();
			var imageDataSaver = context.Resolve<ImageDataSaver<Rgba32>>();
			var arrayPool = ArrayPool<Bgra32>.Create(Array.MaxLength, 20);
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
		}).As<ImageSaver<Bgra32>>();

		builder.RegisterType<ImagesCleaner>();
	}

	private static void AddTraining(this ContainerBuilder builder)
	{
		builder.RegisterType<TypeSwitchTrainer>()
			.As<Trainer<ReadOnlyTag, ReadOnlyAsset>>();

		builder.RegisterType<DFineTrainer>()
			.As<Trainer<ReadOnlyTag, ReadOnlyItemsAsset<ReadOnlyAssetItem>>>();
		
		builder.Register(context =>
		{
			TrainDataExporter<ReadOnlyTag, ReadOnlyItemsAsset<ReadOnlyAssetItem>> exporter = new COCODetectorDataSetExporter();
			exporter = new DistributedTrainDataExporter<ReadOnlyTag, ReadOnlyItemsAsset<ReadOnlyAssetItem>>(exporter);
			var transformer = context.Resolve<TrainDataTransformer<ReadOnlyTag, ReadOnlyItemsAsset<ReadOnlyAssetItem>>>();
			exporter = new TransformingTrainDataExporter<ReadOnlyTag, ReadOnlyItemsAsset<ReadOnlyAssetItem>>(exporter, transformer);
			return exporter;
		}).As<TrainDataExporter<ReadOnlyTag, ReadOnlyItemsAsset<ReadOnlyAssetItem>>>();

		builder.RegisterType<CropTransformer<ReadOnlyTag, ReadOnlyItemsAsset<ReadOnlyAssetItem>>>()
			.As<TrainDataTransformer<ReadOnlyTag, ReadOnlyItemsAsset<ReadOnlyAssetItem>>>();

		builder
			.RegisterType<RandomItemsCropRectanglesProvider<ReadOnlyItemsAsset<ReadOnlyAssetItem>, ReadOnlyAssetItem>>()
			.As<CropRectanglesProvider<ReadOnlyItemsAsset<ReadOnlyAssetItem>>>();

		builder.RegisterType<ItemsAssetCropper<ReadOnlyAssetItem>>()
			.As<AssetCropper<ReadOnlyItemsAsset<ReadOnlyAssetItem>>>();

		builder.RegisterType<AssetItemCropper>()
			.As<ItemCropper<ReadOnlyAssetItem>>();

		builder.RegisterType<RandomItemsCropSettings>();
	}

	private static void AddHotKeys(this ContainerBuilder builder)
	{
		builder.Register(_ => new ReactiveGlobalHook(runAsyncOnBackgroundThread: true))
			.SingleInstance()
			.As<IReactiveGlobalHook>();

		builder.Register(context =>
		{
			var hook = context.Resolve<IReactiveGlobalHook>();
			var gestures = hook.ObserveInputStates().Filter().ToGesture();
			return new BindingsManager(gestures);
		});
	}
}