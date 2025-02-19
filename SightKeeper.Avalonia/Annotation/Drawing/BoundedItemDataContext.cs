using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Avalonia.Annotation.Drawing;

public interface BoundedItemDataContext : DrawerItemDataContext
{
	Tag Tag { get; }
	Bounding Bounding { get; set; }
}