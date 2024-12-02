using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets.Poser;

namespace SightKeeper.Avalonia.Annotation.Drawing.Poser;

internal abstract class KeyPointViewModel : ViewModel
{
	public abstract KeyPointTag Tag { get; }
	public abstract PoserItemViewModel Item { get; }
	public abstract Vector2<double> Position { get; set; }
}