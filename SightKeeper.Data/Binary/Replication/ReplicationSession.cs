using System.Collections.Immutable;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets.Weights;

namespace SightKeeper.Data.Binary.Replication;

internal sealed class ReplicationSession
{
	public ImmutableDictionary<ushort, Game>? Games { get; set; }
	public ImmutableDictionary<ushort, Weights>? Weights { get; set; }
}