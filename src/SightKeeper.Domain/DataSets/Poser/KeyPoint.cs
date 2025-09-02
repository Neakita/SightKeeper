using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Domain.DataSets.Poser;

public interface KeyPoint : TagUser
{
	Tag Tag { get; }
	Vector2<double> Position { get; set; }
}