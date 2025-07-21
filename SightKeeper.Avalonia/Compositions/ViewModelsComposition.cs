using Material.Icons;
using Pure.DI;
using SightKeeper.Application;
using SightKeeper.Avalonia.Dialogs;
using SightKeeper.Avalonia.ImageSets;
using SightKeeper.Avalonia.ImageSets.Capturing;
using SightKeeper.Avalonia.ImageSets.Card;
using SightKeeper.Avalonia.ImageSets.Commands;
using SightKeeper.Domain.Images;

namespace SightKeeper.Avalonia.Compositions;

public sealed class ViewModelsComposition
{
	private void Setup() => DI.Setup(nameof(ViewModelsComposition), CompositionKind.Internal)

		.Bind<TabItemViewModel>()
		.To(context =>
		{
			context.Inject(out ImageSetsViewModel viewModel);
			return new TabItemViewModel(MaterialIconKind.FolderMultipleImage, "Images", viewModel);
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
		.To<CapturingSettingsViewModel>();
}