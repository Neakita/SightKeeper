using System.Collections.Generic;
using SightKeeper.Avalonia.Annotation.Drawing.Poser;
using SightKeeper.Domain.Screenshots;

namespace SightKeeper.Avalonia.Annotation.Drawing;

public interface DrawerDataContext
{
	Screenshot? Screenshot { get; }
	IReadOnlyCollection<DrawerItemDataContext> Items { get; }
	BoundedItemDataContext? SelectedItem { get; set; }
	BoundingDrawerDataContext BoundingDrawer { get; }
	KeyPointDrawerDataContext KeyPointDrawer { get; }
}