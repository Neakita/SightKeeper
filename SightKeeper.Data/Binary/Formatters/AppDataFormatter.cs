using MemoryPack;
using SightKeeper.Data.Binary.Conversion;
using SightKeeper.Data.Binary.Conversion.DataSets;
using SightKeeper.Data.Binary.Conversion.Profiles;
using SightKeeper.Data.Binary.Services;

namespace SightKeeper.Data.Binary.Formatters;

public sealed class AppDataFormatter : MemoryPackFormatter<AppData>
{
	public AppDataFormatter(
		FileSystemScreenshotsDataAccess screenshotsDataAccess,
		FileSystemWeightsDataAccess weightsDataAccess)
	{
		_dataSetsConverter = new DataSetsConverter(screenshotsDataAccess, weightsDataAccess);
		_profilesConverter = new ProfilesConverter(weightsDataAccess);
	}
	
	public override void Serialize<TBufferWriter>(ref MemoryPackWriter<TBufferWriter> writer, scoped ref AppData? value)
	{
		if (value == null)
		{
			writer.WriteNullObjectHeader();
			return;
		}
		ConversionSession session = new();
		RawAppData raw = new(
			GamesConverter.Convert(value.Games, session),
			_dataSetsConverter.Convert(value.DataSets, session),
			_profilesConverter.Convert(value.Profiles, session),
			value.ApplicationSettings);
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
		var raw = reader.ReadPackable<RawAppData>();
		if (raw == null)
		{
			value = null;
			return;
		}
		ReverseConversionSession session = new();
		value = new AppData(
			GamesConverter.ConvertBack(raw.Games, session),
			_dataSetsConverter.ConvertBack(raw.DataSets, session),
			_profilesConverter.ConvertBack(raw.Profiles, session),
			raw.ApplicationSettings);
	}

	private readonly DataSetsConverter _dataSetsConverter;
	private readonly ProfilesConverter _profilesConverter;
}