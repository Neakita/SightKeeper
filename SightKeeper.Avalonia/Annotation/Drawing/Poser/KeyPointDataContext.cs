using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Avalonia.Annotation.Drawing.Poser;

public interface KeyPointDataContext
{
	Tag Tag { get; }
	Vector2<double> Position { get; set; }
}