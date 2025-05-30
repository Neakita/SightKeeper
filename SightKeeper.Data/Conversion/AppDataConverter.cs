using System.Collections.Immutable;
using SightKeeper.Data.Conversion.DataSets;
using SightKeeper.Data.Services;

namespace SightKeeper.Data.Conversion;

internal sealed class AppDataConverter
{
	public AppDataConverter(FileSystemImageRepository imageRepository)
	{
		_imageRepository = imageRepository;
	}

	public PackableAppData Convert(AppData data)
	{
		ConversionSession session = new();
		ImageSetConverter imageSetConverter = new(_imageRepository);
		DataSetsConverter dataSetsConverter = new(session, _imageRepository);
		return new PackableAppData
		{
			ImageSets = imageSetConverter.ConvertImageSets(data.ImageSets).ToImmutableArray(),
			DataSets = dataSetsConverter.ConvertDataSets(data.DataSets).ToImmutableArray()
		};
	}

	private readonly FileSystemImageRepository _imageRepository;
}