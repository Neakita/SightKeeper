using System.Collections.Generic;
using SightKeeper.Avalonia.Annotation.Drawing.Bounded;
using SightKeeper.Avalonia.Annotation.Drawing.Poser;

namespace SightKeeper.Avalonia.Annotation.Drawing;

public interface DrawerDataContext
{
	ImageDataContext? Image { get; }
	IReadOnlyCollection<DrawerItemDataContext> Items { get; }
	BoundedItemDataContext? SelectedItem { get; set; }
	BoundingDrawerDataContext BoundingDrawer { get; }
	KeyPointDrawerDataContext KeyPointDrawer { get; }
}