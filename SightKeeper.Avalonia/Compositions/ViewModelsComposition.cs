using Material.Icons;
using Pure.DI;
using SightKeeper.Avalonia.Dialogs;
using SightKeeper.Avalonia.ImageSets;

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
		.To<DialogManager>();
}