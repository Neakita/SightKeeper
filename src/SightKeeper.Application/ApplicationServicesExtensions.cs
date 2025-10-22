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

		builder.RegisterType<ImageDataConverterMiddleware<Bgra32, Rgba32>>()
			.As<ImageDataSaver<Bgra32>>();
		
		builder.RegisterDecorator<BufferedImageDataSaverMiddleware<Bgra32>, ImageDataSaver<Bgra32>>();

		builder.RegisterType<ImmediateImageSaver<Bgra32>>()
			.As<ImageSaver<Bgra32>>();

		builder.RegisterType<ImagesCleaner>()
			.SingleInstance()
			.AsSelf()
			.As<UnusedImagesLimitManager>();
	}

	private static void AddTraining(this ContainerBuilder builder)
	{
		builder.RegisterType<TypeSwitchTrainer>()
			.As<Trainer<ReadOnlyTag, ReadOnlyAsset>>();

		builder.RegisterType<DFineTrainer>()
			.As<Trainer<ReadOnlyTag, ReadOnlyItemsAsset<ReadOnlyDetectorItem>>>();
		
		builder.Register(context =>
		{
			TrainDataExporter<ReadOnlyTag, ReadOnlyItemsAsset<ReadOnlyDetectorItem>> exporter = new COCODetectorDataSetExporter();
			exporter = new DistributedTrainDataExporter<ReadOnlyTag, ReadOnlyItemsAsset<ReadOnlyDetectorItem>>(exporter);
			var transformer = context.Resolve<TrainDataTransformer<ReadOnlyTag, ReadOnlyItemsAsset<ReadOnlyDetectorItem>>>();
			exporter = new TransformingTrainDataExporter<ReadOnlyTag, ReadOnlyItemsAsset<ReadOnlyDetectorItem>>(exporter, transformer);
			return exporter;
		}).As<TrainDataExporter<ReadOnlyTag, ReadOnlyItemsAsset<ReadOnlyDetectorItem>>>();

		builder.RegisterType<CropTransformer<ReadOnlyTag, ReadOnlyItemsAsset<ReadOnlyDetectorItem>>>()
			.As<TrainDataTransformer<ReadOnlyTag, ReadOnlyItemsAsset<ReadOnlyDetectorItem>>>();

		builder
			.RegisterType<RandomItemsCropRectanglesProvider<ReadOnlyItemsAsset<ReadOnlyDetectorItem>, ReadOnlyDetectorItem>>()
			.As<CropRectanglesProvider<ReadOnlyItemsAsset<ReadOnlyDetectorItem>>>();

		builder.RegisterType<ItemsAssetCropper<ReadOnlyDetectorItem>>()
			.As<AssetCropper<ReadOnlyItemsAsset<ReadOnlyDetectorItem>>>();

		builder.RegisterType<AssetItemCropper>()
			.As<ItemCropper<ReadOnlyDetectorItem>>();

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