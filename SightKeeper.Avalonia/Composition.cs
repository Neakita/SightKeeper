using Pure.DI;
#if OS_LINUX
using SightKeeper.Application.Linux.X11;
#endif
#if OS_WINDOWS
#endif

namespace SightKeeper.Avalonia;

public sealed partial class Composition
{
	private void Setup() => DI.Setup(nameof(Composition))
		.Hint(Hint.Resolve, "Off")

		.Root<MainWindow>(nameof(MainWindow));
}