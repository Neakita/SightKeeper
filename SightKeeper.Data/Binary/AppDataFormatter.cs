using System.Collections.Immutable;
using MemoryPack;
using SightKeeper.Data.Binary.Conversion;
using SightKeeper.Data.Binary.Conversion.DataSets;
using SightKeeper.Data.Binary.Conversion.Profiles;
using SightKeeper.Data.Binary.Replication;
using SightKeeper.Data.Binary.Replication.DataSets;
using SightKeeper.Data.Binary.Replication.Profiles;
using SightKeeper.Data.Binary.Services;

namespace SightKeeper.Data.Binary;

public sealed class AppDataFormatter : MemoryPackFormatter<AppData>
{
	public AppDataFormatter(FileSystemScreenshotsDataAccess screenshotsDataAccess)
	{
		_screenshotsDataAccess = screenshotsDataAccess;
	}
	
	public override void Serialize<TBufferWriter>(ref MemoryPackWriter<TBufferWriter> writer, scoped ref AppData? value)
	{
		if (value == null)
		{
			writer.WriteNullObjectHeader();
			return;
		}
		ConversionSession session = new();
		var games = GamesConverter.Convert(value.Games, session);
		MultiDataSetConverter dataSetConverter = new(_screenshotsDataAccess, session);
		var dataSets = dataSetConverter.Convert(value.DataSets);
		var profiles = _profileConverter.Convert(value.Profiles, session).ToImmutableArray();
		PackableAppData packed = new(
			games,
			dataSets,
			profiles,
			value.ApplicationSettings);
		writer.WritePackable(packed);
	}

	public override void Deserialize(ref MemoryPackReader reader, scoped ref AppData? value)
	{
		if (reader.PeekIsNull())
		{
			reader.Advance(1); // skip null block
			value = null;
			return;
		}
		var packed = reader.ReadPackable<PackableAppData>();
		if (packed == null)
		{
			value = null;
			return;
		}
		ReplicationSession session = new();

		var games = GameReplicator.Replicate(packed.Games, session);
		MultiDataSetReplicator dataSetReplicator = new(_screenshotsDataAccess, session);
		var dataSets = dataSetReplicator.Replicate(packed.DataSets);
		var profiles = ProfileReplicator.Replicate(packed.Profiles, session).ToHashSet();
		value = new AppData(games, dataSets, profiles, packed.ApplicationSettings);
	}

	private readonly FileSystemScreenshotsDataAccess _screenshotsDataAccess;
	private readonly ProfileConverter _profileConverter = new();
}