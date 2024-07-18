using System.Collections.Immutable;
using FlakeId;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets;

namespace SightKeeper.Data.Binary.Conversion;

internal sealed class ReverseConversionSession
{
	public ImmutableDictionary<ushort, Game>? Games { get; set; }
	public Dictionary<Id, Tag> Tags { get; } = new();
	public Dictionary<Id, Screenshot> Screenshots { get; } = new();
	public Dictionary<Id, Weights> Weights { get; } = new();
}