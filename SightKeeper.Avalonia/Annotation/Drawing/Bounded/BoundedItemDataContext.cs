using Avalonia.Media;
using SightKeeper.Domain.DataSets.Assets.Items;

namespace SightKeeper.Avalonia.Annotation.Drawing.Bounded;

public interface BoundedItemDataContext : DrawerItemDataContext
{
	string Name { get; }
	Color Color { get; }
	Bounding Bounding { get; set; }
}