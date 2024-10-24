using MemoryPack;
using SightKeeper.Data.Binary.Conversion;
using SightKeeper.Data.Binary.Replication;
using SightKeeper.Data.Binary.Services;

namespace SightKeeper.Data.Binary;

public sealed class AppDataFormatter : MemoryPackFormatter<AppData>
{
	public AppDataFormatter(FileSystemScreenshotsDataAccess screenshotsDataAccess, object conversionLock)
	{
		_conversionLock = conversionLock;
		_converter = new AppDataConverter(screenshotsDataAccess);
		_replicator = new AppDataReplicator(screenshotsDataAccess);
	}

	public override void Serialize<TBufferWriter>(ref MemoryPackWriter<TBufferWriter> writer, scoped ref AppData? value)
	{
		if (value == null)
		{
			writer.WriteNullObjectHeader();
			return;
		}
		PackableAppData packed;
		lock (_conversionLock)
			packed = _converter.Convert(value);
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
		value = _replicator.Replicate(packed);
	}

	private readonly object _conversionLock;
	private readonly AppDataConverter _converter;
	private readonly AppDataReplicator _replicator;
}