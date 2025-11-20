using System.Reactive.Subjects;
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
using SightKeeper.Application.Interop;
using SightKeeper.Application.Misc;
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

namespace SightKeeper.Application.Infrastructure;

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
		builder.RegisterType<RFDETRDetectorTrainer>()
			.WithParameter((info, _) => info.Position == 2, (_, context) => context.ResolveNamed<IObserver<object>>("training progress"))
			.As<Trainer>();
		
		builder.RegisterType<DistributedTrainDataExporter<ReadOnlyTag, ReadOnlyItemsAsset<ReadOnlyDetectorItem>>>()
			.WithParameter((info, _) => info.Position == 0, (_, context) => context.ResolveNamed<TrainDataExporter<ReadOnlyTag, ReadOnlyItemsAsset<ReadOnlyDetectorItem>>>("train"))
			.WithParameter((info, _) => info.Position == 1, (_, context) => context.ResolveNamed<TrainDataExporter<ReadOnlyTag, ReadOnlyItemsAsset<ReadOnlyDetectorItem>>>("validation"))
			.WithParameter((info, _) => info.Position == 2, (_, context) => context.ResolveNamed<TrainDataExporter<ReadOnlyTag, ReadOnlyItemsAsset<ReadOnlyDetectorItem>>>("test"))
			.As<TrainDataExporter<ReadOnlyTag, ReadOnlyItemsAsset<ReadOnlyDetectorItem>>>();
		
		builder.RegisterTrainingDataExporter("train", "train");
		builder.RegisterTrainingDataExporter("valid", "validation");
		builder.RegisterTrainingDataExporter("test", "test");

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

		builder.RegisterType<CastingTrainDataExporter<ReadOnlyTag, ReadOnlyItemsAsset<ReadOnlyDetectorItem>>>()
			.As<TrainDataExporter<ReadOnlyTag, ReadOnlyAsset>>();

		builder.RegisterType<RFDETROutputParser>()
			.As<OutputParser>();
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
		builder.RegisterType<ParallelImageExporter>()
			.WithParameter((info, _) => info.Position == 1, (_, context) => context.ResolveNamed<IObserver<object>>("training progress"))
			.As<ImageExporter>();

		builder.RegisterInstance(new BehaviorSubject<object>("Idle"))
			.Named<IObservable<object>>("training progress")
			.Named<IObserver<object>>("training progress");
		
		builder.RegisterType<OutputHandlingTrainer>()
			.WithParameter((info, _) => info.Position == 2, (_, context) => context.ResolveNamed<IObserver<object>>("training progress"));
		
		builder.RegisterDecorator<Trainer>((context, _, trainer) =>
		{
			var outputParser = context.Resolve<OutputParser>();
			var progressObserver = context.ResolveNamed<IObserver<object>>("training progress");
			var logger = context.Resolve<ILogger>().ForContext<OutputHandlingTrainer>();
			return new OutputHandlingTrainer(trainer, outputParser, progressObserver, logger);
		});
			
	}

	private static void RegisterHotKeys(this ContainerBuilder builder)
	{
		builder.Register(_ => new ReactiveGlobalHook(runAsyncOnBackgroundThread: true))
			.SingleInstance()
			.As<IReactiveGlobalHook>();

		builder.RegisterType<BindingsManager>()
			.WithParameter((info, _) => info.Position == 0, (_, context) => context.Resolve<IReactiveGlobalHook>().ObserveInputStates().ToGesture());
	}

	private static void RegisterTrainingDataExporter(this ContainerBuilder builder, string subDirectory, string name)
	{
		builder.RegisterType<COCODetectorDataSetExporter>()
			.OnActivated(args =>
			{
				args.Instance.DataFileName = "_annotations.coco.json";
				args.Instance.ImagesSubDirectoryPath = string.Empty;
				args.Instance.DirectoryPath = Path.Combine(RFDETRDetectorTrainer.DataSetPath, subDirectory);
			})
			.Named<TrainDataExporter<ReadOnlyTag, ReadOnlyItemsAsset<ReadOnlyDetectorItem>>>(name);
	}
}