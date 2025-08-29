using Avalonia.Platform;
using Material.Icons;
using Pure.DI;
using SightKeeper.Application;
using SightKeeper.Avalonia.Annotation;
using SightKeeper.Avalonia.Annotation.Drawing;
using SightKeeper.Avalonia.Annotation.Images;
using SightKeeper.Avalonia.Annotation.Tooling;
using SightKeeper.Avalonia.DataSets;
using SightKeeper.Avalonia.Dialogs;
using SightKeeper.Avalonia.ImageSets;
using SightKeeper.Avalonia.ImageSets.Capturing;
using SightKeeper.Avalonia.ImageSets.Card;
using SightKeeper.Avalonia.ImageSets.Commands;
using SightKeeper.Avalonia.Misc;
using SightKeeper.Avalonia.Training;
using SightKeeper.Domain.Images;
using SixLabors.ImageSharp.PixelFormats;

namespace SightKeeper.Avalonia.Compositions;

public sealed class ViewModelsComposition
{
	private void Setup() => DI.Setup(nameof(ViewModelsComposition), CompositionKind.Internal)

		.Bind<TabItemViewModel>(1)
		.To(context =>
		{
			context.Inject(out ImageSetsViewModel viewModel);
			return new TabItemViewModel(MaterialIconKind.FolderMultipleImage, "Images", viewModel);
		})

		.Bind<TabItemViewModel>(2)
		.To(context =>
		{
			context.Inject(out DataSetsViewModel viewModel);
			return new TabItemViewModel(MaterialIconKind.ImageAlbum, "Datasets", viewModel);
		})

		.Bind<TabItemViewModel>(3)
		.To(context =>
		{
			context.Inject(out AnnotationTabViewModel viewModel);
			return new TabItemViewModel(MaterialIconKind.ImageEdit, "Annotation", viewModel);
		})

		.Bind<TabItemViewModel>(4)
		.To(context =>
		{
			context.Inject(out TrainingViewModel viewModel);
			return new TabItemViewModel(MaterialIconKind.School, "Training", viewModel);
		})

		.Bind<DialogManager>()
		.As(Lifetime.Singleton)
		.To<DialogManager>()

		.Bind<ImageSetsViewModel>()
		.To(context =>
		{
			context.Inject(out CreateImageSetCommand createImageSetCommand);
			context.Inject(out ObservableListRepository<ImageSet> imageSetRepository);
			context.Inject(out ImageSetCardDataContextFactory imageSetCardDataContextFactory);
			context.Inject(out CapturingSettingsDataContext capturingSettingsDataContext);
			return new ImageSetsViewModel(
				createImageSetCommand,
				imageSetRepository,
				imageSetCardDataContextFactory,
				capturingSettingsDataContext);
		})

		.Bind<ImageSetCardDataContextFactory>()
		.To<ImageSetCardViewModelFactory>()

		.Bind<CapturingSettingsDataContext>()
		.To<CapturingSettingsViewModel>()

		.Bind<AdditionalToolingSelection>()
		.Bind<SideBarDataContext>()
		.As(Lifetime.Singleton)
		.To<SideBarViewModel>()

		.Bind<ImagesDataContext>()
		.Bind<ImageSelection>()
		.As(Lifetime.Singleton)
		.To<ImagesViewModel>()

		.Bind<ImageSetSelectionDataContext>()
		.Bind<ImageSetSelection>()
		.As(Lifetime.Singleton)
		.To<ImageSetSelectionViewModel>()

		.Bind<DataSetSelection>()
		.Bind<DataSetSelectionDataContext>()
		.As(Lifetime.Singleton)
		.To<DataSetSelectionViewModel>()

		.Bind<DrawerDataContext>()
		.Bind<SelectedItemProvider>()
		.As(Lifetime.Singleton)
		.To<DrawerViewModel>()
	
		.Bind<ActionsDataContext>()
		.To<ActionsViewModel>()
	
		.Bind<WriteableBitmapImageLoader>()
		.To(context =>
		{
			context.Inject(out WriteableBitmapPool bitmapPool);
			context.Inject(out ImageLoader<Rgba32> imageLoader);
			return new WriteableBitmapImageLoader<Rgba32>(bitmapPool, PixelFormat.Rgb32, imageLoader);
		})
	
		.Bind()
		.As(Lifetime.Singleton)
		.To<WriteableBitmapPool>();
}