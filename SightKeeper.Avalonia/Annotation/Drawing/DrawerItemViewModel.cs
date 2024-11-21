using SightKeeper.Domain.Model.DataSets.Assets;
using SightKeeper.Domain.Model.DataSets.Tags;

namespace SightKeeper.Avalonia.Annotation.Drawing;

internal abstract class DrawerItemViewModel : ViewModel
{
	public abstract Tag Tag { get; }
	public abstract Bounding Bounding { get; }
}