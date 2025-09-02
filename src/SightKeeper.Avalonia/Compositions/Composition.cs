using Pure.DI;

namespace SightKeeper.Avalonia.Compositions;

public sealed partial class Composition
{
	private void Setup() => DI.Setup(nameof(Composition))
		.Hint(Hint.Resolve, "Off")
		.Hint(Hint.ThreadSafe, "Off")
		.Hint(Hint.ToString, "On")

		.DependsOn(nameof(PersistenceComposition))
		.DependsOn(nameof(ServicesComposition))
		.DependsOn(nameof(ViewModelsComposition))
		.DependsOn(nameof(PngImagePersistenceComposition))

		.RootBind<MainWindow>(nameof(MainWindow)).To(context =>
		{
			context.Inject(out MainViewModel viewModel);
			return new MainWindow
			{
				DataContext = viewModel
			};
		});
}