using System.Collections.Immutable;
using MemoryPack;
using SightKeeper.Data.Binary.Conversion;
using SightKeeper.Data.Binary.Conversion.DataSets;
using SightKeeper.Data.Binary.Replication;
using SightKeeper.Data.Binary.Replication.DataSets;
using SightKeeper.Data.Binary.Services;
using SightKeeper.Domain.Model.DataSets.Weights;

namespace SightKeeper.Data.Binary;

public sealed class AppDataFormatter : MemoryPackFormatter<AppData>
{
	public AppDataFormatter(FileSystemScreenshotsDataAccess screenshotsDataAccess)
	{
		_dataSetConverter = new MultiDataSetConverter(screenshotsDataAccess);
		_dataSetReplicator = new MultiDataSetReplicator(screenshotsDataAccess);
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
		var weightsLookupBuilder = ImmutableDictionary.CreateBuilder<Weights, ushort>();
		var dataSets = _dataSetConverter.Convert(value.DataSets, session, weightsLookupBuilder);
		session.WeightsIds = weightsLookupBuilder.ToImmutable();
		PackableAppData raw = new(
			value.ApplicationSettings,
			games,
			dataSets);
		writer.WritePackable(raw);
	}

	public override void Deserialize(ref MemoryPackReader reader, scoped ref AppData? value)
	{
		if (reader.PeekIsNull())
		{
			reader.Advance(1); // skip null block
			value = null;
			return;
		}
		var packable = reader.ReadPackable<PackableAppData>();
		if (packable == null)
		{
			value = null;
			return;
		}
		ReplicationSession session = new();

		var games = GameReplicator.Replicate(packable.Games, session);
		var weightsLookupBuilder = ImmutableDictionary.CreateBuilder<ushort, Weights>();
		var dataSets = _dataSetReplicator.Replicate(packable.DataSets, session, weightsLookupBuilder);
		session.Weights = weightsLookupBuilder.ToImmutable();
		value = new AppData(games, dataSets, packable.ApplicationSettings);
	}

	private readonly MultiDataSetConverter _dataSetConverter;
	private readonly MultiDataSetReplicator _dataSetReplicator;
}