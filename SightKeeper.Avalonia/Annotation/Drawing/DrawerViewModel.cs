using System.Collections.Generic;

namespace SightKeeper.Avalonia.Annotation.Drawing;

internal abstract class DrawerViewModel : ViewModel
{
	public abstract IReadOnlyCollection<DrawerItemViewModel> Items { get; }
}