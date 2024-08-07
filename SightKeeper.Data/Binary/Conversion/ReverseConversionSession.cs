using System.Collections.Immutable;
using FlakeId;
using SightKeeper.Domain.Model.DataSets.Screenshots;
using SightKeeper.Domain.Model.DataSets.Tags;
using SightKeeper.Domain.Model.DataSets.Weights;

namespace SightKeeper.Data.Binary.Conversion;

internal sealed class ReverseConversionSession
{
	public ImmutableDictionary<ushort, Domain.Model.Game>? Games { get; set; }
	public Dictionary<Id, Tag> Tags { get; } = new();
	public Dictionary<Id, Screenshot> Screenshots { get; } = new();
	public Dictionary<Id, Weights> Weights { get; } = new();
}