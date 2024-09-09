using System.Collections.Immutable;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets.Tags;
using SightKeeper.Domain.Model.DataSets.Weights;

namespace SightKeeper.Data.Binary.Conversion;

internal sealed class ConversionSession
{
	public ImmutableDictionary<Game, ushort>? GameIds { get; set; }
	public Dictionary<Tag, (byte TagId, byte? KeyPointTagId)> TagsIds { get; } = new();
	public ImmutableDictionary<Weights, ushort>? WeightsIds { get; set; }
	public ushort WeightsIdCounter { get; set; }
}