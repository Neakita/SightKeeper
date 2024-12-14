using FlakeId;
using SightKeeper.Domain.Screenshots;

namespace SightKeeper.Data.Binary.Replication;

internal sealed class ReplicationSession
{
	public Dictionary<Id, Screenshot> Screenshots { get; } = new();
}