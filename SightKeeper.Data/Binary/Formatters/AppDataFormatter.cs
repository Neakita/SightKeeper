using MemoryPack;
using SightKeeper.Data.Binary.Services;

namespace SightKeeper.Data.Binary.Formatters;

public sealed class AppDataFormatter : MemoryPackFormatter<AppData>
{
	public AppDataFormatter(
		FileSystemScreenshotsDataAccess screenshotsDataAccess,
		FileSystemWeightsDataAccess weightsDataAccess)
	{
	}
	
	public override void Serialize<TBufferWriter>(ref MemoryPackWriter<TBufferWriter> writer, scoped ref AppData? value)
	{
		if (value == null)
		{
			writer.WriteNullObjectHeader();
			return;
		}
	}

	public override void Deserialize(ref MemoryPackReader reader, scoped ref AppData? value)
	{
		if (reader.PeekIsNull())
		{
			reader.Advance(1); // skip null block
			value = null;
			return;
		}
		var raw = reader.ReadPackable<RawAppData>();
		if (raw == null)
		{
			value = null;
			return;
		}
	}
}