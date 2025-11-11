using Autofac;
using FluentValidation;
using HotKeys;
using HotKeys.SharpHook;
using Serilog;
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
using SightKeeper.Application.Training.RFDETR;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Tags;
using SixLabors.ImageSharp.PixelFormats;

namespace SightKeeper.Application;

public static class ApplicationServicesExtensions
{
	public static void RegisterApplicationServices(this ContainerBuilder builder)
	{
		builder.RegisterValidators();
		builder.RegisterImageCapturing();
		builder.RegisterTraining();
		builder.RegisterHotKeys();

		builder.RegisterGeneric(typeof(ComposeObservableListRepository<>))
			.As(typeof(ObservableListRepository<>));

		builder.RegisterType<ImageSetCreator>();
		builder.RegisterType<ImageSetEditor>();
		builder.RegisterType<DataSetCreator>();
		builder.RegisterType<DataSetEditor>();
	}

	public static void RegisterClassifierServices(this ContainerBuilder builder)
	{
	}

	public static void RegisterDetectorServices(this ContainerBuilder builder)
	{
		builder.RegisterType<CastingTrainer<ReadOnlyTag, ReadOnlyItemsAsset<ReadOnlyDetectorItem>>>()
			.As<Trainer<ReadOnlyTag, ReadOnlyAsset>>();
		builder.RegisterType<RFDETRDetectorTrainer>()
			.As<Trainer<ReadOnlyTag, ReadOnlyItemsAsset<ReadOnlyDetectorItem>>>();
	}

	public static void RegisterPoserServices(this ContainerBuilder builder)
	{
	}

	private static void RegisterValidators(this ContainerBuilder builder)
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

	private static void RegisterImageCapturing(this ContainerBuilder builder)
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

	private static void RegisterTraining(this ContainerBuilder builder)
	{
		builder.RegisterType<LifetimeTrainer>()
			.As<Trainer<ReadOnlyTag, ReadOnlyAsset>>();

		builder.RegisterType<COCODetectorDataSetExporter>()
			.OnActivated(args =>
			{
				args.Instance.DataFileName = "_annotations.coco.json";
				args.Instance.ImagesSubDirectoryPath = string.Empty;
			})
			.As<TrainDataExporter<ReadOnlyTag, ReadOnlyItemsAsset<ReadOnlyDetectorItem>>>();
		
		builder.RegisterDecorator<TrainDataExporter<ReadOnlyTag, ReadOnlyItemsAsset<ReadOnlyDetectorItem>>>((context, _, exporter) =>
		{
			var logger = context.Resolve<ILogger>();
			logger = logger.ForContext<DistributedTrainDataExporter<ReadOnlyTag, ReadOnlyItemsAsset<ReadOnlyDetectorItem>>>();
			return new DistributedTrainDataExporter<ReadOnlyTag, ReadOnlyItemsAsset<ReadOnlyDetectorItem>>(exporter, logger)
			{
				TrainDirectoryName = "train",
				ValidationDirectoryName = "valid",
				TestDirectoryName = "test"
			};
		});

		builder.RegisterDecorator<
			TransformingTrainDataExporter<ReadOnlyTag, ReadOnlyItemsAsset<ReadOnlyDetectorItem>>,
			TrainDataExporter<ReadOnlyTag, ReadOnlyItemsAsset<ReadOnlyDetectorItem>>>();

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

		builder.RegisterType<ParallelImageExporter>()
			.As<ImageExporter>();
	}

	private static void RegisterHotKeys(this ContainerBuilder builder)
	{
		builder.Register(_ => new ReactiveGlobalHook(runAsyncOnBackgroundThread: true))
			.SingleInstance()
			.As<IReactiveGlobalHook>();

		builder.RegisterType<BindingsManager>()
			.WithParameter((info, _) => info.Position == 0, (_, context) => context.Resolve<IReactiveGlobalHook>().ObserveInputStates().ToGesture());
	}
}