using MemoryPack;
using SightKeeper.Data.Binary.Conversion;
using SightKeeper.Data.Binary.Conversion.DataSets;
using SightKeeper.Data.Binary.Services;

namespace SightKeeper.Data.Binary;

public sealed class AppDataFormatter : MemoryPackFormatter<AppData>
{
	public AppDataFormatter(
		FileSystemScreenshotsDataAccess screenshotsDataAccess,
		FileSystemWeightsDataAccess weightsDataAccess)
	{
		_dataSetConverter = new MultiDataSetConverter(screenshotsDataAccess);
	}
	
	public override void Serialize<TBufferWriter>(ref MemoryPackWriter<TBufferWriter> writer, scoped ref AppData? value)
	{
		if (value == null)
		{
			writer.WriteNullObjectHeader();
			return;
		}
		ConversionSession session = new();
		PackableAppData raw = new(
			value.ApplicationSettings,
			GamesConverter.Convert(value.Games, session),
			_dataSetConverter.Convert(value.DataSets, session));
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
		throw new NotImplementedException();
	}

	private readonly MultiDataSetConverter _dataSetConverter;
}