using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Avalonia.Annotation.Drawing;

internal abstract class DrawerItemViewModel : ViewModel
{
	public abstract Tag Tag { get; }
	public abstract Bounding Bounding { get; set; }

	public Bounding DisplayBounding
	{
		get;
		set => SetProperty(ref field, value);
	}
}