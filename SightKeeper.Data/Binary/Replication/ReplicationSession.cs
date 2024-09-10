using System.Collections.Immutable;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Model.DataSets.Weights;
using Tag = SightKeeper.Domain.Model.DataSets.Tags.Tag;

namespace SightKeeper.Data.Binary.Replication;

internal sealed class ReplicationSession
{
	public ImmutableDictionary<ushort, Game>? Games { get; set; }
	public Dictionary<(DataSet dataSet, byte tagId), Tag> Tags { get; } = new();
	public Dictionary<ushort, Weights> Weights { get; } = new();
}