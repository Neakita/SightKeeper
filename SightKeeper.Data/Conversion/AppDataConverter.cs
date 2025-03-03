using System.Collections.Immutable;
using SightKeeper.Data.Conversion.DataSets;
using SightKeeper.Data.Services;

namespace SightKeeper.Data.Conversion;

internal sealed class AppDataConverter
{
	public AppDataConverter(FileSystemImageDataAccess imageDataAccess)
	{
		_imageDataAccess = imageDataAccess;
	}

	public PackableAppData Convert(AppData data)
	{
		ConversionSession session = new();
		ImageSetConverter imageSetConverter = new(_imageDataAccess);
		DataSetsConverter dataSetsConverter = new(session, _imageDataAccess);
		return new PackableAppData
		{
			ImageSets = imageSetConverter.ConvertImageSets(data.ImageSets).ToImmutableArray(),
			DataSets = dataSetsConverter.ConvertDataSets(data.DataSets).ToImmutableArray(),
			ApplicationSettings = data.ApplicationSettings,
		};
	}

	private readonly FileSystemImageDataAccess _imageDataAccess;
}