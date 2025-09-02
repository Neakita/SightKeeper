using Avalonia.Media;
using SightKeeper.Domain;

namespace SightKeeper.Avalonia.Annotation.Drawing.Poser;

public interface KeyPointDataContext : DrawerItemDataContext
{
	string Name { get; }
	Color Color { get; }
	new Vector2<double> Position { get; set; }
	Vector2<double> DrawerItemDataContext.Position => Position;
}