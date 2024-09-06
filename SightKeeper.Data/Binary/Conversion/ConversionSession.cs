using System.Collections.Immutable;
using SightKeeper.Domain.Model;

namespace SightKeeper.Data.Binary.Conversion;

internal sealed class ConversionSession
{
	public ImmutableDictionary<Game, ushort>? GameIds { get; set; }
}