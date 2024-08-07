using System.Collections.Immutable;
using FlakeId;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets.Tags;

namespace SightKeeper.Data.Binary.Conversion;

internal sealed class ConversionSession
{
	public ImmutableDictionary<Game, ushort>? Games { get; set; }
	public Dictionary<Tag, Id> Tags { get; } = new();
}