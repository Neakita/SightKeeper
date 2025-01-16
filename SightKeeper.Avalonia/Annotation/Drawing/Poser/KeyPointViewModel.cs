using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Avalonia.Annotation.Drawing.Poser;

public abstract class KeyPointViewModel : ViewModel
{
	public abstract Tag Tag { get; }
	public abstract PoserItemViewModel Item { get; }
	public abstract Vector2<double> Position { get; set; }
}