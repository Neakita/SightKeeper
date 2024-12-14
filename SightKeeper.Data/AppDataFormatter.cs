using MemoryPack;
using SightKeeper.Data.Conversion;
using SightKeeper.Data.Replication;
using SightKeeper.Data.Services;

namespace SightKeeper.Data;

public sealed class AppDataFormatter : MemoryPackFormatter<AppData>
{
	public AppDataFormatter(FileSystemScreenshotsDataAccess screenshotsDataAccess, AppDataEditingLock editingLock)
	{
		_editingLock = editingLock;
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
		lock (_editingLock)
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

	private readonly AppDataEditingLock _editingLock;
	private readonly AppDataConverter _converter;
	private readonly AppDataReplicator _replicator;
}