using System.Linq;
using System.Threading;
using FluentValidation;
using HotKeys;
using HotKeys.SharpHook;
using Material.Icons;
using Pure.DI;
using Serilog;
using SharpHook.Reactive;
using SightKeeper.Application;
using SightKeeper.Application.Annotation;
using SightKeeper.Application.DataSets.Creating;
using SightKeeper.Application.DataSets.Editing;
using SightKeeper.Application.ImageSets;
using SightKeeper.Application.ImageSets.Creating;
using SightKeeper.Application.ImageSets.Editing;
using SightKeeper.Application.ScreenCapturing;
using SightKeeper.Application.ScreenCapturing.Saving;
using SightKeeper.Avalonia.Annotation;
using SightKeeper.Avalonia.Annotation.Images;
using SightKeeper.Avalonia.Annotation.Tooling;
using SightKeeper.Avalonia.Annotation.Tooling.Classifier;
using SightKeeper.Avalonia.Annotation.Tooling.Commands;
using SightKeeper.Avalonia.DataSets;
using SightKeeper.Avalonia.Dialogs;
using SightKeeper.Avalonia.ImageSets;
using SightKeeper.Data;
using SightKeeper.Data.Services;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.Images;
using SixLabors.ImageSharp.PixelFormats;
using PoserToolingViewModel = SightKeeper.Avalonia.Annotation.Tooling.Poser.PoserToolingViewModel;
using TagAttribute = SightKeeper.Application.TagAttribute;
#if OS_LINUX
using SightKeeper.Application.Linux.X11;
#endif
#if OS_WINDOWS
using SightKeeper.Application.Windows;
#endif

namespace SightKeeper.Avalonia;

public sealed partial class Composition
{
	private void Setup() => DI.Setup(nameof(Composition))
		.Hint(Hint.Resolve, "Off")
		.RootBind<AppDataAccess>(nameof(AppDataAccess)).Bind<DataSaver>().As(Lifetime.Singleton).To<AppDataAccess>()
		.RootBind<FileSystemImageRepository>(nameof(FileSystemImageRepository)).Bind().Bind<ImageRepository>()
		.Bind<ObservableImageRepository>().As(Lifetime.Singleton)
		.To<FileSystemImageRepository>()
		.Bind<ReadRepository<DomainImageSet>>().Bind<ObservableRepository<DomainImageSet>>()
		.Bind<WriteRepository<DomainImageSet>>().As(Lifetime.Singleton).To<AppDataImageSetsRepository>()
		.Bind<ReadRepository<DataSet>>().Bind<ObservableRepository<DataSet>>().Bind<WriteRepository<DataSet>>()
		.As(Lifetime.Singleton).To<AppDataDataSetsRepository>()
		.Bind<PendingImagesCountReporter>().Bind<ImageSaver<Bgra32>>().As(Lifetime.Singleton)
		.To<BufferedImageSaverMiddleware<Bgra32>>(context =>
		{
			context.Inject(out PixelConverter<Bgra32, Rgba32> converter);
			context.Inject(out ImageRepository imageRepository);
			ImageSaverConverterMiddleware<Bgra32, Rgba32> converterMiddleware = new()
			{
				Converter = converter,
				NextMiddleware = imageRepository
			};
			return new BufferedImageSaverMiddleware<Bgra32>
			{
				NextMiddleware = converterMiddleware 
			};
		})
		.Bind<ObservableListRepository<TT>>().To<ComposeObservableListRepository<TT>>()
		.Bind<ScreenBoundsProvider>().To<SharpHookScreenBoundsProvider>()
		.Bind<ImageCapturer>().As(Lifetime.Singleton).To<HotKeyScreenCapturer<Bgra32>>()
		.Bind<PixelConverter<Bgra32, Rgba32>>().To<Bgra32ToRgba32PixelConverter>()
#if OS_WINDOWS
		.Bind<ScreenCapture<Bgra32>>().To<DX11ScreenCapture>()
#elif OS_LINUX
		.Bind<ScreenCapturer<Bgra32>>().To<X11ScreenCapturer>()
#endif
		.Bind<IReactiveGlobalHook>().As(Lifetime.Singleton).To<SimpleReactiveGlobalHook>(_ =>
		{
			SimpleReactiveGlobalHook hook = new();
			hook.RunAsync();
			return hook;
		})
		.Bind<BindingsManager>().As(Lifetime.Singleton).To(context =>
		{
			context.Inject(out IReactiveGlobalHook hook);
			var observableGesture = hook.ObserveInputStates().Filter().ToGesture();
			return new BindingsManager(observableGesture);
		})
		.Bind().As(Lifetime.Singleton).To<DialogManager>()
		.Bind<TabItemViewModel>().To(context =>
		{
			context.Inject(out ImageSetsViewModel viewModel);
			return new TabItemViewModel(MaterialIconKind.FolderMultipleImage, "Images", viewModel);
		})
		.Bind<TabItemViewModel>(2).To(context =>
		{
			context.Inject(out DataSetsViewModel viewModel);
			return new TabItemViewModel(MaterialIconKind.ImageAlbum, "Datasets", viewModel);
		})
		.Bind<TabItemViewModel>(3).To(context =>
		{
			context.Inject(out AnnotationTabViewModel viewModel);
			return new TabItemViewModel(MaterialIconKind.ImageEdit, "Annotation", viewModel);
		})
		.Bind().As(Lifetime.Singleton).To<MainViewModel>()
		.RootBind<MainWindow>(nameof(MainWindow)).To(context =>
		{
			context.Inject(out MainViewModel viewModel);
			return new MainWindow { DataContext = viewModel };
		})
		.Bind().As(Lifetime.Singleton).To<WriteableBitmapPool>()
		.Root<ImageLoader>(nameof(ImageLoader))
		.Bind().Bind<ChangeListener>().As(Lifetime.Singleton).To<PeriodicDataSaver>()
		.Root<PeriodicDataSaver>(nameof(PeriodicDataSaver))
		.Root<AppDataFormatter>(nameof(AppDataFormatter))
		.Bind(typeof(AppData)).As(Lifetime.Singleton).To<Lock>()
		.TagAttribute<TagAttribute>()
		.Bind<ILogger>().To(context => Log.ForContext(context.ConsumerTypes.Single()))
		.Bind<IValidator<ImageSetData>>().To<NewImageSetDataValidator>()
		.Bind<IValidator<ExistingImageSetData>>().To<ExistingImageSetDataValidator>()
		.RootBind<ImagesViewModel>(nameof(ImagesViewModel)).Bind<ImageSelection>().As(Lifetime.Singleton)
		.To<ImagesViewModel>()
		.Bind<DataSetEditor>().To<AppDataDataSetEditor>()
		.Bind<ClassifierAnnotator>().As(Lifetime.Singleton).To<AppDataClassifierAnnotator>()
		.Root<ClassifierToolingViewModel>(nameof(ClassifierToolingViewModel))
		.Bind<BoundingAnnotator>().Bind<ObservableBoundingAnnotator>().As(Lifetime.Singleton)
		.To<AppDataBoundingAnnotator>()
		.Bind<BoundingEditor>().To<AppDataBoundingEditor>()
		.Root<PoserToolingViewModel>(nameof(PoserToolingViewModel))
		.Bind<PoserAnnotator>().Bind<ObservablePoserAnnotator>().As(Lifetime.Singleton).To<AppDataPoserAnnotator>()
		.Bind<AnnotationSideBarComponent>().As(Lifetime.Singleton).To<SideBarViewModel>()
		.Bind<ImageSetEditor>().As(Lifetime.Singleton).To<AppDataImageSetEditor>()
		.Bind<SelfActivityProvider>().To<AvaloniaSelfActivityProvider>()
		.Bind<AssetsDeleter>().Bind<AssetsMaker>().Bind<ObservableAnnotator>().As(Lifetime.Singleton).To<AppDataAnnotator>()
		.Bind<AnnotationButtonDefinitionFactory>().To<DeleteSelectedImageButtonDefinitionFactory>()
		.Bind<AnnotationButtonDefinitionFactory>(2).To<DeleteSelectedAssetButtonDefinitionFactory>()
		.Bind<DataSetSelectionViewModel>().Bind<DataSetSelectionDataContext>().Bind<DataSetSelection>().As(Lifetime.Singleton).To<DataSetSelectionViewModel>()
		.Bind<FileSystemDataAccess<Image>>().As(Lifetime.Singleton).To(_ => new FileSystemDataAccess<Image>(".bin"))
		.Bind<WriteImageDataAccess>().Bind<ReadImageDataAccess>().As(Lifetime.Singleton).To<FileSystemImageDataAccess>()
		.Bind<IValidator<NewDataSetData>>().To<NewDataSetDataValidator>()
		.Bind<IValidator<ExistingDataSetData>>().To<ExistingDataSetDataValidator>();
}