using System.Collections.Immutable;
using SightKeeper.Domain.Model;

namespace SightKeeper.Data.Binary.Replication;

internal sealed class ReplicationSession
{
	public ImmutableDictionary<ushort, Game>? Games { get; set; }
}