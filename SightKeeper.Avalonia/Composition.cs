using System.Linq;
using System.Threading;
using FluentValidation;
using HotKeys;
using HotKeys.Bindings;
using HotKeys.SharpHook;
using Material.Icons;
using Pure.DI;
using Serilog;
using SharpHook.Reactive;
using SightKeeper.Application;
using SightKeeper.Application.Annotation;
using SightKeeper.Application.DataSets.Editing;
using SightKeeper.Application.ImageSets;
using SightKeeper.Application.ImageSets.Creating;
using SightKeeper.Application.ScreenCapturing;
using SightKeeper.Application.ScreenCapturing.Saving;
using SightKeeper.Avalonia.Annotation;
using SightKeeper.Avalonia.Annotation.Drawing;
using SightKeeper.Avalonia.Annotation.Screenshots;
using SightKeeper.Avalonia.Annotation.Tooling;
using SightKeeper.Avalonia.DataSets;
using SightKeeper.Avalonia.Dialogs;
using SightKeeper.Data;
using SightKeeper.Data.Services;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.Images;
using SixLabors.ImageSharp.PixelFormats;
using ScreenshotsLibrariesViewModel = SightKeeper.Avalonia.ScreenshotsLibraries.ScreenshotsLibrariesViewModel;
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
		.RootBind<AppDataAccess>(nameof(AppDataAccess)).Bind<ApplicationSettingsProvider>().As(Lifetime.Singleton).To<AppDataAccess>()
		.Bind().Bind<ImageDataAccess>().Bind<ObservableImageDataAccess>().As(Lifetime.Singleton)
		.To<FileSystemImageDataAccess>()
		.Bind<ReadDataAccess<ImageSet>>().Bind<ObservableDataAccess<ImageSet>>()
		.Bind<WriteDataAccess<ImageSet>>().As(Lifetime.Singleton).To<AppDataImageSetsDataAccess>()
		.Bind<ReadDataAccess<DataSet>>().Bind<ObservableDataAccess<DataSet>>().Bind<WriteDataAccess<DataSet>>()
		.As(Lifetime.Singleton).To<AppDataDataSetsDataAccess>()
		.Bind<PendingImagesCountReporter>().Bind<ImageSaver<Bgra32>>().As(Lifetime.Singleton)
		.To<BufferedImageSaver<Bgra32>>()
		.Bind<ObservableRepository<TT>>().To<DataAccessObservableRepository<TT>>()
		.Bind<ScreenBoundsProvider>().To<SharpHookScreenBoundsProvider>()
		.Bind<HotKeyScreenCapture>().To<HotKeyScreenCapture<Bgra32>>()
		.Bind<PixelConverter<Bgra32, Rgba32>>().To<Bgra32ToRgba32PixelConverter>()
#if OS_WINDOWS
		.Bind<ScreenCapture<Bgra32>>().To<DX11ScreenCapture>()
#elif OS_LINUX
		.Bind<ScreenCapture<Bgra32>>().To<X11ScreenCapture>()
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
			context.Inject(out ScreenshotsLibrariesViewModel viewModel);
			return new TabItemViewModel(MaterialIconKind.FolderMultipleImage, "Screenshots", viewModel);
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
		.Root<ScreenshotImageLoader>(nameof(ScreenshotImageLoader))
		.Bind().As(Lifetime.Singleton).To<PeriodicAppDataSaver>()
		.Root<PeriodicAppDataSaver>(nameof(PeriodicAppDataSaver))
		.Root<AppDataFormatter>(nameof(AppDataFormatter))
		.Bind(typeof(AppData)).As(Lifetime.Singleton).To<Lock>()
		.TagAttribute<TagAttribute>()
		.Bind<ILogger>().To(context => Log.ForContext(context.ConsumerTypes.Single()))
		.Bind<IValidator<ImageSetData>>().To<ImageSetDataValidator>()
		.Bind<IValidator<ImageSetData>>("new").To<NewImageSetDataValidator>()
		.Bind<ImageSetDeleter>().To<LockingImageSetDeleter>()
		.RootBind<ScreenshotsViewModel>(nameof(ScreenshotsViewModel)).Bind<AnnotationScreenshotsComponent>().Bind<ScreenshotSelection>().As(Lifetime.Singleton).To<ScreenshotsViewModel>()
		.Bind<DataSetEditor>().To<AppDataDataSetEditor>()
		.Bind<ClassifierAnnotator>().To<AppDataClassifierAnnotator>()
		.Root<ClassifierAnnotationViewModel>(nameof(ClassifierAnnotationViewModel))
		.Bind<BoundingAnnotator>().Bind<ObservableBoundingAnnotator>().As(Lifetime.Singleton).To<AppDataBoundingAnnotator>()
		.Bind<BoundingEditor>().To<AppDataBoundingEditor>()
		.Root<PoserToolingViewModel>(nameof(PoserToolingViewModel))
		.Bind<PoserAnnotator>().Bind<ObservablePoserAnnotator>().As(Lifetime.Singleton).To<AppDataPoserAnnotator>()
		.Bind<AnnotationDrawerComponent>().To<DrawerViewModel>()
		.Bind<AnnotationSideBarComponent>().To<SideBarViewModel>();
}