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
using SightKeeper.Application.DataSets.Editing;
using SightKeeper.Application.ScreenshotsLibraries;
using SightKeeper.Application.ScreenshotsLibraries.Creating;
using SightKeeper.Application.Screenshotting;
using SightKeeper.Application.Screenshotting.Saving;
using SightKeeper.Avalonia.Annotation;
using SightKeeper.Avalonia.Annotation.Screenshots;
using SightKeeper.Avalonia.Annotation.Tooling;
using SightKeeper.Avalonia.DataSets;
using SightKeeper.Avalonia.Dialogs;
using SightKeeper.Data;
using SightKeeper.Data.Services;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.Screenshots;
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
		.Bind().Bind<ScreenshotsDataAccess>().Bind<ObservableScreenshotsDataAccess>().As(Lifetime.Singleton)
		.To<FileSystemScreenshotsDataAccess>()
		.Bind<ReadDataAccess<ScreenshotsLibrary>>().Bind<ObservableDataAccess<ScreenshotsLibrary>>()
		.Bind<WriteDataAccess<ScreenshotsLibrary>>().As(Lifetime.Singleton).To<AppDataScreenshotsLibrariesDataAccess>()
		.Bind<ReadDataAccess<DataSet>>().Bind<ObservableDataAccess<DataSet>>().Bind<WriteDataAccess<DataSet>>()
		.As(Lifetime.Singleton).To<AppDataDataSetsDataAccess>()
		.Bind<PendingScreenshotsCountReporter>().Bind<ScreenshotsSaver<Bgra32>>().As(Lifetime.Singleton)
		.To<BufferedScreenshotsSaver<Bgra32>>()
		.Bind<ObservableRepository<TT>>().To<DataAccessObservableRepository<TT>>()
		.Bind<ScreenBoundsProvider>().To<SharpHookScreenBoundsProvider>()
		.Bind<Screenshotter>().To<Screenshotter<Bgra32>>()
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
		.Bind<KeyManager>().As(Lifetime.Singleton).To(context =>
		{
			context.Inject(out IReactiveGlobalHook hook);
			KeyManagerFilter<FormattedKeyCode> keyboardManager = new(new SharpHookKeyboardKeyManager(hook));
			KeyManagerFilter<FormattedButton> mouseManager = new(new SharpHookMouseButtonsManager(hook));
			AggregateKeyManager keyManager = new([keyboardManager, mouseManager]);
			return keyManager;
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
		.Bind<IValidator<ScreenshotsLibraryData>>().To<ScreenshotsLibraryDataValidator>()
		.Bind<IValidator<ScreenshotsLibraryData>>("new").To<NewScreenshotsLibraryDataValidator>()
		.Bind<ScreenshotsLibrariesDeleter>().To<LockingScreenshotsLibrariesDeleter>()
		.RootBind<ScreenshotsViewModel>(nameof(ScreenshotsViewModel)).Bind<ScreenshotSelection>().As(Lifetime.Singleton).To<ScreenshotsViewModel>()
		.Bind<DataSetEditor>().To<AppDataDataSetEditor>()
		.Bind<ClassifierAnnotator>().To<AppDataClassifierAnnotator>()
		.Root<ClassifierAnnotationViewModel>(nameof(ClassifierAnnotationViewModel))
		.Root<TagSelectionViewModel>(nameof(TagSelectionViewModel))
		.Bind<BoundingAnnotator>().Bind<ObservableBoundingAnnotator>().As(Lifetime.Singleton).To<AppDataBoundingAnnotator>()
		.Bind<BoundingEditor>().To<AppDataBoundingEditor>();
}