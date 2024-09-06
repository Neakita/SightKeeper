using System.Collections.Immutable;
using CommunityToolkit.Diagnostics;
using SightKeeper.Data.Binary.Model.DataSets;
using SightKeeper.Data.Binary.Model.DataSets.Compositions;
using SightKeeper.Data.Binary.Model.DataSets.Tags;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Model.DataSets.Screenshots;
using SightKeeper.Domain.Model.DataSets.Tags;

namespace SightKeeper.Data.Binary.Replication;

internal abstract class DataSetReplicator
{
	public DataSet Replicate(PackableDataSet packed, ReplicationSession session)
	{
		Guard.IsNotNull(session.Games);
		var game = packed.GameId == null ? null : session.Games[packed.GameId.Value];
		var composition = ReplicateComposition(packed.Composition);
		var dataSet = CreateDataSet(packed.Name, packed.Description, game, composition);
		var tagsLookup = ReplicateTags(dataSet.Tags, packed.GetTags());
		return dataSet;
	}

	protected abstract DataSet CreateDataSet(string name, string description, Game? game, Composition? composition);

	protected virtual Tag ReplicateTag(
		TagsLibrary library,
		PackableTag packed,
		ImmutableDictionary<(byte, byte?), Tag>.Builder lookupBuilder)
	{
		byte insertIndex = (byte)library.Count;
		var tag = library.CreateTag(packed.Name);
		tag.Color = packed.Color;
		lookupBuilder.Add((insertIndex, null), tag);
		return tag;
	}

	private static Composition ReplicateComposition(PackableComposition? composition)
	{
		return composition switch
		{
			PackableTransparentComposition transparentComposition =>
				new TransparentComposition(
					transparentComposition.MaximumScreenshotsDelay,
					transparentComposition.Opacities),
			_ => throw new ArgumentOutOfRangeException(nameof(composition))
		};
	}

	private ImmutableDictionary<(byte, byte?), Tag> ReplicateTags(TagsLibrary library, ImmutableArray<PackableTag> tags)
	{
		var lookupBuilder = ImmutableDictionary.CreateBuilder<(byte, byte?), Tag>();
		foreach (var tag in tags)
			ReplicateTag(library, tag, lookupBuilder);
		return lookupBuilder.ToImmutable();
	}
}