using Autofac;
using Avalonia.Platform;
using Material.Icons;
using SightKeeper.Application;
using SightKeeper.Application.Windows;
using SightKeeper.Avalonia.Annotation;
using SightKeeper.Avalonia.Annotation.Drawing;
using SightKeeper.Avalonia.Annotation.Drawing.Bounded;
using SightKeeper.Avalonia.Annotation.Drawing.Poser;
using SightKeeper.Avalonia.Annotation.Images;
using SightKeeper.Avalonia.Annotation.Tooling;
using SightKeeper.Avalonia.Annotation.Tooling.Classifier;
using SightKeeper.Avalonia.Annotation.Tooling.Commands;
using SightKeeper.Avalonia.Annotation.Tooling.Detector;
using SightKeeper.Avalonia.Annotation.Tooling.Poser;
using SightKeeper.Avalonia.DataSets;
using SightKeeper.Avalonia.DataSets.Card;
using SightKeeper.Avalonia.DataSets.Commands;
using SightKeeper.Avalonia.DataSets.Dialogs;
using SightKeeper.Avalonia.Dialogs;
using SightKeeper.Avalonia.ImageSets;
using SightKeeper.Avalonia.ImageSets.Capturing;
using SightKeeper.Avalonia.ImageSets.Card;
using SightKeeper.Avalonia.ImageSets.Commands;
using SightKeeper.Avalonia.Misc;
using SightKeeper.Avalonia.Training;
using SightKeeper.Domain.Images;
using SixLabors.ImageSharp.PixelFormats;
#if OS_WINDOWS

#elif OS_LINUX
using SightKeeper.Application.Linux;
#endif

namespace SightKeeper.Avalonia;

internal static class ContainerBuilderServicesExtensions
{
	public static void AddAvaloniaServices(this ContainerBuilder builder)
	{
		builder.RegisterType<AvaloniaSelfActivityProvider>()
			.As<SelfActivityProvider>();
	}

	public static void AddOSSpecificServices(this ContainerBuilder builder)
	{
#if OS_WINDOWS
		builder.AddWindowsServices();
#elif OS_LINUX
		builder.AddLinuxServices();
#endif
	}

	public static void AddLogger(this ContainerBuilder builder)
	{
		builder.RegisterModule(new MiddlewareModule(new SerilogMiddleware()));
	}

	public static void AddPresentationServices(this ContainerBuilder builder)
	{
		builder.RegisterType<DialogManager>()
			.SingleInstance();

		builder.Register(context =>
		{
			var bitmapPool = context.Resolve<WriteableBitmapPool>();
			var imageLoader = context.Resolve<ImageLoader<Rgba32>>();
			return new WriteableBitmapImageLoader<Rgba32>(bitmapPool, PixelFormat.Rgb32, imageLoader);
		}).As<WriteableBitmapImageLoader>();

		builder.RegisterType<WriteableBitmapPool>()
			.SingleInstance();

		builder.RegisterType<ObservableSharpHookGesture>()
			.SingleInstance();

		builder.RegisterType<ImageSetViewModelsObservableListRepository>();
		builder.RegisterType<TagSelectionProvider>();
		builder.RegisterType<DataSetViewModelsObservableListRepository>();
		builder.RegisterType<ToolingViewModelFactory>();
		builder.RegisterType<DrawerItemsFactory>();
		builder.RegisterType<DataSetCardViewModelFactory>();
	}

	public static void AddViewModels(this ContainerBuilder builder)
	{
		AddTabs(builder);

		builder.Register(context =>
		{
			var createImageSetCommand = context.Resolve<CreateImageSetCommand>();
			var imageSetRepository = context.Resolve<ObservableListRepository<ImageSet>>();
			var imageSetCardDataContextFactory = context.Resolve<ImageSetCardDataContextFactory>();
			var capturingSettingsDataContext = context.Resolve<CapturingSettingsDataContext>();
			return new ImageSetsViewModel(
				createImageSetCommand,
				imageSetRepository,
				imageSetCardDataContextFactory,
				capturingSettingsDataContext);
		});

		builder.RegisterType<ImageSetCardViewModelFactory>()
			.As<ImageSetCardDataContextFactory>();

		builder.RegisterType<CapturingSettingsViewModel>()
			.As<CapturingSettingsDataContext>();

		builder.RegisterType<SideBarViewModel>()
			.SingleInstance()
			.As<AdditionalToolingSelection>()
			.As<SideBarDataContext>();

		builder.RegisterType<ImagesViewModel>()
			.SingleInstance()
			.As<ImagesDataContext>()
			.As<ImageSelection>();

		builder.RegisterType<ImageSetSelectionViewModel>()
			.SingleInstance()
			.As<ImageSetSelectionDataContext>()
			.As<ImageSetSelection>();

		builder.RegisterType<DataSetSelectionViewModel>()
			.SingleInstance()
			.As<DataSetSelection>()
			.As<DataSetSelectionDataContext>();

		builder.RegisterType<DrawerViewModel>()
			.SingleInstance()
			.As<DrawerDataContext>()
			.As<SelectedItemProvider>();

		builder.RegisterType<ActionsViewModel>()
			.As<ActionsDataContext>();

		builder.RegisterType<MainViewModel>();
		builder.RegisterType<CreateDataSetViewModel>();
		builder.RegisterType<BoundingDrawerViewModel>();
		builder.RegisterType<AssetItemsViewModel>();
		builder.RegisterType<KeyPointDrawerViewModel>();
		builder.RegisterType<ClassifierToolingViewModel>();
		builder.RegisterType<DetectorToolingViewModel>();
		builder.RegisterType<PoserToolingViewModel>();
	}

	public static void AddTabs(ContainerBuilder builder)
	{
		builder.AddTabItemViewModel<ImageSetsViewModel>(MaterialIconKind.FolderMultipleImage, "Images");
		builder.AddTabItemViewModel<DataSetsViewModel>(MaterialIconKind.ImageAlbum, "Datasets");
		builder.AddTabItemViewModel<AnnotationTabViewModel>(MaterialIconKind.ImageEdit, "Annotation");
		builder.AddTabItemViewModel<TrainingViewModel>(MaterialIconKind.School, "Training");
	}

	public static void AddTabItemViewModel<TContent>(
		this ContainerBuilder builder,
		MaterialIconKind iconKind,
		string header)
		where TContent : notnull
	{
		builder.RegisterType<TContent>();
		builder.Register(context =>
		{
			var content = context.Resolve<TContent>();
			return new TabItemViewModel(iconKind, header, content);
		}).As<TabItemViewModel>();
	}

	public static void AddCommands(this ContainerBuilder builder)
	{
		builder.RegisterType<CreateImageSetCommand>();
		builder.RegisterType<EditImageSetCommand>();
		builder.RegisterType<DeleteImageSetCommand>();
		builder.RegisterType<CreateDataSetCommand>();
		builder.RegisterType<ImportDataSetCommand>();
		builder.RegisterType<EditDataSetCommand>();
		builder.RegisterType<ExportDataSetCommand>();
		builder.RegisterType<DeleteDataSetCommand>();
		builder.RegisterType<DeleteSelectedImageCommand>();
		builder.RegisterType<DeleteSelectedAssetCommand>();
	}
}