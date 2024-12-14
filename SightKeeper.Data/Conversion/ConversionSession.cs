using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.DataSets.Weights;

namespace SightKeeper.Data.Conversion;

internal sealed class ConversionSession
{
	public Dictionary<Tag, byte> TagsIndexes { get; } = new();
	public Dictionary<Weights, ushort> WeightsIds { get; } = new();
	public ushort WeightsIdCounter { get; set; }
}