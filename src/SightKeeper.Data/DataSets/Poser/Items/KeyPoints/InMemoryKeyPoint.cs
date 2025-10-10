using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.DataSets.Poser.Items.KeyPoints;

internal sealed class InMemoryKeyPoint : KeyPoint
{
	public required Tag Tag { get; init; }
	public Vector2<double> Position { get; set; }
}