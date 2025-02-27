using System.Collections.Generic;
using SightKeeper.Avalonia.Annotation.Drawing.Poser;
using SightKeeper.Domain.Images;

namespace SightKeeper.Avalonia.Annotation.Drawing;

public interface DrawerDataContext
{
	Image? Image { get; }
	IReadOnlyCollection<DrawerItemDataContext> Items { get; }
	BoundedItemDataContext? SelectedItem { get; set; }
	BoundingDrawerDataContext BoundingDrawer { get; }
	KeyPointDrawerDataContext KeyPointDrawer { get; }
}